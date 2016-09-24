





using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Dogu
{
    public class Boar : Enemy
    {

        //Doesn't need to be a struct, 
        private bool _onFloor;
        private bool _inRange;
        //As I learn more math and solutions, I'll think of better tways to manage higher difficulty
        private int discreteSpeedIncrement;

        void Start()
        {
            discreteSpeedIncrement = 0;
            baseSpeed = 10.0f;
        }

        public override void Prepare()
        {
            //Static doesn't work same way as c++ in terms of inside functions interesting.
           
            //Unless I add more, with how I made it modular, won't need to actually override it.
            base.Prepare();
            //Will just do stats here, since those will actually change with each wave to increase difficulty.
           
            //Post increment so that it increases for next time on same line but increments it by current value.
            enemyStats = new GeneralUse.Stats(1, baseSpeed + (discreteSpeedIncrement++));
            //Will need to pass in argument or something to change these or make the construct itself variable,
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void EnemyMovement()
        {
            //This won't chase player but will do specific pattern honestly could prob just make it walk left, then when not on floor then quit movement/change to drop down, now that's 2 classes using onfloor bool, is it worth making it into namespace?
            Debug.Log("Boar Moving");
            if (!_onFloor)
            {
                transform.Translate(-transform.up * Time.deltaTime * GeneralUse.GRAVITY);
            }
            else
                transform.Translate(-transform.right * Time.deltaTime * enemyStats.speedAmp);
        }
    }
}