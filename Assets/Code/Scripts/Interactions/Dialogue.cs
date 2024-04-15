using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public bool inUse;
    private bool diamode;
    private int index;
    public int sizeOfChain;

    // Start is called before the first frame update
    void Start()
    {
        //for empty string to type into:
        textComponent.text = string.Empty;
                lines[0]="Hello! ";
        lines[1]="This is an introduction to the dialouge system. ";
        lines[2]="I see you have spotted the buttons. ";
        lines[3]="The one ... accepts orders from customers. ";
        lines[4]="the one ... starts the readables. ";
        lines[5]="the two buttons on the left are for talking to people and making decisions. ";
        lines[6]="Try it!";
        SetLines('s', 7, null);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SetLines(char type, int arrayLen, string[] toDisplay)
    {
        if (inUse) return;
        if (type == 'c')
        {//customer Chain
            sizeOfChain=arrayLen;
            lines=toDisplay;
            StartDialogue();
        }
        else if (type == 'r')
        {//readables
            sizeOfChain=arrayLen;
            lines=toDisplay;
            StartMonologue();
        }
        else if (type == 'd')
        {//dialogue
            sizeOfChain=arrayLen;
            lines=toDisplay;
            StartDialogue();
        }
        else if (type == 's')
        {//start or introduction
            sizeOfChain=7;
            StartMonologue();
        }
        else return;
    }
    public void StartDialogue()
    {
        if (inUse)return;
        index = 0;
        gameObject.SetActive(true);
        textComponent.text="";
        textComponent.text = lines[index];
        inUse=true;
        diamode=true;
    }
    public void StartMonologue()
    {
        if (inUse) return;
        index = 0;
        gameObject.SetActive(true);
        textComponent.text="";
        StartCoroutine(TypeLine());
        inUse=true;
        diamode=false;
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        textComponent.text += " ";
    }

    public void NextLine()
    {
        if (diamode)NextLineDialogue();
        else NextLineMonologue();
    }
    private void NextLineDialogue()
    {
        if(index < lines.Length -1)
        {
            index++;
            textComponent.text = lines[index];
        }
        else
        {
            inUse=false;
            //gameObject.SetActive(false);
        }
    }
    private void NextLineMonologue()
    {
        if(index < lines.Length -1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            inUse=false;
            //gameObject.SetActive(false);
        }
    }
}
