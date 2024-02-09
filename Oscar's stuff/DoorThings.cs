using UnityEngine;

public class DoorFunc : DoorBehav

private bool isOpen = false;
private bool isPlayerClose = false;

void start(){
    base.Start();
}

void update(){
    if(isPlayerClose && Input.GetKeyDown(KeyCode.E)){
        isOpen = !isOpen;
    }
}

private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    
    protected void AnimateDoor(bool open){
        // Implementation of door animation
       
    }
