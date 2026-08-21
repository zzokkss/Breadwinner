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

    public float jumpPower = 5f;

    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;

    public Animator anim;

    public int facingDirection = 1;
    private float horizontal;

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

            if (mousePosition.x > transform.position.x && facingDirection < 0)
            {
                Flip();
            }
            else if (mousePosition.x < transform.position.x && facingDirection > 0)
            {
                Flip();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime);
            rb.MovePosition(newPosition);
            anim.SetBool("isWalking", true);
        }

        if (isMoving == false)
        {
            anim.SetBool("isWalking", false);
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

                float direction = mousePosition.x - transform.position.x;

                if (direction > 0.1f && facingDirection < 0)
                {
                    Flip();
                }
                else if (direction < -0.1f && facingDirection > 0)
                {
                    Flip();
                }
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
                rb.linearVelocity = Vector2.zero;
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
            rb.linearVelocity = Vector2.zero;
            targetAction.Disable();
        }
    }

    // JUMP STUFF
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (GroundCheck())
        {
            if (ctx.performed)
            {
                Debug.Log("jump");
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                float direction = Mathf.Sign(mousePosition.x - transform.position.x);
                rb.linearVelocity = new Vector2(direction * moveSpeed, jumpPower);

                isMoving = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }

    private bool GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
    }

    private void Flip()
    {
        facingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
