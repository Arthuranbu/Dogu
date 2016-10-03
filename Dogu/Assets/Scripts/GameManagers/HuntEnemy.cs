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
            base.prepareGame();
           
        }
     
        
    }
}