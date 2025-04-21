using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityDamager : AbstractCollisionDamager
{
    [SerializeField] private float _minimalDamage;
    [SerializeField] private float _minimalDamageSpeed;

    [SerializeField] private float _maximalDamage;
    [SerializeField] private float _maximalDamageSpeed;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override float CalculateDamage(Collision collision)
    {
        var point = collision.contacts[0].point;
        var velocity = _rigidbody.GetPointVelocity(point).magnitude;

        if (velocity < _minimalDamage)
            return 0;

        velocity = Mathf.Clamp(velocity, _minimalDamageSpeed, _maximalDamageSpeed);
        var damage = velocity.RemapRaza(_minimalDamageSpeed, _maximalDamageSpeed, _minimalDamage, _maximalDamage);

        return damage;
    }
}
