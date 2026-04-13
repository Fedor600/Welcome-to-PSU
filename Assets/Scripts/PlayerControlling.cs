using UnityEngine;

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
    
    private void Update()
    {

        movement = playerControl.Movement.Move.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
}
