using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment")]
public class Equipment : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public GameObject itemPrefab;
    public int damageModifier;
    public int defenseModifer;
}