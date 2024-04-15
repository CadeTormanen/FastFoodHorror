using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Register : MonoBehaviour, Interaction
{

    [SerializeField] private CustomerQueue customers;
    [SerializeField] private Player player;
    [SerializeField] private OrderList orders;


    public string interactionText { get; set; }
    public void ExecuteInteraction()
    {
        //get the customer instance
        Customer customer = customers.GetFirstCustomer();
        if (customer == null) return;

        /*
         
        //tell the player to enter dialogue mode
        player.state = Player.PLAYERSTATES.dialogue;

        */

        //attempt to add the order
        Order order = customer.order;
        bool result = orders.AddOrder(order);
        if (result == false) { return; }

        //let the customer whose order we just took leave.
        customers.ReleaseCustomer();








    }
    public void ValidateInteraction()
    {
        


    }
    public bool Possible()
    {
        if (customers.CustomerReady())
        {
            interactionText = "Speak To\nCustomer";
            return true;
        }
        return false;
    }



}
