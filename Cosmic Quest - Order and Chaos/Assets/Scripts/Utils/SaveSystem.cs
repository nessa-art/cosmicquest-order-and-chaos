using System;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    /// <summary>
    /// Creates a game save with the specified save number, overwriting
    /// any existing saves with that number
    /// </summary>
    /// <param name="saveNumber">The save number of the save</param>
    public static void SaveToSlot(int saveNumber)
    {
        SaveObject saveData = SerializeToSaveObject();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(GetSavePath(saveNumber), FileMode.Create);
        
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    /// <summary>
    /// Loads game data from a specific save file, identified by its save number.
    /// </summary>
    /// <param name="saveNumber">The save number to load</param>
    public static void LoadFromSlot(int saveNumber)
    {
        string savePath = GetSavePath(saveNumber);
        if (!File.Exists(savePath))
        {
            Debug.LogError("Save file not found in " + savePath);
            return;
        }
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Open);
        
        SaveObject saveData = formatter.Deserialize(stream) as SaveObject;
        stream.Close();
        
        DeserializeFromSaveObject(saveData);
    }

    /// <summary>
    /// Gets the file path to a specific save.
    /// </summary>
    /// <param name="saveNumber">The save number of the save file</param>
    /// <returns>The file path to the specific save</returns>
    private static string GetSavePath(int saveNumber)
    {
        return Path.Combine(Application.persistentDataPath, "saves", "savegame-" + saveNumber + ".dat");
    }

    /// <summary>
    /// Serializes the current game to a SaveObject for serialization later.
    /// </summary>
    /// <returns>The SaveObject to serialize to a save file</returns>
    private static SaveObject SerializeToSaveObject()
    {
        SaveObject saveObject = new SaveObject();

        saveObject.playerManagerData = PlayerManager.Instance.Serialize() as PlayerManager.PlayerManagerData;
        saveObject.chaosVoids = new ChaosVoid.ChaosVoidData[LevelManager.Instance.chaosVoids.Length];
        
        for (int i = 0; i < saveObject.chaosVoids.Length; i++)
        {
            saveObject.chaosVoids[i] = LevelManager.Instance.chaosVoids[i].Serialize() as ChaosVoid.ChaosVoidData;
        }

        return saveObject;
    }

    /// <summary>
    /// Loads from serialized data into the game from a SaveObject.
    /// </summary>
    /// <param name="saveObject">The SaveObject to deserialize from</param>
    private static void DeserializeFromSaveObject(SaveObject saveObject)
    {
        PlayerManager.Instance.FromSerialized(saveObject.playerManagerData);
        
        foreach (var chaosVoid in saveObject.chaosVoids)
        {
            string levelName = chaosVoid.levelName;
            LevelManager.Instance.chaosVoids.First(cv => cv.name == levelName).FromSerialized(chaosVoid);
        }
    }

    /// <summary>
    /// Serializable class for defining the structure of the binary save files.
    /// </summary>
    [Serializable]
    private class SaveObject
    {
        public PlayerManager.PlayerManagerData playerManagerData;
        public ChaosVoid.ChaosVoidData[] chaosVoids;
    }
}