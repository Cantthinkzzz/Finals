using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;
    private Chest[] chests;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseComponents();
        loadGame();
    }


    public void InitialiseComponents() {
        saveLocation= Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<HotbarController>();
        chests = FindObjectsOfType<Chest>();
    }

    public void saveGame() {
        SaveData saveData = new SaveData {
            playerPosition= GameObject.FindGameObjectWithTag("Player").transform.position,
            boundaryName= FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name, 
            inventorySaveData = inventoryController.getInventoryData(),
            hotbarSaveData = hotbarController.getHotbarData(),
            chestSaveData = GetChestStates()

        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void loadGame() {
        if(File.Exists(saveLocation)) {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position= saveData.playerPosition;
            Collider2D savedBoundary = GameObject.Find(saveData.boundaryName).GetComponent<Collider2D>();
            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = savedBoundary;
            //MapControllerManual.Instance?.HighlightArea(saveData.boundaryName);
            MapControllerDynamic.Instance?.GenerateMap(savedBoundary);
            inventoryController.setInventoryData(saveData.inventorySaveData);
            hotbarController.setHotbarData(saveData.hotbarSaveData);
            LoadChestStates(saveData.chestSaveData);
        }
        else {
            saveGame();
            MapControllerDynamic.Instance?.GenerateMap();
            inventoryController.setInventoryData(new List<InventorySaveData>());

            hotbarController.setHotbarData(new List<InventorySaveData>());
        }
    }
    private List<ChestSaveData> GetChestStates() {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();
        foreach(Chest chest in chests) {
            ChestSaveData chestSaveData= new ChestSaveData {
                chestID = chest.chestID,
                isOpened = chest.IsOpened
            };
            chestStates.Add(chestSaveData);
        }
        return chestStates;

    }
    private void LoadChestStates(List<ChestSaveData> chestSaves) {
        foreach(Chest chest in chests) {
            ChestSaveData chestSaveData = chestSaves.FirstOrDefault(c => c.chestID == chest.chestID);
            if(chestSaveData != null) {
                chest.setOpened(chestSaveData.isOpened);
            }
        }
    }
}
