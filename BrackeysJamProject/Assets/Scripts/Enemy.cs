using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour, IDamagable
{
    float _minAttackDistance = 2f;
    float _damage = 100f;
    bool _enemyAlive = false;

    [SerializeField] float _maxHealth = 100;
    [SerializeField] float _currentHealth;

    [SerializeField] NavMeshAgent _agent;

    [SerializeField] Hitbox _attackHitbox;

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
        _currentHealth = _maxHealth;
        _attackHitbox.InitializeHitbox(_damage);
    }

    void Update()
    {
        if (_currentHealth <= 0)
        {
            _state = State.Dead;
        }

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
                StartCoroutine(ActivateAttackHitbox());
                _state = State.Waiting;

                break;

            case State.Waiting:
                break;

            case State.Dead:
                _enemyAlive = false;
                GameManager.Instance.ChangeGameState(GameManager.GameState.Normal);
                Destroy(gameObject);
                break;
        }
    }

    void Move()
    {
        Vector3 offset = _target + (transform.position - _target).normalized;
        _agent.SetDestination(offset);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }
        //Apply knockback
    }

    public void Damage(float damage)
    {
        TakeDamage(damage);
    }

    IEnumerator ActivateAttackHitbox()
    {
        _attackHitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _attackHitbox.gameObject.SetActive(false);
    }
}
