using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerControllingAnimated : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Анимация")]
    [SerializeField] private float framesPerSecond = 8f;

    [Header("Спрайты покоя")]
    [SerializeField] private Sprite idleDown;
    [SerializeField] private Sprite idleUp;
    [SerializeField] private Sprite idleRight;
    [SerializeField] private Sprite idleLeft;

    [Header("Спрайты ходьбы")]
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

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        SetIdleSprite();
    }

    private void OnEnable()
    {
        if (playerControl == null)
            playerControl = new PlayerControl();

        playerControl.Enable();
    }

    private void OnDisable()
    {
        if (playerControl != null)
            playerControl.Disable();
    }

    private void Update()
    {
        movement = playerControl.Movement.Move.ReadValue<Vector2>();

        if (movement.sqrMagnitude > 1f)
            movement.Normalize();

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition =
            rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);
    }

    private void UpdateAnimation()
    {
        if (movement.sqrMagnitude > 0.01f)
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

    private void UpdateDirection()
    {
        Direction newDirection;

        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            newDirection =
                movement.x > 0f
                ? Direction.Right
                : Direction.Left;
        }
        else
        {
            newDirection =
                movement.y > 0f
                ? Direction.Up
                : Direction.Down;
        }

        if (newDirection != currentDirection)
        {
            currentDirection = newDirection;
            frameIndex = 0;
            animationTimer = 0f;
        }
    }

    private void AnimateWalk()
    {
        Sprite[] animation = GetWalkAnimation();

        if (animation == null || animation.Length == 0)
        {
            SetIdleSprite();
            return;
        }

        if (frameIndex >= animation.Length)
            frameIndex = 0;

        animationTimer += Time.deltaTime;

        if (animationTimer >= 1f / framesPerSecond)
        {
            animationTimer = 0f;
            frameIndex = (frameIndex + 1) % animation.Length;
        }

        if (animation[frameIndex] != null)
        {
            spriteRenderer.sprite = animation[frameIndex];
        }
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
        Sprite idleSprite = null;

        switch (currentDirection)
        {
            case Direction.Up:
                idleSprite = idleUp;
                break;

            case Direction.Down:
                idleSprite = idleDown;
                break;

            case Direction.Right:
                idleSprite = idleRight;
                break;

            case Direction.Left:
                idleSprite = idleLeft;
                break;
        }

        if (idleSprite != null)
            spriteRenderer.sprite = idleSprite;
    }

    private void OnValidate()
    {
        if (moveSpeed < 0f)
            moveSpeed = 0f;

        if (framesPerSecond <= 0f)
            framesPerSecond = 1f;
    }
}