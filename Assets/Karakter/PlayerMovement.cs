using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isMovingToTarget;
    private bool movementLocked;
    private Vector3 targetPosition;
    private float targetStopDistance;

    public Vector3 CurrentMoveDirection { get; private set; }

    public void MoveTo(Vector3 position, float stopDistance)
    {
        targetPosition = position;
        targetStopDistance = stopDistance;
        isMovingToTarget = true;
    }

    public void CancelMoveTo()
    {
        isMovingToTarget = false;
    }

    public void SetMovementLocked(bool locked)
    {
        movementLocked = locked;
        if (locked)
        {
            CancelMoveTo();
        }
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = movementLocked
            ? Vector3.zero
            : new Vector3(h, 0, v).normalized;
        if (moveDir.magnitude >= 0.1f)
        {
            isMovingToTarget = false;
        }
        else if (isMovingToTarget)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            if (direction.magnitude <= targetStopDistance)
            {
                isMovingToTarget = false;
                moveDir = Vector3.zero;
            }
            else
            {
                moveDir = direction.normalized;
            }
        }

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