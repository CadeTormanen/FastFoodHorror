using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFoodDone : MonoBehaviour, Interaction
{

    [SerializeField] OrderList orderlist;
    [SerializeField] BurgerAssembler foodtray;

    private AudioSource successSFX;
    private AudioSource failureSFX;


    public string interactionText { get; set; }
    private bool buttonPressed;
    

    public void ExecuteInteraction()
    {
        Press();
    }

    public bool Possible()
    {
        interactionText = "Complete Order";
        if (foodtray.Ready() == false) return false;
        return (!buttonPressed);
    }

    public void ValidateInteraction()
    {
        
    }

    public void Press()
    {
        Order order = foodtray.FetchContents();
        bool result = orderlist.MatchOrder(order);
        if (result == true) { successSFX.Play(); }
        if (result == false) { failureSFX.Play(); }
        foodtray.ClearTray();
    }

    public void Start()
    {
        successSFX = GetComponents<AudioSource>()[1];
        failureSFX = GetComponents<AudioSource>()[0];
    }

}
