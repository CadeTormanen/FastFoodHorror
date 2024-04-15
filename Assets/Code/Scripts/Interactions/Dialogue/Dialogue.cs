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
    //public int sizeOfChain;

    // Start is called before the first frame update
    void Start()
    {
        //for empty string to type into:
        textComponent.text = string.Empty;
        //StartMonologue();
        StartMonologue();
        inUse=true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    public void StartDialogue()
    {
        if (inUse)return;
        index = 0;
        gameObject.SetActive(true);
        textComponent.text = lines[index];
        inUse=true;
        diamode=true;
    }
    public void StartMonologue()
    {
        if (inUse) return;
        index = 0;
        gameObject.SetActive(true);
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
