using System;
using UnityEngine;

public class DoorThings : MonoBehaviour
{
    public bool isOpen = false;
    public bool isPlayerClose = false;

    
    public  void TestInitialize(bool initialOpenState, bool initialPlayerCloseState)
    {
        // Initialize the door state and player proximity for testing purposes
        isOpen = initialOpenState;
        isPlayerClose = initialPlayerCloseState;
    }

    void Start()
    {
    }

    public void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
        }
    }

    protected void AnimateDoor(bool open)
    {
        // Implementation of door animation.
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        AnimateDoor(isOpen); // Call AnimateDoor to handle the door's opening and closing animations.
    }

    // Conceptual place for a method similar to UpdateOtherKey
    public void UpdateWithOtherKey(KeyCode key)
    {
        // This method would be used for testing and is not called during normal gameplay.
        // In a testing framework, you'd simulate pressing a different key here.
        // For illustrative purposes:
        if (key != KeyCode.E && isPlayerClose) // Check if the pressed key is NOT the door control key and if the player is close
        {
            
            
        }

    }

   
}
