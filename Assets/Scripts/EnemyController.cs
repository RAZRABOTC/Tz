using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private enum States
    {
        Walk, Persue
    }
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform[] _poses;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _forgetTime = 5f;
    [SerializeField] private float _stayTime = 3f;
    [SerializeField] private float _visionAngle = 45f;
    [SerializeField] private float _visionDistance = 10f;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private string _walkName = "IsWalk";
    private Transform _currentPos;
    private float _lastSeenTime;
    private States _currentState = States.Walk;
    private float _waitUntilTime;
    [SerializeField] private AnimatorBoolPlayer _doorPlayer;
    [SerializeField] private float _rayDist;
    [SerializeField] private float _closeTime;
    [SerializeField] private CutsceneManager _cutsceneManager;
    [SerializeField] private Transform _rayPoint;

    private void Start()
    {
        UpdateNewRandomPos(); 
    }

    private void Update()
    {
        if (_playerPos == null) return;
        CheckForPlayer();
        switch (_currentState)
        {
            case States.Walk:
                HandleWalkState();
                break;
            case States.Persue:
                HandlePersueState();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")))
        {
            _cutsceneManager.StartCutscene();
        }
    }

    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerPos.position);
        if (distanceToPlayer > _visionDistance) return;

        Vector3 directionToPlayer = (_playerPos.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, directionToPlayer);
        float requiredDot = Mathf.Cos(_visionAngle * 0.5f * Mathf.Deg2Rad);

        if (dot > requiredDot)
        {
            RaycastHit hit;
            if (Physics.Raycast(_rayPoint.position, directionToPlayer, out hit, _visionDistance))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Player"))
                {
                    _currentState = States.Persue;
                    _lastSeenTime = Time.time;
                }
            }
        }
    }

    private void HandleWalkState()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (_anim.GetBool(_walkName))
            {
                _anim.SetBool(_walkName, false);
                _waitUntilTime = Time.time + _stayTime;
            }

            if (Time.time >= _waitUntilTime)
            {
                UpdateNewRandomPos();
                _anim.SetBool(_walkName, true);
            }
        }
    }

    private void HandlePersueState()
    {
        _anim.SetBool(_walkName, true);
        _agent.SetDestination(_playerPos.position);

        if (Time.time - _lastSeenTime >= _forgetTime)
        {
            _currentState = States.Walk;
            UpdateNewRandomPos();
        }
        else
        {
            CheckForPlayer();
        }
    }

    private void UpdateNewRandomPos()
    {
        if (_poses.Length == 0) return;
        Transform newPos = _poses[UnityEngine.Random.Range(0, _poses.Length)];
        _currentPos = newPos;
        _agent.SetDestination(_currentPos.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _visionDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -_visionAngle * 0.5f, 0) * transform.forward * _visionDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, _visionAngle * 0.5f, 0) * transform.forward * _visionDistance;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
        Gizmos.DrawRay(_rayPoint.position, transform.forward);
        Gizmos.DrawLine(transform.position + leftBoundary, transform.position + rightBoundary);
    }
}
