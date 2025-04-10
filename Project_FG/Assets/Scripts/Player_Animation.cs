using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    
    public Animator _anim;
    private Player _player;
    public float ismoving { private get; set; }
    public bool isGrounded { private get; set; }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _anim.SetFloat("isMoving", Mathf.Abs(ismoving));
        if (_player.isOnRealGround == true)
        {
            _anim.SetBool("isJumping", false); // В воздухе — прыжок
        }
        else
        {
            _anim.SetBool("isJumping", true); // На земле — нет прыжка
        }
    }
}
