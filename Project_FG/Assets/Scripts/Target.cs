using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private bool _facingRight = true;
    private float _targetPosition;
    
    [Header("Movement Settings")]
    [Range(1f, 10f)] public float speed = 2f;
    [Range(1f, 10f)] public float moveRange = 5f;

    private Vector2 _startPosition;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
        ChooseNewTargetPosition();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_targetPosition, transform.position.y), step);

        if (Mathf.Abs(transform.position.x - _targetPosition) < 0.1f)
        {
            ChooseNewTargetPosition();
        }

        if ((_targetPosition > transform.position.x && !_facingRight) || (_targetPosition < transform.position.x && _facingRight))
        {
            Flip();
        }
    }

    private void ChooseNewTargetPosition()
    {
        _targetPosition = _startPosition.x + Random.Range(-moveRange, moveRange);
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}