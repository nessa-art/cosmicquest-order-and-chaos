using UnityEngine;

/// <summary>
/// Class for items which are collected on interact
/// </summary>
public class Collectable : Interactable
{
    // TODO will need a generalized class for including power-ups
    public Equipment item;
    
    /// <summary>
    /// Handles the start of an interaction event with a player
    /// </summary>
    /// <param name="target">The Transform who interacted with this object</param>
    public override void StartInteract(Transform target)
    {
        if (CanInteract(target))
        {
            // Player attempts to pick up the item
            bool pickedUp = target.GetComponent<PlayerStatsController>().inventory.Add(item);
            
            // Remove item from scene if successfully picked up
            if (pickedUp)
                Destroy(gameObject);
        }
    }
}