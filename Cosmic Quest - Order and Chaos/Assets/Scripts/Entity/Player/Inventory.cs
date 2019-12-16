using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [Tooltip("Max number of items that this inventory can hold. Setting to 0 means no limit")]
    public int maxSize = 0;

    // Callback for when an item is added or removed
    // TODO only useful if we have some sort of inventory UI
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    [SerializeField] private List<Equipment> items;
    private int _currentItem = 0;

    public Inventory()
    {
        items = new List<Equipment>();
    }

    public bool Add(Equipment item)
    {
        if (maxSize != 0 && items.Count >= maxSize)
            return false;
        
        // Add item to inventory
        items.Add(item);
        onItemChangedCallback?.Invoke();
        return true;
    }

    public bool Remove(Equipment item)
    {
        if (!items.Contains(item))
            return false;
        
        // Modify current item index if necessary
        int itemIndex = items.IndexOf(item);
        if (itemIndex < _currentItem)
            _currentItem -= 1;

        // Remove item from inventory
        items.Remove(item);
        onItemChangedCallback?.Invoke();
        return true;
    }

    public Equipment GetNextItem()
    {
        if (items.Count == 0)
            return null;
        
        _currentItem += 1;
        if (_currentItem >= items.Count)
            _currentItem = 0;

        return items[_currentItem];
    }

    public Equipment GetPreviousItem()
    {
        if (items.Count == 0)
            return null;
        
        _currentItem -= 1;
        if (_currentItem < 0)
            _currentItem = items.Count - 1;

        return items[_currentItem];
    }

    public Equipment GetCurrentItem()
    {
        return items.Count == 0 ? null : items[_currentItem];
    }
}
