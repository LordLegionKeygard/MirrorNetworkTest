using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    [SerializeField] private ScoreController _scoreController;

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<TakeDamage>(out TakeDamage takeDamage))
        {
            if(!takeDamage.CanTakeDamage()) return;
            
            _scoreController.ChangeScore();
            takeDamage.PlayerTakeDamage();
        }
    }
}
