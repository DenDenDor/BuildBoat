using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageApplier : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        var damager = collision.gameObject.GetComponent<AbstractCollisionDamager>();

        if (damager == null)
            return;

        var damage = damager.CalculateDamage(collision);

        _health.TakeDamage(damage);
    }
}
