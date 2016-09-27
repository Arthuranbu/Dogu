using UnityEngine;
using System.Collections;

namespace Dogu
{
    //Might turn to abstract class, but updateUI only thing that needs to be by default implemented
    public abstract class IGameType:MonoBehaviour
    {
        private short _goalAmount;
        //Startgame function in GameManager will call the prepareGame function in the concret gameTypes
        private GameUI updateUI { set; get; }
        public virtual void prepareGame()
        {
            updateUI.goalProgress = GoalAmount;
        }
        public abstract void increaseDifficulty();
        public virtual short GoalAmount
        {
            set
            {
                _goalAmount = value;
                updateProgressOnUI();
            }
            get { return _goalAmount; }
        }

        public Enemy GoalTarget { set; get; }
        public string targetName { set; get; }
        protected void updateProgressOnUI()
        {
            updateUI.currentProgress = GoalAmount;
        }
        void Awake()
        {
            updateUI = GetComponent<GameUI>();

        }
       //void nextRound();//This and increase difficulty could be same, but maybe keep seperate next round will be front end side decided by gamemanger
       //increase difficulty will be back end.

    }
}
