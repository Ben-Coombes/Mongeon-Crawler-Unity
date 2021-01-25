using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Inventory : ScriptableObject
{
    public List<Item> items = new List<Item>();
    public IntValue coins;
    public void addItem(Item itemtoAdd)
    {
        items.Add(itemtoAdd);
    }
    public bool CheckForItem(Item item)
    {
        if(items.Contains(item))
        {
            return true;
        }
        return false;
    }
}
