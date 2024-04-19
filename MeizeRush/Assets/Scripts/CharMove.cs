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
    private CharacterController charController;
    private Vector3 movement;
    public Animator animator;
    private int lastState = (int)PlayerMoveLastState.front;

    private void Awake() { }
    private void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0,
                               Input.GetAxisRaw("Vertical"));
        charController.Move(movement * Time.deltaTime * speed);
        if (movement.x != 0 || movement.z != 0)
        {
            if (movement.x > 0)
            {
                animator.SetFloat("xVelocity", (int)PlayerMoveStates.playerWalkRight);
                lastState = (int)PlayerMoveLastState.right;
            }
            else if (movement.x < 0)
            {
                animator.SetFloat("xVelocity", (int)PlayerMoveStates.playerWalkLeft);
                lastState = (int)PlayerMoveLastState.left;
            }
            else
            {
                if (movement.z > 0)
                {
                    animator.SetFloat("xVelocity", (int)PlayerMoveStates.playerWalkBack);
                    lastState = (int)PlayerMoveLastState.back;
                }
                else
                {
                    animator.SetFloat("xVelocity", (int)PlayerMoveStates.playerWalk);
                    lastState = (int)PlayerMoveLastState.front;
                }
            }
        }
        else
        {
            animator.SetFloat("xVelocity", lastState * 2);
        }
    }
}
