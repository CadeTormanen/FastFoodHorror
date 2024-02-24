using UnityEngine;

public class BurgerAsm : monoBehaviour{
    public GameObject[] ingredients;// Where we add the ingredients
    private GameObject currentBurg; // Sets the current burger the player is working on
    //New Borger 
    void Start(){
        StartNewBurger();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){
            AddIngredient;
        }
    }
    // once done with one the new burger appears
    void StartNewBurger(){
        if(currentBurg != null){
            Destroy(currentBurg);
        }
        currentBurg = new GameObject("Burger");
    }
     public void AddIngredient(string ingredientName){
    
        foreach (Ingredient ingredient in ingredients){
        
            if (ingredient.ingredientName.Equals(ingredientName, System.StringComparison.OrdinalIgnoreCase))
            {
                Vector3 spawnPosition = currentBurger.transform.childCount > 0 ? 
                    spawnPoint.position + new Vector3(0, 0.05f, 0) : spawnPoint.position;

                Instantiate(ingredient.prefab, spawnPosition, Quaternion.identity, currentBurger.transform);

                if (ingredientName.ToLower().Contains("topbun")){
                    FinishBurger();
                }
                return; 
            }
        }
    }
}