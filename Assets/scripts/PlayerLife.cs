using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class PlayerLife : LifeSystem
{
    [Header("Ustawienia Œmierci")]
    public GameObject deathScreenUI;
    public AudioClip deathSound;
    public float restartDelay = 5f;

    private bool isDead = false;
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    private LevelManager levelManager;

    private void Awake()
    {
        hp = 150;

        if (deathScreenUI != null)
        {
            videoPlayer = deathScreenUI.GetComponent<VideoPlayer>();
            audioSource = deathScreenUI.GetComponent<AudioSource>();

            deathScreenUI.SetActive(false);

            if (videoPlayer != null)
            {
                videoPlayer.Prepare();
            }
        }
    }

    void Update()
    {
        if (!isAlive() && !isDead)
        {
            StartCoroutine(DieAndRestart());
        }

        if (hp > maxhp)
        {
            maxhp = hp;
        }
    }

    IEnumerator DieAndRestart()
    {
        isDead = true;

        if (TryGetComponent(out Renderer rend)) rend.enabled = false;
        if (TryGetComponent(out Collider col)) col.enabled = false;
        foreach (var childRend in GetComponentsInChildren<Renderer>())
            childRend.enabled = false;

        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);

            if (videoPlayer != null)
            {
                videoPlayer.frame = 0;
                videoPlayer.Play();
            }
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
        }

        yield return new WaitForSeconds(restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}