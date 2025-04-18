using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
        // Start is called before the first frame update
    void Start()
    {
        canvasGroup= GetComponent<CanvasGroup>();
    }

    Transform originalParent;
    CanvasGroup canvasGroup;
    public float minDropDistance = 2f;
    public float maxDropDistance = 4f;
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts=false;
        canvasGroup.alpha= 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts=true;
        canvasGroup.alpha=1f;
        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        Slot originalSlot = originalParent.GetComponent<Slot>();
        if(dropSlot==null) {
            GameObject item = eventData.pointerEnter;
            if(item!=null) {
                dropSlot= item.GetComponentInParent<Slot>();
            }
        }

        if(dropSlot!=null) {
            if(dropSlot.currentItem!=null) {
                dropSlot.currentItem.transform.SetParent(originalParent.transform);
                originalSlot.currentItem= dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition=Vector2.zero;


            }
            else {
                originalSlot.currentItem= null;
            }
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem= gameObject;
        }
        else {
            //if dropping outside inventory
            if(!isWithinInventory(eventData.position)) {
                DropItem(originalSlot);
            }
            else transform.SetParent(originalParent.transform);
        }
        GetComponent<RectTransform>().anchoredPosition=Vector2.zero;
    }

    bool isWithinInventory(Vector2 mousePosition) {
        RectTransform rectTransform = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition);
    }
    void DropItem(Slot originalSlot) {
        originalSlot.currentItem=null;
        //locate player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if(playerTransform==null) {
            Debug.LogError("Missing Player tag");
            return;
        }
        //randomise drop location
        Vector2 vectorOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + vectorOffset;
        //instantiate drop item
        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        dropItem.GetComponent<BounceEffect>().startBounce();
        //destroy dropped item
        Destroy(gameObject);
    }


}
