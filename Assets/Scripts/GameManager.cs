using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Objectives
{
    Steal,
    Destroy,
    Escape,
    Assassinate,
    KillAll,
    Max
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int score = 0;
    private float timer = 0f;
    private float maxTimer = 120.0f;

    

    private Objectives objective;

    private void Awake() 
    { 
        if (Instance != null && Instance != this) Destroy(this); // can only be 1 instance
        else Instance = this; 

        timer = maxTimer;

        int rand = UnityEngine.Random.Range(0, (int)Objectives.Max);
        objective = (Objectives)rand;
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

    public int GetScore()
    {
        return score;
    }

    public void IncreaseScoreBy(int amount)
    {
        score += amount;
    }

    public Objectives GetObjective()
    {
        return objective;
    }
}
