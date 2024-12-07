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
    [HideInInspector] public DisabledState disabledState;

    [HideInInspector] public NavMeshAgent nmAgent;
    [HideInInspector] public Transform player;
    [HideInInspector] public EnemyDetection detection;
    [HideInInspector] public EnemyHearing hearing;
    [HideInInspector] public Light turret;
    [HideInInspector] public Light spotlight;
    [SerializeField] public LayerMask laserLayerMask;
    [HideInInspector] public Transform patrolPath;

    [HideInInspector] public Vector3[] waypoints;
    [HideInInspector] public int patrolIndex = 0;
    [HideInInspector] public bool beenShot = false;
    [HideInInspector] public GameObject huntAlertObj;
    [HideInInspector] public GameObject combatAlertObj;
    //[HideInInspector] public int disableForSecs = 0;

    [SerializeField] public Transform testingSphere;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        combatState = new CombatState(this);

        investigateState = new InvestigateState(this);

        huntState = new HuntState(this);
        patrolState = new PatrolState(this);

        disabledState = new DisabledState(this);

        nmAgent = GetComponent<NavMeshAgent>();
        detection = transform.GetChild(2).GetComponent<EnemyDetection>();
        hearing = GetComponent<EnemyHearing>();
        turret = transform.GetChild(4).GetComponent<Light>();
        spotlight = transform.GetChild(1).GetComponent<Light>();

        huntAlertObj = transform.GetChild(6).gameObject;
        combatAlertObj = transform.GetChild(7).gameObject;

        patrolPath = transform.parent.GetChild(1);

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
