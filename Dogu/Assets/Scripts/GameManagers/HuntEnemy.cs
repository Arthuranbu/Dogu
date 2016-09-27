using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class HuntEnemy : IGameType
    {
       
        public override void increaseDifficulty()
        {
            GoalAmount = 5;
        }

        public override void prepareGame()
        {
            
            int enemyIndex = Random.Range(0, GeneralUse.allEnemies.Length);
            GoalTarget = GeneralUse.allEnemies[enemyIndex];
            targetName = GeneralUse.allEnemyNames[enemyIndex];
        }
     
        
    }
}