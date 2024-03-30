using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    Node root;
    Node current;
    
    // Start is called before the first frame update
    void Start()
    {
        Node root=new Node();
	root.SetNode("Let's Practice dialouge.", "say something", "If you see this there was an error");
        Node leaf1=new Node();
	leaf1.SetNode("Inquire", "Like that. Anything really. Nice job.", "Like what?");
        Node leaf2=new Node();
	leaf2.SetNode("compliment", "Gee thanks.", "You look nice.");
	root.ConnectNode(leaf1);
	root.ConnectNode(leaf2);
        current=root;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //initialize tree
    //print current node of tree
    public string FetchTheResponse()
    {
        string fetchLine=current.PrintResponse();
	return fetchLine;
    }
    public string FetchThePrompt()
    {
        string fetchLine=current.PrintPrompt();
	return fetchLine;
    }
    public void Make Decision(int branchNum)
    {
	    Node prev=current;
	    if (branchNum == 1) current=current.FetchNode(1);
	    if (branchNum == 2) current=current.FetchNode(2);
	    if (current == null) current=prev;
    }

}
