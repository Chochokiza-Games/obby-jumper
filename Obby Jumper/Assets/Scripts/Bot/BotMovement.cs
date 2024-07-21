using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BotMovement : MonoBehaviour 
{
    public bool Locked 
    {
        set 
        {
            _locked = value;
            if (_locked)
            {
                _agent.enabled = false;
                StopAllCoroutines();
                _animator.PlayState(PlayerAnimator.States.Idle);
            }
            else
            {
                _agent.enabled = true;
                StartCoroutine(WalkRoutine());
            }
        }
    }

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _radius; //radius of sphere
    [SerializeField] private Transform _centrePoint;
    [SerializeField] private float _minPause;
    [SerializeField] private float _maxPause;
    [SerializeField] private PlayerAnimator _animator;

    private bool _locked;

    private Vector3 _point;

    private void OnDisable()
    {
        _agent.enabled = false;
        StopAllCoroutines();
        _animator.PlayState(PlayerAnimator.States.Idle);
    }

    private void OnEnable()
    {
        _agent.enabled = true;
        StartCoroutine(WalkRoutine());
    }

    private IEnumerator WalkRoutine()
    {
        while (true)
        {
            while (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (RandomPoint(_centrePoint.position, _radius, out _point))
                {
                    _agent.SetDestination(_point);
                    yield return null;
                }
            }
            yield return new WaitForSeconds(Random.Range(_minPause, _maxPause));
        }
    }

    private void Update()
    {
        if (_locked) 
        {
            _animator.PlayState(PlayerAnimator.States.Idle);
            return;
        }

        if (Vector3.Distance(transform.position, _point) > 0.2)
        {
            _animator.PlayState(PlayerAnimator.States.Running);
        } 
        else
        {
            _animator.PlayState(PlayerAnimator.States.Idle);
        }
    }
    
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}