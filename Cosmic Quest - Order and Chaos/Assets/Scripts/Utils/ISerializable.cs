using System;

/// <summary>
/// Interface for serializable objects whose state should be included in save and load.
/// </summary>
public interface ISerializable
{
    /// <summary>
    /// Abstract function for serializing class data.
    /// </summary>
    /// <returns>Serialized class data object</returns>
    object Serialize();

    /// <summary>
    /// Loads class data from a serialized data class.
    /// </summary>
    /// <param name="data">Serialized data class to load from</param>
    void FromSerialized(object data);
}