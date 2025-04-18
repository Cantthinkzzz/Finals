using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractible
{

    public bool IsOpened {get; private set;}
    public string chestID {get;private set;}
    public GameObject itemPrefab;
    public Sprite openedSprite;
    public bool canInteract()
    {
        return !IsOpened;
    }

    public void interact()
    {
        if(!canInteract()) return;
        OpenChest();

    }

    // Start is called before the first frame update
    void Start()
    {
        chestID ??= UniversalHelper.generateUniqueID(gameObject);
    }

    public void setOpened(bool opened) {
        IsOpened = opened;
        if(IsOpened) {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
    private void OpenChest() {
        setOpened(true);
        SoundEffectManager.Play("Chest");

        if(itemPrefab) {
            GameObject dropItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            dropItem.GetComponent<BounceEffect>().startBounce();
        }
    }
}
