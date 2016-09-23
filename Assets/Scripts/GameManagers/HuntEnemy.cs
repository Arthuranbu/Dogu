using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class HuntEnemy : MonoBehaviour, IGameType
    {
        public Enemy GoalTarget { set; get; }
        public int GoalAmount { set; get; }
        public void increaseDifficulty()
        { }

        public void prepareGame()
        {

        }
        
    }
}