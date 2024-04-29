using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

enum PlayerMoveStates
{
    idle,
    playerWalk,
    idleBack,
    playerWalkBack,
    idleLeft,
    playerWalkLeft,
    idleRight,
    playerWalkRight,
}
;

enum PlayerMoveLastState { front, back, left, right }
;

public class CharMove : MonoBehaviour
{
    [SerializeField]
    public float speed;
    // private CharacterController charController;
    private Rigidbody2D rigidbody2D;
    private Vector2 movement;
    public Animator animator;
    private int lastState = (int)PlayerMoveLastState.front;

    private void Awake() { }
    private void Start()
    {
        // charController = GetComponent<CharacterController>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        // movement = new Vector2(, );
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // movement.x = Input.GetAxisRaw("Horizontal");
        // movement.y = Input.GetAxisRaw("Vertical");
        // rigidbody2D.velocity.Set(movement.x * speed * Time.deltaTime, movement.y * speed * Time.deltaTime);
        // charController.Move(movement * Time.deltaTime * speed);

        rigidbody2D.velocity = movement * speed;

        if (movement.x != 0 || movement.y != 0)
        {
            if (movement.x > 0)
            {
                animator.SetInteger("xVelocity", (int)PlayerMoveStates.playerWalkRight);
                lastState = (int)PlayerMoveLastState.right;
            }
            else if (movement.x < 0)
            {
                animator.SetInteger("xVelocity", (int)PlayerMoveStates.playerWalkLeft);
                lastState = (int)PlayerMoveLastState.left;
            }
            else
            {
                if (movement.y > 0)
                {
                    animator.SetInteger("xVelocity", (int)PlayerMoveStates.playerWalkBack);
                    lastState = (int)PlayerMoveLastState.back;
                }
                else
                {
                    animator.SetInteger("xVelocity", (int)PlayerMoveStates.playerWalk);
                    lastState = (int)PlayerMoveLastState.front;
                }
            }
        }
        else
        {
            animator.SetInteger("xVelocity", lastState * 2);
        }
        Debug.Log(lastState);
    }
}
