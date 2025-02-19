using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    float _moveSpeed = 5f;
    float _rotationSpeed = 720f;

    Vector3 _movePosition = Vector3.zero;

    [SerializeField] Rigidbody _rb;

    void Start()
    {
        _movePosition = transform.position;
    }

    void Update()
    {
        Move();
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
    }
}
