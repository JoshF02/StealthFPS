using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    private TextMeshProUGUI scoreTMPUGUI;
    private TextMeshProUGUI timerTMPUGUI;
    private void Awake()
    {
        scoreTMPUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        timerTMPUGUI = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        scoreTMPUGUI.text = GameManager.Instance.GetScore().ToString();
        timerTMPUGUI.text = GameManager.Instance.GetTimer().ToString();
    }
}
