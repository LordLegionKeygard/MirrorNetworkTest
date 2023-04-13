using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInputController : NetworkBehaviour
{
    [SerializeField] private DealDamageController _dealDamageController;
    [SerializeField] private PlayerCameraController _playerCameraController;
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private DashController _dashController;

    public override void OnStartAuthority()
    {
        _playerCameraController.PrepareCamera();
        _playerMovementController.enabled = true;

        InputManager.Controls.Player.Attack.performed += ctx => _dealDamageController.CmdAttack();
        InputManager.Controls.Player.Look.performed += ctx => _playerCameraController.Look(ctx.ReadValue<Vector2>());
        InputManager.Controls.Player.Attack.performed += ctx => _dashController.isTryingToDash = true;
        InputManager.Controls.Player.Attack.canceled += ctx => _dashController.isTryingToDash = false;

        InputManager.Controls.Player.Move.performed += ctx => _playerMovementController.SetMovement(ctx.ReadValue<Vector2>());
        InputManager.Controls.Player.Move.canceled += ctx => _playerMovementController.ResetMovement();
    }

    [ClientCallback]
    private void OnEnable() => InputManager.Controls.Enable();

    [ClientCallback]
    private void OnDisable() => InputManager.Controls.Disable();
}
