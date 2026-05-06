using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlling : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rb;
    private PlayerControl playerControl;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControl = new PlayerControl();
    }

    private void OnEnable()
    {
        playerControl.Enable();
    }

    // ⚠️ Обязательно добавляем отключение ввода
    private void OnDisable()
    {
        playerControl.Disable();
    }

    private void Update()
    {
        movement = playerControl.Movement.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Физически корректное перемещение
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}