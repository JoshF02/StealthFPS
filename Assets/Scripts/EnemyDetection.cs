using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public bool DetectingSuspicious { get; private set; } = false;
    public bool DetectingDecoy { get; private set; } = false;
    public Transform SuspicousObject { get; private set; } = null;
    public Transform Decoy { get; private set; } = null;
    [SerializeField] private LayerMask _detectionLayerMask;
    private readonly float _viewDistance = 10.0f;
    private readonly float _viewAngle = 32;
    private Transform _playerTransform = null;

    public bool GetDetectingPlayer()
    {
        if (_playerTransform == null || _playerTransform.GetComponent<PlayerActions>().InvisPerkActive) return false;
        else if (Vector3.Distance(transform.position, _playerTransform.position) < 3.0f) return true; // edit value / remove / keep in mind, can detect through thin objs
        else return CanSeeObject(_playerTransform);
    }
    public bool GetDetectingTarget(bool targetIsPlayer)
    {
        if (targetIsPlayer) return GetDetectingPlayer();
        else return DetectingDecoy;
    }
    public void StartDetectingSuspicious(Transform target)
    {
        SuspicousObject = target;
        DetectingSuspicious = true;
    }
    public void StopDetectingSuspicious(bool shouldDisableBool = false)
    {
        SuspicousObject = null;
        if (shouldDisableBool) DetectingSuspicious = false;
    }
    public void StopDetectingDecoy(bool shouldDisableBool = false)
    {
        Decoy = null;
        if (shouldDisableBool) DetectingDecoy = false;
    }

    public void SetMoreAware()  // initial radius should be more aware size
    { 
        Debug.Log("more aware");
        //GetComponent<SphereCollider>().radius *= 2;
        //GetComponent<BoxCollider>().size *= 2;
    }
    public void SetLessAware()
    { 
        Debug.Log("less aware");
        //GetComponent<SphereCollider>().radius *= 0.5f;
        //GetComponent<BoxCollider>().size *= 0.5f;
    } 

    private void OnTriggerEnter(Collider other) // only objects with rigidbodies set off triggers
    {                                   
        if (other.name == "Player")
        {
            _playerTransform = other.transform;
        }
        else if ((other.tag == "Suspicious") && !DetectingSuspicious)
        {
            DetectingSuspicious = CanSeeObject(other.transform);
            if (DetectingSuspicious) SuspicousObject = other.transform;
        }
        else if (other.tag == "Decoy")
        {
            DetectingDecoy = CanSeeObject(other.transform);
            if (DetectingDecoy) Decoy = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            _playerTransform = other.transform;
        }
        else if ((other.tag == "Suspicious") && !DetectingSuspicious)
        {
            DetectingSuspicious = CanSeeObject(other.transform);
            if (DetectingSuspicious) SuspicousObject = other.transform;
        }
        else if (other.tag == "Decoy")
        {
            DetectingDecoy = CanSeeObject(other.transform);
            if (DetectingDecoy) Decoy = other.transform;
        }
    }

    private bool CanSeeObject(Transform obj)    // could change to detection meter filling up, rather than instant
    {
        if (Vector3.Distance(transform.position, obj.position) > _viewDistance) return false;    // out of range

        Vector3 dir = (obj.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);

        if (angle > _viewAngle) return false; // out of view cone

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, _viewDistance, _detectionLayerMask) && (hit.collider.gameObject.name != obj.name)) return false; // obstructed

        return true;    
    }

    private void OnDrawGizmos() // shows viewcone lines
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.parent.forward * _viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _viewAngle, 0) * transform.parent.forward * _viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -_viewAngle, 0) * transform.parent.forward * _viewDistance);
    }
}
