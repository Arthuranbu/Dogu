using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class HuntEnemy : MonoBehaviour, IGameType
    {
        public Enemy GoalTarget { set; get; }
        public string targetName { set; get; }
        public int GoalAmount { set; get; }

        public void increaseDifficulty()
        {
            GoalAmount = 5;
        }

        public void prepareGame()
        {
            int enemyIndex = Random.Range(0, GeneralUse.allEnemies.Length);
            GoalTarget = GeneralUse.allEnemies[enemyIndex];
            targetName = GeneralUse.allEnemyNames[enemyIndex];
        }
        
    }
}