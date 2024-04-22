using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    public Dialogue storyChain;
    public TextMeshProUGUI textComponent1;
    public TextMeshProUGUI textComponent2;
    public TextMeshProUGUI textComponentSwitch;
    public string[] storyLines;
    private bool reading;
    private DecisionTree decisions;
    private Dictionary<string, string[]> readables;
    // Start is called before the first frame update
    void Start()
    {
	textComponentSwitch.text="Start readables";
        textComponent1.text="decision1";
        textComponent2.text="decision2";
        reading=false;
	string[] lines={"Here is a readable.", "Sometimes it helpful information, sometimes it has interesting facts.","There are several options of readables at anyone point.","When you approach a readable object the key will become available","...", "I know you can't get around yet but that is what the plan is."};
        string[] morelines={"This is also a readable.", "At some point there will be more than two.","Have you ever been to a tide pool?","They are pretty interesting.","Anemonies live in the ones on Cannon Beach."};
        readables= new Dictionary<string, string[]>();
        readables.Add("note 1", lines);
        readables.Add("note 2", morelines);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void InitializeReadables()
    {
reading=!reading;
       if (reading)
       {
            textComponentSwitch.text="Dialogue";
            storyLines[0]="Here is the readables page.";
            storyLines[1]="It looks like you have a choice.";
            storyLines[2]="Pick a readable.";
            storyLines[3]="Press a button to your right.";
            reading=true;
            textComponent1.text="Note1";
            textComponent2.text="Note2";
            storyChain.SetLines('r',4,storyLines);
       }
       else
       {
            //upload the dialog chain from current point.
            if (storyChain.inUse)return;
            textComponentSwitch.text="Readables";
            textComponent1.text="Decision 1";
            textComponent2.text="Decision 2";
            storyLines[0]="This is the dialogue page. Someone will say something and you will respond with one of the options on the right.";
            storyLines[1]="We have two options here for display. Say something.";
            storyChain.SetLines('d',2,storyLines);
       }
    }
    void DisplayReadables(string title)
    {
    }
    void DisplayDialouge()
    {
	    storyChain.lines[0]=decisions.FetchTheResponse();
	    stroyChain.lines[1]=decisions.FetchThePrompt();
    }
    void chooseoption1()
    {
	if (reading)
        {
            if (storyChain.inUse)return;
            storyLines=readables["note 1"];
            storyChain.SetLines('r', 6 ,storyLines);
        }
        else
        {
            //change current node
            if (storyChain.inUse)return;
       	    storyLines[0]="You: Something. Nothing but filler words here.";
            storyLines[1]="Excellent. That works just fine.";
            storyChain.SetLines('d', 2, storyLines);
        }    
    }
    void chooseoption2()
    {
	if (reading)
        {
 	    storyLines=readables["note 2"];
            storyChain.SetLines('r', 5,storyLines);
        }
        else
        {
            //change current node
            if (storyChain.inUse)return;
            storyLines[0]="La di da. Just trying to make this work.";
            storyLines[1]="Yeah, not the best thing to be stuck in a haunted fast food place.";
            storyChain.SetLines('d', 2, storyLines);
        }
    }
}
