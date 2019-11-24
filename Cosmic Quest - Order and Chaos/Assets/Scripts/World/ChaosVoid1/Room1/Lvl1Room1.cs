using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1Room1 : Room
{
    public List<int> m_LeverOrder;

    // Update is called once per frame
    void Update()
    {
        if (this.ArePlatformsActivated())
        {
            Debug.Log("All platforms activated - Show hint");
            Anim.SetBool("IsRevealed", true);
        }
    }

    // Overriden: Returns whether levers have been pulled in the correct order
    public override bool AreLeversPulled()
    {
        bool leversPulled = true;

        if (m_Levers == null || m_Levers.Length == 0) return true;

        // Check if every lever has been activated
        foreach (GameObject lever in m_Levers)
        {
            Transform handle = lever.transform.Find("Handle");

            if (!handle.GetComponent<Animator>().GetBool("LeverPulled"))
            {
                // If at least 1 lever isn't activated, return false
                leversPulled = false;
            }
        }

        return leversPulled;
    }
}
