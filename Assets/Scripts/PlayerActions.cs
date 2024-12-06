using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    //controls stuff
    [SerializeField] private float speed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float forceMagnitude;
    [HideInInspector] public float scrollWheelRotation = 0f;
    private CharacterController controller;
    private Transform cam;
    float xRotation = 0f;
    private float verticalSpeed = 0f;
    private readonly float gravity = 30f;


    private Transform weaponHolder;
    public bool isAiming = false;
    private bool isCrouching = false;
    private bool isSprinting = false;

    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot; 

    public float smooth = 10f;
    float smoothRot = 12f;

    [Header("Bobbing")]
    public float speedCurve;
    float curveSin {get => Mathf.Sin(speedCurve);}
    float curveCos {get => Mathf.Cos(speedCurve);}

    public Vector3 travelLimit = Vector3.one * 0.0025f;
    public Vector3 bobLimit = Vector3.one * 0.001f;
    Vector3 bobPosition;

    public float bobExaggeration;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;

    [SerializeField] private GameObject smokeGrenade;
    [SerializeField] private GameObject stone;
    private int grenadesLeft = 3;
    private int stonesLeft = 10;





    private void Awake()
    {
        //controls
        Cursor.lockState = CursorLockMode.Locked;
        controller = transform.GetComponent<CharacterController>();
        cam = transform.GetChild(0);
        weaponHolder = cam.GetChild(1);
    }


    private void Update()
    {
        Move();
        Look();
        ThrowGrenade();
        if (!isAiming || !isCrouching) ApplyBobAndSway();
        else ApplySway();
    }


    private void Move()
    {
        // movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        isSprinting = false;

        // crouching
        if (Input.GetKey(KeyCode.CapsLock))
        {
            controller.height = 1.5f;
            speed = 2f;
            isCrouching = true;
        }
        else 
        {
            controller.height = 2f;
            speed = 4f;
            isCrouching = false;

            // sprinting
            if (Input.GetKey(KeyCode.LeftShift) && vertical > 0) 
            {
                vertical *= 1.5f;
                isSprinting = true;
            }
        }

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // sprinting
        //if (Input.GetKey(KeyCode.LeftShift)) speed = 7.5f;
        //else speed = 4f;

        // gravity
        if (controller.isGrounded) verticalSpeed = 0;
        else verticalSpeed -= gravity * Time.deltaTime;
        Vector3 vert = new Vector3(0f, verticalSpeed, 0f) * Time.deltaTime;


        controller.Move((move * speed * Time.deltaTime) + vert);

        CalculateBob(horizontal * speed * 0.25f, vertical * speed * 0.25f);

        // sound for moving
        if (move != Vector3.zero && !isCrouching) {
            int multiplier = isSprinting ? 2 : 1;
            PlayerSound.Instance.StartSound("movement", 10 * multiplier);
        }
        else PlayerSound.Instance.StopSound("movement");
    }


    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        CalculateSway(mouseX, mouseY);

        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        
    }

    private void ThrowGrenade()
    {
        if (Input.GetKeyDown(KeyCode.G) && grenadesLeft > 0) {
            GameObject smokeGrenadeObj = Instantiate(smokeGrenade, transform.position + (transform.forward * 2f), Quaternion.identity);
            grenadesLeft--;
        }

        if (Input.GetKeyDown(KeyCode.B) && stonesLeft > 0) {
            GameObject stoneObj = Instantiate(stone, transform.position + (transform.forward * 2f), Quaternion.identity);
            stonesLeft--;
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit) // allows player to push rigidbodies
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude * speed, transform.position, ForceMode.Impulse);
        }
    }




    void CalculateSway(float mouseX, float mouseY){
        Vector2 lookInput = new Vector2(mouseX, mouseY);
        // sway
        Vector2 invertLook = lookInput *-step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    
        // sway rotation
        invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);
        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    void CalculateBob(float horizontal, float vertical){
        Vector2 walkInput = new Vector2(horizontal, vertical).normalized;

        // more bobbing when sprinting, and when not aiming
        float scale = (isSprinting ? 1.75f : 1);
        if (!isAiming) scale *= 10;

        // bob offset
        float sum = (vertical == 0 ? horizontal : vertical);
        speedCurve += Time.deltaTime * (sum * bobExaggeration);

        bobPosition.x = (curveCos*bobLimit.x)-(walkInput.x * travelLimit.x * scale);
        bobPosition.y = (curveSin*bobLimit.y)-(vertical * travelLimit.y * scale);
        bobPosition.z = -(walkInput.y * travelLimit.z * scale);
    
        // bob rotation
        bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * scale * (Mathf.Sin(2*speedCurve)) : multiplier.x * scale * (Mathf.Sin(2*speedCurve) / 2));
        bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * scale * curveCos : 0);
        bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * scale * curveCos * walkInput.x : 0);
    }

    void ApplyBobAndSway(){
        weaponHolder.localPosition = Vector3.Lerp(weaponHolder.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);
        weaponHolder.localRotation = Quaternion.Slerp(weaponHolder.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    void ApplySway() {
        weaponHolder.localPosition = Vector3.Lerp(weaponHolder.localPosition, swayPos, Time.deltaTime * smooth);
        weaponHolder.localRotation = Quaternion.Slerp(weaponHolder.localRotation, Quaternion.Euler(swayEulerRot), Time.deltaTime * smoothRot);
    }
}
