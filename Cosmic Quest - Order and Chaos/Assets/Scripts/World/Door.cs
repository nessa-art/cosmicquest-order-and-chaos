﻿using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    private AudioSource _audio;
    private Animator _anim;
    private BoxCollider _col;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _col = GetComponent<BoxCollider>();
    }

    public void Open()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            StartCoroutine("OpenDoor");
        }
    }

    private IEnumerator OpenDoor()
    {
        // Play door opening audio clip
        if (_audio)
            _audio.PlayDelayed(0f);
        
        // Play door opening animation
        if (_anim)
            _anim.SetBool("UnlockDoor", true);

        // Wait until the animation completes
        yield return new WaitForSeconds(1f);
        
        // Disable the door collider
        _col.enabled = false;
    }
}
