using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevitationMagic))]
public class LevitationMagicForEnemyBodyModificator : MonoBehaviour
{
    private LevitationMagic _levitationMagic;

    private void Start()
    {
        _levitationMagic = GetComponent<LevitationMagic>();
    }

    [SerializeField] private float _modifier;

    public void PowerForceIfGrabbedIsEnemy(Rigidbody rigidbody)
    {
        if (rigidbody.transform.root.TryGetComponent(out Animator enemy))
        {
            _levitationMagic.PowerForce(_modifier);
        }
    }

    public void UnPowerForceIfDropedEnemy(Rigidbody rigidbody)
    {
        if (rigidbody.transform.root.TryGetComponent(out Animator enemy))
        {
            _levitationMagic.PowerForce(1 / _modifier);
        }
    }
}
