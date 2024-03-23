using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    private DoorThings door;

    [SetUp]
    public void SetUp()
    {
        var doorObj = new GameObject("TestDoor");
        door = doorObj.AddComponent<DoorThings>();
    }

    // Test if the door opens when the player is nearby and 'E' is pressed.
    [UnityTest]
    public IEnumerator DoorOpensWhenPlayerIsNearbyAndEIsPressed()
    {
        door.TestInitialize(true, false);
        door.Update();
        yield return null;
        Assert.IsTrue(door.isOpen);
    }

    // Test if the door closes when it's open and 'E' is pressed again.
    [UnityTest]
    public IEnumerator DoorClosesWhenOpenAndEIsPressed()
    {
        door.TestInitialize(true, true);
        door.Update();
        yield return null;
        Assert.IsFalse(door.isOpen);
    }

    // Test if the door remains closed when the player is not nearby.
    [UnityTest]
    public IEnumerator DoorRemainsClosedWhenPlayerIsNotNearby()
    {
        door.TestInitialize(false, false);
        door.Update();
        yield return null;
        Assert.IsFalse(door.isOpen);
    }

    // Test if the door does not change its state when other keys are pressed.
    [UnityTest]
    public IEnumerator DoorDoesNotChangeStateOnOtherKeys()
    {
        door.TestInitialize(true, true);
        door.UpdateWithOtherKey(KeyCode.A);
        yield return null;
        Assert.IsTrue(door.isOpen);
    }
}
