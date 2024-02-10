using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interaction
{
    public string interactionText { get; set; }
    void ExecuteInteraction();  //Activate the interaction that is currently possible
    void ValidateInteraction(); //Assert that the interaction was defined properly in the inspector
    bool Possible();            //Should the interaction be active right now? true/false. Also sets the string which should be displayed
    
    
}
