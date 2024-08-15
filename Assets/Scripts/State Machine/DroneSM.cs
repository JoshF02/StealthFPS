using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSM : StateMachine
{
    [HideInInspector] public CombatState combatState;
    [HideInInspector] public InvestigateState investigateState;
    [HideInInspector] public HuntState huntState;
    [HideInInspector] public PatrolState patrolState;

    private void Awake()
    {
        combatState = new CombatState(this);

        investigateState = new InvestigateState(this);

        huntState = new HuntState(this);
        patrolState = new PatrolState(this);
    }

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }
}
