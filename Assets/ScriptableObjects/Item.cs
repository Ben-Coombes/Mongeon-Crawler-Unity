using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Item : ScriptableObject
{
    public Sprite itemSprite;
    public string itemName;
    public string itemDescription;
    public int itemPrice;
}
