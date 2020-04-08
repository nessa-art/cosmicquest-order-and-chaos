using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class MashActionPuzzle : ActionPuzzle
{
    [Tooltip("Number of mash actions needed to be performed")]
    [SerializeField] protected int numActionsRequired;
    [Tooltip("Image to fill up as players mash their button(s)")]
    [SerializeField] private Image mashProgressBar;
    
    private int totalNumActions = 0;

    /// <summary>
    /// Set up the puzzle
    /// </summary>
    protected override void Setup()
    {
        totalNumActions = 0;
        // empty the dictionary of device ids that have completed the actions
        completedActions.Clear();
        for (int i = 0; i < playerInputs.Count; i++)
        {
            ReadOnlyArray<InputAction> inputActions = playerInputs[i].currentActionMap.actions;
            // Disable all required actions
            for (int j = 0; j < requiredActions.Length; j++)
            {
                InputAction action = playerInputs[i].currentActionMap.actions.First(e => e.name == requiredActions[j].actionName);
                action.Disable();
            }
        }
    }

    /// <summary>
    /// Listen for events from the player action map
    /// </summary>
    public override void ListenForActions()
    {
        EnableOverlay();
        requiredActions[currentAction].actionImage.enabled = true;
        // Listen for the currently required action being triggered on each player's input
        for (int i = 0; i < playerInputs.Count; i++)
        {
            int deviceId = playerInputs[i].devices.FirstOrDefault().deviceId;
            ActionTriggeredEvent actionListenerEvent = new ActionTriggeredEvent(deviceId, requiredActions[currentAction].actionName, playerInputs[i].currentActionMap);
            actionListenerEvent.AddListener(CompleteAction);
        }
    }

    /// <summary>
    /// Registers that a device with `deviceId` has completed the currently required action
    /// </summary>
    /// <param name="deviceId">Id of the device that completed the action</param>
    protected override void CompleteAction(int deviceId)
    {
        // increment the total number of mashes by each player
        if (completedActions.ContainsKey(deviceId))
        {
            completedActions[deviceId] += 1;
        }
        else
        {
            completedActions.Add(deviceId, 1);
        }

        totalNumActions += 1;

        // fill up the progress bar
        mashProgressBar.fillAmount = totalNumActions / numActionsRequired;

        if (completedActions[deviceId] > numActionsRequired)
        {
            requiredActions[currentAction].SetComplete();
        }

        // Complete if all players have performed the currently required action
        if (completedActions.Count == playerInputs.Count)
        {
            StopListeningForActions();
            SetComplete();
        }
    }
}
