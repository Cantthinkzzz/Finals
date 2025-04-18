using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
        /*for(int i = 0; i < slotCount; i++) {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if(i < itemPrefabs.Length) {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem=item;
            }
        }*/

    }
    public bool AddItem(GameObject itemPrefab) {
        foreach(Transform slotTransform in inventoryPanel.transform) {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.currentItem== null && slot.transform.childCount==0) {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem=newItem;
                return true;
            }
        }
        Debug.Log("The inventory is full!");
        return false;
    }


    public List<InventorySaveData> getInventoryData() {
        List<InventorySaveData> inventorySaveData = new List<InventorySaveData>();
        foreach(Transform transform in inventoryPanel.transform) {
            Slot slot = transform.GetComponent<Slot>();
            if(slot.currentItem!= null) {
                Item item = slot.currentItem.GetComponent<Item>();
                inventorySaveData.Add(new InventorySaveData{itemID= item.ID, slotIndex = transform.GetSiblingIndex()});
            }
        }
        return inventorySaveData;
    }
    public void setInventoryData(List<InventorySaveData> inventorySaveDatas) {
        //delete inventory
        foreach(Transform slot in inventoryPanel.transform) {
            Destroy(slot.gameObject);
        }
        for(int i=0; i<slotCount; i++) {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
        //fill slots with items
        foreach(InventorySaveData invSaveData in inventorySaveDatas) {
            if(invSaveData.slotIndex < slotCount) {
                Slot slot = inventoryPanel.transform.GetChild(invSaveData.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(invSaveData.itemID);
                if(itemPrefab != null) {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition=Vector2.zero;
                    slot.currentItem=item;
                }
            }
        }
    }

    
}
