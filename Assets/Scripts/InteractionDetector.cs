using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractible interactibleInRange= null;
    public GameObject interactionIcon;

    // Start is called before the first frame update
    void Start()
    {
        interactionIcon.SetActive(false);
    }
    public void OnInteract(InputAction.CallbackContext context) {
        if(context.performed) {
            interactibleInRange?.interact();
            if(!interactibleInRange.canInteract()) {
                interactionIcon.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractible interactible) && interactible.canInteract()) {
            interactibleInRange = interactible;
            interactionIcon.SetActive(true);
        }       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractible interactible) && interactible == interactibleInRange) {
            interactibleInRange = null;
            interactionIcon.SetActive(false);
        }       
    }
}
