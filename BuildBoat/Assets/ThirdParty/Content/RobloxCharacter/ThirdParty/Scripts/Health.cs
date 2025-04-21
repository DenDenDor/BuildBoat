using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{ 
    [SerializeField] private float _currentHealth;

    private float _maxHealth;
    
    private bool _isAlive = true;
    public float CurrentHealth  => _currentHealth;

    public float MaxHealth => _maxHealth;

    public bool IsAlive  => _isAlive;
    
    public System.Action LoadedNewHealth;

    public System.Action DeathTaken;
    public System.Action DamageTaken;
    public System.Action Healed;
    private Transform _transform;
    private bool _isCooldown = false;

    private void Awake()
    {
        _maxHealth = _currentHealth;
    }

    public void UpdateHealth(float health)
    {
        _maxHealth = health;
        _currentHealth = health;
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        LoadedNewHealth?.Invoke();
        _isAlive = true;
    }

    private IEnumerator Cooldown()
    {
        _isCooldown = true;
        yield return new WaitForSeconds(0.1f);
        _isCooldown = false;
    }

    [ContextMenu("Kill")]
    public void Kill()
    {
        TakeDamage(100f);
    }

    public void TakeDamage(float attackDamage)
    {
        if (_isCooldown)
        {
            return;
        }
        
        if (!_isAlive)
            return;

        if (attackDamage < 0)
        {
            Debug.Log("health. )  == )");
            attackDamage = 0;
        }

        _currentHealth -= attackDamage;

        if (_currentHealth < 0 )
            _currentHealth = 0;

        if (Mathf.Approximately( _currentHealth, 0 ))
        {
            _isAlive = false;
            DeathTaken?.Invoke();
        }
        else
        {
            DamageTaken?.Invoke();
        }

        StartCoroutine(Cooldown());
    }


    public void Heal(float health)
    {
        _currentHealth += health;
        
        if (_currentHealth >= _maxHealth)
            _currentHealth = _maxHealth;
        
        Healed?.Invoke();
    }

    public Vector3 GetPosition()
    {
        if (_transform == null)
            _transform = transform;

        return _transform.position;
    }
}
