using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MightyFlightCard", menuName = "ScriptableObject/Card/AbilityCard/MightyFlight")]
public class MightyFlightCardObject : AbilityCardObject {
    
    public override void Ability() {
        Debug.Log("MightyFlight");
    }

}
