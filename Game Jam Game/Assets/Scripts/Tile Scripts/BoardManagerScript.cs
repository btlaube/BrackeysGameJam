using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManagerScript : MonoBehaviour {
    
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();
    public GridLayoutGroup layoutGroup;

    public GameController gameController;
    private bool firstBoardSetUp = true;
    public GameObject birdGroup;

    void Awake() {
        gameController = GameController.instance;
    }

    void Start() {
        SetUpRound();
    }

    public void SetUpRound() {
        StartCoroutine(SetUpRoundEnum());
    }

    IEnumerator SetUpRoundEnum() {
        ResetBoardCorruption();
        if (!firstBoardSetUp) {
            SetBoardBack();
            yield return new WaitForSeconds(2.5f);
        }
        firstBoardSetUp = false;
        RandomizeBoard();

        //Set player index
        gameController.playerIndex = tiles.FindIndex(tile => tile.GetComponent<TileBehavior>().locationName == "Phoenix Cage");
        //Set birdGroup as child of player index Tile
        birdGroup.transform.SetParent(tiles[gameController.playerIndex].transform.GetChild(0));
        birdGroup.transform.position = birdGroup.transform.parent.position;

        SetBoardFace();
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

    public void SetBoardFace() {
        StartCoroutine(SetBoardFaceEnum());
    }

    IEnumerator SetBoardFaceEnum() {
        foreach (Transform tile in transform) {
            yield return new WaitForSeconds(0.1f);

            tile.GetComponent<TileBehavior>().SetFace();
        }
    }

    public void SetBoardBack() {
        StartCoroutine(SetBoardBackEnum());
    }

    IEnumerator SetBoardBackEnum() {
        foreach (Transform tile in transform) {
            yield return new WaitForSeconds(0.1f);

            tile.GetComponent<TileBehavior>().SetBack();
        }
    }

    public void ResetBoardCorruption() {
        foreach (Transform tile in transform) {
            tile.GetComponent<TileBehavior>().ResetCorruption();
        }
    }

    public void Move() {
        foreach (Transform tile in transform) {
            tile.GetComponent<TileBehavior>().tileButton.interactable = true;
        }
    }

    public void UpdatePlayerIndex(GameObject newTile) {
        Debug.Log("UpdatePlayerIndex");
        //Set player index
        gameController.playerIndex = tiles.FindIndex(tile => tile == newTile);
        //Set birdGroup as child of player index Tile
        birdGroup.transform.SetParent(tiles[gameController.playerIndex].transform.GetChild(0));
        birdGroup.transform.position = birdGroup.transform.parent.position;
    }

}
