using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Dogu
{
    public class EnemySpawner : MonoBehaviour
    {

        public GameObject SpawnEnemy(string monsterName)
        {
            string prefabPath = "Prefabs/";
            prefabPath += monsterName;
            //Does game amanger need reference to all?, well can either check every update or keep reference to it here and check if active.
            GameObject spawnedEnemy = Instantiate(Resources.Load(prefabPath) as GameObject);
            spawnedEnemy.transform.position = GameObject.Find("PreppingArea").transform.position;
            return spawnedEnemy;
        }

        //Actually since could be all same name could make them all overloaded functions and let the compiler find it implicitly that way.
        /*public void PrepEnemy(ref Enemy toPrep)
        {
            if (toPrep)
           
        }*/

        //Okay wait, so for everyone of these functions I'm going to be setting it to Prepped in the end. So maybe have prep part be in derived enemy classes instead?
        //I could move these to derived areas, and then during loop after getting returned Enemy object I can then call prepMethod inside it.
        //Yeah sinc always preps in end better as virtual then overloaded functions.
        private void PrepEnemy(ref Boar toPrep)
        {
            //Will transfer boar to boar spawn location and then set him to prepped,
        }
        private void PrepEnemy(ref Ghost toPrep)
        {

        }
        private void PrepEnemy()
        { }
        
    }
}