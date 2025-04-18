using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapControllerDynamic : MonoBehaviour
{
    [Header("UI REFERENCES")]
    public RectTransform mapParent;
    public GameObject areaPrefab;
    public RectTransform playerIcon;

    [Header("Colors")]
    public Color defaultColor = Color.gray;
    public Color currentColor = Color.red;
    [Header("Map Settings")]
    public GameObject mapBounds;  //the parent of all bound colliders
    public Collider2D initialArea;
    public float mapScale= 10f;
    private Collider2D[] mapAreas;
    private Dictionary<string, RectTransform> uiAreas = new Dictionary<string, RectTransform>(); //map colliders to ui elements

    public static MapControllerDynamic Instance {get;set;}

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance=this;
        }
        mapAreas= mapBounds.GetComponentsInChildren<PolygonCollider2D>();

    } 
    //generate map
    public void GenerateMap(Collider2D newArea = null) {
        Collider2D curr = newArea != null?newArea: initialArea;
        clearMap();
        foreach(Collider2D area in mapAreas) {
            //create area ui
            CreateAreaUI(area, area == curr);
        }
        MovePlayerIcon(curr.name);

    }
    //clear map
    private void clearMap() {
        foreach(Transform child in mapParent) {
            Destroy(child.gameObject);
        }
        uiAreas.Clear();
    }
    private void CreateAreaUI(Collider2D area, bool isCurrent) {
        GameObject areaImage = Instantiate(areaPrefab, mapParent);
        RectTransform rectTransform = areaImage.GetComponent<RectTransform>();
        Bounds bounds = area.bounds;
        rectTransform.sizeDelta = new Vector2(bounds.size.x * mapScale, bounds.size.y * mapScale);
        rectTransform.anchoredPosition=bounds.center;
        areaImage.GetComponent<Image>().color = isCurrent?currentColor:defaultColor;
        uiAreas[area.name] = rectTransform;
    }
    private void MovePlayerIcon(string newCurrArea) {
        if(uiAreas.TryGetValue(newCurrArea, out RectTransform areaUI)) {
            playerIcon.anchoredPosition = areaUI.anchoredPosition;
        }
    }
    public void updateCurrentArea(string newCurr) {
        foreach(KeyValuePair<string, RectTransform> area in uiAreas) {
            area.Value.GetComponent<Image>().color= area.Key == newCurr? currentColor: defaultColor;
        }
        MovePlayerIcon(newCurr);
    }


}
