 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//Game manager should be handler I was talking about, right now it's just going to handle spawning.
//This will handle enemy spawner it will call for products.
namespace Dogu
{
    public  class GameManager : MonoBehaviour
    {
        bool GameStarted;
        #region Cameras
        Camera mainSceneCamera;
        Camera mainMenuCamera;
        #endregion
        #region Player Variables
        Player playerRef;
        GameObject DoguPrefab;
        Transform playerSpawnPoint;
        #endregion
        #region UI
        GameObject mainMenuUI;
        GameObject gameUI;
        GameObject waveNoticeUI;

        Text waveInfo;
        GameObject deathScreenUI;
        #endregion
        #region GameType Variables
        GeneralUse.GameTypes currentGameType;
        #endregion
        #region Enemy Variables
        protected Enemy enemyToKill;
        bool currentlyChecking;
        EnemySpawner spawnEnemies;
        static GameObject[] enemiesInScene;
        List<Transform> spawnPoints;
        private int _wave;
        private int nEnemiesToSpawn;
        #endregion
        #region Gameplay Variables
        //what this represents varies between each derivative.
        protected int _amount;
        #endregion
        // Use this for initialization

        IEnumerator CurrentGameInfo(string info)
        {
            waveInfo.text = string.Format("Wave {0}", info);
            waveInfo.transform.parent.gameObject.SetActive(true);
            yield return new WaitForSeconds(2.0f);

            waveInfo.transform.parent.gameObject.SetActive(false);
        }


        void Awake()
        {
            DoguPrefab = Resources.Load("Prefabs/Dogu") as GameObject;
            playerSpawnPoint = GameObject.Find("PlayerSpawn").GetComponent<Transform>();

            mainSceneCamera = GameObject.Find("MainSceneCamera").GetComponent<Camera>();
            mainMenuCamera = GameObject.Find("MainMenuCamera").GetComponent<Camera>();

            deathScreenUI = GameObject.Find("DeathScreen");
            gameUI = GameObject.Find("GameUI");
            mainMenuUI = GameObject.Find("MainMenu");

            waveInfo = GameObject.Find("WaveNumber").GetComponent<Text>();
            spawnPoints = new List<Transform>();
            spawnEnemies = GetComponent<EnemySpawner>();
            foreach (GameObject point in GameObject.FindGameObjectsWithTag("SpawnPoint"))
            {
                spawnPoints.Add(point.transform);
            }
        }
        void Start()
        {

            gameUI.SetActive(false);

        }

        #region GameManaging functions called by UI

        protected virtual void PrepGame()
        { }
       

        public virtual void StartGame()
        {
            //Setting up UI
            mainMenuUI.SetActive(false);
            gameUI.SetActive(true);
            deathScreenUI.SetActive(false);
            //Setting player up
            GameObject Player = Instantiate(DoguPrefab, playerSpawnPoint.transform.position, Quaternion.identity) as GameObject;
            playerRef = Player.GetComponent<Player>();
            playerRef.Respawn();

            //Changing screen view
            mainMenuCamera.gameObject.SetActive(false);
            //Below worked, it's same way I did in rhythm game but trying diff method and it works too.
            //  mainMenuCamera.targetDisplay = 2;
            //  mainSceneCamera.targetDisplay = 0;

            GameStarted = true;
        }
        //All types of game restarts is always respawning player.
        public virtual void RestartGame()
        {
            playerRef.Respawn();
            playerRef.transform.position = playerSpawnPoint.position;
            
        }

        public void BackToMainMenu()
        {
            mainMenuCamera.gameObject.SetActive(true);
            mainMenuUI.SetActive(true);
            gameUI.SetActive(false);
            Destroy(playerRef.gameObject);
        }
        #endregion

        protected virtual void Update()
        {
            //temporary check in Update, just to increase spawns and get waves going for now.
            if (GameStarted)
            {
                if (!currentlyChecking)
                    StartCoroutine(CheckDead());
            }
        }
       

        IEnumerator ChooseSpawnPoints(GameObject[] enemies)
        {
            //Instead of getting transforms will get along a line, like any point on a line, so that's math.
            yield return new WaitForSeconds(2.0f);
            foreach (GameObject enemy in enemies)
            {
                //delay so they're not all pile don top of each other.
                yield return new WaitForSeconds(0.5f);
                int randSpawnPoint = Random.Range(0, spawnPoints.Count);
                enemy.transform.position = spawnPoints[randSpawnPoint].position;
                enemy.GetComponent<Enemy>().Prepped = true;
            }

        }

        IEnumerator CheckDead()
        {
            //Probably bad thread practice getting rid of this, haven't looked too much into threads past what I learned in c++
            //but getting rid of it in this case gets rid of delay with death UI, so it's beneficial. 
            currentlyChecking = true;
            enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");

            if (playerRef.Dead)
            {

                deathScreenUI.SetActive(true);
                //Wait until player respawns
                yield return new WaitUntil(() => !playerRef.Dead);
                //Then destroys all of the 
                foreach (GameObject enemy in enemiesInScene)
                {
                    Destroy(enemy);
                }

            }
            else if (!playerRef.Dead)
            {
                if (deathScreenUI.activeInHierarchy)
                    deathScreenUI.SetActive(false);
                
            }
            currentlyChecking = false;
        }

    }
}