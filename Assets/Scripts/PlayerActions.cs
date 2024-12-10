using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    [Header("Controls")]
    public bool IsAiming = false;
    private float _speed;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private float _forceMagnitude;
    private CharacterController _controller;
    private Transform _cam;
    private float _xRotation = 0f;
    private float _verticalSpeed = 0f;
    private readonly float _gravity = 30f;
    private Transform _weaponHolder;
    private bool _isCrouching = false;
    private bool _isSprinting = false;

    [Header("Sway")]
    [SerializeField] private float _step = 0.01f;
    [SerializeField] private float _maxStepDistance = 0.06f;
    private Vector3 _swayPos;

    [Header("Sway Rotation")]
    [SerializeField] private float _rotationStep = 4f;
    [SerializeField] private float _maxRotationStep = 5f; 
    [SerializeField] private float _smooth = 10f;
    private Vector3 _swayEulerRot;
    private float _smoothRot = 12f;

    [Header("Bobbing")]
    [SerializeField] private float _speedCurve;
    [SerializeField] private Vector3 _travelLimit = Vector3.one * 0.0025f;
    [SerializeField] private Vector3 _bobLimit = Vector3.one * 0.001f;
    [SerializeField] private float _bobExaggeration;
    private Vector3 _bobPosition;
    private float _curveSin {get => Mathf.Sin(_speedCurve);}
    private float _curveCos {get => Mathf.Cos(_speedCurve);}

    [Header("Bob Rotation")]
    [SerializeField] private Vector3 _multiplier;
    private Vector3 _bobEulerRotation;

    [Header("Throwables")]
    [SerializeField] private SmokeGrenade _smokeGrenade;
    [SerializeField] private StoneThrowable _stone;
    [SerializeField] private ThrowingKnife _throwingKnife;
    [SerializeField] private EMPGrenade _empGrenade;
    [SerializeField] private DecoyThrowable _decoyThrowable;
    [SerializeField] private TeleporterThrowable _teleporterThrowable;
    private int _grenadesLeft = 3;
    private int _stonesLeft = 20;
    private int _throwingKnivesLeft = 100;
    private int _empGrenadesLeft = 100;
    private int _decoysLeft = 100;
    private int _teleportersLeft = 10;
    private Transform _activeTeleporter = null;
    [field:SerializeField] public bool InvisPerkActive { get; private set; } = false;
    public bool HasTeleported { get; private set; } = false;

    private enum ThrowableTypes
    {
        Stone,
        Smoke,
        Knife,
        EMP,
        Decoy,
        Teleporter,
        Max
    }

    private Throwable _slot1Prefab;
    private Throwable _slot2Prefab;
    private int _slot1Ammo;
    private int _slot2Ammo;


    public void SetActiveTeleporter(Transform teleporter)
    {
        _activeTeleporter = teleporter;
    }

    private void Awake()
    {
        //controls
        Cursor.lockState = CursorLockMode.Locked;
        _controller = transform.GetComponent<CharacterController>();
        _cam = transform.GetChild(0);
        _weaponHolder = _cam.GetChild(1);

        switch((ThrowableTypes)GameManager.Instance.ThrowableSlot1)
        {
            case ThrowableTypes.Stone:
                _slot1Prefab = _stone;
                _slot1Ammo = 100;
                break;
            case ThrowableTypes.Smoke:
                _slot1Prefab = _smokeGrenade;
                _slot1Ammo = 10;
                break;
            case ThrowableTypes.Knife:
                _slot1Prefab = _throwingKnife;
                _slot1Ammo = 10;
                break;
            case ThrowableTypes.EMP:
                _slot1Prefab = _empGrenade;
                _slot1Ammo = 10;
                break;
            case ThrowableTypes.Decoy:
                _slot1Prefab = _decoyThrowable;
                _slot1Ammo = 10;
                break;
            case ThrowableTypes.Teleporter:
                _slot1Prefab = _teleporterThrowable;
                _slot1Ammo = 10;
                break;
        }

        switch((ThrowableTypes)GameManager.Instance.ThrowableSlot2)
        {
            case ThrowableTypes.Stone:
                _slot2Prefab = _stone;
                _slot2Ammo = 100;
                break;
            case ThrowableTypes.Smoke:
                _slot2Prefab = _smokeGrenade;
                _slot2Ammo = 10;
                break;
            case ThrowableTypes.Knife:
                _slot2Prefab = _throwingKnife;
                _slot2Ammo = 10;
                break;
            case ThrowableTypes.EMP:
                _slot2Prefab = _empGrenade;
                _slot2Ammo = 10;
                break;
            case ThrowableTypes.Decoy:
                _slot2Prefab = _decoyThrowable;
                _slot2Ammo = 10;
                break;
            case ThrowableTypes.Teleporter:
                _slot2Prefab = _teleporterThrowable;
                _slot2Ammo = 10;
                break;
        }
    }

    private void Update()
    {
        Move();
        Look();
        Throwables();
        if (!IsAiming || !_isCrouching) ApplyBobAndSway();
        else ApplySway();
    }

    private void Move()
    {
        // movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _isSprinting = false;

        // crouching
        if (Input.GetKey(KeyCode.CapsLock))
        {
            _controller.height = 1.5f;
            _speed = 2f;
            _isCrouching = true;
        }
        else 
        {
            _controller.height = 2f;
            _speed = 4f;
            _isCrouching = false;

            // sprinting
            if (Input.GetKey(KeyCode.LeftShift) && (vertical > 0)) 
            {
                vertical *= 1.5f;
                _isSprinting = true;
            }
        }

        if (GameManager.Instance.Faster) _speed *= 1.5f;

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // gravity
        if (_controller.isGrounded) _verticalSpeed = 0;
        else _verticalSpeed -= _gravity * Time.deltaTime;

        Vector3 vert = new Vector3(0f, _verticalSpeed, 0f) * Time.deltaTime;

        _controller.Move((move * _speed * Time.deltaTime) + vert);
        CalculateBob(horizontal * _speed * 0.25f, vertical * _speed * 0.25f);

        // sound for moving
        if ((move != Vector3.zero) && !_isCrouching)
        {
            int multiplier = _isSprinting ? 2 : 1;
            if (GameManager.Instance.SilentStep) multiplier -= 1;
            PlayerSound.Instance.StartSound("movement", 10 * multiplier);
        }
        else PlayerSound.Instance.StopSound("movement");

        // invisibility when still perk
        if (GameManager.Instance.InvisWhenStill && (move == Vector3.zero)) InvisPerkActive = true;
        else InvisPerkActive = false;
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        CalculateSway(mouseX, mouseY);

        mouseX *= _mouseSensitivity;
        mouseY *= _mouseSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _cam.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Throwables()   // throwable gadget usage
    {
        if (Input.GetKeyDown(KeyCode.U) && (_slot1Ammo > 0))
        {
            Throwable slot1Obj = Instantiate<Throwable>(_slot1Prefab, _cam.position + (_cam.forward * 2f), transform.localRotation);
            float force, torque;
            if ((ThrowableTypes)GameManager.Instance.ThrowableSlot1 == ThrowableTypes.Stone)
            {
                force = 2500f;
                torque = 20f;
            }
            else if ((ThrowableTypes)GameManager.Instance.ThrowableSlot1 == ThrowableTypes.Knife)
            {
                force = 7500f;
                torque = 200f;
            }
            else
            {
                force = 1500f;
                torque = 50f;
            }

            slot1Obj.Init(_cam, force, torque);
            _slot1Ammo--;
        }

        if (Input.GetKeyDown(KeyCode.I) && _slot2Ammo > 0)
        {
            Throwable slot2Obj = Instantiate<Throwable>(_slot2Prefab, _cam.position + (_cam.forward * 2f), transform.localRotation);
            float force, torque;
            if ((ThrowableTypes)GameManager.Instance.ThrowableSlot2 == ThrowableTypes.Stone)
            {
                force = 2500f;
                torque = 20f;
            }
            else if ((ThrowableTypes)GameManager.Instance.ThrowableSlot2 == ThrowableTypes.Knife)
            {
                force = 7500f;
                torque = 200f;
            }
            else
            {
                force = 1500f;
                torque = 50f;
            }

            slot2Obj.Init(_cam, force, torque);
            _slot2Ammo--;
        }

        if (Input.GetKeyDown(KeyCode.G) && _grenadesLeft > 0)
        {
            SmokeGrenade smokeGrenadeObj = Instantiate<SmokeGrenade>(_smokeGrenade, transform.position + (_cam.forward * 2f), transform.localRotation);
            smokeGrenadeObj.Init(_cam, 1500f, 50f);
            _grenadesLeft--;
        }

        if (Input.GetKeyDown(KeyCode.B) && _stonesLeft > 0)
        {
            StoneThrowable stoneObj = Instantiate<StoneThrowable>(_stone, transform.position + (_cam.forward * 2f), transform.localRotation);
            stoneObj.Init(_cam, 2500f, 20f);
            _stonesLeft--;
        }

        if (Input.GetKeyDown(KeyCode.N) && _throwingKnivesLeft > 0)
        {
            ThrowingKnife throwingKnifeObj = Instantiate<ThrowingKnife>(_throwingKnife, _cam.position + (_cam.forward * 2f), transform.localRotation);
            throwingKnifeObj.Init(_cam, 7500f, 200f);
            _throwingKnivesLeft--;
        }

        if (Input.GetKeyDown(KeyCode.M) && _empGrenadesLeft > 0)
        {
            EMPGrenade empGrenadeObj = Instantiate<EMPGrenade>(_empGrenade, transform.position + (_cam.forward * 2f), transform.localRotation);
            empGrenadeObj.Init(_cam, 2500f, 20f);
            _empGrenadesLeft--;
        }

        if (Input.GetKeyDown(KeyCode.K) && _decoysLeft > 0)
        {
            DecoyThrowable decoyThrowableObj = Instantiate<DecoyThrowable>(_decoyThrowable, transform.position + (_cam.forward * 2f), transform.localRotation);
            decoyThrowableObj.Init(_cam, 2500f, 20f);
            _decoysLeft--;
        }

        if (Input.GetKeyDown(KeyCode.L) && _teleportersLeft > 0)
        {
            TeleporterThrowable teleporterThrowableObj = Instantiate<TeleporterThrowable>(_teleporterThrowable, transform.position + (_cam.forward * 2f), transform.localRotation);
            teleporterThrowableObj.Init(_cam, 2500f, 20f);
            _teleportersLeft--;
        }

        if (HasTeleported) HasTeleported = false;

        if (Input.GetKeyDown(KeyCode.P) && _activeTeleporter != null)
        {
            Debug.Log("teleporting");
            _controller.enabled = false;
            transform.position = _activeTeleporter.position + new Vector3(0, 1.08f, 0);
            _controller.enabled = true;
            HasTeleported = true;
            //activeTeleporter = null;
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

            rigidbody.AddForceAtPosition(forceDirection * _forceMagnitude * _speed, transform.position, ForceMode.Impulse);
        }
    }

    private void CalculateSway(float mouseX, float mouseY)
    {
        Vector2 lookInput = new Vector2(mouseX, mouseY);
        // sway
        Vector2 invertLook = lookInput *-_step;
        invertLook.x = Mathf.Clamp(invertLook.x, -_maxStepDistance, _maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -_maxStepDistance, _maxStepDistance);

        _swayPos = invertLook;
    
        // sway rotation
        invertLook = lookInput * -_rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -_maxRotationStep, _maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -_maxRotationStep, _maxRotationStep);
        _swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    private void CalculateBob(float horizontal, float vertical)
    {
        Vector2 walkInput = new Vector2(horizontal, vertical).normalized;

        // more bobbing when sprinting, and when not aiming
        float scale = (_isSprinting ? 1.75f : 1);
        if (!IsAiming) scale *= 10;

        // bob offset
        float sum = (vertical == 0 ? horizontal : vertical);
        _speedCurve += Time.deltaTime * (sum * _bobExaggeration);

        _bobPosition.x = (_curveCos*_bobLimit.x)-(walkInput.x * _travelLimit.x * scale);
        _bobPosition.y = (_curveSin*_bobLimit.y)-(vertical * _travelLimit.y * scale);
        _bobPosition.z = -(walkInput.y * _travelLimit.z * scale);
    
        // bob rotation
        _bobEulerRotation.x = (walkInput != Vector2.zero ? _multiplier.x * scale * (Mathf.Sin(2*_speedCurve)) : _multiplier.x * scale * (Mathf.Sin(2*_speedCurve) / 2));
        _bobEulerRotation.y = (walkInput != Vector2.zero ? _multiplier.y * scale * _curveCos : 0);
        _bobEulerRotation.z = (walkInput != Vector2.zero ? _multiplier.z * scale * _curveCos * walkInput.x : 0);
    }

    private void ApplyBobAndSway()
    {
        _weaponHolder.localPosition = Vector3.Lerp(_weaponHolder.localPosition, _swayPos + _bobPosition, Time.deltaTime * _smooth);
        _weaponHolder.localRotation = Quaternion.Slerp(_weaponHolder.localRotation, Quaternion.Euler(_swayEulerRot) * Quaternion.Euler(_bobEulerRotation), Time.deltaTime * _smoothRot);
    }

    private void ApplySway()
    {
        _weaponHolder.localPosition = Vector3.Lerp(_weaponHolder.localPosition, _swayPos, Time.deltaTime * _smooth);
        _weaponHolder.localRotation = Quaternion.Slerp(_weaponHolder.localRotation, Quaternion.Euler(_swayEulerRot), Time.deltaTime * _smoothRot);
    }
}
