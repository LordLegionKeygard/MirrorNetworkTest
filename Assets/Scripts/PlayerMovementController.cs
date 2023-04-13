using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private CharacterController _controller = null;
    private Vector2 _previousInput;
    public Vector3 MovementVector = Vector3.zero;

    [ClientCallback]
    private void Update() => Move();

    [Client]
    public void SetMovement(Vector2 movement) => _previousInput = movement;

    [Client]
    public void ResetMovement() => _previousInput = Vector2.zero;

    [Client]
    private void Move()
    {
        Vector3 right = _controller.transform.right;
        Vector3 forward = _controller.transform.forward;
        right.y = 0f;
        forward.y = 0f;

        MovementVector = right.normalized * _previousInput.x + forward.normalized * _previousInput.y;

        _controller.Move(MovementVector * _movementSpeed * Time.deltaTime);
    }
}
