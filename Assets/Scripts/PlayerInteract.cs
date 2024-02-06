using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerInteractUI playerInteractUI;
    [SerializeField] private LayerMask layerMask;
    private float range = 5f;
    private Camera fpsCam;
    private Slider slider;
    private float timer = 0f;

    private void Awake()
    {
        slider = playerInteractUI.gameObject.GetComponent<Slider>();
        fpsCam = transform.GetChild(0).GetComponent<Camera>();
    }

    private void Update()
    {
        IInteractible interactible = GetInteractibleObjectRaycast();   // if interactible found, show UI popup
        if(interactible != null) {
            playerInteractUI.Show(interactible);
    
            if (Input.GetKey(KeyCode.E)) {  // if E held, fill the circular slider up over time
                timer += Time.deltaTime;

                if(timer >= 1.0f) { // when slider full, trigger interaction
                    timer = 0f;
                    interactible.Interact(transform);  
                }
            } else {
                if(timer >= 0f) timer -= Time.deltaTime;
            }

        } else {
            playerInteractUI.Hide();
            timer = 0f;
        }

        slider.value = timer;
    }

    private IInteractible GetInteractibleObjectRaycast()
    {
        IInteractible target = null;

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask)) {
            target = hit.transform.GetComponent<IInteractible>();
        }

        return target;
    }
}
