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

    void Start()
    {
        _movePosition = transform.position;
    }

    void Update()
    {
        Move();

        if (Input.GetMouseButtonDown(0) && _canInteract)
        {
            if (_interactable != null)
            {
                if (_pickables.Count > 0)
                {
                    if (CheckIfStackable())
                    {
                        _interactable.OnInteract();
                    }
                }
                else
                {
                    _interactable.OnInteract();
                }                
            }
        }
        else if ( Input.GetMouseButtonDown(0) && !_canInteract)
        {
            if (!CheckIfStackable())
            {
                _pickables.Peek().Drop();
                RemoveFromStack(_pickables.Peek());
            }
        }
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
        _interactable = pickable;
    }

    public void RemoveFromStack(PickableObject pickable)
    {
        _pickables.Pop();
        pickable.Drop();
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
        _interactable = null;
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
        _canInteract = false;
        _interactable = null;
    }
}
