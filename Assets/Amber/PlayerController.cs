using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private Vector3 targetPosition;
    private bool isMoving = false;
    public bool isGrounded;

    private Rigidbody2D rb;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string noJump = "Jump";

    private InputAction targetAction;

    public float moveSpeed = 5f;
    float horizontalMovement;

    public float jumpPower = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;

        if (playerInput != null)
        {
            targetAction = playerInput.actions.FindAction(noJump);
        }
    }

    private void Update()
    {
        if (!isGrounded && Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            targetPosition = mousePosition;
            isMoving = true;
        }

        if (Mathf.Abs(horizontalMovement) > 0.01f)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime);
            rb.MovePosition(newPosition);
        }

        if (Vector2.Distance(rb.position, targetPosition) < 0.05f)
        {
            isMoving = false;
        }

    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (isGrounded == true)
        {
            if (ctx.performed)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                targetPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                targetPosition = new Vector2(mousePosition.x, rb.position.y);
                isMoving = true;
            }
        }

        if (isGrounded == false)
        {
            if (ctx.performed)
            {
                isMoving = true;
            }

            if (ctx.canceled)
            {
                isMoving = false;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("OnMove");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Land"))
        {
            Debug.Log("land");
            isGrounded = true;
            rb.gravityScale = 1f;
            targetAction.Enable();
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("water"); 
            isGrounded = false;
            rb.gravityScale = 0f;
            targetAction.Disable();
        }
    }

    // JUMP STUFF

    public void Move(InputAction.CallbackContext ctx)
    {
        horizontalMovement = ctx.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            float direction = Mathf.Sign(mousePosition.x - transform.position.x);
            rb.linearVelocity = new Vector2(direction * moveSpeed, jumpPower);

            isMoving = false;
        }
    }

    }

