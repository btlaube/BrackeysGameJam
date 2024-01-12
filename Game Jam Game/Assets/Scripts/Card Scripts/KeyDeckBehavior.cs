using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class KeyDeckBehavior : MonoBehaviour {
    
    [SerializeField] private List<CardObject> drawPile = new List<CardObject>();
    [SerializeField] private List<CardObject> discardPile = new List<CardObject>();
    [SerializeField] public List<CardObject> hand = new List<CardObject>();

    //Control card display
    [SerializeField] private Image deckImage;
    [SerializeField] private Image discardImage;
    [SerializeField] private Sprite deckSprite;
    [SerializeField] private Sprite emptyDeckSprite;
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text descriptionText;

    public GameObject cardPrefab;
    public Transform handParent;

    private GameController gameController;

    //Control limiting number of cards player draws per draw phase
    [SerializeField] private UnityEvent cardDrawn;
    public int numCardsInHand;

    void Start() {
        gameController = GameController.instance;
    }

    public void DrawCard() {
        if (drawPile.Count > 0) {
            CardObject randCard = drawPile[Random.Range(0, drawPile.Count)];
            drawPile.Remove(randCard);

            //Check if card is a Corruption Spreads card before adding it to the deck
            if (randCard.name == "CorruptionSpreads") {
                gameController.corruptionLevel++;
            }
            else {
                hand.Add(randCard);
            }
            
            //Invoke onCardDrawn event
            numCardsInHand = hand.Count;
            cardDrawn.Invoke();

            //Set deck to display card drawn
            deckImage.sprite = randCard.cardFace;
            cardNameText.text = randCard.cardName;
            descriptionText.text = randCard.description;

            //Check if card is a Corruption Spreads card before adding it to the deck
            if (randCard.name != "CorruptionSpreads") {
                StartCoroutine(AddToHand(randCard));
            }
        }
    }

    public void DrawCardStartingHand() {
        //TODO change this to not draw any corruption cards
        if (drawPile.Count > 0) {
            CardObject randCard = drawPile[Random.Range(0, drawPile.Count)];

            //Prevent corruption spreads cards from being draw when dealing
            while (randCard.name == "CorruptionSpreads") {
                randCard = drawPile[Random.Range(0, drawPile.Count)];
            }

            drawPile.Remove(randCard);
            hand.Add(randCard);

            //Set deck to display card drawn
            deckImage.sprite = randCard.cardFace;
            cardNameText.text = randCard.cardName;
            descriptionText.text = randCard.description;

            //Check if card is a Corruption Spreads card before adding it to the deck
            StartCoroutine(AddToHand(randCard));
            
        }
    }

    IEnumerator AddToHand(CardObject newCard) {
        yield return new WaitForSeconds(1f);
        //Reset deck display
        deckImage.sprite = deckSprite;
        cardNameText.text = "";
        descriptionText.text = "";

        GameObject newCardObject = Instantiate(cardPrefab, handParent);
        newCardObject.GetComponent<CardBehavior>().cardObject = newCard;

        if (drawPile.Count == 0) {
            ShuffleDiscard();
        }
    }

    public void DiscardCard(CardObject card) {
        hand.Remove(card);
        discardPile.Add(card);

        discardImage.sprite = deckSprite;
    }

    public void ShuffleDiscard() {
        foreach (KeyCardObject card in discardPile) {
            drawPile.Add(card);
        }
        discardPile.Clear();
        //Change discard pile sprite to deck placeholder sprite
        discardImage.sprite = emptyDeckSprite;
    }

    public void OnCardDrawn() {
        cardDrawn.Invoke();
    }

}
