using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
public class Ini : MonoBehaviour
{
    private Player _player;
    private Rigidbody2D _rb;

    [Header("Default Player Settings")]
    [SerializeField] private float defaultJumpForce = 210f;
    [SerializeField] private int defaultJumpValueIteration = 60;
    [SerializeField] private float defaultCayoteTime = 0.2f;
    [SerializeField] private float defaultSpeed = 1f;
    [SerializeField] private float defaultHealth = 100f;
    [SerializeField] private Vector2 spawnPoint = new Vector2(0f, 0f);

    [Header("Default Rigidbody2D Settings")]
    [SerializeField] private float defaultGravityScale = 1f;
    [SerializeField] private float defaultMass = 1f;
    [SerializeField] private float defaultDrag = 0f;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();

        InitializePlayer();
        InitializePhysics();
    }

    private void InitializePlayer()
    {
        _player.jumpForce = defaultJumpForce;
        _player.jumpValueIteration = defaultJumpValueIteration;
        _player._cayoteTime = defaultCayoteTime;
    }

    private void InitializePhysics()
    {
        _rb.gravityScale = defaultGravityScale;
        _rb.mass = defaultMass;
        _rb.linearDamping = defaultDrag;
        _rb.linearVelocity = Vector2.zero;

        // Убедимся, что масштаб не сломан
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x); // оставим правильный поворот
        scale.y = 1f; // стандартная высота
        scale.z = 1f;
        transform.localScale = scale;
    }
}