using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnDie;
    public event EventHandler OnDamage;

    [SerializeField] private float health = 100f;
    private float healthMax;

    private void Start()
    {
        healthMax = health;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if(health <= 0 )
        {
            health = 0;
            
        }
        OnDamage?.Invoke(this, EventArgs.Empty);
        if(health == 0 ) Die();
        Debug.Log(health);
        
    }

    private void Die()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return health / healthMax;
    }

}
