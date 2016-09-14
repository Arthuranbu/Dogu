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
        //Todo: add levelManager struct or class, but that's polishing wave generation, for now keep simple first.
        bool GameStarted;
        #region Cameras
        CameraManager manageCameras;
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

        #region Enemy Variables
        bool currentlyChecking;
        EnemySpawner enemyRef;
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


        void Awake()
        {
            enemiesInScene = new List<GameObject>();
            DoguPrefab = Resources.Load("Prefabs/Dogu") as GameObject;
            playerSpawnPoint = GameObject.Find("PlayerSpawn").GetComponent<Transform>();

            deathScreenUI = GameObject.Find("DeathScreen");
            gameUI = GameObject.Find("GameUI");
            mainMenuUI = GameObject.Find("MainMenu");
            manageCameras = GameObject.Find("LevelThreshhold").GetComponent<CameraManager>();

            waveInfo = GameObject.Find("WaveNumber").GetComponent<Text>();
            
            enemyRef = GetComponent<EnemySpawner>();
           
        }
        void Start()
        {
          
            gameUI.SetActive(false);

        }

        IEnumerator SetEnemiesToSpawn(int enemyCount)
        {
            for (int es = 0; es < enemyCount; ++es)
            {
                string enemyToSpawn = GeneralUse.enemyTypes[Random.Range(0, GeneralUse.enemyTypes.Length)];
                GameObject enemySpawned = enemyRef.SpawnEnemy(enemyToSpawn);
                SetEnemySpawnLocation(enemyToSpawn, enemySpawned);
                //To find enemy should be to find Boar.
                //Getting component enemy should work since it has boar.
                //Just getting enemy should work, BECAUSE THATS HOW INHERITENCE WORKS.
                enemiesInScene.Add(enemySpawned);

                //AND IT DOES SO WTF.
            }
            foreach (GameObject enemy in enemiesInScene)
            {
                enemy.GetComponent<Enemy>().Prepare();
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
                x.GetComponent<Enemy>().Prepare();
            }
        }
        #region GameManaging functions called by UI
        public void StartGame()
        {
            //Setting up UI
            mainMenuUI.SetActive(false);
            gameUI.SetActive(true);
            deathScreenUI.SetActive(false);

            manageCameras.switchCameras();

            //Setting player up
            GameObject player = Instantiate(DoguPrefab, playerSpawnPoint.transform.position, Quaternion.identity) as GameObject;
            player.transform.parent = GameObject.Find("Level").GetComponent<Transform>();
            playerRef = player.GetComponent<Player>();
            playerRef.Spawn();
            //Even amount of all or have randon on what will spawnper wave.
            //Could have this be loop then randomize each time rather than loop inside the spawn enemy function itself.
           // StartCoroutine(SetEnemiesToSpawn(5));
            GameStarted = true;
        }
        
        public void RestartGame()
        {
            playerRef.Spawn();
            playerRef.transform.localPosition = playerSpawnPoint.localPosition;
            SetEnemiesToSpawn(5);

        }

        public void BackToMainMenu()
        {
            mainMenuUI.SetActive(true);
            gameUI.SetActive(false);
            Destroy(playerRef.gameObject);
        }
        #endregion

        void Update()
        {
            //temporary check in Update, just to increase spawns and get waves going for now.
            if (GameStarted)
            {
                if (!currentlyChecking)
                    StartCoroutine(CheckDead());

           }
        }
        
       

        IEnumerator CheckDead()
        {
            //Probably bad thread practice getting rid of this, haven't looked too much into threads past what I learned in c++
            //but getting rid of it in this case gets rid of delay with death UI, so it's beneficial. 
            currentlyChecking = true;

            if (playerRef.Dead)
            {

                deathScreenUI.SetActive(true);
                //Wait until player respawns
                yield return new WaitUntil(() => !playerRef.Dead);

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