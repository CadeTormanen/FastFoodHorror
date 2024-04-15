using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance;
    public GameObject playerCharacterObject;
    public GameObject playerObject;
    [SerializeField] private GameObject interactionBox;
    [SerializeField] private GameObject interactionText;
    private TextMeshProUGUI textComponent;
    private Queue<Interaction> interactionQueue;
    private int interactionSelected;

    private int lowerBound;
    private int upperBound;
    private int interactions;

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
                Interaction[] interactions = child.GetComponents<Interaction>();
                foreach(Interaction interaction in interactions)
                {
                    if (interaction.Possible() == true)
                    {
                        
                        interactionQueue.Enqueue(interaction);
                    }
                }

            }
        }
    }

    /// <summary>
    /// Go through each interaction in our queue and present them to the player as options.
    /// </summary>
    private void StageInteractions()
    {
        textComponent.text = "";
        interactionBox.SetActive(false);

        interactions = interactionQueue.Count;

        //reset the range of interactions to display if the number of interactions changed
        if (interactionQueue.Count == 0) { return; }
        
        
        //set interaction display box to correct height, and activate
        interactionBox.SetActive(true);
        if (interactionSelected >= interactions) { interactionSelected = 0; }
        if (interactionSelected < 0)             { interactionSelected = 0; }

        if (interactionSelected == 0) { lowerBound = 0;upperBound = 2; }
        else{
            upperBound = interactionSelected + 1;
            lowerBound = interactionSelected - 1;
        }

        //take all interactions from queue and put into array
        Interaction[] actionArray = new Interaction[interactions];
        for (int i = 0; i < interactions; i++)
        {
            actionArray[i] = interactionQueue.Dequeue();
        }

        //display three of the interactions of the screen at a time
        string displayText = "";
        
        for (int i = lowerBound;  i <= upperBound; i++)
        {
            if ((i < 0) || (i >= interactions)){continue;}
            if (i == interactionSelected){
<<<<<<< HEAD:Assets/Code/Scripts/Interactions/InteractionController.cs
<<<<<<< HEAD:Assets/Code/Scripts/Interactions/Interface and Controller/InteractionController.cs
                displayText += ("   " + actionArray[i].interactionText + "\n");
=======
                displayText += ("> " + i.ToString() + " " + actionArray[i].interactionText + "\n");
>>>>>>> parent of 2cb752b8 (Incorporate 'Cade/Pathing' paths, Create customers, register):Assets/Code/Scripts/Interactions/InteractionController.cs
=======
                displayText += ("> " + i.ToString() + " " + actionArray[i].interactionText + "\n");
>>>>>>> parent of 2cb752b8 (Incorporate 'Cade/Pathing' paths, Create customers, register):Assets/Code/Scripts/Interactions/Interface and Controller/InteractionController.cs
            }else{
                displayText += (" " + actionArray[i].interactionText + "\n");
            }
        }

        //check player input: for activating the selected interaction
        if (playerObject.GetComponent<Player>().interactionEnabled){
            
            actionArray[interactionSelected].ExecuteInteraction();
        }

        textComponent.text = displayText;
            
    }

    public void NextInteraction()
    {
        if (interactionSelected == interactions-1){return;}
        interactionSelected++;
    }

    public void PreviousInteraction()
    {
        if (interactionSelected == 0) { return; }
        interactionSelected--;
    }

    /// <summary>
    /// Setup the text box so it is ready to write to.
    /// </summary>
    private void setupTextbox()
    {
        interactionBox.SetActive(false);
        interactionQueue = new Queue<Interaction>();
        textComponent = interactionText.GetComponent<TextMeshProUGUI>();
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
