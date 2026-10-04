using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float turnSpeed = 720f;

    private CharacterController controller;
    private Animator animator;

    private float moveInput;
    private bool sprint;
    private bool jumpPressed;

    private float verticalVelocity;
    private float fallTime;
    private float lineZ; // уровень 2.5D - игрок всегда на одной линии по Z

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        lineZ = transform.position.z;
    }

    void OnDisable()
    {
        // скрипт выключается при смерти - чтобы не перебирал ногами на месте
        if (animator != null)
            animator.SetFloat("Speed", 0);
    }

    // эти методы вызывает PlayerInput (Behavior = Send Messages)
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>().x;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
            jumpPressed = true;
    }

    void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }

    void Update()
    {
        bool grounded = controller.isGrounded;

        if (grounded)
        {
            fallTime = 0;
            if (verticalVelocity < 0)
                verticalVelocity = -2f; // прижимаем к земле, иначе isGrounded мигает

            if (jumpPressed)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetBool("Jump", true);
            }
        }
        else
        {
            fallTime += Time.deltaTime;
        }
        jumpPressed = false; // нажатие в воздухе не запоминаем, второго прыжка нет

        verticalVelocity += gravity * Time.deltaTime;

        float speed = sprint ? runSpeed : walkSpeed;
        Vector3 move = new Vector3(moveInput * speed, verticalVelocity, 0) * Time.deltaTime;
        move.z = lineZ - transform.position.z; // если об угол сдвинуло по Z - возвращаем на линию
        CollisionFlags flags = controller.Move(move);

        // ударились головой о платформу
        if ((flags & CollisionFlags.Above) != 0 && verticalVelocity > 0)
            verticalVelocity = 0;

        // разворот в сторону движения
        if (moveInput != 0)
        {
            Quaternion look = Quaternion.LookRotation(new Vector3(moveInput, 0, 0));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, turnSpeed * Time.deltaTime);
        }

        // анимации
        grounded = controller.isGrounded;
        animator.SetFloat("Speed", Mathf.Abs(moveInput) * speed, 0.1f, Time.deltaTime);
        animator.SetFloat("MotionSpeed", 1f);
        animator.SetBool("Grounded", grounded);
        if (grounded)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("FreeFall", false);
        }
        else if (fallTime > 0.15f)
        {
            animator.SetBool("FreeFall", true);
        }
    }
}
