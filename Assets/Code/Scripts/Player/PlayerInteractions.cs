using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public KeyCode interactionKey;
    public bool scrollUp;
    public bool scrollDown;
    public bool interactionEnabled;

    void Update()
    {
        if (Input.GetKeyDown(interactionKey)){
            interactionEnabled = true;
        }else{
            interactionEnabled = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)){
            scrollUp = true;
        }
        else
        {
            scrollUp = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            scrollDown = true;
        }
        else
        {
            scrollDown = false;
        }
    }
}
