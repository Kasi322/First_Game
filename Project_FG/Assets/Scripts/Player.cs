using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private float _horizontalMovement = 0f;
    private bool _facingRight = true; 
    private Player_Animation _playerAnimation;
    public Animator _animator;
    public Collider2D collider2d;
    public Vector2 spawnPoint = new Vector2(11f, 11f);
    private PlayerHealth playerHealth;

    [Header("Player Movement")]
    [Range(0f, 1f)] public float speed = 1f;
    private bool _jumpControl;
    private int _jumpIteration = 0;
    public float jumpForce = 450f;
    public int jumpValueIteration = 60;
    private bool _isJumpingNow = false;
    public float _cayoteTime = 0.2f;
    private float _cayoteTimeCounter= 0f;
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter = 0f;
    
    private bool _jumpPressed;
    private bool _jumpReleased;

    [Space]
    [Header("Ground Check")]
    public bool grounded = false;
    public bool IsOnRealGround { get; private set; } = false;
    [Range(0f, 5f)] public float groundCheckRadius = 0.3f;
    [Range(-5f, 5f)] public float groundCheckOfSetY = -1.8f;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        collider2d = GetComponent<Collider2D>();
        _rb2d = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponentInChildren<Player_Animation>();
        playerHealth = GetComponentInChildren<PlayerHealth>();
    }

    private void Update()
    {
        if (playerHealth.isDead) return;

        // ——— 1) Читаем Input ———
        _horizontalMovement = Input.GetAxis("Horizontal") * speed;
        _jumpPressed  = Input.GetKeyDown(KeyCode.W);
        _jumpReleased = Input.GetKeyUp(KeyCode.W);

        // Escape / Damage тест
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        if (Input.GetKeyDown(KeyCode.P))
            playerHealth.takeDamage(10);

        // ——— 2) Таймеры для CoyoteTime и JumpBuffer ———
        if (grounded)
            _cayoteTimeCounter = _cayoteTime;
        else
            _cayoteTimeCounter -= Time.deltaTime;

        if (_jumpPressed)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // сброс флагов прыжка при отпускании
        if (_jumpReleased)
        {
            _jumpControl = false;
            _jumpIteration = 0;
            _cayoteTimeCounter = 0f;
        }

        // обновляем анимацию движения/земли (это чисто отображение)
        _playerAnimation.ismoving   = Math.Abs(_horizontalMovement);
        _playerAnimation.isGrounded = !grounded;
        
        // поворот в Update() или FixedUpdate() — не критично
        if (_horizontalMovement > 0 && !_facingRight) Flip();
        else if (_horizontalMovement < 0 && _facingRight) Flip();
    }

    private void FixedUpdate()
    {
        if (playerHealth.isDead) return;

        // ——— 3) Физика: движение по X ———
        Vector2 targetVelocity = new Vector2(_horizontalMovement * 10f, _rb2d.linearVelocity.y);
        _rb2d.linearVelocity = targetVelocity;

        // ——— 4) Физика: прыжок (логика осталась без изменений) ———
        Jump();

        // ——— 5) Проверка земли ———
        CheckGround();
    }

    private void Jump()
    {
        // (логика точно такая же, как у тебя была)
        if (jumpBufferCounter > 0f && _cayoteTimeCounter > 0f)
        {
            Debug.Log("work1");
            Vector3 velocity = _rb2d.linearVelocity;
            velocity.y = 0;
            _rb2d.linearVelocity = velocity;

            _jumpControl = true;
            _isJumpingNow = true;
            jumpBufferCounter = 0f;
        }

        if (_jumpControl)
        {
            if (_jumpIteration++ < jumpValueIteration)
            {
                _rb2d.AddForce(Vector2.up * jumpForce / _jumpIteration);
            }
            else
            {
                _jumpControl = false;
                _jumpIteration = 0;
            }
        }
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, transform.position.y + groundCheckOfSetY),
            groundCheckRadius
        );

        if (colliders.Length > 1)
        {
            grounded = true;
            IsOnRealGround = true;
        }
        else
        {
            IsOnRealGround = false;
            grounded = !_isJumpingNow;
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            new Vector2(transform.position.x, transform.position.y + groundCheckOfSetY),
            groundCheckRadius
        );
    }
}
