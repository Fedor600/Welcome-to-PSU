using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Animation")]
    [SerializeField] private float framesPerSecond = 8f;
    [SerializeField] private Sprite idleDown;
    [SerializeField] private Sprite idleUp;
    [SerializeField] private Sprite idleRight;
    [SerializeField] private Sprite idleLeft;

    [SerializeField] private Sprite[] walkDown;
    [SerializeField] private Sprite[] walkUp;
    [SerializeField] private Sprite[] walkRight;
    [SerializeField] private Sprite[] walkLeft;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerControl playerControl;

    private Vector2 movement;
    private Direction currentDirection = Direction.Down;

    private float animationTimer;
    private int frameIndex;

    private enum Direction
    {
        Down,
        Up,
        Right,
        Left
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControl = new PlayerControl();

        SetIdleSprite();
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }

    private void OnDisable()
    {
        playerControl.Disable();
    }

    private void Update()
    {
        movement = playerControl.Movement.Move.ReadValue<Vector2>();

        if (movement.sqrMagnitude > 1f)
            movement.Normalize();

        if (movement != Vector2.zero)
        {
            UpdateDirection();
            AnimateWalk();
        }
        else
        {
            animationTimer = 0f;
            frameIndex = 0;
            SetIdleSprite();
        }
    }

    private void FixedUpdate()
    {
        if (movement == Vector2.zero)
            return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void UpdateDirection()
    {
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            currentDirection = movement.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            currentDirection = movement.y > 0 ? Direction.Up : Direction.Down;
        }
    }

    private void AnimateWalk()
    {
        Sprite[] animation = GetWalkAnimation();

        if (animation == null || animation.Length == 0)
            return;

        animationTimer += Time.deltaTime;

        if (animationTimer >= 1f / framesPerSecond)
        {
            animationTimer -= 1f / framesPerSecond;
            frameIndex = (frameIndex + 1) % animation.Length;
        }

        spriteRenderer.flipX = false;
        spriteRenderer.sprite = animation[frameIndex];
    }

    private Sprite[] GetWalkAnimation()
    {
        switch (currentDirection)
        {
            case Direction.Up:
                return walkUp;
            case Direction.Down:
                return walkDown;
            case Direction.Right:
                return walkRight;
            case Direction.Left:
                return walkLeft;
            default:
                return walkDown;
        }
    }

    private void SetIdleSprite()
    {
        spriteRenderer.flipX = false;

        switch (currentDirection)
        {
            case Direction.Up:
                if (idleUp != null) spriteRenderer.sprite = idleUp;
                break;

            case Direction.Down:
                if (idleDown != null) spriteRenderer.sprite = idleDown;
                break;

            case Direction.Right:
                if (idleRight != null) spriteRenderer.sprite = idleRight;
                break;

            case Direction.Left:
                if (idleLeft != null) spriteRenderer.sprite = idleLeft;
                break;
        }
    }

    private void OnValidate()
    {
        if (framesPerSecond <= 0f)
            framesPerSecond = 1f;

        if (moveSpeed < 0f)
            moveSpeed = 0f;
    }
}