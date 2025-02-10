using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb2d;
    private Vector2 _input;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Player_Animation _anim;
    public float jumpForce = 10f;
    public Transform groundCheck; // Точка проверки земли
    public LayerMask groundLayer; // Слой земли
    private bool _isGrounded;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Player_Animation>();
    }

    private void Update()
    {
        // Получаем горизонтальный ввод
        _input = new Vector2(Input.GetAxis("Horizontal"), 0);

        // Зеркальный поворот спрайта
        if (_input.x != 0)
        {
            spriteRenderer.flipX = _input.x < 0;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Устанавливаем состояние движения
        _anim.ismoving = Mathf.Abs(_input.x) > 0.1f;
    }

    private void FixedUpdate()
    {
        // Двигаем персонажа через физику
        rb2d.linearVelocity = new Vector2(_input.x * moveSpeed, rb2d.linearVelocity.y);
    }
    [Obsolete("Obsolete")]
    private void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
    }
}