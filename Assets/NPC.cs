using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractible
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    public bool canInteract()
    {
        return !isDialogueActive;
    }

    public void interact()
    {
        if(dialogueData== null || (PauseController.IsGamePaused && !isDialogueActive)) return;
    }
}
