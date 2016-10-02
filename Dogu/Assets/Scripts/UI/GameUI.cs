using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

    GameObject mainMenuUI;
    GameObject gameUI;
    GameObject waveNoticeUI;
    GameObject deathScreenUI;
    GameObject wonGameUI;

    Text showGoalUI;
    Text progressInfo;

    private short _currentProgress;

    // Use this for initialization
    void Awake()
    {
        wonGameUI = GameObject.Find("WonGameUI");
        mainMenuUI = GameObject.Find("MainMenu");
        gameUI = GameObject.Find("GameUI");
        deathScreenUI = GameObject.Find("DeathScreen");
        showGoalUI = GameObject.Find("DisplayGoal").GetComponent<Text>();
        progressInfo = GameObject.Find("ProgressInfo").GetComponent<Text>();
    }

    public void StartGameUI()
    {
        mainMenuUI.SetActive(false);
        gameUI.SetActive(true);
        deathScreenUI.SetActive(false);
    }
    public void WonGameUI()
    {
        wonGameUI.SetActive(true);
    }

    public IEnumerator ShowGoal(string goal)
    {

        showGoalUI.text = string.Format("Target: {0}", goal);
        yield return new WaitForSeconds(2.0f);
        showGoalUI.transform.parent.gameObject.SetActive(false);
    }

    public void EndGameUI()
    {
        deathScreenUI.SetActive(true);
    }
    public void MainMenuUI()
    {
        wonGameUI.SetActive(false);
        mainMenuUI.SetActive(true);
        gameUI.SetActive(false);
        deathScreenUI.SetActive(false);
    }


    public short currentProgress
    {
        set
        {
            _currentProgress = value;
        }
        get { return _currentProgress; }

    }

    public void updateProgressInfo(short current,short goal)
    {
        string progressText = string.Format("{0}/{1}", current, goal);
        progressInfo.text = progressText;
    }
}
