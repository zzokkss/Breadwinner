using UnityEngine;
using UnityEngine.InputSystem;

public class JumpController : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private string noJump = "Jump";

    private InputAction targetAction;

    public float moveSpeed = 5f;
    float horizontalMovement;

    private Rigidbody2D rb;
    public float jumpPower = 5f;

    private Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;

        if (playerInput != null)
        {
            targetAction = playerInput.actions.FindAction(noJump);
        }

    }

    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
    }

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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            targetAction.Disable();
        }

        if (collision.CompareTag("Land"))
        {
            targetAction.Enable();
        }
    }
}
