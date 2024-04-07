using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderTracker : MonoBehaviour
{
    private int ordernumber;
    public Dialogue orderBoardDisplay;
    //private HashTable Orderboard;
    //private boolean inUse;
    private LinkedList<string> orderList;
    // Start is called before the first frame update
    void Start()
    {
        ordernumber=0;
        //Orderboard=new HashTable();
        orderList= new LinkedList<string>();
    }

    // Update is called once per frame
    void Update()
    {
        //when order board clicked call OrderBoardRead
        //switch orderboardreading boolean
        //if (!inUse){when button clicked orderBoardclear or OrderBoardChanged}
    }
    void OnMouseDown()
    {
        int index=0;
        if (orderBoardDisplay.inUse==true)return;
        //when order board clicked display text
        foreach(string str in orderList)
        {
            orderBoardDisplay.lines[index]=str;
            index++;
            //Consol.WriteLine(str);
        }
        orderBoardDisplay.StartDialogue();
    }
    public void OrderBoardUpdate(string order, int itemNumber)
    {
        ordernumber += 1;
        orderList.AddLast(order);
    }
    void OrderBoardClear()
    {
        //cycle through and delete everything.
        //list.Clear();
    }
    void OrderBoardChange(string orderPrev, string orderChange)
    {
        if (orderList.Contains(orderPrev) == false)return;
        orderList.Remove(orderPrev);
        //get input
        orderList.AddLast(orderChange);
    }
}
