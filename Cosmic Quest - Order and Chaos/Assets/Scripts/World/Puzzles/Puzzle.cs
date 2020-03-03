using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public UnityEvent onCompletion;
    
    public bool IsComplete { get; private set; }

    protected void SetComplete()
    {
        IsComplete = true;
        
        // Invoke any event functions
        onCompletion?.Invoke();
    }
}
