using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    float _minAttackDistance = 2f;

    [SerializeField] NavMeshAgent _agent;

    Vector3 _target;

    enum State
    {
        Chasing,
        Attacking,
        Waiting,
        Dead
    }
    [SerializeField] State _state;

    IEnumerator _enumerator = null;

    void Start()
    {
        
    }

    void Update()
    {
        switch (_state)
        {
            case State.Chasing:

                _target = GameManager.Instance.PlayerGet.transform.position;
                Move();

                float distance = Vector3.Distance(transform.position, _target);

                if (distance < _minAttackDistance)
                {
                    _state = State.Attacking;
                }

                break;

            case State.Attacking:

                Debug.Log("Player Attacked");
                _state = State.Waiting;

                break;

            case State.Waiting:
                break;

            case State.Dead:
                break;
        }
    }

    void Move()
    {
        Vector3 offset = _target + (transform.position - _target).normalized;
        _agent.SetDestination(offset);
    }
}
