using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupUIController : MonoBehaviour
{
    public static PickupUIController Instance {get; private set;}
    public int maxPopups = 5;
    public GameObject popupPrefab;
    public float popupDuration=2f;
    private readonly Queue<GameObject> activePopups = new();
    private void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }
        else{
            Debug.LogWarning("what");
            Destroy(gameObject);
        }
    }

    public void ShowItemPickup(string name, Sprite icon) {
        GameObject popup = Instantiate(popupPrefab, transform);
        popup.GetComponentInChildren<TMP_Text>().text= name;
        //popup.GetComponentInChildren<Image>().sprite=icon;
        Image image = popup.transform.Find("ItemIcon")?.GetComponent<Image>();
        if(image) {
            image.sprite=icon;
        }
        activePopups.Enqueue(popup);
        if(activePopups.Count>maxPopups) {
            Destroy(activePopups.Dequeue());
        }
        StartCoroutine(fadeOutAndDestroy(popup));

    }
    private IEnumerator fadeOutAndDestroy(GameObject popup) {
        yield return new WaitForSeconds(popupDuration);
        if(popup == null) yield break;
        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for(float timePassed = 0f; timePassed<1f; timePassed+=Time.deltaTime) {
            if(popup == null) yield break;
            canvasGroup.alpha=1-timePassed;
            yield return null;
        }
        Destroy(popup);
    }
}
