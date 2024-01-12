using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CleansingFlameCard", menuName = "ScriptableObject/Card/AbilityCard/CleansingFlame")]
public class CleansingFlameCardObject : AbilityCardObject {

    public override void Ability() {
        Debug.Log("CleansingFlame");
    }

}
