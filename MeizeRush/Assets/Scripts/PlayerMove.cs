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
    private new Rigidbody2D rigidbody2D;
    public Vector2 movement, pointer;
    public Animator walkAnimator;

    public AttackArea attackArea;
    // public Animator attackAnimator;
    private void Awake() { }
    private void Start()
    {
        // charController = GetComponent<CharacterController>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        walkAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        pointer = GetPointerInput();
        attackArea.gameObject.transform.right = (pointer - (Vector2)attackArea.gameObject.transform.position).normalized;
        attackArea.pointerPosition = pointer;
        rigidbody2D.velocity = speed * movement;

        if (movement.x != 0 || movement.y != 0)
        {
            walkAnimator.SetFloat("X", movement.x);
            walkAnimator.SetFloat("Y", movement.y);
            walkAnimator.SetBool("isWalking", true);

        }
        else
        {
            walkAnimator.SetBool("isWalking", false);


        }
    }
    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
