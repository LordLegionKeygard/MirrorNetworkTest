using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DealDamageController : NetworkBehaviour
{
    [SerializeField] private Collider _col;


    [Command]
    public void CmdAttack()
    {
        RpcAttack();
    }

    [ClientRpc]
    private void RpcAttack()
    {
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        _col.enabled = true;
        yield return new WaitForSeconds(1f);
        _col.enabled = false;
    }
}
