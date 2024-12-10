using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject _containerGameObject;
    [SerializeField] private TextMeshProUGUI _interactTextMeshProUGUI;

    public void Show(IInteractible interactible)
    {
        _containerGameObject.SetActive(true);
        _interactTextMeshProUGUI.text = interactible.GetInteractText();
    }

    public void Hide()
    {
        _containerGameObject.SetActive(false);
    }
}
