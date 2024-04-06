using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    public Dialogue storyChain;
    //private HashTable readables;
    private DecisionTree decisions;
    private Hashtable readables;
    // Start is called before the first frame update
    void Start()
    {
	InitializeReadables();
	DisplayReadables();	
    }

    // Update is called once per frame
    void Update()
    {
        if (!storyChain.inUse)
        {
		DisplayReadables();
        }
    }
    void InitializeReadables()
    {
        //read in file
        //store in string
        string key="Note1";
        string message="Cats Are Great. Do we really need a file for this message?";
        //put in hashtable
        readables.Add(key, message);
    }
    void DisplayReadables(string title)
    {
        if (storyChain.inUse)return;
	/*if (readables.ContainsKey(title))
        {
            storyChain.lines[0] = (string) readables[title];
            storyChain.StartMonologue();  
        }*/
	storyChain.StartMonologue();
    }
    void DisplayDialouge()
    {
	    storyChain.lines[0]=decisions.FetchTheResponse();
	    stroyChain.lines[1]=decisions.FetchThePrompt();
    }
    void chooseoption1()
    {
	    decisions.MakeDecision(1);
	    DisplayDialouge();
    }
    void chooseoption2()
    {
	    decisions.MakeDecision(2);
	    DisplayDialouge();
    }
}
