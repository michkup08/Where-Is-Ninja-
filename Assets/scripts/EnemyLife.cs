using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class LifeSystem: MonoBehaviour
{
    public float maxhp = 100;
    public float hp = 100;
    public Image healthbarForeground;
    public Canvas healthbarCanvas;
    public Animator animator;

    public bool isAlive()
    {
        return hp > 0;
    }

    public void takeDamage(float damage)
    {
        hp -= damage;
        healthbarForeground.fillAmount = hp/maxhp;
        if (healthbarCanvas != null)
        {
            healthbarCanvas?.gameObject.SetActive(true);
        }
    }
}

public class EnemyLife : LifeSystem
{
    [Header("Ustawienia Bossa")]
    public bool isBoss = false;

    private bool hasDied = false;

    private void Start()
    {
        if (maxhp <= 0) maxhp = 100;
        hp = maxhp;

        hasDied = false;

        if (healthbarCanvas != null)
            healthbarCanvas.gameObject.SetActive(true);
    }

    void Update()
    {
        if (hp <= 0 && !hasDied)
        {
            Die();
        }
    }

    void Die()
    {
        hasDied = true;

        if (animator != null) animator.SetTrigger("Death1");
        if (healthbarCanvas != null) healthbarCanvas.gameObject.SetActive(false);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (isBoss)
        {
            UnityEngine.Debug.Log("BOSS POKONANY! Uruchamiam zakoñczenie.");
            GameEnding ending = FindObjectOfType<GameEnding>();

            if (ending != null)
            {
                ending.TriggerEndingSequence();
            }
            else
            {
                UnityEngine.Debug.LogError("Nie znaleziono GameEnding!");
            }
        }
        else
        {
            UnityEngine.Debug.Log($"Wróg {gameObject.name} pokonany.");
            Destroy(gameObject, 5f);
        }
    }
}