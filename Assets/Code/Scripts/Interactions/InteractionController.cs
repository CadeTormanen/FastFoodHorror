using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance;
    public GameObject playerCharacterObject;
    public GameObject playerControllerObject;
    private Queue<Interaction> interactionQueue;

    private int interactionSelected;
    private int interactionNumber;

    private GameObject textObject;
    private Text textComponent;

    /// <summary>
    /// Get all interactions within 'interactionDistance' and enqueue them.
    /// </summary>
    private void FetchNearbyInteractions()
    {
        foreach (Transform child in transform)
        {
            
            if (Vector3.Distance(child.position, playerCharacterObject.transform.position) <= interactionDistance)
            {
                Interaction interaction = child.GetComponent<Interaction>();
                interactionQueue.Enqueue(interaction);
            }
        }
        interactionNumber = interactionQueue.Count;
    }

    /// <summary>
    /// Go through each interaction in our queue and present them to the player as options.
    /// </summary>
    private void StageInteractions()
    {
        string displayText = "";
        int interactions = interactionQueue.Count;
        Interaction[] actionArray = new Interaction[interactions];

        if (interactionSelected >= interactions) { interactionSelected = 0; }

        //check player input: for changing interaction selection
        if (playerControllerObject.GetComponent<PlayerInteractions>().scrollUp){
            if (interactionSelected != 0) { interactionSelected++; }
        }

        if (playerControllerObject.GetComponent<PlayerInteractions>().scrollDown){
            if (interactionSelected != interactions-1){ interactionSelected++; }
        }
        
        //take all interactions from queue and put into array
        for (int i = 0; i < interactions; i++)
        {
            Interaction interaction = interactionQueue.Dequeue();
            actionArray[i] = interaction;
        }

        //go through all interactions in the array and display on screen
        for (int i = 0;  i < interactions; i++)
        {
            if (i == interactionSelected){
                displayText += ("[ " + actionArray[i].display + " ]\n");
            }else{
                displayText += (actionArray[i].display + "\n");
            }
        }

        //check player input: for activating the selected interaction
        if (playerControllerObject.GetComponent<PlayerInteractions>().interactionEnabled){
            actionArray[interactionSelected].ExecuteInteraction();
        }

        textComponent.text = displayText;
            
    }

    /// <summary>
    /// Setup the text box so it is ready to write to.
    /// </summary>
    private void setupTextbox()
    {
        interactionQueue = new Queue<Interaction>();
        textObject = new GameObject("InteractionText");
        textComponent = textObject.AddComponent<Text>();
        textObject.transform.SetParent(GameObject.Find("PlayerHUD").transform, false);

        textComponent.text = "";
        textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComponent.fontSize = 12;

        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, -250, 0);
    }

    public void Update()
    {
        FetchNearbyInteractions();
        StageInteractions();
    }


    public void Start()
    {
        setupTextbox();


    }
}
