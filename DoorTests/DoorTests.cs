using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


public class DoorThingsTests

{
    

    [UnityTest]
    public IEnumerator DoorOpensWhenPlayerIsNearbyAndEIsPressed()
    {
        var door = new DoorThings();
        door.TestInitialize(true, false); // Assuming you have a method to set initial conditions for the test
        door.Update(); // Simulate pressing 'E' when the player is nearby
        yield return null; // Wait for a frame to ensure the update is processed
        Assert.IsTrue(door.isOpen); // Test if door opens correctly
    }

    [UnityTest]
    public IEnumerator DoorClosesWhenOpenAndEIsPressed()
    {
        var door = new DoorThings();
        door.TestInitialize(true, true); // Door is initially open
        door.Update(); // Simulate pressing 'E' again
        yield return null; // Wait for a frame to ensure the update is processed
        Assert.IsFalse(door.isOpen); // Test if door closes correctly
    }

    [UnityTest]
    public IEnumerator DoorRemainsClosedWhenPlayerIsNotNearby()
    {
        var door = new DoorThings();
        door.TestInitialize(false, false); // Player is not nearby
        door.Update(); // Attempt to open the door
        yield return null; // Wait for a frame to ensure the update is processed
        Assert.IsFalse(door.isOpen); // Ensure door remains closed
    }

    [UnityTest]
    public IEnumerator DoorDoesNotChangeStateOnOtherKeys()
    {
        var door = new DoorThings();
        door.TestInitialize(true, true); // Door is initially open, player is nearby
                                         // Simulate pressing a different key, for example, KeyCode.A
        door.UpdateWithOtherKey(KeyCode.A); // Simulate pressing a key that should not affect the door state
        yield return null; // Wait for a frame to ensure the update is processed
        Assert.IsTrue(door.isOpen); // Ensure the door state remains unchanged (still open)
    }
}
internal class UnityTestAttribute : Attribute
{
}