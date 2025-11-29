using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    private CharacterController cc;
    private GameObject cam;

    // Movement parameters
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float WalkSpeed = 1;
    [SerializeField] private float RunSpeed = 1;
    [SerializeField] private float CrouchSpeed = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private float YVelocity;
    private float currentSpeed;
    float PlrHeight = 2;

    // Look/View parameters
    [SerializeField] private float mouseSens = 1f;
    [SerializeField] private float bobbingScale = 1;
    [SerializeField] private float ZModifier;
    private float X, Y, Z;
    int zoomVal = 75;

    // State flags
    [SerializeField] private bool isStand, isWalk, isSprint, isCrouch, isFalling, isJump;
    [SerializeField] private bool platform = true; //true - PC, false - Phone


    // Input
    Vector2 center;
    private float H, V;

    // Serialized debug/editor fields
    [SerializeField] float spd;
    [SerializeField] float Magnitude;

    // Sound service
    [SerializeField] private Sounds snd_;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main.gameObject;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mouseSens /= 10f;
    }

    void Update()
    {

        HandleJump();
        HandleCrouch();
        HandleSprint();
        CameraMovement();
        PlayerMovement();
        FootstepSnd();
        zoom();
    }

    void HandleJump()
    {
        if (cc.isGrounded == true)
        {
            //FallDamage();
            isFalling = false;
            isJump = false;
        }
        else
        {
            isFalling = true;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
            snd_.playsoundatpoint_(8, transform.position - Vector3.down * 1f, 0.5f);
            YVelocity = jumpForce;
            isFalling = true;
        }
    }

    void HandleSprint()
    {
        if (isCrouch == true || isFalling == true)
        {
            isSprint = false;
            return;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprint = true;
            currentSpeed = RunSpeed;
        }
        else
        {
            isSprint = false;
            currentSpeed = WalkSpeed;
        }
    }
    
    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouch = true;
            PlrHeight = 1.3f;
            currentSpeed = CrouchSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.SphereCast(transform.position - Vector3.up * 2f, 1.5f, Vector3.up, out _, 5)) return;
            isCrouch = false;
            PlrHeight = 2;
            currentSpeed = WalkSpeed;
        }

        transform.localScale = new Vector3(1, PlrHeight, 1);
    }
    void PlayerMovement()
    {
        if (platform == true && isFalling == false)
        {
            H = Input.GetAxis("Horizontal");
            V = Input.GetAxis("Vertical");
        }

        Magnitude = new Vector3(cc.velocity.x, 0f, cc.velocity.z).magnitude;
        spd = currentSpeed*3;

        if (Magnitude > 0)
        {
            isWalk = true;
            isStand = false;
        }
        else
        {
            isWalk = false;
            isStand = true;
        }

        if (V < 0) spd = Mathf.Clamp(spd, 0, 9);

        Vector3 moveDir = transform.forward * V * spd + transform.right * H * spd;
        moveDir = Vector3.ClampMagnitude(moveDir, spd);

        RaycastHit hit_;
        if (YVelocity > -1 && Physics.SphereCast(gameObject.transform.position, 1, gameObject.transform.up, out hit_, transform.localScale.y/1.7f))
        {
            if (hit_.collider.gameObject)
            {
                YVelocity = -1;
            }
        }

        if (!cc.isGrounded && YVelocity != 0)
        {
            YVelocity += gravity * Time.deltaTime;
            YVelocity = Mathf.Max(YVelocity, -50);
        }
        cc.Move((moveDir + Vector3.up * YVelocity) * Time.deltaTime);
        wd -= Magnitude * Time.deltaTime / 6.2f;
    }

    [SerializeField] private float walkDist = 0.5f;
    private float wd;
    void FootstepSnd()
    {
        if (isFalling || isCrouch) return;
        if (wd <= 0)
        {
            wd = walkDist;
            snd_.playsoundatpoint_(0, transform.position - Vector3.down * 1f, 0.1f);
        }
    }

    float XRot;
    public bool moving = false;
    byte A = 1;
    void CameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse Y") * mouseSens;
        float mouseY = Input.GetAxis("Mouse X") * mouseSens;

        X -= mouseX;
        Y += mouseY;
        if (X > 80) X = 80;
        else if (X < -80) X = -80;

        float SinY = 0;
        float SinX = 0;

        if (isFalling == false)
        {
            SinY = Mathf.Sin(Time.time * spd*1) * Magnitude / 150 * bobbingScale;
            SinX = Mathf.Sin(Time.time * spd*0.5f) * Magnitude / 100 * bobbingScale;
        }

        Z = Mathf.Lerp(Z, mouseY * ZModifier - (H * 2), 0.5f);
        Vector3 pos = Vector3.Lerp(cam.transform.position, transform.position + transform.right * SinX + Vector3.up * transform.localScale.y * SinY  + cam.transform.forward * 0.5f + Vector3.up * transform.localScale.y/2, Time.deltaTime * 25);
        cam.transform.position = Vector3.Lerp(cam.transform.position, pos , 0.7f);
        
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(X, Y, Z), Time.deltaTime * 30);
        if (isFalling == true) return;
        transform.rotation = Quaternion.Euler(0,Y,0);

        if (cam.transform.rotation.x*100 == XRot*100) moving = false;
        else moving = true;
        XRot = cam.transform.rotation.x;

        if (moving == false) A = 0;
        else A = 1;

    }

    void zoom()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            zoomVal = 30;
            snd_.playsound_(9);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            zoomVal = 75;
            snd_.playsound_(9);
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomVal, Time.deltaTime * 15);
    }
    private bool NVen = false;
}
