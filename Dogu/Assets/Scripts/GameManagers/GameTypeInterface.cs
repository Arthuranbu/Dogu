using UnityEngine;
using System.Collections;

namespace Dogu
{
    //Game Manager will interface with this and then interface with GameUI
    //Might turn to abstract class, but updateUI only thing that needs to be by default implemented
    public class IGameType
    {
        private short _goalAmount;
        //Startgame function in GameManager will call the prepareGame function in the concret gameTypes
       
        public virtual void prepareGame()
        {
            targetName = "Ghost";
            //targetName = GeneralUse.enemyNames[Random.Range(0, GeneralUse.enemyNames.Length)];
            GoalAmount = 5;
        }
        public virtual void increaseDifficulty()
        { }
    
            
        public virtual short GoalAmount
        {
            set
            {
                _goalAmount = value;
            }
            get { return _goalAmount; }
        }
    
        public Enemy GoalTarget { set; get; }
        public string targetName { set; get; }
       //void nextRound();//This and increase difficulty could be same, but maybe keep seperate next round will be front end side decided by gamemanger
       //increase difficulty will be back end.

    }
}
