using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerInteractUI _playerInteractUI;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _interactionLength = 1.0f;
    private float _range = 2.5f;
    private Camera _fpsCam;
    private Slider _slider;
    private float _timer = 0f;

    private void Awake()
    {
        _slider = _playerInteractUI.gameObject.GetComponent<Slider>();
        _fpsCam = transform.GetChild(0).GetComponent<Camera>();
    }

    private void Update()
    {
        IInteractible interactible = GetInteractibleObjectRaycast();   // if interactible found, show UI popup
        if(interactible != null)
        {
            _playerInteractUI.Show(interactible);
    
            if (Input.GetKey(KeyCode.E))    // if E held, fill the circular slider up over time
            {  
                _timer += Time.deltaTime;

                if(_timer >= _interactionLength)  // when slider full, trigger interaction
                { 
                    _timer = 0f;
                    interactible.Interact(transform);  
                }
            }
            else if(_timer >= 0f) _timer -= Time.deltaTime;

        }
        else
        {
            _playerInteractUI.Hide();
            _timer = 0f;
        }

        _slider.value = _timer / _interactionLength;
    }

    private IInteractible GetInteractibleObjectRaycast()
    {
        IInteractible target = null;

        if (Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out RaycastHit hit, _range, _layerMask))
        {
            target = hit.transform.GetComponent<IInteractible>();
        }

        return target;
    }
}
