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
    void AddIngredient(){
        // add something that allows the user to add things tot the burger
    }
}