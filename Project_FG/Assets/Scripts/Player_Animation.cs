using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private Animator _anim;
    public bool ismoving { private get; set; }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _anim.SetBool(IsMoving, ismoving);
    }
}
