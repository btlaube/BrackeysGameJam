using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionsManager : MonoBehaviour {
    
    [SerializeField] private UnityEvent actionCompleted;
    [SerializeField] private UnityEvent endTurn;

    public void OnActionCompleted() {
        actionCompleted.Invoke();
    }

    public void OnEndTurn() {
        endTurn.Invoke();
    }

}
