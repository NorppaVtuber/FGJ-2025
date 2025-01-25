using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Events;

//TODO: sprint
public class PlayerMovement : MonoBehaviour
{
    [Header("SetInInspector")]
    [SerializeField] Transform startPoint;
    [SerializeField] Rigidbody rgb;

    [Header("Movement values")]
    float speed = 25f;
    [SerializeField] float normalSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float speedMultiplier = 10f;
    [SerializeField] Transform orientation;
    [SerializeField] float groundDrag;

    [SerializeField] float jumpForce;
    [SerializeField] float jumpCoolDown;
    [SerializeField] float airMultiplier;
    bool isReadyToJump;
    MoveStates currentMoveState = MoveStates.NONE;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    bool isGrounded;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    bool isDead = false;

    GameManager managerInstance;

    private void Start()
    {
        rgb.freezeRotation = true;
        isReadyToJump = true;
        isDead = false;

        managerInstance = GameManager.Instance;
        managerInstance.GetPlayerHealth().OnDeath.AddListener(onDeath);
        currentMoveState = MoveStates.WALK;
    }

    private void Update()
    {
        //check if player is on ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);

        collectInput();
        speedControl();
        moveStateHandler();

        if (isGrounded)
            rgb.linearDamping = groundDrag;
        else
            rgb.linearDamping = 0f;
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    void collectInput()
    {
        if (isDead)
            return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && isReadyToJump && isGrounded)
        {
            jump();

            Invoke(nameof(resetJump), jumpCoolDown); //invoke this to allow for continuous jumping by holding down jump key
        }
    }

    void movePlayer()
    {
        if (isDead)
            return;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
            rgb.AddForce(moveDirection.normalized * speed * speedMultiplier, ForceMode.Force);
        else if(!isGrounded)
            rgb.AddForce(moveDirection.normalized * speed * speedMultiplier * airMultiplier, ForceMode.Force);
    }

    void speedControl()
    {
        Vector3 _flatVelocity = new Vector3(rgb.linearVelocity.x, 0f, rgb.linearVelocity.z);

        //limit velocity if speed gets too fast
        if(_flatVelocity.magnitude > speed)
        {
            Vector3 _limitedVelocity = _flatVelocity.normalized * speed;
            rgb.linearVelocity = new Vector3(_limitedVelocity.x, rgb.linearVelocity.y, _limitedVelocity.z);
        }
    }

    void jump()
    {
        if (isDead)
            return;
        //if player isn't ready to jump, don't let them jump again
        if (!isReadyToJump)
            return;

        isReadyToJump = false;
        //reset jump velocity before jumping
        rgb.linearVelocity = new Vector3(rgb.linearVelocity.x, 0f, rgb.linearVelocity.z);

        rgb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void resetJump()
    {
        isReadyToJump = true;
    }

    void onDeath()
    {
        isDead = true;
    }

    void moveStateHandler()
    {
        if(isGrounded && Input.GetKey(sprintKey))
        {
            currentMoveState = MoveStates.SPRINT;
            speed = sprintSpeed;
        }
        else if(isGrounded)
        {
            currentMoveState = MoveStates.WALK;
            speed = normalSpeed;
        }
        else
        {
            currentMoveState = MoveStates.JUMP;
        }
    }
}
