using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance;
    public GameObject playerCharacterObject;
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
        textComponent.text = "";
        while (interactionQueue.Count > 0)
        {
            Interaction interaction = interactionQueue.Dequeue();
            textComponent.text = interaction.display;
        }
    }

    private void setupTextbox()
    {
        interactionQueue = new Queue<Interaction>();
        textObject = new GameObject("InteractionText");
        textComponent = textObject.AddComponent<Text>();
        textObject.transform.SetParent(GameObject.Find("PlayerHUD").transform, false);

        textComponent.text = "";
        textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComponent.fontSize = 24;

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
