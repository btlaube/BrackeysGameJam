using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    
    public CardObject cardObject;
    [SerializeField] private Image cardFace;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject useCardText;
    [SerializeField] private GameObject discardCard;

    //Contol card size in layout
    [SerializeField] private LayoutElement layoutElement;

    //Info panel manager
    [SerializeField] private InfoPanelManager infoPanelManager;

    //Other references
    [SerializeField] private NewBoardManager board;
    [SerializeField] private KeyDeckBehavior keyDeck;

    void Awake() {
        infoPanelManager = GameObject.Find("Info Panel Group").GetComponent<InfoPanelManager>();
        board = GameObject.Find("Board").GetComponent<NewBoardManager>();
        keyDeck = GameObject.Find("Key Card Group").GetComponent<KeyDeckBehavior>();
    }

    void Start() {
        cardFace.sprite = cardObject.cardFace;
        cardNameText.text = cardObject.cardName;
        descriptionText.text = cardObject.description;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        layoutElement.minWidth = 300;
        layoutElement.minHeight = 450;
        if (cardObject.name == "WaterKeyCard" || cardObject.name == "EarthKeyCard" || cardObject.name == "WindKeyCard" || cardObject.name == "FireKeyCard") {
            discardCard.SetActive(true);
        }
        else if (cardObject.name == "MightyFlightCard" || cardObject.name == "CleansingFlameCard") {
            useCardText.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        layoutElement.minWidth = 200;
        layoutElement.minHeight = 300;
        discardCard.SetActive(false);
        useCardText.SetActive(false);
    }

    public void ShowInfoPanel() {
        if (cardObject.name == "WaterKeyCard" || cardObject.name == "EarthKeyCard" || cardObject.name == "WindKeyCard" || cardObject.name == "FireKeyCard") {
            infoPanelManager.ShowKeyCardInfoPanel();
        }
        else if (cardObject.name == "MightyFlightCard") {
            infoPanelManager.ShowMightFlightCardInfoPanel();
        }
        else if (cardObject.name == "CleansingFlameCard") {
            infoPanelManager.ShowCleansingFireCardInfoPanel();
        }
    }

    public void UseCard() {
        if (cardObject.name == "WaterKeyCard" || cardObject.name == "EarthKeyCard" || cardObject.name == "WindKeyCard" || cardObject.name == "FireKeyCard") {
            keyDeck.DiscardCard(cardObject);
        }
        if (cardObject.name == "MightyFlightCard") {
            board.UseMightyFlightCard();
        }
        else if (cardObject.name == "CleansingFlameCard") {
            board.UseCleansingFlameCard();
        }
        Destroy(gameObject);
    }

}
