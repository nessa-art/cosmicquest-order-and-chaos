﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMedicCombatController : PlayerCombatController
{
    [Header("Primary Attack")]
    [Tooltip("The maximum range the player's melee attack can reach")]
    public float primaryAttackRadius = 2f;
    [Tooltip("The angular distance around the player where enemies are affected by the primary attack")]
    public float primaryAttackAngle = 100f;
    
    [Header("Secondary Attack")]
    [Tooltip("The force to launch the healing projectile at")]
    public float secondaryAttackLaunchForce = 500f;
    [Tooltip("The range which the healing projectile can travel")]
    public float secondaryAttackRange = 20f;
    [Tooltip("The prefab for the healing projectile")]
    public GameObject projectilePrefab;
    
    protected override void PrimaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = primaryAttackCooldown;
        
        // Check all enemies within attack radius of the player
        List<Transform> enemies = GetSurroundingEnemies(primaryAttackRadius);
        
        // Attack any enemies within the attack sweep and range
        foreach (var enemy in enemies.Where(enemy => CanDamageTarget(enemy, primaryAttackRadius, primaryAttackAngle)))
        {
            // TODO can this attack affect multiple enemies?
            // Calculate and perform damage
            StartCoroutine(PerformDamage(enemy.GetComponent<EntityStatsController>(), Stats.ComputeDamageModifer(), 0.6f));
        }
        
        // Primary attack animation
        Anim.SetTrigger("PrimaryAttack");
    }
    
    protected override void SecondaryAttack()
    {
        if (AttackCooldown > 0)
            return;

        AttackCooldown = secondaryAttackCooldown;
        
        // Launch projectile in the direction the player is facing
        StartCoroutine(LaunchProjectile(projectilePrefab, transform.forward, secondaryAttackLaunchForce, secondaryAttackRange, 0.5f));
        
        // Launch orb animation
        Anim.SetTrigger("SecondaryAttack");
    }
    
    protected override void UltimateAbility()
    {
        // TODO implement melee class ultimate ability
    }
}
