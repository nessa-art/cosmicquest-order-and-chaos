using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChaosVoid : ScriptableObject, ISerializable
{
    // Reference to the scene for this chaos void
    public Object scene;
    // List of chaos voids required to be completed before starting this one
    public List<ChaosVoid> prerequisites;

    // Returns whether all the prerequisite levels have been completed
    public bool IsLocked
    {
        get
        {
            return prerequisites.Count(cv => cv.cleared) < prerequisites.Count;
        }
    }

    // Whether this chaos void has been cleared
    public bool cleared;
    // Whether this chaos void has been started
    public bool started;
    // The final boss for this chaos void
    public GameObject boss;

    public void Initialize()
    {
        // Initialize whatever...
        // Find all serializables in the level
        // Find the final boss in the scene
        started = true;
    }

    public void Initialize(string saved)
    {
        
    }

    public object Serialize()
    {
        return new ChaosVoidData(name, cleared, started);
    }

    public void FromSerialized(object data)
    {
        cleared = ((ChaosVoidData) data).cleared;
        started = ((ChaosVoidData) data).started;
    }

    // Class used for serializing data for saves
    [System.Serializable]
    public class ChaosVoidData
    {
        public string levelName;
        public bool cleared;
        public bool started;

        public ChaosVoidData(string levelName, bool cleared, bool started)
        {
            this.levelName = levelName;
            this.cleared = cleared;
            this.started = started;
        }
    }
}
