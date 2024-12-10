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

    [Header("Objectives")]
    [SerializeField] private GameObject _stealItem;
    private int _score = 0;
    private float _timer = 0f;
    private float _maxTimer = 12000.0f;
    private Objectives _objective;
    private bool _objectiveComplete = false;

    [Header("Perks and Throwables")]
    [SerializeField] private int _perkNum;
    [field:SerializeField] public bool SilentStep { get; private set; } = false; 
    [field:SerializeField] public bool Faster { get; private set; } = false;
    [field:SerializeField] public bool NoBodies { get; private set; } = false;
    [field:SerializeField] public bool InvisWhenStill { get; private set; } = false;
    [field:SerializeField] public int ThrowableSlot1 { get; private set; }
    [field:SerializeField] public int ThrowableSlot2 { get; private set; }
    

    private void Awake() 
    { 
        if ((Instance != null) && (Instance != this)) Destroy(this); // can only be 1 instance
        else Instance = this; 

        _timer = _maxTimer;

        //int rand = UnityEngine.Random.Range(0, (int)Objectives.Max);
        //_objective = (Objectives)rand;
        _objective = Objectives.Steal;

        switch (_objective)
        {
            case Objectives.Steal:
                Instantiate(_stealItem);
                break;
            default:
                Debug.Log("not steal");
                break;
        }

        ThrowableSlot1 = UnityEngine.Random.Range(0, 6);
        ThrowableSlot2 = ThrowableSlot1;
        while (ThrowableSlot2 == ThrowableSlot1) ThrowableSlot2 = UnityEngine.Random.Range(0, 6);
        _perkNum = UnityEngine.Random.Range(0, 4);

        switch(_perkNum)
        {
            case 0:
                SilentStep = true;
                break;
            case 1:
                Faster = true;
                break;
            case 2:
                NoBodies = true;
                break;
            case 3:
                InvisWhenStill = true;
                break;
        }
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            EndGame();
        }
    }

    public double GetTimer()
    {
        return Math.Round(_timer, 2);
    }

    public int GetScore()
    {
        return _score;
    }

    public void IncreaseScoreBy(int amount)
    {
        _score += amount;
    }

    public Objectives GetObjective()
    {
        return _objective;
    }

    public void CompleteObjective()
    {
        _objectiveComplete = true;
    }

    public bool GetObjectiveComplete()
    {
        return _objectiveComplete;
    }

    public void EndGame()
    {
        Debug.Log("Game Over");
    }
}
