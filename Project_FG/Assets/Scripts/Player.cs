using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private float _horizontalMovement = 0f;
    private bool _facingRight = true; 
    private Player_Animation player_animation;
    
    
    [Header("Player Movement")]
    [Range(0f, 1f)] public float speed = 1f;
    [Range(0, 15f)] public float jumpSpeed = 8f;
    [Range (1, 10)] public int jumpCountMax = 2;
    private int jumpCount;

     Animator animator;
    
     [Space]
    [Header("Ground Check")]
    public bool grounded = false;
    [Range(0f, 5f)] public float groundCheckRadius = 0.3f;
    [Range(-5f, 5f)] public float groundCheckOfSetY = -1.8f;
    private void Start()
    {
       _rb2d = GetComponent<Rigidbody2D>();
       player_animation = GetComponentInChildren<Player_Animation>();
    }

    private void Update()
    {
       if (grounded && Input.GetKeyDown(KeyCode.Space))
       {
          jumpCount = 0;
          _rb2d.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
       } else if (jumpCount < jumpCountMax && Input.GetKeyDown(KeyCode.Space))
       {
          jumpCount++;
          _rb2d.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
       }
       _horizontalMovement = Input.GetAxis("Horizontal") * speed;
        player_animation.ismoving = Math.Abs(_horizontalMovement);
        player_animation.isGrounded = !grounded;
       if (_horizontalMovement > 0 && !_facingRight)
       {
          Flip();
       } else if (_horizontalMovement < 0 && _facingRight)
       {
          Flip();
       }
    }

    private void FixedUpdate()
    {
       Vector2 targetVelocity = new Vector2((_horizontalMovement * 10f), _rb2d.linearVelocity.y);
       _rb2d.linearVelocity = targetVelocity;
       CheckGround();
       
    }

    private void Flip()
    {
       _facingRight = !_facingRight;
       Vector3 theScale = transform.localScale;
       theScale.x *= -1;
       transform.localScale = theScale;
    }

    private void CheckGround()
    {
       Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + groundCheckOfSetY), groundCheckRadius);
       if (colliders.Length > 1)
       {
          grounded = true;
       }
       else
       {
          grounded = false;
       }
    }
}