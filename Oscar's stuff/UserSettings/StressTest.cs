using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

public class stressTest
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

    [UnityTest]
    public IEnumerator StressIncreasesCorrectly()
    {
        // Test increasing stress works within bounds
        playerStress.OnTaskFail(30f);
        yield return null; // Wait for one frame for the change to potentially apply
        Assert.AreEqual(30f, playerStress.GetCurrentStress()); // Check if stress increased correctly
    }

    [UnityTest]
    public IEnumerator StressDecreasesCorrectly()
    {
        // Test decreasing stress works within bounds
        playerStress.IncreaseStress(50f); // Increase stress first to then decrease it
        yield return null; // Wait for one frame for the increase to apply
        playerStress.OnTaskPass(20f);
        yield return null; // Wait for one frame for the decrease to apply
        Assert.AreEqual(30f, playerStress.GetCurrentStress()); // Check if stress decreased correctly
    }

    [UnityTest]
    public IEnumerator StressDoesNotExceedMaximum()
    {
        // Test stress does not exceed maximum
        playerStress.OnTaskFail(150f); // Exceed max stress
        yield return null; // Wait for one frame for the change to potentially apply
        Assert.AreEqual(playerStress.maxStress, playerStress.GetCurrentStress()); // Check if stress does not exceed maximum
    }

    [UnityTest]
    public IEnumerator StressDoesNotFallBelowZero()
    {
        // Test stress does not fall below zero
        playerStress.OnTaskPass(50f); // Try to decrease stress below zero
        yield return null; // Wait for one frame for the change to potentially apply
        Assert.AreEqual(0f, playerStress.GetCurrentStress()); // Check if stress does not fall below zero

    }
    }
