using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    public void Show(IInteractible interactible)
    {
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = interactible.GetInteractText();
    }

    public void Hide()
    {
        containerGameObject.SetActive(false);
    }
}
