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
    [SerializeField] public Transform patrolPath;

    [HideInInspector] public Vector3[] waypoints;
    [HideInInspector] public int patrolIndex = 0;

    private void Awake()
    {
        combatState = new CombatState(this);

        investigateState = new InvestigateState(this);

        huntState = new HuntState(this);
        patrolState = new PatrolState(this);

        nmAgent = GetComponent<NavMeshAgent>();
        detection = transform.GetChild(1).GetComponent<EnemyDetection>();
        turret = transform.GetChild(3).GetComponent<Light>();

        waypoints = new Vector3[patrolPath.childCount];

        for (int i = 0; i < waypoints.Length; i++) {
            waypoints[i] = patrolPath.GetChild(i).position;
        }
    }

    protected override BaseState GetInitialState()
    {
        return patrolState;
    }

    void OnDrawGizmos() // visualises patrol path
    {
        Vector3 startPos = patrolPath.GetChild(0).position;
        Vector3 prevPos = startPos;

        foreach(Transform waypoint in patrolPath) {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(prevPos, waypoint.position);
            prevPos = waypoint.position;
        }
        
        Gizmos.DrawLine(prevPos, startPos);
    }
}
