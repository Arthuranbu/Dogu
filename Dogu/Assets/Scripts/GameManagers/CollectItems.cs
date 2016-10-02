using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Dogu
{
    public class CollectItems : IGameType
    {
        //Design needs to be taken accounted for for how difficulty ramps up. For now just get one round going
        //List is only used in here, but it's always same so should be static and created at compile time, efficiency vs logically makes sense
        //Maybe other classes will need it
        
        
        public override void increaseDifficulty()
        {
        }
        
        public override void prepareGame()
        {
            base.prepareGame();
            targetName += "Drop";
            increaseDifficulty();
        }
       


    }
}