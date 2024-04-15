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
    //private Hashtable readables;
    // Start is called before the first frame update
    void Start()
    {
	textComponentSwitch.text="Start readables";
        textComponent1.text="decision1";
        textComponent2.text="decision2";
        reading=true;	
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
            textComponentSwitch.text="Readables";
            textComponent1.text=decisions.fetchNextButton(1);
            textComponent2.text=decisions.fetchNextButton(2);
            storyLines[0]=decisions.fetchTheResponse();
            storyLines[1]=decisions.fetchThePrompt();
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
            storyLines[0]="Hello!";
            storyLines[1]="I want to talk about cats.";
            storyLines[2]="Cats are wonderful.";
            storyLines[3]="That is all.";
            storyChain.SetLines('r',4,storyLines);
            reading=false;
        }
        else
        {
            //change current node
            if (storyChain.inUse)return;
            decisions.MakeDecision(1);
            //fetch full response and print dialougePrompt
            DisplayDialouge();
        }    
    }
    void chooseoption2()
    {
	if (reading)
        {
            storyLines[0]="Here is a readable.";
            storyLines[1]="It will tell you interesting information.";
            storyLines[2]="Sometimes they will be important to the story";
            storyLines[3]="Sometimes it will be extra tidbit of information";
            storyChain.SetLines('r',4,storyLines);
            reading=false;
        }
        else
        {
            //change current node
            if (storyChain.inUse)return;
            decisions.MakeDecision(2);
            //fetch full response and print dialougePrompt
            DisplayDialouge();
        }
    }
}
