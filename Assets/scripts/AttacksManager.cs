using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    //static readonly KeyCode[] breakKeys = new KeyCode[]
    //{
    //    KeyCode.W,
    //    KeyCode.A,
    //    KeyCode.S,
    //    KeyCode.D,
    //    KeyCode.Mouse0,
    //    KeyCode.Mouse1,
    //    KeyCode.Mouse3,
    //    KeyCode.Mouse4
    //};

    public override void onUpdateFunc()
    {
        if (Input.GetKey(keyCode))
        {
            particle.Play();
        }
        else
        {
            if (particle.isPlaying)
            {
                particle.Stop();
            }
        }
    }

    public ContinuousAttack(ParticleSystem particle, KeyCode key) : base(particle, key) {}
}

public class AttacksManager : MonoBehaviour 
{
    private string[] continousAttacks = { "Fire" };

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
        leftClickAttack = continousAttacks.Contains(leftAttackParticle.name) ? new ContinuousAttack(leftAttackParticle, KeyCode.Mouse0) : new BasicAttack(leftAttackParticle, KeyCode.Mouse0);
        rightClickAttack = continousAttacks.Contains(rightAttackParticle.name) ? new ContinuousAttack(rightAttackParticle, KeyCode.Mouse1) : new BasicAttack(rightAttackParticle, KeyCode.Mouse1);
        additionalAttack1 = continousAttacks.Contains(add1AttackParticle.name) ? new ContinuousAttack(add1AttackParticle, KeyCode.Mouse4) : new BasicAttack(add1AttackParticle, KeyCode.Mouse4);
        additionalAttack2 = continousAttacks.Contains(add2AttackParticle.name) ? new ContinuousAttack(add2AttackParticle, KeyCode.Mouse3) : new BasicAttack(add2AttackParticle, KeyCode.Mouse3);
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
