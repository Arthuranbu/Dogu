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

        public override void Prepare()
        {
            //Unless I add more, with how I made it modular, won't need to actually override it.
            base.Prepare();
         
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
                transform.Translate(-transform.right * Time.deltaTime * EnemyStats.speedAmp);
        }
    }
}