using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _rotationSpeed = 720f;
    [SerializeField] float _maxHealth = 100;
    [SerializeField] float _currentHealth;

    [SerializeField] bool _canInteract = false;

    Vector3 _movePosition = Vector3.zero;

    [SerializeField] Rigidbody _rb;
    [SerializeField] LayerMask _layer;

    IInteractable _interactable = null;

    InteractiveObject _interactiveObject = null;

    Stack<PickableObject> _pickables = new Stack<PickableObject>();

    [SerializeField] Animator _animator = null;

    [SerializeField] Transform _pickablePos = null;

    [SerializeField] HealthBar _healthBar;

    Wallet _wallet = new Wallet();

    public Transform PickablePos { get => _pickablePos; }

    public int PickablesAmount { get => _pickables.Count; }

    public InteractiveObject InteractiveObject { get { return _interactiveObject; } }
    public float Money { get { return _wallet.Money; } }

    [SerializeField] private Transform _smearDriver;
    public float SmearDriver => (_smearDriver.localPosition.y - 0.5f) * 100;

    void Start()
    {
        //_movePosition = transform.position;
        _currentHealth = _maxHealth;
    }

    private void OnEnable()
    {
        Actions.OnItemRefund += CalculateRefund;
    }

    private void OnDisable()
    {
        Actions.OnItemRefund -= CalculateRefund;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (_canInteract)
            {
                if (_interactable != null)
                {
                    _interactable.OnInteract();
                    _animator.SetTrigger((_interactable as PickableObject).GetAnimationTrigger);
                }
                else if (_interactiveObject != null)
                {
                    _interactiveObject.OnInteract();
                }
            }        
            else
            {
                if (!CheckIfStackable())
                {
                    _pickables.Peek().OnInteract();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!CheckIfStackable())
            {
                _pickables.Peek().Drop();
                RemoveFromStack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) //For debug purposes
        {
            //AddMoney(100f);
            TakeDamage(5f);
        }

        bool hasAxe = _interactable is Axe;
        _animator.SetBool("HasAxe", hasAxe);
        _animator.SetBool("HasPickable", _pickables.Count > 0 && !hasAxe);
    }

    private void FixedUpdate()
    {
        Move();

        _rb.linearVelocity = _movePosition;
    }

    void Move()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);
        moveDirection.Normalize();

        //_movePosition = ((moveDirection * _moveSpeed) * Time.fixedDeltaTime) + _rb.position;
        _movePosition = ((moveDirection * _moveSpeed));

        if (moveDirection != Vector3.zero)
        {
            Quaternion towardsRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            _rb.rotation = Quaternion.RotateTowards(transform.rotation, towardsRotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        _animator.SetBool("Moving", moveDirection != Vector3.zero);
    }

    public void AddToStack(PickableObject pickable)
    {
        if (pickable.IsStackable)
        {
            //Check if the Stack is not empty
            if (_pickables.Count > 0)
            {
                //Check if the first object of the Stack is stackable
                if (_pickables.Peek().IsStackable)
                {
                    //If true, push it to the Stack
                    _pickables.Push(pickable);
                }//If false, do nothing
            }
            else //If stack is empty
            {
                _pickables.Push(pickable);
            }
        }
        else //If pickable is not stackable
        {
            if (_pickables.Count == 0) //Check if the Stack is empty
            {
                _pickables.Push(pickable);
            }//If false, do nothing
        }
        _interactable = _pickables.Peek();
    }

    public void RemoveFromStack()
    {
        _pickables.Pop().Drop();
        
        if (_pickables.Count == 0)
        {
            _interactable = null;
            return;
        }
        _interactable = _pickables.Peek();

        Debug.Log(_pickables.Count);
    }

    bool CheckIfStackable()
    {
        if (_pickables.Count > 0)
        {
            return _pickables.Peek().IsStackable;
        }
        return true;
    }

    public void CanInteract()
    {        
        _canInteract = !_canInteract;
    }

    public PickableObject PutDownIngredient()
    {
        PickableObject objectToPutDown = null;
        if (_pickables.Count > 0)
        {
            if (CheckIfStackable())
            {
                objectToPutDown = _pickables.Pop();
                objectToPutDown.Drop();
                return objectToPutDown;
            }
        }
        return objectToPutDown;
    }

    public void AddMoney(float moneyToAdd)
    {
        _wallet.AddMoney(moneyToAdd);
    }

    public bool BuyItem(float itemPrice)
    {
        return _wallet.TryBuyItem(itemPrice);
    }

    public void CalculateRefund(int quantity, float price)
    {
        _wallet.AddMoney(price * quantity);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        UpdateHealthBar();
    }

    public void RestoreHealth(float amount)
    {
        //_currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        _healthBar.HealthBarUpdate(_currentHealth / _maxHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            _canInteract = true;
            _interactiveObject = other.GetComponent<InteractiveObject>();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Pickable"))
        {
            _canInteract = true;

            if (_interactable == null)
            {
                _interactable = other.GetComponent<PickableObject>();
            }
            /*
            //IPickable pickable = other.GetComponent<IPickable>();
            PickableObject pickable = other.GetComponent<PickableObject>();

            if (pickable.IsStackable)
            {
                if (_pickables.Count > 0)
                {
                    if (_pickables.Peek().IsStackable)
                    {
                        _pickables.Push(pickable);
                        //Pickable goes to the hands of Player
                        pickable.PickUp();
                    }
                }
                else
                {
                    _pickables.Push(pickable);
                    //Pickable goes to the hands of Player
                    pickable.PickUp();
                }
            }
            else
            {
                if (_pickables.Count == 0)
                {
                    _pickables.Push(pickable);
                    //Pickable goes to the hands of Player
                    pickable.PickUp();
                }
            }*/
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            _interactiveObject.OnTriggerLeave();
            _interactiveObject = null;

            if (_pickables.Count == 0)
            {
                _canInteract = false;
            }
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("Pickable"))
        {
            if (_pickables.Count == 0)
            {
                _canInteract = false;
                _interactable = null;
            }
        }
    }

    public void Damage(float damage)
    {
        TakeDamage(damage);
    }
}
