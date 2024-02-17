using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeReference]
    private Inventory inventory;
    public KeyCode interactionKey;
    public bool scrollUpEnabled { get; private set; }
    public bool scrollDownEnabled { get; private set; }
    public bool interactionEnabled { get; private set; }
    public bool flashEnabled { get; private set; }
    public bool flashingMonster { get; private set; }

    public GameObject monsterObject;

    [SerializeReference]
    private GameObject flashlight;

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

    private bool FacingMonster()
    {
        //Calculate Flashlight Direction Unit Vector
        double flashlightAngle = flashlight.transform.eulerAngles.y * Math.PI / 180;
        double xComponent = Math.Sin(flashlightAngle);
        double zComponent = Math.Cos(flashlightAngle);
        Vector2 flashlightUnitVec2D = new Vector2((float)xComponent, (float)zComponent);

        //Calculate Player-to-Monster Unit Vector
        Vector3 monsterLoc = monsterObject.transform.position - flashlight.transform.position;
        monsterLoc.Normalize();
        Vector2 monsterUnitVec2D = new Vector2(monsterLoc.x, monsterLoc.z);

        //Dot product the two vectors to see if the player is facing the monster
        return Vector3.Dot(flashlightUnitVec2D, monsterUnitVec2D) > 0.97;
    }

    private void GetInputs()
    {
        interactionEnabled = Input.GetKeyDown(interactionKey);
        int scrollResult = Scroll();

        if (scrollResult > 0){scrollUpEnabled = true;
        }else{scrollUpEnabled = false;}

       if (scrollResult < 0){scrollDownEnabled = true; }
        else { scrollDownEnabled = false; }

       if (Input.GetMouseButton((int) MouseButton.Right) == true){ flashEnabled = true; }
       else { flashEnabled = false; }

    }

    #endregion

    #region handle input
    private void HandleInputs()
    {
        if (scrollDownEnabled){ inventory.SelectPreviousSlot();}
        if(scrollUpEnabled   ){ inventory.SelectNextSlot();}

        flashingMonster = flashEnabled && FacingMonster(); // are we facing the monster and flashing?
        flashlight.SetActive(flashEnabled);                // turn on flashlight if enabled
    }

    #endregion

    void Update()
    {
        GetInputs();
        HandleInputs();
    }
}
