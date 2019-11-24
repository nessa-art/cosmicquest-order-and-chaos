﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

// Base class for Chaos Void rooms
public class Room : MonoBehaviour
{
    public Animator Anim;           // Door animation to be played when all puzzles have been solved
    protected Collider m_Collider;  // Collider of the door
   
    protected List<GameObject> m_Enemies; // All enemies in the room
   
    protected GameObject[] m_Platforms; // All rock platforms in the room

    protected GameObject[] m_Levers;    // All levers in the room
    protected string m_LeverOrder;        // Hard-coded lever pattern to unlock the room.
    public StringBuilder LeverInput;  // Pattern of lever input from players. 

    void Awake ()
    {
        // Track all rock platforms in the room
        m_Platforms = GameObject.FindGameObjectsWithTag("Platform");

        // Track all levers in the room
        m_Levers = GameObject.FindGameObjectsWithTag("Lever");

        // Populate enemy list with enemies in the room
        m_Enemies = new List<GameObject>();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            m_Enemies.Add(enemy);
        }

        m_Collider = GetComponent<Collider>();
        Anim = GetComponent<Animator>();
    }

    // Returns whether all rocks have been positioned on their respective platforms
    public virtual bool ArePlatformsActivated ()
    {
        bool platformsActivated = true;

        if (m_Platforms == null || m_Platforms.Length == 0) return true;

        // Check if every platform in the room has a rock placed on it
        foreach (GameObject plat in m_Platforms)
        {
            if (!plat.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlatformActivated"))
            {
                // If a platform hasn't been activated yet, return false
                platformsActivated = false;
                break;
            }
        }

        return platformsActivated;
    }

    // Returns whether all levers in the room have been pulled
    public virtual bool AreLeversPulled ()
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
                break;
            }
        }

        return leversPulled;
    }

    // Returns whether all enemies in the room have been killed
    public virtual bool AreAllEnemiesKilled ()
    {
        // If enemy list is empty, all enemies in the room have been killed
        if (m_Enemies.Count == 0)
        {   
            return true;
        }

        // Check for dead enemies and remove them from the enemy list
        foreach (GameObject enemy in m_Enemies)
        {
            if (enemy != null && enemy.GetComponent<EnemyStatsController>().isDead == true)
            {
                m_Enemies.Remove(enemy);
            }
        }

        return false;
    }

    public virtual void PauseDoorAnimEvent()
    {
        Anim.enabled = false;
    }

    public IEnumerator SetDoorAnimTrigger ()
    {

        yield return new WaitForSeconds(1);
        Anim.SetTrigger("OpenDoor");
        m_Collider.enabled = false;
        yield break;
    }
}
