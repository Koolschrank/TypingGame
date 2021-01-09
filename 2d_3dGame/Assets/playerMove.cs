using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{
    public float speed = 0f;
    public InputTest _playerInput;
    private Rigidbody rb;

    Vector3 movement;
    private float movementX;
    private float movementY;

    private void Awake() {
        _playerInput = new InputTest();
    }

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
    }

    

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnEnable() 
    {
        _playerInput.Enable();
    }

    private void OnDisable() 
    {
        _playerInput.Disable();
        
    }

    private void Update()
    {
         movement = new Vector3 (_playerInput.Player.Move.ReadValue<Vector2>().x,0, _playerInput.Player.Move.ReadValue<Vector2>().y);

    }

    private void FixedUpdate()
    {
        rb.AddForce(movement * speed);
    }

}