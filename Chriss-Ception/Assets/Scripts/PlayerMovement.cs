using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float airDrag;
    public float gravityScale;
    public float jumpForce;
    public float jumpCooldown;
    public float dashForce;
    public float dashCooldown;
    public float airMultiplier;

    bool readyToJump = true;
    bool readyToDash = true;
    

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    bool touchedGround=false;

    [Header("Combat")]
    public float attack1Cooldown;
    public float attack2Cooldown;
    public float maxHealth;
    public Transform swordOrientation;
    bool canAttack = true;
    float health;
    


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
        health = maxHealth;
    }

    private void Update()
    {
        MyInput();
        //SpeedControl();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        

        if (touchedGround==false){
            touchedGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        }

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }
    // Update is called once per frame

    private void FixedUpdate()
    {
        //MyInput();
        MovePlayer();
        rb.AddForce(Physics.gravity * -rb.mass);
        rb.AddForce(Physics.gravity * rb.mass*gravityScale);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKey(dashKey) && readyToDash && touchedGround){
            readyToDash = false;
            touchedGround = false;
            Dash(horizontalInput,verticalInput);
            Invoke(nameof(ResetDash), dashCooldown);
        }


        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            canAttack = false;
            Attack1();
            Invoke(nameof(ResetAttack), attack1Cooldown);
        }

        if (Input.GetMouseButtonDown(1) && canAttack)
        {
            canAttack = false;
            Attack2();
            Invoke(nameof(ResetAttack), attack2Cooldown);
        }
        //Debug.LogWarning("H-Inp: "+horizontalInput);
        //Debug.LogWarning("V-Inp: "+verticalInput);


    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
         Vector3 flatVel;


        if (grounded){
            flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            if(flatVel.magnitude > moveSpeed){
                rb.AddForce(moveDirection.normalized * moveSpeed * -10f, ForceMode.Force);
            }

        }
        else if (!grounded){
            flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            if(flatVel.magnitude > moveSpeed){
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * -airMultiplier, ForceMode.Force);
            }

            
        }

        
        //Debug.LogWarning("Force: "+(moveDirection.normalized*moveSpeed*10f, ForceMode.Force));
        //Debug.LogWarning("Move Dir: "+moveDirection);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }


    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    private void Dash(float x_move,float y_move) //x is sideways, y is forward
    {   
        
        moveDirection = orientation.forward * y_move + orientation.right * x_move;
        
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

        rb.AddForce(moveDirection.normalized * dashForce * 10f, ForceMode.Impulse);

        if (x_move==0 && y_move==0){
            readyToDash = true;
            touchedGround = true;
        }
    }


    private void ResetJump()
    {
        readyToJump = true;
    }
        private void ResetDash()
    {
        readyToDash = true;
    }

    private void ResetAttack(){
        canAttack=true;
        Vector3 attackRotation = new Vector3(-90f,0f,0f);
        swordOrientation.Rotate(attackRotation,Space.Self);
    }
    private void Attack1() 
    {   
        //Needs animation
        Debug.Log("light attack");
        //Transform rot =swordOrientation.
        Vector3 attackRotation = new Vector3(90f,0f,0f);
        swordOrientation.Rotate(attackRotation,Space.Self);
    }

    private void Attack2() 
    {   
        //Needs animation
        Debug.Log("preparing attack");
        Vector3 attackRotation = new Vector3(-45f,0f,0f);
        swordOrientation.Rotate(attackRotation,Space.Self);
        Invoke(nameof(Attack2P2), 1.5f);
    }

    private void Attack2P2() {
        Debug.Log("heavy attack");

        Vector3 attackRotation = new Vector3(135f,0f,0f);
        swordOrientation.Rotate(attackRotation,Space.Self);
    }


    
}