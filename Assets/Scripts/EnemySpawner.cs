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
       
    }
}