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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

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
    }void DisplayReadables(string title)
    {
        if (readables.ContainsKey(title))
        {
            storyChain.lines[0]=readables[title];
            storyChain.StartMonologue();  
        }
    }
}
