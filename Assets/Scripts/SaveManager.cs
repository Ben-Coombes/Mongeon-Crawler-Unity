using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager gameSave;
    public List<ScriptableObject> objects = new List<ScriptableObject>();
    public List<IntValue> intValues = new List<IntValue>();
    public List<FloatValue> floatValues = new List<FloatValue>();
    public List<BoolValue> boolValues = new List<BoolValue>();
    public Inventory playerInventory;
    public VectorValue playerPosition;
    public Inventory itemList;
    public Inventory intialItemList;
    public void ResetScriptables()
    {
        playerPosition.RuntimeValue = playerPosition.intialValue;
        itemList.items.Clear();
        itemList.items.AddRange(intialItemList.items);
        playerInventory.items.Clear();
        for(int i = 0; i < intValues.Count; i++)
        {
            intValues[i].RuntimeValue = intValues[i].intialValue;
        }
        for (int i = 0; i < floatValues.Count; i++)
        {
            floatValues[i].RuntimeValue = floatValues[i].intialValue;
        }
        for (int i = 0; i < boolValues.Count; i++)
        {
            boolValues[i].RuntimeValue = boolValues[i].intialValue;
        }
        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            playerInventory.items.RemoveAt(i);
        }
        for (int i = 0; i <objects.Count; i++)
        {
            
            if (File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
            {
                File.Delete(Application.persistentDataPath + string.Format("/{0}.dat", i));
            }
        }
    }
    public void SaveScriptables()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}.dat", i));
            BinaryFormatter binary = new BinaryFormatter();
            var json = JsonUtility.ToJson(objects[i]);
            binary.Serialize(file, json);
            file.Close();
        }
    }
    public void LoadScriptables()
    {
        itemList.items.Clear();
        itemList.items.AddRange(intialItemList.items);
        for (int i = 0; i < objects.Count; i++)
        {
            if(File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
            {
                FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}.dat", i), FileMode.Open);
                BinaryFormatter binary = new BinaryFormatter();
                JsonUtility.FromJsonOverwrite((string)binary.Deserialize(file), objects[i]);
                file.Close();
            }
        }
    }
}
