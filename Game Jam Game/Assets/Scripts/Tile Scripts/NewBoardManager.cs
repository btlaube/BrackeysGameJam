using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NewBoardManager : MonoBehaviour {

    //List of tiles in board
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();
    //Control layout group controlling layout of tiles
    [SerializeField] private GridLayoutGroup layoutGroup;

    [SerializeField] private  GameController gameController;
    //The Object representing where the player (and the birds they have saved) is on the board
    [SerializeField] private GameObject birdGroup;
    //Control placement of elemental birds at start
    [SerializeField] private GameObject burrowingOwl;
    [SerializeField] private GameObject falcon;
    [SerializeField] private GameObject albatross;
    //When resetting the board, do not Flip first if it is the first set up
    private bool firstBoardSetUp = true;

    //To check player's hand
    [SerializeField] private KeyDeckBehavior keyDeck;

    //Game event to message game controller when board is set up
    [SerializeField] private UnityEvent onBoardSetUpFinished;

    //Game event to signal action completed
    [SerializeField] private UnityEvent actionCompleted;

    public void SetUpRound() {
        StartCoroutine(SetUpRoundEnum());
    }

    IEnumerator SetUpRoundEnum() {
        ResetBoardCorruption();
        if (!firstBoardSetUp) {
            StartCoroutine(FlipBoardEnum());
            yield return new WaitForSeconds(2.5f);
        }
        firstBoardSetUp = false;
        RandomizeBoard();

        //Set player index to Phoenix Cage tile
        gameController.playerIndex = tiles.FindIndex(tile => tile.name == "Phoenix Cage");
        ////Set birdGroup as child of player index Tile once board is flipped
        Invoke("SetBirdGroupParent", 3f);
        Invoke("SetElementalBirdParents", 3f);
        StartCoroutine(FlipBoardEnum());

        //Start initial corruption card draw process in game controller
        //gameController.CorruptionBegins();
    }

    IEnumerator FlipBoardEnum() {
        foreach (Transform tile in transform) {
            yield return new WaitForSeconds(0.1f);
            tile.GetComponent<NewTileBehavior>().Flip();
        }
        yield return new WaitForSeconds(1f);
        onBoardSetUpFinished.Invoke();
    }

    private void SetBirdGroupParent() {
        birdGroup.transform.SetParent(transform.GetChild(gameController.playerIndex));
        birdGroup.transform.position = birdGroup.transform.parent.position;
    }

    public void SetElementalBirdParents() {
        burrowingOwl.transform.SetParent(transform.GetChild(tiles.FindIndex(tile => tile.name == "Burrowing Owl Cage")));
        burrowingOwl.transform.localPosition = new Vector2(-47, -13);

        falcon.transform.SetParent(transform.GetChild(tiles.FindIndex(tile => tile.name == "Falcon Cage")));
        falcon.transform.localPosition = new Vector2(-47, -13);

        albatross.transform.SetParent(transform.GetChild(tiles.FindIndex(tile => tile.name == "Albatross Cage")));
        albatross.transform.localPosition = new Vector2(-47, -13);
        
    }

    private void RandomizeBoard() {
        layoutGroup.enabled = true;
        foreach (GameObject tile in tiles) {
            tile.transform.SetSiblingIndex(Random.Range(0, tiles.Count));
        }
        //Reset Tile list to match new order
        tiles = new List<GameObject>();
        foreach (Transform tile in transform) {
            tiles.Add(tile.gameObject);
        }
        Invoke("DisableLayoutGroup", 0.5f);
    }

    private void DisableLayoutGroup() {
        layoutGroup.enabled = false;
    }

    public void ResetBoardCorruption() {
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().CleanseHere();
        }
    }

    public void Move() {
        gameController.ShowTempMessage("Select a tile to fly to");
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().ShadeTile();
        }
        tiles[gameController.playerIndex].GetComponent<NewTileBehavior>().UnshadeTile();
        switch (gameController.playerIndex) {
            case 0:
                //Top left corner of grid, activate tiles to right and below
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 1:
            case 2:
                //Two top edge tiles, activate tile to right, left, and below
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 3:
                //top right corner of grid, activate tiles to left and below
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 4:
            case 8:
                //Left two edge tiles, activate above, below, and right
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 5:
            case 6:
            case 9:
            case 10:
                //Four center tiles, activate all adjacent tiles
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 7:
            case 11:
                //Right two edge tiles, activate above, below, and left
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 12:
                //Bottom left corner, activate above and right
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 13:
            case 14:
                //Bottom two edge tiles, activate left, right, and above
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 15:
                //Bottom right corner, activate above and left
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateMoveButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
        }
    }

    public void CompleteMove() {
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().DeactivateMoveButton();
            tile.GetComponent<NewTileBehavior>().UnshadeTile();
        }
    }

    public void Cleanse() {
        gameController.ShowTempMessage("Select a tile to cleanse");
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().ShadeTile();
        }
        tiles[gameController.playerIndex].GetComponent<NewTileBehavior>().ActivateCleanseButton();
        tiles[gameController.playerIndex].GetComponent<NewTileBehavior>().UnshadeTile();
        switch (gameController.playerIndex) {
            case 0:
                //Top left corner of grid, activate tiles to right and below
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 1:
            case 2:
                //Two top edge tiles, activate tile to right, left, and below
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 3:
                //top right corner of grid, activate tiles to left and below
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 4:
            case 8:
                //Left two edge tiles, activate above, below, and right
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 5:
            case 6:
            case 9:
            case 10:
                //Four center tiles, activate all adjacent tiles
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 7:
            case 11:
                //Right two edge tiles, activate above, below, and left
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 12:
                //Bottom left corner, activate above and right
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 13:
            case 14:
                //Bottom two edge tiles, activate left, right, and above
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex+1].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
            case 15:
                //Bottom right corner, activate above and left
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().ActivateCleanseButton();
                tiles[gameController.playerIndex-4].GetComponent<NewTileBehavior>().UnshadeTile();
                tiles[gameController.playerIndex-1].GetComponent<NewTileBehavior>().UnshadeTile();
                break;
        }
    }

    public void CompleteCleanse() {
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().DeactivateCleanseButton();
            tile.GetComponent<NewTileBehavior>().UnshadeTile();
        }
    }

    public void UseMightyFlightCard() {
        gameController.ShowTempMessage("Select any tile to fly to");
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().ActivateMoveButton();
        }
        keyDeck.DiscardCard(keyDeck.hand[keyDeck.hand.FindIndex(card => card.name == "MightyFlightCard")]);
    }

    public void UseCleansingFlameCard() {
        gameController.ShowTempMessage("Select any tile to cleanse");
        foreach (Transform tile in transform) {
            tile.GetComponent<NewTileBehavior>().ActivateCleanseButton();
        }
        keyDeck.DiscardCard(keyDeck.hand[keyDeck.hand.FindIndex(card => card.name == "CleansingFlameCard")]);
    }

    public void SaveElementalBird() {
        GameObject playerTile = tiles[gameController.playerIndex];
        if (tiles[gameController.playerIndex].name == "Burrowing Owl Cage") {
            //Check if the tile is corrupted
            if (playerTile.GetComponent<NewTileBehavior>().isCorrupted) {
                //Debug.Log($"{playerTile} tile is corrupted");
                gameController.ShowTempMessage("Clear corruption before saving this bird");
            }
            else {
                //check if player has enough burrowing owl cards
                int count = 0;
                foreach (CardObject card in keyDeck.hand) {
                    if (card.name == "Earth Key Card") {
                        count++;
                    }
                }
                if (count < 3) {
                    //Debug.Log("Not enough cards");
                    gameController.ShowTempMessage("Not enough Earth Key Cards");
                }
                else {
                    //Debug.Log($"YOU SAVED {playerTile}");
                    gameController.ShowTempMessage("You saved Burrowing Owl the Earth Elemental!");
                    burrowingOwl.transform.SetParent(birdGroup.transform);
                    burrowingOwl.transform.position = birdGroup.transform.position;
                }
            }
        }
        else if(tiles[gameController.playerIndex].name == "Albatross Cage") {
            //Debug.Log("On Albatross Cage");
            //Check if the tile is corrupted
            if (playerTile.GetComponent<NewTileBehavior>().isCorrupted) {
                //Debug.Log($"{playerTile} tile is corrupted");
                gameController.ShowTempMessage("Clear corruption before saving this bird");
            }
            else {
                //check if player has enough burrowing owl cards
                int count = 0;
                foreach (CardObject card in keyDeck.hand) {
                    if (card.name == "Water Key Card") {
                        count++;
                    }
                }
                if (count < 3) {
                    //Debug.Log("Not enough cards");
                    gameController.ShowTempMessage("Not enough Water Key Cards");
                }
                else {
                    //Debug.Log($"YOU SAVED {playerTile}");
                    gameController.ShowTempMessage("You saved Albatross the Water Elemental!");
                    albatross.transform.SetParent(birdGroup.transform);
                    albatross.transform.position = birdGroup.transform.position;
                }
            }
        }
        else if(tiles[gameController.playerIndex].name == "Falcon Cage") {
            //Debug.Log("On Falcon Cage");
            //Check if the tile is corrupted
            if (playerTile.GetComponent<NewTileBehavior>().isCorrupted) {
                //Debug.Log($"{playerTile} tile is corrupted");
                gameController.ShowTempMessage("Clear corruption before saving this bird");
            }
            else {
                //check if player has enough burrowing owl cards
                int count = 0;
                foreach (CardObject card in keyDeck.hand) {
                    if (card.name == "Wind Key Card") {
                        count++;
                    }
                }
                if (count < 3) {
                    //Debug.Log("Not enough cards");
                    gameController.ShowTempMessage("Not enough Wind Key Cards");
                }
                else {
                    //Debug.Log($"YOU SAVED {playerTile}");
                    gameController.ShowTempMessage("You saved Falcon the Wind Elemental!");
                    falcon.transform.SetParent(birdGroup.transform);
                    falcon.transform.position = birdGroup.transform.position;
                }
            }
        }
        else {
            //Debug.Log("Not at an elemental bird cage");
            gameController.ShowTempMessage("Not at an Elemental Bird cage");
        }
    }

    public void OnActionCompleted() {
        actionCompleted.Invoke();
    }

    public void UpdatePlayerIndex(GameObject newTile) {
        //Debug.Log("UpdatePlayerIndex");
        //Set player index
        gameController.playerIndex = tiles.FindIndex(tile => tile == newTile);
        //Set birdGroup as child of player index Tile
        SetBirdGroupParent();
    }

}
