using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//Game manager should be handler I was talking about, right now it's just going to handle spawning.
//This will handle enemy spawner it will call for products.
namespace Dogu
{
    public class GameManager : MonoBehaviour
    {
        bool checkingDeath;
        GameUI manageUI;
        bool GameStarted;
        #region Cameras
        CameraManager manageCameras;
        #endregion
        #region Player Variables
        Player playerRef;
        GameObject DoguPrefab;
        Transform playerSpawnPoint;
        #endregion
        //Need to move all this to a gameUI class.
        #region UI
        GameObject mainMenuUI;
        GameObject gameUI;
        GameObject waveNoticeUI;

        Text progressInfo;
        Text waveInfo;
        GameObject deathScreenUI;
        #endregion

        #region Enemy Variables
        //bool currentlyChecking;
        EnemySpawner enemySpawner;
        List<GameObject> enemiesInScene;
        private int _wave;
        private int nEnemiesToSpawn;
        #endregion
        // Use this for initialization
        //Will change name to starting next round, so more general name, this will be virtual
        IEnumerator StartingNextWave()
        {
           // waveInfo.text = string.Format("Wave {0}", Wave);
            waveInfo.transform.parent.gameObject.SetActive(true);
            yield return new WaitForSeconds(2.0f);

            waveInfo.transform.parent.gameObject.SetActive(false);
        }
        public IGameType currentGameType
        {
            set;
            get;
        }


        void Awake()
        {

            manageUI = GetComponent<GameUI>();

            enemiesInScene = new List<GameObject>();
            DoguPrefab = Resources.Load("Prefabs/Dogu") as GameObject;
            playerSpawnPoint = GameObject.Find("PlayerSpawn").GetComponent<Transform>();

            manageCameras = GameObject.Find("LevelThreshhold").GetComponent<CameraManager>();
            
            enemySpawner = GetComponent<EnemySpawner>();
           
        }
        void Start()
        {

            manageUI.MainMenuUI();

        }

        IEnumerator SetEnemiesToSpawn(int enemyCount)
        {
            for (int es = 0; es < enemyCount; ++es)
            {
                string enemyToSpawn = "Ghost";
                //string enemyToSpawn = GeneralUse.enemyNames[Random.Range(0, GeneralUse.enemyNames.Length)];
                GameObject enemySpawned = enemySpawner.SpawnEnemy(enemyToSpawn);
                SetEnemySpawnLocation(enemyToSpawn, enemySpawned);
                enemiesInScene.Add(enemySpawned);

            }
            foreach (GameObject enemy in enemiesInScene)
            {
                enemy.GetComponent<Enemy>().PrepareEnemy();
                yield return new WaitForEndOfFrame();
            }
        }
        
        void SetEnemySpawnLocation(string enemyToSpawn, GameObject enemySpawned)
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(enemyToSpawn + "Spawn");
            enemySpawned.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].GetComponent<Transform>().position;
        }
       
        void PrepareEnemies()
        {
            foreach (var x in enemiesInScene)
            {
                x.GetComponent<Enemy>().PrepareEnemy();
            }
        }
        #region GameManaging functions called by UI
        public void StartGame(string gameType)
        {
            switch (gameType)
            {
                case "Hunt":
                    currentGameType = new HuntEnemy();
                    break;
                case "Clear":
                    currentGameType = new ClearWave();
                    break;
                case "Collect":
                    currentGameType = new CollectItems();
                    break;
            }
            currentGameType.prepareGame();
          
            //Setting up UI
            manageUI.goalProgress = currentGameType.GoalAmount;
            manageUI.StartGameUI();
            StartCoroutine(manageUI.ShowGoal(currentGameType.targetName));
            manageCameras.switchCameras();

            //Setting player up
            GameObject player = Instantiate(DoguPrefab, playerSpawnPoint.transform.position, Quaternion.identity) as GameObject;
            player.transform.parent = GameObject.Find("Level").GetComponent<Transform>();
            playerRef = player.GetComponent<Player>();
            playerRef.Spawn();
            //Even amount of all or have randon on what will spawnper wave.
            //Could have this be loop then randomize each time rather than loop inside the spawn enemy function itself.
            StartCoroutine(SetEnemiesToSpawn(5));
            GameStarted = true;
        }
        public void UpdateUI()
        {
            manageUI.currentProgress = currentGameType.GoalAmount;
            if (manageUI.currentProgress == 0)
            {
                winGame();
            }
        }
        public void RestartGame()
        {
            manageUI.StartGameUI();
            playerRef.Spawn();
            playerRef.transform.localPosition = playerSpawnPoint.localPosition;
            StartCoroutine(SetEnemiesToSpawn(5));

        }
        void winGame()
        {
            GameStarted = false;
            foreach (GameObject enemy in enemiesInScene)
            {
                Destroy(enemy);

            }
            manageUI.WonGameUI();
        }
        public void BackToMainMenu()
        {
            manageUI.MainMenuUI();
            Destroy(playerRef.gameObject);
        }
        #endregion

        void Update()
        {
            //temporary check in Update, just to increase spawns and get waves going for now.
            if (GameStarted)
            {
                if (playerRef.Dead && !checkingDeath)
                    StartCoroutine(CheckDead());
                manageUI.currentProgress = currentGameType.GoalAmount;
                if (!playerRef.Dead)
                    CheckEnemiesLeft();
            }
         
        }
        
       void CheckEnemiesLeft()
        {
            int stillAlive = 0;
            foreach (var x in enemiesInScene)
            {
                if (x.activeInHierarchy)
                {
                    stillAlive++;
                }
            }
            if (stillAlive < 3)
            {
                StartCoroutine(SetEnemiesToSpawn(5));
            }
        }

        IEnumerator CheckDead()
        {
            //Probably bad thread practice getting rid of this, haven't looked too much into threads past what I learned in c++
            //but getting rid of it in this case gets rid of delay with death UI, so it's beneficial. 
            //   currentlyChecking = true;

            checkingDeath = true;
            manageUI.EndGameUI();
            //Wait until player respawns


            foreach (GameObject enemy in enemiesInScene)
            {
                Destroy(enemy);

            }
            enemiesInScene.Clear();
            yield return new WaitUntil(() => !playerRef.Dead);
            checkingDeath = false;

        }
     

    }
}