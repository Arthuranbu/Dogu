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

    private short _currentProgress;

    // Use this for initialization
    void Awake()
    {
        mainMenuUI = GameObject.Find("MainMenu");
        gameUI = GameObject.Find("GameUI");
        deathScreenUI = GameObject.Find("DeathScreen");

        waveInfo = GameObject.Find("WaveNumber").GetComponent<Text>();
        progressInfo = GameObject.Find("ProgressInfo").GetComponent<Text>();
    }

    public void StartGameUI()
    {
        mainMenuUI.SetActive(false);
        gameUI.SetActive(true);
        updateProgressInfo();
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
    
    public short currentProgress {
        set
        {
            _currentProgress = value;
            updateProgressInfo();
        }
        get { return _currentProgress; }
    }

    public short goalProgress { set; get; }

    private void updateProgressInfo()
    {
        string progressText = string.Format("{0}/{1}", currentProgress, goalProgress);
        progressInfo.text = progressText;
    }
}
