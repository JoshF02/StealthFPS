using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractible : MonoBehaviour, IInteractible
{
    private string _interactText;
    [SerializeField] private PlayerActions _playerActions;
    [SerializeField] private int _closeTime;
    private float _timer = 0f;
    private bool _moving = false;
    private Rigidbody _rb;
    private Quaternion _rotateFrom;
    private Quaternion _rotateTo;


    private void Awake()
    {
        //_playerActions.interactibles.Add(transform, this);
        _rb = GetComponent<Rigidbody>();
        _interactText = "Open Door";
    }


    public void Interact(Transform interactorTransform)
    {
        if (transform.localEulerAngles.y == 0)  // start opening door
        {  
            _interactText = "Close Door";
            _moving = true;
            _rotateFrom = _rb.rotation;
            _rotateTo = _rotateFrom * Quaternion.Euler(0, -90f, 0);   // rotate by -90 degrees
        }
        else if (transform.localEulerAngles.y == 270)   // start closing door
        {   
            _interactText = "Open Door";   
            _moving = true;
            _rotateFrom = _rb.rotation;
            _rotateTo = _rotateFrom * Quaternion.Euler(0, 90f, 0);    // rotate by +90 degrees
        }
    }


    public string GetInteractText()
    {
        return _interactText;
    }


    public Transform GetTransform()
    {
        return transform;
    }


    private void FixedUpdate()  // performs opening/closing rotation
    {
        if (_moving)
        {
            _timer += Time.fixedDeltaTime;
            _rb.MoveRotation(Quaternion.Slerp(_rotateFrom, _rotateTo, _timer / _closeTime));

            if (_timer >= _closeTime)   // stops opening/closing when done
            {   
                _timer = 0f;
                _moving = false;
            }
        }
    }
}
