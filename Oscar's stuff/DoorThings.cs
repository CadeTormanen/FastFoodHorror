using UnityEngine;

public class DoorFunc : DoorBehav

private bool isOpen = false;

void start(){
    // this is where I would put the animation stuff
}

void update(){
    if(Input.GetKeyDown(KeyCode.E)){
        isOpen = !isOpen;
    }
}