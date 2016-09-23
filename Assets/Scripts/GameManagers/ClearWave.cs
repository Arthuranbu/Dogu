using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class ClearWave : MonoBehaviour, IGameType
    {

       
        
        //This kind of useless since waves, target is all enemies so ust pass in enemy instance of objects rather than scripts
        public Enemy GoalTarget { set; get; } 
        //in this case it will be amount per wave.
        public int GoalAmount { set; get; }
        public void increaseDifficulty()
        { }
        
        public void prepareGame()
        {
        }
        void Start()
        {
            GoalTarget = new Boar();
        }
        void Update()
        {
            //Sweet so theory works.
        }
    }
}