using UnityEngine;
using System.Collections;

namespace Dogu
{
    public interface IGameType
    {
        //Startgame function in GameManager will call the prepareGame function in the concret gameTypes
        void prepareGame();
        void increaseDifficulty();
        int GoalAmount { set; get; }
        Enemy GoalTarget { set; get; } 
       //void nextRound();//This and increase difficulty could be same, but maybe keep seperate next round will be front end side decided by gamemanger
        //increase difficulty will be back end.
    
    }
}
