using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    private TextMeshProUGUI scoreTMPUGUI;
    private TextMeshProUGUI timerTMPUGUI;
    private TextMeshProUGUI objTMPUGUI;
    private void Awake()
    {
        scoreTMPUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        timerTMPUGUI = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        objTMPUGUI = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Objectives objective = GameManager.Instance.GetObjective();
        
        objTMPUGUI.text = objective switch {        // switch statement of different objective types
            Objectives.Steal => "Steal The Item",
            Objectives.Destroy => "Destroy The Item",
            Objectives.Escape => "Escape The Level",
            Objectives.Assassinate => "Assassinate The Target",
            Objectives.KillAll => "Kill All Enemies",
            _ => "Other Objective",
        };
    }

    private void Update()
    {
        scoreTMPUGUI.text = GameManager.Instance.GetScore().ToString();
        timerTMPUGUI.text = GameManager.Instance.GetTimer().ToString();

        if (GameManager.Instance.GetObjectiveComplete()) objTMPUGUI.text = "Objective Complete";
    }
}
