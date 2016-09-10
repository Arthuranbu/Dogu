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

        #region Enemy Variables
        bool currentlyChecking;
        EnemySpawner spawnEnemies;
        static GameObject[] enemiesInScene;
        List<Transform> spawnPoints;
        private int _wave;
        private int nEnemiesToSpawn;
        #endregion
        // Use this for initialization

        IEnumerator StartingNextWave()
        {
            waveInfo.text = string.Format("Wave {0}", Wave);
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
        public void StartGame()
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

            
            Wave = 1;
            
            GameStarted = true;
        }
        #endregion
        public void RestartGame()
        {
            playerRef.Respawn();
            playerRef.transform.position = playerSpawnPoint.position;
            Wave = 1;
        }

        public void BackToMainMenu()
        {
            mainMenuCamera.gameObject.SetActive(true);
            mainMenuUI.SetActive(true);
            gameUI.SetActive(false);
            Destroy(playerRef.gameObject);
        }
        void Update()
        {
            //temporary check in Update, just to increase spawns and get waves going for now.
            if (GameStarted)
            {
                if (!currentlyChecking)
                    StartCoroutine(CheckDead());

           }
        }
        int Wave
        {
            get { return _wave; }
            set
            {
                _wave = value;

                /*if (_wave != 0)
                {
                    StartCoroutine(StartingNextWave());
                    if (nEnemiesToSpawn == 0)
                        nEnemiesToSpawn += 2;
                    else
                        nEnemiesToSpawn += 3;
                    //Jusst soldier for now
                    GameObject[] enemies = spawnEnemies.SpawnEnemy("Soldier", nEnemiesToSpawn);
                    if (enemies != null || enemies.Length != 0)
                        StartCoroutine(ChooseSpawnPoints(enemies));
                }
                else
                    nEnemiesToSpawn = 0;
             
               */
            }
        }

        IEnumerator ChooseSpawnPoints(GameObject[] enemies)
        {
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
                Wave = 0;

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
                if (enemiesInScene.Length == 0 && Wave != 0)
                    ++Wave;
            }
           currentlyChecking = false;
        }

        void FindEnemiesInScene()
        {
            //Was doing data race, so it won't go down toa ctivate the death ui cause it's waiting till no longer dead, but that cna only happen via death UI, error in my logic, that's what happens coding for 18 hours straight LOL.

            //yield return new WaitUntil(() => !playerRef.Dead);

            //Just noticed made exact same function as I wrote in EnemySpawner, but slightly diff
            //method of doing it. LOL I'm a dumbass. Prob will stick to this, will flesh out later get core done for jam first.

            //int soldiers = 0;
            //int spearman = 0;
            GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
            //Supposed to stop at here then pass this array in parameter
            /*if (enemiesInScene != null)
            {
                foreach (GameObject enemy in enemiesInScene)
                {
                    if (enemy.GetComponent<SpearMan>())
                        ++spearman;
                    else
                        //need to eidt this but since only two enemies this else is fine/
                        ++soldiers;
                }
            }*/
            //If all enemies in scene are dead, destroyed/inactive, prob make a pool for them later.
            if (enemiesInScene.Length == 0)
            {
                ++Wave;
            }
            //Will move this block to seperate func
            /*else if (playerRef.Dead)
            {
                Wave = 0;
                
                deathScreenUI.SetActive(true);
                //Wait until player respawns
                yield return new WaitUntil(() => !playerRef.Dead);
                foreach( GameObject enemy in enemiesInScene)
                {
                    Destroy(enemy);
                }
                //waiting for speed of death anim.
                yield return new WaitForSeconds(1.0f);
                
            }
            else if (!playerRef.Dead && deathScreenUI.activeInHierarchy)
                deathScreenUI.SetActive(false);
            */

        }
    }
}