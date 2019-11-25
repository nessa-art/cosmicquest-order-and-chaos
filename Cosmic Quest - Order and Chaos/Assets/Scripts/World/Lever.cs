using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// Class for player-lever interaction
public class Lever : Interactable
{
    public Animator Anim;
    protected StringBuilder LeverInput; // Order of lever input to unlock room.

    private void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
        //LeverInput = gameObject.GetComponent<Lvl1Room1>().LeverInput;
    }

    private void Reset()
    {
        // Set default value for interactable fields
        isTrigger = true;
    }
    
    public override void StartInteract(Transform target)
    {
        if (CanInteract(target))
        {
            Debug.Log("Interacted with " + target.name);
            Anim.enabled = true;
            Anim.Play("LeverAnimation");
            Anim.SetBool("LeverPulled", true);

            switch (colour)
            {
                case (CharacterColour.Green):
                    LeverInput.Append("g");
                    break;
                case (CharacterColour.Purple):
                    LeverInput.Append("p");
                    break;
                case (CharacterColour.Yellow):
                    LeverInput.Append("y");
                    break;
                case (CharacterColour.Red):
                    LeverInput.Append("r");
                    break;
                default:
                    break;
            }
        }
    }

    void PauseAnimationEvent ()
    {
        Anim.enabled = false;
    }
}
