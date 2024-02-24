using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance;
    public GameObject playerCharacterObject;
    public GameObject playerObject;
    private Queue<Interaction> interactionQueue;

    private int interactionSelected;

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
                //add nearby interaction to the queue if it is valid in the current context
                Interaction interaction = child.GetComponent<Interaction>();
                if (interaction.Possible() == true)
                {
                    interactionQueue.Enqueue(interaction);
                }
            }
        }
    }

    /// <summary>
    /// Go through each interaction in our queue and present them to the player as options.
    /// </summary>
    private void StageInteractions()
    {
        int interactions = interactionQueue.Count;
        if (interactions == 0) { return; }
        if (interactionSelected >= interactions) { interactionSelected = 0; }

        //take all interactions from queue and put into array
        Interaction[] actionArray = new Interaction[interactions];
        for (int i = 0; i < interactions; i++)
        {
            actionArray[i] = interactionQueue.Dequeue();
        }

        //go through all interactions in the array and display on screen
        string displayText = "";
        for (int i = 0;  i < interactions; i++)
        {
            if (i == interactionSelected){
                displayText += ("[ " + actionArray[i].interactionText + " ]\n");
            }else{
                displayText += (actionArray[i].interactionText + "\n");
            }
        }

        //check player input: for activating the selected interaction
        if (playerObject.GetComponent<Player>().interactionEnabled){
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
        textComponent.text = "";
        FetchNearbyInteractions();
        StageInteractions();
    }

    public void Start()
    {  
        setupTextbox();
    }
}
