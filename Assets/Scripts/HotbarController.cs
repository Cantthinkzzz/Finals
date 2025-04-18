using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount=8;
    private ItemDictionary itemDictionary;
    private Key[] keys;
    private void Awake()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
        keys = new Key[slotCount];
        for(int i=0; i< slotCount; i++) {
            keys[i] = i<9? (Key)((int)Key.Digit1+i): Key.Digit0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i< slotCount; i++) {
            if(Keyboard.current[keys[i]].wasPressedThisFrame){
                useItemInSlot(i);
            }
        }
    }
    void useItemInSlot(int index) {
        Slot slot =hotbarPanel.transform.GetChild(index).GetComponent<Slot>();
        if(slot.currentItem!=null) {
            Item item = slot.currentItem.GetComponent<Item>();
            item.useItem();
        }
    }
     public List<InventorySaveData> getHotbarData() {
        List<InventorySaveData> hotbarSaveData = new List<InventorySaveData>();
        foreach(Transform transform in hotbarPanel.transform) {
            Slot slot = transform.GetComponent<Slot>();
            if(slot.currentItem!= null) {
                Item item = slot.currentItem.GetComponent<Item>();
                Debug.Log($"saving hotbar item: {item.itemName}");
                hotbarSaveData.Add(new InventorySaveData{itemID= item.ID, slotIndex = transform.GetSiblingIndex()});
            }
        }
        return hotbarSaveData;
    }
    public void setHotbarData(List<InventorySaveData> inventorySaveDatas) {
        //delete inventory
        //foreach(Transform slot in hotbarPanel.transform) {
        //    Destroy(slot.gameObject);
        //}
        foreach(InventorySaveData inventorySaveData in inventorySaveDatas) {
            Debug.Log(inventorySaveData.itemID);
            Debug.Log(inventorySaveData.slotIndex);
        }
        //for(int i=0; i<slotCount; i++) {
        //    Instantiate(slotPrefab, hotbarPanel.transform);
        //}
        //fill slots with items
        foreach(InventorySaveData invSaveData in inventorySaveDatas) {
            if(invSaveData.slotIndex < slotCount) {
                Slot slot = hotbarPanel.transform.GetChild(invSaveData.slotIndex).GetComponent<Slot>();
                if(slot != null) Debug.Log("Nihi!");
                GameObject itemPrefab = itemDictionary.GetItemPrefab(invSaveData.itemID);
                if(itemPrefab != null) {
                    Debug.Log("what the flip");
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    Debug.Log(item.ToString());
                    item.GetComponent<RectTransform>().anchoredPosition=Vector2.zero;
                    slot.currentItem=item;
                }
            }
        }
    }
}
