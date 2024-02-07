using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    private TextMeshProUGUI timerTMPUGUI;
    private void Awake()
    {
        timerTMPUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        timerTMPUGUI.text = GameManager.Instance.GetTimer().ToString();
    }
}
