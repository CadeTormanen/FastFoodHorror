using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeReference]
    private Inventory inventory;

    public KeyCode interactionKey;
    public bool scrollUpEnabled;
    public bool scrollDownEnabled;
    public bool interactionEnabled;

    #region get input

    //Returns the scroll direction. 0 is none | -1 is down | 1 is up.
    private int Scroll()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            return 1;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            return -1;
        }
        return 0;
    }

    private void GetInputs()
    {
        interactionEnabled = Input.GetKeyDown(interactionKey);
        int scrollResult = Scroll();

        if (scrollResult > 0){scrollUpEnabled = true;
        }else{scrollUpEnabled = false;}

       if (scrollResult < 0){scrollDownEnabled = true; }
        else { scrollDownEnabled = false; }
    }

    #endregion

    #region handle input
    private void HandleInputs()
    {
        if (scrollDownEnabled){ inventory.SelectPreviousSlot();}
        if(scrollUpEnabled   ){ inventory.SelectNextSlot();    }
    }

    #endregion

    void Update()
    {
        GetInputs();
        HandleInputs();
    }
}
