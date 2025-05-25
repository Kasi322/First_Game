using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    
    public Animator _anim;
    private Player _player;
    public float ismoving { private get; set; }
    public bool isGrounded { private get; set; }
    private PlayerHealth _playerHealth;
    
    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _player = GetComponent<Player>();
        _playerHealth = GetComponentInChildren<PlayerHealth>();
    }

    private void Update()
    {
        _anim.SetFloat("isMoving", Mathf.Abs(ismoving));
        if (_player.IsOnRealGround == true)
        {
            _anim.SetBool("isJumping", false); 
        }
        else if (_player.IsOnRealGround == false && _playerHealth.isDead == false)
        {
            _anim.SetBool("isJumping", true); 
        } else if (_playerHealth.isDead)
        {
            _anim.SetBool("isJumping", false);
            _anim.SetBool("isDead", true);
        }
    }
}
