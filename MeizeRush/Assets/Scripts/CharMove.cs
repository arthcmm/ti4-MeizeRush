using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CharMove : MonoBehaviour
{
    [SerializeField]
    public float speed = 3;
    // private CharacterController charController;
    private Rigidbody2D rigidbody2D;
    public Vector2 movement;
    public Animator animator;
    private void Awake() { }
    private void Start()
    {
        // charController = GetComponent<CharacterController>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        rigidbody2D.velocity = speed * movement;

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("X", movement.x);

            animator.SetFloat("Y", movement.y);
            animator.SetBool("isWalking", true);

        }
        else
        {
            animator.SetBool("isWalking", false);

        }
    }
}
