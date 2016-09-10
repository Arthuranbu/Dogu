using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Dogu
{
    public class EnemySpawner : MonoBehaviour
    {
        
        public GameObject[] SpawnEnemy(string prefab,int amountToSpawn)
        {
            string prefabPath = "Prefabs/";
            prefabPath += prefab;
            
            GameObject enemyPrefab = Resources.Load(prefabPath) as GameObject;
            GameObject[] spawnedEnemies = new GameObject[amountToSpawn];
            for (int i = 0; i < amountToSpawn; i++)
            {   
                spawnedEnemies[i] = Instantiate(enemyPrefab);
                spawnedEnemies[i].transform.position = GameObject.Find("PreppingArea").transform.position;
            }
            return spawnedEnemies;
        }
        //This one was pretty improvised, not at all planned, so probably will flesh this out better later.
        public string ChooseEnemyToSpawn(List<Enemy> currentlySpawned, int wave)
        {
            string enemyToSpawn = "";
            List<SpearMan> allSpawnedSpearMan = new List<SpearMan>();
            List<Enemy> allSpawnedSwordSoldiers = new List<Enemy>();

            IEnumerator<Enemy> decideWhich = currentlySpawned.GetEnumerator();
            if (currentlySpawned.Count != 0)
            {
                //Will flesh this out more later
                while (decideWhich.MoveNext())
                {
                    if (decideWhich.Current is SpearMan)
                    {
                        allSpawnedSpearMan.Add(decideWhich.Current as SpearMan);
                    }
                    else
                        allSpawnedSwordSoldiers.Add(decideWhich.Current);

                    if (allSpawnedSwordSoldiers.Count - allSpawnedSpearMan.Count < 0)
                        enemyToSpawn = "Soldier";

                    else
                    {
                        List<bool> spawnSpearManConds = new List<bool>();
                        spawnSpearManConds.Add(allSpawnedSpearMan.Count < 2 && wave > 3);
                        spawnSpearManConds.Add(allSpawnedSpearMan.Count < 4 && wave > 5);

                        if (spawnSpearManConds.Contains(true))
                            enemyToSpawn = "SpearMan";
                        else
                            enemyToSpawn = "Soldier";
                    }
                        
                }
            }
            else
                enemyToSpawn = "Soldier";
            return enemyToSpawn;
        }
    }
}