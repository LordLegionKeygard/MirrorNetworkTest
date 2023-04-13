using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _dashForce = 30f;
    public bool IsDashing;
    public bool isTryingToDash;

    [Header("Time")]
    [SerializeField] private float _defaultDashTime;
    private float _currentDashTime;

    private void Update()
    {
        HandleDash();
    }

    public void HandleDash()
    {
        if (isTryingToDash && !IsDashing)
        {
            OnStartDash();
        }

        if (IsDashing)
        {
            _currentDashTime -= Time.deltaTime;
            _characterController.Move(transform.forward * _dashForce * Time.deltaTime);

            if (_currentDashTime <= 0) OnEndDash();
        }
    }

    private void OnStartDash()
    {
        IsDashing = true;
        _currentDashTime = _defaultDashTime;
    }

    private void OnEndDash()
    {
        IsDashing = false;
        _currentDashTime = 0;
    }
}
