using UnityEngine;
using UnityEngine.UI;
// Once I get the UI made I will be adding mroe things
/*This is the an outline of how the meter will work with more
of the stuff we work on.
*/
public class PlayerStress: MonoBehaviour{
    public Slider stressMeter;
    public float maxStress = 100f;
    private float currentStress = 0f;

void Update{
    // add things here for increase and decrease
    // that way it stays constant to what the stress should be and add other things
}

public void OnTaskFail(float stressAmount){
    IncreaseStress(stressAmount);
}

public void OnTaskPass(float stressRelief){
    DecreaseStress(stressRelief);
}
public void IncreaseStress(float amount){
    currentStress += amount;
    currentStress = Mathf.Clamp(currentStress, 0, maxStress); // sets a cap at 100 for stress
    UpdateStressMeter(); // Lets the UI know to change
}

public void DecreaseStress(float amount){
    currentStress -= amount;
    currentStress = Mathf.Clamp(currentStress, 0, maxStress); // sets a cap at 100 for stress
    UpdateStressMeter();
}
//updates the UI to match the current level of stres
void UpdateStressMeter(){
    if(stressMeter != null){
        stressMeter.value = currentStress / maxStress;
    }
}
}