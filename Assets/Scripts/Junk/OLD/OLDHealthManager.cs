using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDHealthManager : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 10;
    private int _health;

    private void Awake()
    {
        _health = _maxHealth;
    }

    private void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}