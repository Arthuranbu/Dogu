using UnityEngine;
using System.Collections;
namespace Dogu
{
    public class CollectItems : MonoBehaviour, IGameType
    {
        //Design needs to be taken accounted for for how difficulty ramps up. For now just get one round going
        public Enemy GoalTarget {  set; get; }
        public int GoalAmount {  set; get; }
        public void increaseDifficulty()
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