using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score;
    private float timer;
    private float maxTimer = 120.0f;

    private void Awake() 
    { 
        if (Instance != null && Instance != this) Destroy(this); // can only be 1 instance
        else Instance = this; 

        timer = maxTimer;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0) {
            Debug.Log("GAME OVER");
        }
    }

    public double GetTimer()
    {
        return Math.Round(timer, 2);
    }
}
