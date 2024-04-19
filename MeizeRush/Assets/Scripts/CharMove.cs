using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    [SerializeField] public float speed;
    private CharacterController charController;
    private Vector3 movement;
    public Animator animator;
    private bool lastFacedBack = false;

    private void Awake()
    {

    }
    private void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        charController.Move(movement * Time.deltaTime * speed);
        if (movement.x != 0 || movement.z != 0)
        {
            if (movement.z > 0)
            {
                animator.SetFloat("xVelocity", 3);
                lastFacedBack = true;
            }
            else
            {
                animator.SetFloat("xVelocity", 1);
                lastFacedBack = false;
            }
        }
        else
        {
            if (lastFacedBack)
                animator.SetFloat("xVelocity", 2);
            else
                animator.SetFloat("xVelocity", 0);
        }
    }
}
