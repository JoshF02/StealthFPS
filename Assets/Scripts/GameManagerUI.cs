using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    private TextMeshProUGUI _scoreTMPUGUI;
    private TextMeshProUGUI _timerTMPUGUI;
    private TextMeshProUGUI _objTMPUGUI;
    
    private void Awake()
    {
        _scoreTMPUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _timerTMPUGUI = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _objTMPUGUI = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Objectives objective = GameManager.Instance.GetObjective();
        
        _objTMPUGUI.text = objective switch
        {
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
        _scoreTMPUGUI.text = GameManager.Instance.GetScore().ToString();
        _timerTMPUGUI.text = GameManager.Instance.GetTimer().ToString();

        if (GameManager.Instance.GetObjectiveComplete()) _objTMPUGUI.text = "Objective Complete";
    }
}
