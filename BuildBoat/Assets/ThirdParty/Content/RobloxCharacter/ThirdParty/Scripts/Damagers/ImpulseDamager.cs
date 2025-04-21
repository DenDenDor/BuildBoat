using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseDamager : AbstractCollisionDamager
{
    [SerializeField] private float _minimalDamage;
    [SerializeField] private float _minimalDamageImpulse;

    [SerializeField] private float _maximalDamage;
    [SerializeField] private float _maximalDamageImpulse;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override float CalculateDamage(Collision collision)
    {
        var impulse = collision.impulse.magnitude;

        if (impulse < _minimalDamage)
            return 0;

        impulse = Mathf.Clamp(impulse, _minimalDamageImpulse, _maximalDamageImpulse);
        var damage = impulse.RemapRaza(_minimalDamageImpulse, _maximalDamageImpulse, _minimalDamage, _maximalDamage);

        return damage;
    }
}
