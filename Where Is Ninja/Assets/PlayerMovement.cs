using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement3D : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 8f;
    public bool rotateTowardsMoveDirection = true;

    private Rigidbody rb;
    private Vector3 inputDir;
    private Vector3 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDir = new Vector3(h, 0f, v).normalized;
    }

    private void FixedUpdate()
    {
        // Zamiana kierunku wejœcia na orientacjê izometryczn¹
        Vector3 isoDir = inputDir;

        // Wyznaczenie docelowej prêdkoœci
        Vector3 targetVelocity = isoDir * moveSpeed;
        velocity = Vector3.Lerp(velocity, targetVelocity, Time.fixedDeltaTime * (inputDir.magnitude > 0 ? acceleration : deceleration));

        // Ruch
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        // Rotacja w kierunku ruchu
        if (rotateTowardsMoveDirection && velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(velocity.normalized, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * 10f));
        }
    }

}
