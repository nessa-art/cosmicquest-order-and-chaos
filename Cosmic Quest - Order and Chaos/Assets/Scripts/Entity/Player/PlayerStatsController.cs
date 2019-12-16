using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStatsController : EntityStatsController
{
    // Player specific stats
    public RegenerableStat mana;

    // Player inventory
    public Inventory inventory;
    private Equipment _currentWeapon;
    
    // player collider
    private Collider _collider;

    // ragdoll collider
    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidbodies;

    protected override void Awake()
    {
        base.Awake();
        mana.Init();

        // get the collider attached to the player
        _collider = GetComponent<Collider>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        EnableRagdoll(false);
        Anim.enabled = true;
    }

    private void Start()
    {
        Color playerColour = PlayerManager.colours.GetColour(characterColour);

        // colour the player's weapon
        AssignWeaponColour(gameObject, playerColour);

        // Create a VFX where the player will spawn - just slightly above the stage (0.1f) - and change the VFX colour to match the player colour
        StartCoroutine(VfxHelper.CreateVFX(spawnVFX, transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity, playerColour, 0.5f));
        // "Spawn" the player (they float up through the stage)
        StartCoroutine(Spawn(gameObject, spawnSpeed, spawnDelay, spawnCooldown));
    }

    protected override void Update()
    {
        base.Update();

        if (!isDead)
            mana.Regen();
    }

    protected override void Die()
    {
        Debug.Log(transform.name + " died.");
        isDead = true;
        Anim.enabled = false;
        EnableRagdoll(true);
        StartCoroutine(PlayerDeath());
        StartCoroutine(AudioHelper.PlayAudioOverlap(VocalAudio, entityDeathVocalSFX));
    }

    private IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(5.5f);
        transform.gameObject.SetActive(false);
    }

    private void EnableRagdoll(bool enable)
    {
        foreach (Rigidbody rrb in ragdollRigidbodies)
        {
            rrb.isKinematic = !enable;
        }
        rb.isKinematic = enable;

        foreach (Collider rcol in ragdollColliders)
        {
            rcol.enabled = enable;
        }
        _collider.enabled = !enable;
    }

    private void AssignWeaponColour(GameObject player, Color color)
    {
        // Get the player weapon
        Transform[] children = player.GetComponentsInChildren<Transform>();
        GameObject weapon = null;

        foreach (var child in children)
        {
            if (child.CompareTag("Weapon"))
            {
                weapon = child.gameObject;
                break;
            }
        }

        // Dynamically assign player weapon colours
        if (weapon != null)
        {
            Transform[] weaponComponents = weapon.GetComponentsInChildren<Transform>();
            float intensity = 2.0f;
            foreach (Transform weaponComponent in weaponComponents)
            {
                if (weaponComponent.CompareTag("Weapon Glow"))
                {
                    Material[] weaponMaterials = weaponComponent.GetComponent<Renderer>().materials;
                    // the bow has more than 1 material assigned to one of its weapon parts
                    foreach (Material m in weaponMaterials)
                    {
                        m.EnableKeyword("_EMISSION");
                        m.SetColor("_Color", color);
                        m.SetColor("_EmissionColor", color * intensity);
                    }
                }
            }
        }
    }

    protected override IEnumerator Spawn(GameObject obj, float speed = 0.05f, float delay = 0f, float cooldown = 0)
    {
        PlayerMotorController motorController = GetComponent<PlayerMotorController>();
        motorController.ApplyMovementModifier(0);
        yield return base.Spawn(obj, speed, delay, cooldown);
        motorController.ResetMovementModifier();
    }
    
    // TODO should we use an equipment manager system instead?
    private void SetCurrentWeapon(Equipment weapon)
    {
        if (_currentWeapon != null)
        {
            damage.RemoveModifier(_currentWeapon.damageModifier);
            defense.RemoveModifier(_currentWeapon.defenseModifer);
        }
        damage.AddModifier(weapon.damageModifier);
        defense.AddModifier(weapon.defenseModifer);
        _currentWeapon = weapon;
    }

    private void OnCycleNextWeapon(InputValue value)
    {
        if (value.isPressed)
        {
            Equipment nextWeapon = inventory.GetNextItem();
            
            if (nextWeapon != null)
                SetCurrentWeapon(nextWeapon);
        }
    }
    
    private void OnCyclePreviousWeapon(InputValue value)
    {
        if (value.isPressed)
        {
            Equipment prevWeapon = inventory.GetPreviousItem();
            
            if (prevWeapon != null)
                SetCurrentWeapon(prevWeapon);
        }
    }
}