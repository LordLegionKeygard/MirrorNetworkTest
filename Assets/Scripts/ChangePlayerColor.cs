using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChangePlayerColor : NetworkBehaviour
{
    [SerializeField] private Material _cachedMaterial;
    [SerializeField] private MeshRenderer _meshRend;
    [SerializeField] private float _damageTime = 3;

    private void OnEnable()
    {
        _cachedMaterial = _meshRend.material;
    }

    public void SetColor()
    {
        if (_cachedMaterial == null) _cachedMaterial = _meshRend.material;
        _cachedMaterial.color = Color.red;
        Invoke(nameof(ReturnColor), _damageTime);
    }

    private void ReturnColor()
    {
        _cachedMaterial.color = Color.white;
    }

    private void OnDestroy()
    {
        Destroy(_cachedMaterial);
    }
}
