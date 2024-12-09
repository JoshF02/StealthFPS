using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneSM : StateMachine
{
    public CombatState CombatState { get; private set; }
    public InvestigateState InvestigateState { get; private set; }
    public HuntState HuntState { get; private set; }
    public PatrolState PatrolState { get; private set; }
    public DisabledState DisabledState { get; private set; }

    public NavMeshAgent NmAgent { get; private set; }
    public Transform Player { get; private set; }
    public EnemyDetection Detection { get; private set; }
    public EnemyHearing Hearing { get; private set; }
    public Light Turret { get; private set; }
    public Light Spotlight { get; private set; }
    public Transform PatrolPath { get; private set; }
    public Vector3[] Waypoints { get; private set; }
    public GameObject HuntAlertObj { get; private set; }
    public GameObject CombatAlertObj { get; private set; }
    [HideInInspector] public int patrolIndex = 0;
    [HideInInspector] public bool beenShot = false;
    [HideInInspector] public float reenterCombatFromAlertCooldown = 0f;
    [SerializeField] public LayerMask laserLayerMask;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        CombatState = new CombatState(this);

        InvestigateState = new InvestigateState(this);

        HuntState = new HuntState(this);
        PatrolState = new PatrolState(this);

        DisabledState = new DisabledState(this);

        NmAgent = GetComponent<NavMeshAgent>();
        Detection = transform.GetChild(2).GetComponent<EnemyDetection>();
        Hearing = GetComponent<EnemyHearing>();
        Turret = transform.GetChild(4).GetComponent<Light>();
        Spotlight = transform.GetChild(1).GetComponent<Light>();

        HuntAlertObj = transform.GetChild(6).gameObject;
        CombatAlertObj = transform.GetChild(7).gameObject;

        PatrolPath = transform.parent.GetChild(1);

        Waypoints = new Vector3[PatrolPath.childCount];

        for (int i = 0; i < Waypoints.Length; i++) {
            Waypoints[i] = PatrolPath.GetChild(i).position;
        }
    }

    protected override BaseState GetInitialState()
    {
        return PatrolState;
    }

    void OnDrawGizmos() // visualises patrol path
    {
        PatrolPath = transform.parent.GetChild(1);
        Vector3 startPos = PatrolPath.GetChild(0).position;
        Vector3 prevPos = startPos;

        foreach(Transform waypoint in PatrolPath) {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(prevPos, waypoint.position);
            prevPos = waypoint.position;
        }
        
        Gizmos.DrawLine(prevPos, startPos);
    }
}
