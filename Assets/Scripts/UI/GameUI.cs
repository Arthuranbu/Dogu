using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

    GameObject mainMenuUI;
    GameObject gameUI;
    GameObject waveNoticeUI;
    GameObject deathScreenUI;

    Text progressInfo;
    Text waveInfo;
 
    // Use this for initialization
    void Awake()
    {
        mainMenuUI = GameObject.Find("MainMenu");
        gameUI = GameObject.Find("GameUI");
        deathScreenUI = GameObject.Find("DeathScreen");

        waveInfo = GameObject.Find("WaveNumber").GetComponent<Text>();
        progressInfo = GameObject.Find("ProgressInfo").GetComponent<Text>();
    }
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void StartGameUI()
    {
        mainMenuUI.SetActive(false);
        gameUI.SetActive(true);
        deathScreenUI.SetActive(false);
    }
    public void EndGameUI()
    {
        deathScreenUI.SetActive(true);

    }
    public void MainMenuUI()
    {
        mainMenuUI.SetActive(true);
        gameUI.SetActive(false);
        deathScreenUI.SetActive(false);
    }
}
