using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PMoveController : MonoBehaviour
{
    //Run
    public float movementSpeed;
    public float boostCoefficient;
    //Jump
    public float gConstant = 10;
    public float fallDownCoefficient;
    public float jumpForce;
    public float jumpDecreaseCoefficent;
    public float distanceToGround;
    //Mouse
    public bool invertMouse;
    public float mouseSensitivity;



    public KeyCode jumpButton;
    //public KeyCode WASD;
    public KeyCode dashButton;
    public KeyCode shootButton;

    public UnityEvent jump;
    public UnityEvent move;
    public UnityEvent shoot;
    public UnityEvent dash;


    public GameObject cameraObject;
    public GameObject feet;


    private Vector3 velocity;
    private Vector3 jumpVelocity;
    float jumpLength;
    float _gconst;
    bool isGrounded = false;
    private Rigidbody rb;
    
    void Awake()
    {
        move.AddListener(MoveFixed);
        jump.AddListener(Jump);
        dash.AddListener(InstantSpeedUp);
        rb = gameObject.GetComponent<Rigidbody>();

    }
    // Start is called before the first frame update
    void Start()
    {
        if (!GroundCheck()) _gconst = gConstant * fallDownCoefficient;
        else _gconst = gConstant;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Input.ResetInputAxes();
        jumpVelocity = new Vector3(0, 0, 0);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        Debug.Log(_gconst);
        CameraRotate();
        if (Input.GetKeyUp(dashButton) && dash != null) InstantSlowDown();
        if (Input.GetKeyDown(dashButton) && dash != null) dash.Invoke();
        if (Input.GetKeyDown(jumpButton) && jump != null) jump.Invoke();
        if (Input.GetKeyDown(shootButton) && shoot != null) shoot.Invoke();
        isGrounded = GroundCheck();
        rb.velocity = velocity;
    }

    private void FixedUpdate()
    {
        if (move != null) move.Invoke();
    }
     
    void Move()
    {

    velocity.x = ((Vector3.up*jumpLength+gameObject.transform.forward*Input.GetAxis("Vertical") + gameObject.transform.right*Input.GetAxis("Horizontal"))*movementSpeed*Time.deltaTime+Vector3.down*gConstant*Time.deltaTime).x;
    velocity.z = ((Vector3.up*jumpLength+gameObject.transform.forward*Input.GetAxis("Vertical") + gameObject.transform.right*Input.GetAxis("Horizontal"))*movementSpeed*Time.deltaTime+Vector3.down*gConstant*Time.deltaTime).z;
        velocity.y += jumpLength;
        if (jumpLength > 0) jumpLength = jumpLength - jumpLength * Time.deltaTime * 100f;
        else jumpLength = 0;
    }

    void MoveFixed()
    {
      // velocity =(gameObject.transform.forward * Input.GetAxis("Vertical") + gameObject.transform.right * Input.GetAxis("Horizontal")) * movementSpeed + Vector3.down*gConstant;
        velocity.x = ((Vector3.up * jumpLength + gameObject.transform.forward * Input.GetAxis("Vertical") + gameObject.transform.right * Input.GetAxis("Horizontal")) * movementSpeed + Vector3.down * _gconst ).x;
        velocity.z = ((Vector3.up * jumpLength + gameObject.transform.forward * Input.GetAxis("Vertical") + gameObject.transform.right * Input.GetAxis("Horizontal")) * movementSpeed + Vector3.down * _gconst ).z;
        velocity.y = jumpLength + (Vector3.down * _gconst).y;

        if (jumpLength > 0)
        {

            jumpLength = jumpLength + (Vector3.down * gConstant / jumpDecreaseCoefficent).y;

        }
        else
        {
            if (!GroundCheck()) _gconst = gConstant * fallDownCoefficient;
            else _gconst = gConstant / fallDownCoefficient;
            jumpLength = 0; }


    }




    void CameraRotation()
    {
        var deltaMouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        Vector2 deltaRotation = deltaMouse * mouseSensitivity;
        deltaRotation.y *= invertMouse ? 1.0f : -1.0f;

        float pitchAngle = cameraObject.transform.localEulerAngles.x;

        // turns 270 deg into -90, etc
        if (pitchAngle > 180)
            pitchAngle -= 360;

        pitchAngle = Mathf.Clamp(pitchAngle + deltaRotation.y, -90.0f, 90.0f);

        transform.Rotate(Vector3.up, deltaRotation.x);
        cameraObject.transform.localRotation = Quaternion.Euler(pitchAngle, 0.0f, 0.0f);
    }

    void CameraRotate()
    {
        CameraRotation();
    }
    bool GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit,distanceToGround+ 0.1f) && hit.transform.tag == "Ground")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Jump()
    {
        if (GroundCheck())
        {
            jumpLength = jumpForce;
        }
    }

    void InstantSpeedUp()
    {
        movementSpeed += boostCoefficient;

    }
    void InstantSlowDown()
    {
        movementSpeed -= boostCoefficient;
    }
    

}
