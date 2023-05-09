using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;



    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void update() 
    {
        //MyInput();
    }
    // Update is called once per frame

    private void FixedUpdate(){
        MyInput();
        MovePlayer();
    }
    
    private void MyInput()
    {
        horizontalInput =Input.GetAxisRaw("Horizontal");
        verticalInput =Input.GetAxisRaw("Vertical");

        Debug.LogWarning("H-Inp: "+horizontalInput);
        Debug.LogWarning("V-Inp: "+verticalInput);

                
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized*moveSpeed*10f, ForceMode.Force);
        Debug.LogWarning("Force: "+(moveDirection.normalized*moveSpeed*10f, ForceMode.Force));
        Debug.LogWarning("Move Dir: "+moveDirection);
    }
}