using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
   public int ID;
   public string itemName;

   public virtual void pickup() {
      Sprite itemicon = gameObject.GetComponent<Image>().sprite;
      if(PickupUIController.Instance!= null) {
         PickupUIController.Instance.ShowItemPickup(itemName, itemicon);
      }
   }
   public virtual void useItem() {
      Debug.Log("Using item "+ itemName);
   }
}
