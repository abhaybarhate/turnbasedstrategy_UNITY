using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnDie;

    [SerializeField] private float health = 100f;

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if(health <= 0 )
        {
            health = 0;
            Die();
        }

        Debug.Log(health);
        
    }

    private void Die()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
    }

}
