using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState CurrentState { get; private set; }

    private void Start()
    {
        CurrentState = GetInitialState();
        CurrentState?.Enter();
    }

    private void Update()
    {
        CurrentState?.UpdateLogic();
    }

    private void LateUpdate()
    {
        CurrentState?.UpdatePhysics();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    public void ChangeState(BaseState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
}
