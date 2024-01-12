using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorruptionDeckBehavior : MonoBehaviour {
    
    //Lists keep track of cards that have been drawn and which have not
    [SerializeField] List<GameObject> drawPile = new List<GameObject>();
    [SerializeField] List<GameObject> discardPile = new List<GameObject>();

    //Text field to show name of location that was just drawn
    [SerializeField] private TMP_Text locationNameText;
    [SerializeField] private TMP_Text discardPileText;

    [SerializeField] private Image drawPileImage;
    [SerializeField] private Image discardPileImage;
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private Sprite emptyDeckSprite;

    public void DrawCard() {
        //If drawPile has cards in it
        if (drawPile.Count > 0) {
            GameObject randTile = drawPile[Random.Range(0, drawPile.Count)];
            drawPile.Remove(randTile);
            discardPile.Add(randTile);

            //Show location name on corruption pile draw deck
            locationNameText.text = randTile.GetComponent<NewTileBehavior>().locationName;
            StartCoroutine(DiscardCard(randTile));
        }
    }

    IEnumerator DiscardCard(GameObject tile) {
        //Show card for 1 second before clearing it
        yield return new WaitForSeconds(1f);
        //Make discard pile show what card was just discarded
        discardPileText.text = locationNameText.text;
        
        locationNameText.text = "";
        tile.GetComponent<NewTileBehavior>().Corrupt();

        //Change discard pile sprite to back of card sprite
        discardPileImage.sprite = cardSprite;

        //Check if that was the last card drawn
            //If it was, shuffle the discard into the draw
        if (drawPile.Count == 0) {
            locationNameText.text = "";
            ShuffleDiscard();
        }
    }

    public void ShuffleDiscard() {
        //Debug.Log("Shuffled Corruption Card discard pile into Corruption Card draw pile");
        foreach (GameObject card in discardPile) {
            drawPile.Add(card);
        }
        discardPile.Clear();
        //Change discard pile sprite to deck placeholder sprite
        discardPileImage.sprite = emptyDeckSprite;
        discardPileText.text = "";
    }

    public void ResetDeck() {

    }

}
