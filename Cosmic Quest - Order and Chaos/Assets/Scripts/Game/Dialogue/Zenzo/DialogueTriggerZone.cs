using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    [Tooltip("Dialogue to play when entering a zone")]
    public DialogueTrigger dialogueTrigger;

    /// <summary>
    /// Number of players in the task area
    /// </summary>
    protected int playersInZone = 0;

    /// <summary>
    /// Number of players playing the game
    /// </summary>
    protected int numPlayers;

    private bool shouldTrigger = true;

    protected virtual void Start()
    {
        numPlayers = PlayerManager.Instance.NumPlayers;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInZone += 1;
            
            // Once all players are in the zone, trigger dialogue.
            if (shouldTrigger && playersInZone == numPlayers)
            {
                dialogueTrigger.TriggerDialogue();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldTrigger = false;
        }
    }
}
