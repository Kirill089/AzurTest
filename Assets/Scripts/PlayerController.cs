using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float forwardSpeed = 3f;
    [SerializeField] private float sideOrBackSpeed = 1.5f;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;

    private float vertical;
    private float horizontal;
    private Vector3 moveDirection;

    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        animator.SetFloat("Vertical", vertical);
        animator.SetFloat("Horizontal", horizontal);
        animator.SetBool("IsRunning", moveDirection.magnitude > 0.1f);

        if (moveDirection.magnitude > 0.1f)
        {
            MoveCharacter();
        }
    }

    void MoveCharacter()
    {
        float currentY = transform.position.y;

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        float currentSpeed = vertical > 0.1f ? forwardSpeed : sideOrBackSpeed;

        characterController.Move(moveDir * currentSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, currentY, transform.position.z);

        if (vertical > 0.1f)
        {
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
