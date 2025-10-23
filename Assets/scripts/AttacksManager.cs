using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static UnityEngine.ParticleSystem;

public class BasicAttack
{
    protected ParticleSystem particle;
    protected KeyCode keyCode;

    public virtual void onUpdateFunc() 
    {
        if (Input.GetKeyDown(keyCode))
        {
            particle.Play();
        }
    }

    public BasicAttack(ParticleSystem particle, KeyCode key)
    {
        this.particle = particle;
        this.keyCode = key;
    }
}

public class ContinuousAttack : BasicAttack
{
    static readonly KeyCode[] breakKeys = new KeyCode[]
    {
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Mouse3,
        KeyCode.Mouse4
    };

    public override void onUpdateFunc()
    {
        if (Input.GetKeyDown(keyCode))
        {
            particle.Play();
        }

        foreach (var key in breakKeys)
        {
            //if (key != keyCode && Input.GetKeyDown(key))
            {
                particle.Stop();
                break;
            }
        }
    }

    public ContinuousAttack(ParticleSystem particle, KeyCode key) : base(particle, key) {}
}

public class AttacksManager : MonoBehaviour 
{
    [Header("Available Particles")]
    public List<ParticleSystem> availableAttacks = new List<ParticleSystem>();

    [Header("Assigned Attacks")]
    public ParticleSystem leftAttackParticle;
    public ParticleSystem rightAttackParticle;
    public ParticleSystem add1AttackParticle;
    public ParticleSystem add2AttackParticle;

    BasicAttack leftClickAttack;
    BasicAttack rightClickAttack;
    BasicAttack additionalAttack1;
    BasicAttack additionalAttack2;

    public ManagerGame gameManager;

    void Start()
    {

        ApplySelectedAttacks();
    }
    void Update()
    {
        if(gameManager.gameLoop)
        {
            RotateToMouse();

            leftClickAttack.onUpdateFunc();
            rightClickAttack.onUpdateFunc();
            additionalAttack1.onUpdateFunc();
            additionalAttack2.onUpdateFunc();

        }
        
    }

    public void ApplySelectedAttacks()
    {
        leftClickAttack = new BasicAttack(leftAttackParticle, KeyCode.Mouse0);
        rightClickAttack = new BasicAttack(rightAttackParticle, KeyCode.Mouse1);
        additionalAttack1 = new BasicAttack(add1AttackParticle, KeyCode.Mouse3);
        additionalAttack2 = new BasicAttack(add2AttackParticle, KeyCode.Mouse4);
    }

    void RotateToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 target = hit.point;
            target.y = transform.position.y;

            Vector3 direction = (target - transform.position).normalized;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
