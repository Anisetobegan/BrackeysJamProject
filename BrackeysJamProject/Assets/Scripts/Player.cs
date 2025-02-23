using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _rotationSpeed = 720f;

    [SerializeField] bool _canInteract = false;

    Vector3 _movePosition = Vector3.zero;

    [SerializeField] Rigidbody _rb;
    [SerializeField] LayerMask _layer;

    IInteractable _interactable = null;

    InteractiveObject _interactiveObject = null;

    Stack<PickableObject> _pickables = new Stack<PickableObject>();

    [SerializeField] Animator _animator = null;

    [SerializeField] Transform _pickablePos = null;
    public Transform PickablePos { get => _pickablePos; }

    public int PickablesAmount { get => _pickables.Count; }

    public InteractiveObject InteractiveObject { get { return _interactiveObject; } }

    void Start()
    {
        _movePosition = transform.position;
    }

    void Update()
    {
        Move();

        if (Input.GetMouseButtonDown(0))
        {
            if (_canInteract)
            {
                if (_interactable != null)
                {
                    _interactable.OnInteract();
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
                    _pickables.Peek().Drop();
                    RemoveFromStack();
                }
            }
        }

        _animator.SetBool("HasPickable", _pickables.Count > 0);
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_movePosition);
    }

    void Move()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);
        moveDirection.Normalize();

        _movePosition = ((moveDirection * _moveSpeed) * Time.fixedDeltaTime) + _rb.position;

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
            _interactable = other.GetComponent<PickableObject>();
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
            _canInteract = false;
            _interactiveObject.OnTriggerLeave();
            _interactiveObject = null;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Pickable"))
        {
            _canInteract = false;
            _interactable = null;
        }
    }
}
