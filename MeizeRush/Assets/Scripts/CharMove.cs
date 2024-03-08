using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    [SerializeField] public float speed;
    private CharacterController charController;
    private Vector3 movement;

    private void Awake()
    {

    }
    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }
    void Update()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        charController.Move(movement * Time.deltaTime * speed);
    }
}
