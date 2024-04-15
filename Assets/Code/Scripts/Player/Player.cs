using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PLAYERSTATES
    {
        free,
        dialogue,
        cutscene,
        jumpscare
    }

    public enum SWEEPSTATES
    {
        idle,
        sweeping
    }

    public SWEEPSTATES sweepState;

    
    [SerializeField] private InteractionController interactionController;
    [SerializeReference] private Inventory inventory;

    public PLAYERSTATES state       { get; set; }
    public bool scrollUpEnabled     { get; private set; }
    public bool scrollDownEnabled   { get; private set; }
    public bool interactionEnabled  { get; private set; }
    public bool flashEnabled        { get; private set; }
    public bool flashingMonster     { get; private set; }
    public bool dropEnabled         { get; private set; }
    public bool sweepEnabled        { get; private set; }

    public KeyCode interactionKey;
    public KeyCode dropKey;
    private int numberKeyDown;

    [SerializeReference]
    private GameObject monsterObject;

    [SerializeReference]
    private GameObject flashlight;

    [SerializeReference]
    public GameObject broomObject;

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

    private bool ViewingMonster()
    {
        if (monsterObject == null) { Debug.LogWarning("No monster object found"); return false; }
        if (flashlight    == null) { Debug.LogWarning("No flashlight objet found") ; return false; }
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit h = hits[i];
            Renderer rend = h.transform.GetComponent<Renderer>();

            if (rend)
            {
                // Change the material of all hit colliders
                // to use a transparent shader.
                rend.material.shader = Shader.Find("Transparent/Diffuse");
                Color tempColor = rend.material.color;
                tempColor.a = 0.3F;
                rend.material.color = tempColor;
            }
        }

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
        bool facing =  Vector3.Dot(flashlightUnitVec2D, monsterUnitVec2D) > 0.97;
        if (!facing) { return false; }

        //Check if we have actual line of sight to the monster
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit);

        if (hit.collider == null)                     {return false;}
        if (hit.collider.gameObject.name != "Circler"){return false;}
        

        return true;
    }


    private void GetInputs()
    {
        
        interactionEnabled = Input.GetKeyDown(interactionKey);
        int scrollResult = Scroll();
        dropEnabled = Input.GetKeyDown(dropKey);


        if (scrollResult > 0){scrollUpEnabled = true;
        }else{scrollUpEnabled = false;}

       if (scrollResult < 0){scrollDownEnabled = true; }
        else { scrollDownEnabled = false; }

       if (Input.GetMouseButton((int) MouseButton.Right) == true){ flashEnabled = true; }
       else { flashEnabled = false; }

        sweepEnabled = Input.GetMouseButton((int)MouseButton.Left);

        numberKeyDown = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1)) { numberKeyDown = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { numberKeyDown = 2; };
        if (Input.GetKeyDown(KeyCode.Alpha3)) { numberKeyDown = 3; };
        if (Input.GetKeyDown(KeyCode.Alpha4)) { numberKeyDown = 4; };
        if (Input.GetKeyDown(KeyCode.Alpha5)) { numberKeyDown = 5; };
        if (Input.GetKeyDown(KeyCode.Alpha6)) { numberKeyDown = 6; };
        if (Input.GetKeyDown(KeyCode.Alpha7)) { numberKeyDown = 7; };
        if (Input.GetKeyDown(KeyCode.Alpha8)) { numberKeyDown = 8; };

    }

    #endregion

    #region handle input
    private void HandleInputs()
    {
        if (flashlight != null)  { flashlight.SetActive(flashEnabled);          }
        if (numberKeyDown != -1) { inventory.slotSelected = numberKeyDown-1;    }
        if (dropEnabled)         { inventory.DropItem(-1,transform.position);   }
        if (scrollUpEnabled)   { interactionController.PreviousInteraction();   }
        if (scrollDownEnabled) { interactionController.NextInteraction();       }
        flashingMonster = flashEnabled && ViewingMonster();

        if (sweepState == SWEEPSTATES.idle){
            if (sweepEnabled) { 
                sweepState = SWEEPSTATES.sweeping;
                broomObject.GetComponent<Animation>().Play();
            }
        }else if (sweepState == SWEEPSTATES.sweeping){
            if (broomObject.GetComponent<Animation>().isPlaying == false){
                sweepState = SWEEPSTATES.idle;
            }
        }
       
    }

    #endregion

   
    public Vector3 GetBroomHeadPosition()
    {
        return broomObject.transform.position;
    }
    
    
    private void Start()
    {
        state       = PLAYERSTATES.free;
        sweepState  = SWEEPSTATES.idle;
    }

    private void Update(){
        GetInputs();

        switch (state)
        {
            case (PLAYERSTATES.free):
                this.GetComponent<FirstPersonMovement>().active = true;
                HandleInputs();
                break;
            case (PLAYERSTATES.dialogue):
                this.GetComponent<FirstPersonMovement>().active = false;
                break;
            case (PLAYERSTATES.cutscene):
                this.GetComponent<FirstPersonMovement>().active = false;
                break;
            case (PLAYERSTATES.jumpscare):
                this.GetComponent<FirstPersonMovement>().active = false;

                break;
        }
    }
}