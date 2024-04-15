using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomerOrder : MonoBehaviour
{
    public Dialogue CustomerChain;
    public OrderTracker toTrack;
    public string[] orderChain;
    private float cost;
    private int count;
    System.Random orderGenerator;
    // Start is called before the first frame update
    void Start()
    {
        count=0;
        cost=0;
        orderGenerator= new System.Random();
    }
    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))//change to when click customer
        {
           if (count %8 == 0)
           {
            //start dialogue chain
            CustomerChain.lines[0]="Customer: [greeting]";
            CustomerChain.lines[1]="You: Hello. What can I get you?";
            CustomerChain.lines[2]=this.ChooseOrder();
            CustomerChain.lines[3]="You: Anything Else?";
            CustomerChain.lines[4]="Customer: No, Thank you.";
            CustomerChain.lines[5]="You: Alright That will be $"+this.cost.ToString();
            CustomerChain.lines[6]="You: Have a good day!";
            CustomerChain.lines[7]="...";
            //customer walk away
            CustomerChain.StartDialogue();
           }
           count+=1;
        } */  
    }
    public void StartCustomerChain()
    {
        CustomerChain.lines[0]="Customer: [greeting]";
        CustomerChain.lines[1]="You: Hello. What can I get you?";
        CustomerChain.lines[2]=this.ChooseOrder();
        CustomerChain.lines[3]="You: Anything Else?";
        CustomerChain.lines[4]="Customer: No, Thank you.";
        CustomerChain.lines[5]="You: Alright That will be $2.99";
        CustomerChain.lines[6]="You: Have a good day!";
        CustomerChain.lines[7]="...";
            //customer walk away
        CustomerChain.SetLines('c',8,orderChain);
    }
    private string ChooseOrder()
    {
        //choosing one.
        int people=orderGenerator.Next()%4;
        int orderNumber=1;
        int itemnum;
        int tally0=0;
        int tally1=0;
        int tally2=0;
        int tally3=0;
        
        string order="Customer: ";
        int index;
        for (index=0; index <= people; index++)
        {
            itemnum=orderGenerator.Next()%4;
            if (itemnum == 0)tally0+=1;
            if (itemnum == 1)tally1+=1;
            if (itemnum == 2)tally2+=1;
            if (itemnum == 3)tally3+=1;
        }
        order=order+this.OrderItem(tally0, tally1, tally2, tally3);
        return order;
    }
    /*private float calcCost(int tally0, int tally1, int tally2, int tally3)
    {
        float cost;
        //cost=tally0*1.99+tally1*1.89+tally2*3.49+tally3*3.87;
        cost=34.43;
        return cost;
        
    }*/
     private string OrderItem(int tally0, int tally1, int tally2, int tally3)
    {   
        bool specifications;
        string order="I would like ";
        //the options.
        if (tally0 != 0)
        {
            
            if (tally0 == 1)order=order+"a Hamburger";
            else 
            {
                //convert tally number to string
                order=order+"Hamburger";
            }
            //toTrack.OrderBoardUpdate(order, 1);
        }
        if (tally1 != 0)
        {
            if (tally1 == 1)order=order+"a Cheeseburger";
            else
            {
                //convert tally number to string
                order=order+"Cheeseburgers";
            }
            //toTrack.OrderBoardUpdate(order, 1);
        }
        if (tally2 != 0)
        {
            if (tally2 == 1)order=order+"a Hamburger Combo";
            else
            {
                //convert tally number to string
                order=order+"Hamburger Combos";
            }
            //toTrack.OrderBoardUpdate(order, 1);
        }
        if (tally3 != 0)
        {
            if (tally2 == 1)order=order+"a cheeseburger Combo";
            else
            {
                //convert tally number to string
                order=order+"cheeseburger Combos";
            }
            //toTrack.OrderBoardUpdate(order, 1);
        }
        return "I would like " + order;
    }
}
