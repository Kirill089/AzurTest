using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 2f;
    [SerializeField] Animator animator;
    [SerializeField] private Transform camera;

    private float vertical;
    private float horizontal;
    private float speedMultiplier = 1f;
    private float speedClamp = 0f;

    private Vector3 moveDirection;

    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (vertical > 0)
        {
            speedClamp = 2f;
        }
        else if (horizontal != 0)
        {
            speedClamp = 1.5f;
        }
        else
        {
            speedClamp = 1f;
        }

        if (isSprinting)
        {
            if (speedMultiplier < speedClamp)
                speedMultiplier += 0.02f;
        }
        else
        {
            if (speedMultiplier > 1f)
                speedMultiplier -= 0.02f;
        }

        animator.SetFloat("Vertical", vertical * speedMultiplier);
        animator.SetFloat("Horizontal", horizontal * speedMultiplier);

        if (moveDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir * speed * speedMultiplier * Time.deltaTime);

            if (vertical > 0.1f)
            {
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

    }
}
