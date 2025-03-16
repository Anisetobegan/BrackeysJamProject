using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    float _minAttackDistance = 2f;
    bool _enemyAlive = false;

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

    public bool EnemyAlive {  get { return _enemyAlive; } }

    void Start()
    {
        
    }

    private void OnEnable()
    {
        _enemyAlive = true;
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
                _enemyAlive = false;
                GameManager.Instance.ChangeGameState(GameManager.GameState.Normal);
                break;
        }
    }

    void Move()
    {
        Vector3 offset = _target + (transform.position - _target).normalized;
        _agent.SetDestination(offset);
    }
}
