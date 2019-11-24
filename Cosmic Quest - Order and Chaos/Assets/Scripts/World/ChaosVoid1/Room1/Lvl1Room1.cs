using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Lvl1Room1 : Room
{

    private void Start()
    {
        m_LeverOrder = "pggp";
        LeverInput = new StringBuilder("", 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.ArePlatformsActivated())
        {
            Anim.SetBool("IsRevealed", true);
        }

        if (this.AreLeversPulled())
        {
            Debug.Log("Levers pulled in correct order - open door");
            StartCoroutine(SetDoorAnimTrigger());
            
            // This script is no longer needed. Deactivate to reduce impact on performance.
            enabled = false;
        }
    }

    // Overriden: Returns whether levers have been pulled in the correct order
    public override bool AreLeversPulled()
    {
        Debug.Log(LeverInput.ToString());
        Debug.Log(LeverInput.ToString() + " VS " + m_LeverOrder);
        if ((LeverInput.Length == 4) && (m_LeverOrder == LeverInput.ToString()))
        {
            return true;
        }

        return false;
    }
}
