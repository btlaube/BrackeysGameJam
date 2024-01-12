using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
    
    public int currentPageIndex;
    public GameObject[] pages;
    public GameObject howToPlay;

    void Update() {
        foreach (GameObject page in pages) {
            page.SetActive(false);
        }
        pages[currentPageIndex].SetActive(true);
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    public void ShowHowToPlay() {
        howToPlay.SetActive(true);
    }

    public void HideHowToPlay() {
        howToPlay.SetActive(false);
    }

    public void IncrementPage() {
        if (currentPageIndex == pages.Length-1) {
            currentPageIndex = 0;
        }
        else {
            currentPageIndex++;
        }
    }

    public void DecrementPage() {
        if (currentPageIndex == 0) {
            currentPageIndex = pages.Length-1;
        }
        else {
            currentPageIndex--;
        }
    }

}
