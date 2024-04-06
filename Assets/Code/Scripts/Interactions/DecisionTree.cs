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
        root.playerResponse="Let's practice dialouge";
        root.dialougePrompt="Say Something";
        root.fullResponse="If you see this there was an error";
        root.pointers=new LinkedList<Node>();
        Node leaf1=new Node();
        leaf1.playerResponse="inquire";
        leaf1.fullResponse="Like what?";
        leaf1.dialougePrompt="Like that. Anything Really. Good Job.";
        Node leaf2=new Node();
        leaf2.playerResponse="nothing";
        leaf2.fullResponse="...";
        leaf2.dialougePrompt="Not helpful.";
        root.pointers.AddLast(leaf1);
        root.pointers.AddLast(leaf2);
        current=root;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //initialize tree
    //print current node of tree
    public string PlayerResponse()
    {
        return current.fullResponse;
    }
    public string DialougePrompt()
    {
        return current.dialoguePrompt;
    }

}
