using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class TimeAttackGameType : GameManager
    {
        float timeToComplete = 10.0f;
        float timeLeft;
        

        int KillsNeededLeft
        {
            set { _amount -= value; }
            get { return _amount; }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        protected override void PrepGame()
        {
            timeLeft = timeToComplete;
            //So randomize what enemy to to hunt down and randomize how many. Will prob be static array inside GloballyUsedInterface
                
            //Later it will vary on how many rounds it has been for now, nice and simple.
            _amount = Random.Range(1, 10);
            
        }
    }
}