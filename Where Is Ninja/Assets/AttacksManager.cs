using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksManager : MonoBehaviour
{
    public ParticleSystem leftClickAttack;
    public ParticleSystem rightClickAttack;
    public ParticleSystem additionalAttack1;
    public ParticleSystem additionalAttack2;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            RotateToMouse();
            leftClickAttack.Play();  
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RotateToMouse();
            rightClickAttack.Play();
        }
        if (Input.GetKeyDown(KeyCode.Mouse4))
        {
            RotateToMouse();
            additionalAttack1.Play();
        }
        if (Input.GetKeyDown(KeyCode.Mouse3))
        {
            RotateToMouse();
            additionalAttack2.Play();
        }
    }

    void RotateToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Szukamy punktu na ziemi
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
