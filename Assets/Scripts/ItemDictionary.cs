using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    public Dictionary<int, GameObject> itemDictionary;
    private void Awake(){
        itemDictionary = new Dictionary<int, GameObject>();
        for(int i=0; i<itemPrefabs.Count;i++) {
            if(itemPrefabs[i] != null) {
                itemPrefabs[i].ID= i+1;
            }
        }
        foreach(Item item in itemPrefabs) {
            itemDictionary[item.ID] = item.gameObject;
        }
    }

    public GameObject GetItemPrefab(int itemId) {
        itemDictionary.TryGetValue(itemId, out GameObject prefab);
        if(prefab==null) {
            Debug.LogWarning($"Item with ID {itemId} does not exist in the item dictionary");
        }
        return prefab;

    }
}
