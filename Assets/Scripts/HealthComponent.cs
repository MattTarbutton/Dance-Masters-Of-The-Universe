using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int maxHealth;
    public bool invulnerable;
    private int currentHealth;
    private float invulnerableTime;

    public event EventHandler OnHealthChanged;

	// Use this for initialization
	void Start ()
    {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (invulnerable)
        {
            invulnerableTime -= Time.deltaTime;
            
            invulnerable = invulnerableTime > 0;
        }
	}

    public int GetHealth()
    {
        return currentHealth;
    }

    public void Damage(GameObject damager, int damageAmount, bool activateHitStun)
    {
        currentHealth -= damageAmount;
        HealthChanged();
        
        ActorController c = this.GetComponent<ActorController>();
        
        if (currentHealth <= 0)
        {
            c.DestroyByDamage(damager.transform.position);
        }
        else
        {
            c.OnTakeDamage(damager.transform.position, activateHitStun);
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        HealthChanged();

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    private void HealthChanged()
    {
        if (OnHealthChanged != null)
            OnHealthChanged(this, null);
    }

    public void SetInvulnerable(float duration)
    {
        invulnerable = true;
        invulnerableTime = duration;
    }
}
