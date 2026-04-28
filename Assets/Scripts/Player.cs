using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Move Speed")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float acceleration;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private InputSystem_Actions inputAction;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        inputAction = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        inputAction.Player.Move.Enable();
        inputAction.Player.Move.performed += OnMovePerformed;
        inputAction.Player.Move.canceled += OnMoveCanceled;

        GameManager.OnGameStateChange += OnGameStateChange;
    }

    public void OnDisable()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;

        inputAction.Player.Move.canceled -= OnMoveCanceled;
        inputAction.Player.Move.performed -= OnMovePerformed;
        inputAction.Player.Move.Disable();
    }

    void FixedUpdate()
    {
        if (GameManager.GameState == GameStates.Overworld)
        {
            Vector2 move = inputAction.Player.Move.ReadValue<Vector2>();

            rb.linearVelocity = new Vector2(
                Mathf.MoveTowards(
                    rb.linearVelocityX, // Current Position
                    topSpeed * move.x, // Destination
                    acceleration * Time.fixedDeltaTime // Cap
                ),
                Mathf.MoveTowards(
                    rb.linearVelocityY, // Current Position
                    topSpeed * move.y, // Destination
                    acceleration * Time.fixedDeltaTime // Cap
                )
            );
        }
    }

    // Events
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // this event is mostly used for animation, go in fixed update for how the movement is actually handled
        if(GameManager.GameState == GameStates.Overworld)
        {
            animator.SetBool("isWalking", true);

            Vector2 move = context.ReadValue<Vector2>();

            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                // east - west
                if (Mathf.Sign(move.x) > 0)
                {
                    animator.SetInteger("direction", (int)CardinalDirections.West);
                }
                else
                {
                    animator.SetInteger("direction", (int)CardinalDirections.East);
                }
            }
            else
            {
                // north - south
                if (Mathf.Sign(move.y) > 0)
                {
                    animator.SetInteger("direction", (int)CardinalDirections.North);
                }
                else
                {
                    animator.SetInteger("direction", (int)CardinalDirections.South);
                }
            }
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", false);
    }

    private void OnGameStateChange(GameStates oldGameState, GameStates newGameState)
    {

    }
}
