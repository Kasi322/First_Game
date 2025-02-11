using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    
    public Animator _anim;
    public float ismoving { private get; set; }
    public bool isGrounded { private get; set; }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _anim.SetFloat("isMoving", Mathf.Abs(ismoving));
        if (isGrounded == true)
        {
            _anim.SetBool("isJumping", true);
        } else if (isGrounded == false)
        {
            _anim.SetBool("isJumping", false);
        }
    }
}
