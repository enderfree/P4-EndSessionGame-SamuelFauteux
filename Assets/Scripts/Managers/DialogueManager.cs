using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // The Dialogue Manager does not manage display, only content
    [SerializeField] Image pfpHolder;
    [SerializeField] TMP_Text textbox;
    [SerializeField] GameObject dialogueButton;

    private List<Dialogue> ongoingDialogue = new List<Dialogue> ();
    private Action callback; // who do we notify once the dialogue is over

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Buttons
    public void OnDialogueButtonPressed()
    {
        if (ongoingDialogue.Count > 0)
        {
            DisplayMessage();
        }
        else
        {
            if(callback != null)
            {
                callback();
                callback = null;
            }
        }
    }

    // misc
    private void DisplayMessage()
    {
        pfpHolder.sprite = ongoingDialogue[0].Interlocutor;
        textbox.text = ongoingDialogue[0].Message;

        ongoingDialogue.RemoveAt(0);
    }

    public void StartDialogue(List<Dialogue> dialogue, Action callback)
    {
        dialogueButton.SetActive(true);
        ongoingDialogue = dialogue;
        this.callback = callback;
        DisplayMessage();
    }
}
