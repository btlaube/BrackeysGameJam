using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileBehavior : MonoBehaviour {
    
    public string locationName;
    public GameObject tileFace;
    public GameObject tileBack;
    public GameObject corruption;

    public TMP_Text locationNameText;
    public Button moveButton;
    public Button cleanseButton;
    public Button tileButton;

    private int timer;

    void Start() {
        locationNameText.text = locationName;
        tileButton.interactable = false;
        moveButton.interactable = false;
        cleanseButton.interactable = false;
    }

    IEnumerator Flip(GameObject newSide) {
        for (int i = 0; i < 180; i++) {
            yield return new WaitForSeconds(0.0001f);
            transform.Rotate(new Vector3(0, 1, 0));
            timer++;

            if (timer == 90 || timer == -90) {
                transform.localScale = new Vector3(transform.localScale.x*-1, 1, 1);
                if (newSide == tileFace) {
                    tileFace.SetActive(true);
                    tileBack.SetActive(false);
                }
                else if (newSide == tileBack) {
                    tileFace.SetActive(false);
                    tileBack.SetActive(true);
                }
                
            }
        }
        timer = 0;
    }

    public void SetFace() {
        StartCoroutine(Flip(tileFace));
    }

    public void SetBack() {
        StartCoroutine(Flip(tileBack));
    }

    public void SetCorrupted() {
        if (corruption.activeSelf) {
            tileFace.SetActive(false);
            tileBack.SetActive(false);
        }
        else {
            corruption.SetActive(true);
        }
    }
    
    public void ResetCorruption() {
        corruption.SetActive(false);
    }

    public void MoveHere() {
        Debug.Log($"Move to {this.name}");
        GetComponentInParent<BoardManagerScript>().UpdatePlayerIndex(gameObject);
    }

}
