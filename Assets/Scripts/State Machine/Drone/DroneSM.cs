using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneSM : StateMachine
{
    [HideInInspector] public CombatState combatState;
    [HideInInspector] public InvestigateState investigateState;
    [HideInInspector] public HuntState huntState;
    [HideInInspector] public PatrolState patrolState;

    [HideInInspector] public NavMeshAgent nmAgent;
    public Transform player;
    [HideInInspector] public EnemyDetection detection;
    [HideInInspector] public Light turret;
    [SerializeField] public LayerMask laserLayerMask;

    private void Awake()
    {
        combatState = new CombatState(this);

        investigateState = new InvestigateState(this);

        huntState = new HuntState(this);
        patrolState = new PatrolState(this);

        nmAgent = GetComponent<NavMeshAgent>();
        detection = transform.GetChild(1).GetComponent<EnemyDetection>();
        turret = transform.GetChild(3).GetComponent<Light>();
    }

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }
}
