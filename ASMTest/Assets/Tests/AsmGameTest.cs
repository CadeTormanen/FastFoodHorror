using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AsmGameTests
{
    private AsmGame game;

    [SetUp]
    public void SetUpTestGame()
    {
        var gameObj = new GameObject("TestGame");
        game = gameObj.AddComponent<AsmGame>();
    }

    // Test if adding a single ingredient updates the game state correctly.
    [UnityTest]
    public IEnumerator TestAddIngredient_CreatesIngredient()
    {
        game.StartNewBurger();
        yield return null;

        game.AddIngredient("lettuce");
        yield return null;

        Assert.IsTrue(game.HasIngredient("lettuce"), "Ingredient lettuce was not added to the burger.");
    }

    // Test if resetting the game resets the score to zero.
    [UnityTest]
    public IEnumerator TestResettingScore_ResetsScoreToZero()
    {
        game.AddScore(5);
        yield return null;

        game.ResetGame();
        yield return null;

        Assert.AreEqual(0, game.GetScore(), "Score was not reset to zero.");
    }

    // Test if adding multiple ingredients updates the game state correctly.
    [UnityTest]
    public IEnumerator TestAddingMultipleIngredients_CreatesMultipleIngredients()
    {
        game.StartNewBurger();
        yield return null;

        game.AddIngredient("lettuce");
        game.AddIngredient("tomato");
        yield return new WaitForSeconds(0.2f);

        int ingredientCount = 0;
        foreach (Transform child in game.GetCurrentBurg().transform)
        {
            if (child.name.Contains("LettucePrefab") || child.name.Contains("TomatoPrefab"))
            {
                ingredientCount++;
            }
        }

        Assert.AreEqual(2, ingredientCount, "Not all ingredients were added to the burger.");
    }

    // Test if finishing a burger plays a sound.
    [UnityTest]
    public IEnumerator TestFinishBurger_PlaysSound()
    {
        game.StartNewBurger();
        yield return null;

        game.audioSource = game.gameObject.AddComponent<AudioSource>();
        game.dingSound = AudioClip.Create("TestClip", 44100, 1, 44100, false);

        game.FinishBurger();
        yield return null;

        Assert.IsTrue(game.audioSource.isPlaying, "The sound was not played.");
    }
}
