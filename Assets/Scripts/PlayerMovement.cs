using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Настройки ограничения движения")]
    [SerializeField] private bool limitMovement = false;
    [SerializeField] private Vector2 movementBounds = new Vector2(10f, 10f);

    private Vector2 inputVector;
    private Vector2 currentVelocity;
    private Rigidbody2D rb2D;
    private bool useRigidbody;

    void Start()
    {
        // Пытаемся получить компонент Rigidbody
        rb2D = GetComponent<Rigidbody2D>();

        if (rb2D != null)
        {
            useRigidbody = true;
        }
    }

    void Update()
    {
        // Получаем ввод с клавиатуры
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
            moveY = 1f;
        if (Input.GetKey(KeyCode.S))
            moveY = -1f;
        if (Input.GetKey(KeyCode.D))
            moveX = 1f;
        if (Input.GetKey(KeyCode.A))
            moveX = -1f;

        // Нормализуем диагональное движение
        inputVector = new Vector2(moveX, moveY).normalized;

        // Обновляем движение
        Move();
    }

    void Move()
    {
        Vector3 targetPosition;

        if (useRigidbody)
        {
            // Движение через Rigidbody (для физики)
            Vector2 targetVelocity = inputVector * moveSpeed;

            if (rb2D != null)
            {
                rb2D.linearVelocity = Vector2.SmoothDamp(rb2D.linearVelocity, targetVelocity, ref currentVelocity, smoothTime);
            }
            
        }
        else
        {
            // Прямое перемещение Transform
            targetPosition = transform.position + new Vector3(inputVector.x, inputVector.y, 0) * moveSpeed * Time.deltaTime;

            // Ограничение движения по границам
            if (limitMovement)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, -movementBounds.x, movementBounds.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, -movementBounds.y, movementBounds.y);
            }

            transform.position = targetPosition;
        }
    }

    // Опциональный метод для отображения границ в редакторе
    private void OnDrawGizmosSelected()
    {
        if (limitMovement)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(movementBounds.x * 2, movementBounds.y * 2, 0));
        }
    }
}