using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private Vector3 velocity;

    public Vector3 CurrentMoveDirection { get; private set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(h, 0, v).normalized;
        CurrentMoveDirection = moveDir;

        if (moveDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetAngle, 0), rotationSpeed * Time.deltaTime);

            controller.Move(moveDir * speed * Time.deltaTime);
        }

        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}