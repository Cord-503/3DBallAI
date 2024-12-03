using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMoving : MonoBehaviour
{
    private NavMeshAgent _navAgent;
    private float _distanceToCurrentWaypoint;
    private float _distanceToTarget;

    private int _currentWaypointIndex;
    private Vector3 _enemyPosition, _currentWaypointPosition;

    [SerializeField] private Transform[] _patrolWaypoints;
    [SerializeField] private Transform _player;
    [SerializeField] private float _exitDistance;

    [SerializeField] private bool _alwaysChase;
    [SerializeField] private bool _alwaysPatrol;


    private AiState _currentAiState;
    private Transform _detectedPlayer;

    public enum AiState
    {
        Idle = 0,
        Patrolling = 1,
        Chasing = 2
    }

    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();

        _currentAiState = AiState.Idle;

        if (_patrolWaypoints.Length > 0)
        {
            _currentAiState = AiState.Patrolling;
            _navAgent.SetDestination(_patrolWaypoints[0].position);
            _currentWaypointIndex = 0;
        }

    }

    void Update()
    {
        if (_alwaysChase)
        {
            ChaseBehavior();
            return;
        }

        else if (_alwaysPatrol)
        {
            PatrolBehavior();
            return;
        }

        if (_currentAiState == AiState.Patrolling)
        {
            PatrolBehavior();
        }
        else if (_currentAiState == AiState.Chasing)
        {
            ChaseBehavior();
        }
    }

    private void PatrolBehavior()
    {
        if (!_navAgent.isOnNavMesh || !_navAgent.isActiveAndEnabled) return;

        _enemyPosition = new Vector3(transform.position.x, _patrolWaypoints[0].position.y, transform.position.z);

        _currentWaypointPosition = _patrolWaypoints[_currentWaypointIndex].position;

        _distanceToCurrentWaypoint = Vector3.Distance(_enemyPosition, _currentWaypointPosition);

        if (_distanceToCurrentWaypoint <= 0.1f)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _patrolWaypoints.Length;
        }

        _navAgent.SetDestination(_patrolWaypoints[_currentWaypointIndex].position);
    }

    private void ChaseBehavior()
    {
        if (!_navAgent.isOnNavMesh || !_navAgent.isActiveAndEnabled) return;

        if (_alwaysChase == false && _detectedPlayer == null)
        {   
            _navAgent.SetDestination(_detectedPlayer.position);
            _distanceToTarget = Vector3.Distance(_detectedPlayer.position, transform.position);
            // back if player is out of exit distance
            if (_distanceToTarget >= _exitDistance && _patrolWaypoints.Length > 0)
            {
                _currentAiState = AiState.Patrolling;
            }
            else if (_distanceToTarget >= _exitDistance && _patrolWaypoints.Length == 0)
            {
                _currentAiState = AiState.Idle;
            }
        }

        else
        {
            _navAgent.SetDestination(_player.position);
        }
    }

    public void DetectPlayer(Transform playerTransform)
    {
        _detectedPlayer = playerTransform;
        _currentAiState = AiState.Chasing;
    }
}
