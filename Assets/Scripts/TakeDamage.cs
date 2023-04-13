using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private ChangePlayerColor _changePlayerColor;
    private bool _canTakeDamage = true;

    private float _defaultDamageTime = 3;
    private float _currentDamageTime;

    private void OnEnable()
    {
        ResetTime();
    }

    private void ResetTime()
    {
        _currentDamageTime = _defaultDamageTime;
    }

    private void Update()
    {
        if (_canTakeDamage) return;

        _currentDamageTime -= Time.deltaTime;

        if (_currentDamageTime <= 0)
        {
            _canTakeDamage = true;
            ResetTime();
        }

    }

    public void PlayerTakeDamage()
    {
        _canTakeDamage = false;
        _changePlayerColor.SetColor();
    }

    public bool CanTakeDamage()
    {
        return _canTakeDamage;
    }
}
