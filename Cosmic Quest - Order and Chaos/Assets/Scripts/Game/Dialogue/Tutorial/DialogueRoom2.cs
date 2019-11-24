using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueRoom2 : MonoBehaviour
{
    Animator anim; // Leol animation
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("EnterRoom");
    }

}
