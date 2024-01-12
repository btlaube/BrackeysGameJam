using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelManager : MonoBehaviour {
    
    //Info panels
    [SerializeField] private GameObject keyCardInfoPanel;
    [SerializeField] private GameObject mightFlightCardInfoPanel;
    [SerializeField] private GameObject cleansingFireCardInfoPanel;

    //Reference to board manager
    [SerializeField] private NewBoardManager boardManager;

    public void ShowKeyCardInfoPanel() {
        keyCardInfoPanel.SetActive(true);
    }

    public void ShowMightFlightCardInfoPanel() {
        mightFlightCardInfoPanel.SetActive(true);
    }

    public void ShowCleansingFireCardInfoPanel() {
        cleansingFireCardInfoPanel.SetActive(true);
    }

    public void HideKeyCardInfoPanel() {
        keyCardInfoPanel.SetActive(false);
    }

    public void HideMightFlightCardInfoPanel() {
        mightFlightCardInfoPanel.SetActive(false);
    }

    public void HideCleansingFireCardInfoPanel() {
        cleansingFireCardInfoPanel.SetActive(false);
    }

}
