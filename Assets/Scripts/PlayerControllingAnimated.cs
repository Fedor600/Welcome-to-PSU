using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerControllingAnimated : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Animation")]
    [SerializeField] private float framesPerSecond = 8f;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite[] walkDown;
    [SerializeField] private Sprite[] walkUp;
    [SerializeField] private Sprite[] walkRight;
    [SerializeField] private Sprite[] walkLeft;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerControl playerControl;

    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;

    private float animationTimer;
    private int frameIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControl = new PlayerControl();
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
            lastDirection = movement;
            AnimateWalk();
        }
        else
        {
            frameIndex = 0;
            animationTimer = 0f;

            if (idleSprite != null)
                spriteRenderer.sprite = idleSprite;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void AnimateWalk()
    {
        Sprite[] currentAnimation = GetCurrentAnimation();

        if (currentAnimation == null || currentAnimation.Length == 0)
            return;

        animationTimer += Time.deltaTime;

        if (animationTimer >= 1f / framesPerSecond)
        {
            animationTimer = 0f;
            frameIndex++;

            if (frameIndex >= currentAnimation.Length)
                frameIndex = 0;
        }

        spriteRenderer.sprite = currentAnimation[frameIndex];
    }

    private Sprite[] GetCurrentAnimation()
    {
        if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
        {
            return lastDirection.x > 0 ? walkRight : walkLeft;
        }

        return lastDirection.y > 0 ? walkUp : walkDown;
    }
}
