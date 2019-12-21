﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1Room1 : Room
{   
    private Animator letterReveal;

    private void Start()
    {
        // TODO: Implement random generator for lever code patterns based on input of code length and active player colours
        code = new List<CharacterColour>(new CharacterColour[] {CharacterColour.Purple, CharacterColour.Green, CharacterColour.Green, CharacterColour.Purple});
        input = new List<CharacterColour>();
        letterReveal = transform.parent.Find("ActivatedLetter").gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (AreLeversPulled())
        {
            StartCoroutine(SetAnimTrigger());
            audioClip.Play(0);

            

            // Only need to trigger door animation once. Disable to reduce further impact on performance.
            enabled = false;
        }
    }

    // Returns whether all levers in the room have been pulled
    public override bool AreLeversPulled()
    {
        // Clear input on failed tries
        if (input.Count > code.Count) input.Clear();
        if (input.Count != code.Count) return false;

        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] != code[i]) 
                return false;
        }

        return true;
    }   

}
