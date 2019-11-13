﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterColour
{
    None,
    Red,
    Blue,
    Green,
    Purple
}

public class EntityStatsController : MonoBehaviour
{
    // Common entity regenerable stats
    public RegenerableStat health;

    public bool isDead { get; protected set; }

    // Common base stats
    public Stat damage;
    public Stat defense;

    public CharacterColour characterColour = CharacterColour.None;

    private void Awake()
    {
        health.Init();
    }

    public virtual void TakeDamage(EntityStatsController attacker, int damageValue)
    {
        // Calculate any changes based on stats and modifiers here first
        int hitValue = damageValue - ComputeDefenseModifier();
        health.Subtract(hitValue < 0 ? 0 : hitValue);

        if (health.currentValue == 0)
        {
            Die();
        }
    }
    
    public virtual int ComputeDamageModifer()
    {
        int baseHit = Random.Range(0, damage.GetBaseValue());
        return damage.GetValue() - baseHit;
    }

    public virtual int ComputeDefenseModifier()
    {
        int baseDefense = Random.Range(0, defense.GetBaseValue());
        return defense.GetValue() - baseDefense;
    }

    protected virtual void Die()
    {
        // Meant to be implemented with any death tasks
        isDead = true;
        Debug.Log(transform.name + " died.");
    }
}
