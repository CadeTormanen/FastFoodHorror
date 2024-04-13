using System;
using UnityEngine;

public class Ingredient
{
    public string ingredientName;
    public GameObject prefab;
}

public class AsmGame : MonoBehaviour{

    public Ingredient[] ingredients;
    private GameObject currentBurg;
    private int score = 0;
    public GameObject topBunPrefab;
    public GameObject lettucePrefab;
    public GameObject pattyPrefab;
    public GameObject tomatoesPrefab;
    public GameObject ketchupPrefab;
    public GameObject mustardPrefab;
    public GameObject bottomBunPrefab;
    public Transform spawnPoint;
    public AudioSource audioSource;
    public AudioClip dingSound;

    void Start()
    {
        // Initialize ingredients array
        ingredients = new Ingredient[] {
            new Ingredient() { ingredientName = "top bun", prefab = topBunPrefab },
            new Ingredient() { ingredientName = "bottom bun", prefab = bottomBunPrefab },
            new Ingredient() { ingredientName = "lettuce", prefab = lettucePrefab },
            new Ingredient() { ingredientName = "patty", prefab = pattyPrefab },
            new Ingredient() { ingredientName = "ketchup", prefab = ketchupPrefab },
            new Ingredient() { ingredientName = "mustard", prefab = mustardPrefab }
        };
        currentBurg = new GameObject("Burger");
        currentBurg.transform.position = spawnPoint.position; // Start with an empty burger at the spawn point
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddIngredient("top bun");
        }
    }

    public GameObject GetCurrentBurg()
    {
        return currentBurg;
    }

    public void AddIngredient(string ingredientName)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.ingredientName.Equals(ingredientName, StringComparison.OrdinalIgnoreCase))
            {
                Vector3 spawnPosition = currentBurg.transform.childCount > 0 ? currentBurg.transform.position + new Vector3(0, 0.05f * currentBurg.transform.childCount, 0) : spawnPoint.position;
                Instantiate(ingredient.prefab, spawnPosition, Quaternion.identity, currentBurg.transform);

                if (ingredientName.Equals("top bun", StringComparison.OrdinalIgnoreCase))
                {
                    FinishBurger();
                }
                return;
            }
        }
    }

    public void FinishBurger()
    {
        AddScore(1); // Add one to the score
        Debug.Log("Score: " + score); // Log score

        // Play the ding sound
        if (audioSource != null && dingSound != null)
        {
            audioSource.PlayOneShot(dingSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing.");
        }

        currentBurg = new GameObject("Burger"); // Start a new burger after finishing the current one
        currentBurg.transform.position = spawnPoint.position; // Reset position for new burger
    }
    public int GetScore()
    {
        return score;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}
