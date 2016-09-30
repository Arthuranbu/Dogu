using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Dogu
{
    public class CollectItems : IGameType
    {
        //Design needs to be taken accounted for for how difficulty ramps up. For now just get one round going
        //List is only used in here, but it's always same so should be static and created at compile time, efficiency vs logically makes sense
        //Maybe other classes will need it
        
        public override void dropItem(GameObject enemyKilled)
        {
            Enemy enemy = enemyKilled.GetComponent<Enemy>();
            string itemToDrop = GeneralUse.droppedItems[enemy];
            GameObject droppedItem = Instantiate(Resources.Load("Prefabs/Items/" + itemToDrop) as GameObject); 
            //Spawns the instantiated dropped item to where the enemy just killed was.
            droppedItem.transform.position = enemyKilled.transform.position;
        }
        public override void increaseDifficulty()
        {
            GoalAmount = 5;
        }
        
        public void prepareGame()
        {
            GoalTarget = GeneralUse.allEnemies[Random.Range(0, GeneralUse.allEnemies.Length)];
            increaseDifficulty();
        }
      


    }
}