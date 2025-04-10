using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private float _horizontalMovement = 0f;
    private bool _facingRight = true; 
    private Player_Animation player_animation;
    public float coyoteTime = 0.2f; // Время "прощения" после ухода с платформы
    private float coyoteTimeCounter;
    private bool isJumpingNow = false;
    
    [Header("Player health")]
    [Header("Player Movement")]
    [Range(0f, 1f)] public float speed = 1f;
    private bool jumpControl;
    private int jumpIteration = 0;
    public float jumpForce = 210f;
    public int jumpValueIteration = 60;

     Animator animator;
    
     [Space]
    [Header("Ground Check")]
    public bool grounded = false;
    public bool isOnRealGround {get; private set;} = false;
    [Range(0f, 5f)] public float groundCheckRadius = 0.3f;
    [Range(-5f, 5f)] public float groundCheckOfSetY = -1.8f;
    private void Start()
    {
       _rb2d = GetComponent<Rigidbody2D>();
       player_animation = GetComponentInChildren<Player_Animation>();
    }

    private void Update()
    {
       Jump();
       if (Input.GetKeyDown(KeyCode.Escape))
       {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
       // Выполняем проверку
       Collider2D[] colliders = Physics2D.OverlapCircleAll(
          new Vector2(transform.position.x, transform.position.y + groundCheckOfSetY), 
          groundCheckRadius
       );

       // Если пересекается несколько коллайдеров (игнорируем себя)
       if (colliders.Length > 1)
       {
          grounded = true;
          isOnRealGround = true;
          coyoteTimeCounter = coyoteTime;
       }
       else
       {
          isOnRealGround = false;

          if (!isJumpingNow && coyoteTimeCounter > 0)
          {
             coyoteTimeCounter -= Time.deltaTime;
             grounded = true;  // Разрешаем прыжок благодаря coyote time
          }
          else
          {
             grounded = false;
          }
       }
    }


    private void OnDrawGizmosSelected()
    {
       Gizmos.color = Color.green;
       Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + groundCheckOfSetY), groundCheckRadius);
    }
    void Jump()
    {
       if (Input.GetKeyDown(KeyCode.W) && grounded)
       {
          jumpControl = true; // начинаем прыжок
          isJumpingNow = true; // прыгнули только что
       }

       if (Input.GetKeyUp(KeyCode.W))
       {
          jumpControl = false; // отпустили кнопку — прыжок заканчивается
          jumpIteration = 0;   // сброс итераций
       }

       if (jumpControl)
       {
          if (jumpIteration++ < jumpValueIteration)
          {
             _rb2d.AddForce(Vector2.up * jumpForce / jumpIteration);
          }
          else
          {
             jumpControl = false; // лимит прыжка исчерпан
             jumpIteration = 0;
          }
       }
    }


}