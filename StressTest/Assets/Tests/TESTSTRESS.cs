using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TESTSTRESS
{
    private PlayerStress playerStress;
    private GameObject gameObject;

    [SetUp]
    public void Init()
    {
        gameObject = new GameObject();
        playerStress = gameObject.AddComponent<PlayerStress>();
        playerStress.stressMeter = new GameObject().AddComponent<Slider>();
    }

    // Test if the player's stress increases correctly after failing a task.
    [UnityTest]
    public IEnumerator StressIncreasesCorrectly()
    {
        playerStress.OnTaskFail(30f);
        yield return null;
        Assert.AreEqual(30f, playerStress.GetCurrentStress());
    }

    // Test if the player's stress decreases correctly after passing a task.
    [UnityTest]
    public IEnumerator StressDecreasesCorrectly()
    {
        playerStress.IncreaseStress(50f);
        yield return null;
        playerStress.OnTaskPass(20f);
        yield return null;
        Assert.AreEqual(30f, playerStress.GetCurrentStress());
    }

    // Test if the player's stress does not exceed the maximum limit.
    [UnityTest]
    public IEnumerator StressDoesNotExceedMaximum()
    {
        playerStress.OnTaskFail(150f);
        yield return null;
        Assert.AreEqual(playerStress.maxStress, playerStress.GetCurrentStress());
    }

    // Test if the player's stress does not fall below zero.
    [UnityTest]
    public IEnumerator StressDoesNotFallBelowZero()
    {
        playerStress.OnTaskPass(50f); // Reduce stress more than its current amount
        yield return null;
        Assert.AreEqual(0f, playerStress.GetCurrentStress());
    }
}
