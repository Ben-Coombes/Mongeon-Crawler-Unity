using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public Pickup thisLoot;
    public IntValue lootChance;
}
[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public Loot[] loots;

    public Pickup LootPickup()
    {
        int prob = 0;
        int rand = Random.Range(0, 100);
        for(int i = 0; i < loots.Length; i++)
        {
            prob += loots[i].lootChance.RuntimeValue;
            if(rand <= prob)
            {
                return loots[i].thisLoot;
            }
        }
        return null;
    }
}
