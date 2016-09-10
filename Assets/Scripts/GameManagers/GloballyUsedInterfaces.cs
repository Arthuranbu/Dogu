using System.Collections.Generic;
using UnityEngine;
//Change name of this script
namespace Dogu
{
    //Interface that all classes will implement
    //Doing this because though alot of it will be basic just do animation, different implentations will add seperate things.
    public interface Animations
    {
        void PlayAnimation();
        GeneralUse.CurrentAnimState currentState { get; set; }
        
    }

    public static class GeneralUse
    {
        public struct Stats
        {
            public int hp;
            public int speedAmp;
            public int dmg;
            //Default should take over if I don't pass anything in.
            public Stats(int uHP, int uSpeedAmp = 2, int uDmg = 2)
            {
                hp = uHP;
                speedAmp = uSpeedAmp;
                dmg = uDmg;
            }
           
        }
        /*public struct AnimTriggers
        {
            public string attack;
            public string move;
            public string die;
            public string objectToAnimate;
        }*/
        public enum CurrentAnimState
        {
            IDLE,
            MOVING,
            ATTACKING,
            HIT,
            DYING,
            SHOOTING
        }
       //Will play states directly, will save triggers and params like that for more specificity.
        ///0:Idle,1:Move,2:Attack,3:Hit,4:Die,5:Shoot
        public static Dictionary<CurrentAnimState,string> AnimStates = new Dictionary<CurrentAnimState, string>()
         {
            {CurrentAnimState.IDLE, "Idle" },
            {CurrentAnimState.MOVING, "Move" },
            {CurrentAnimState.ATTACKING, "Attack" },
            {CurrentAnimState.HIT, "Hit" },
            {CurrentAnimState.DYING, "Die"},
            {CurrentAnimState.SHOOTING, "Shoot" }
         };
    }
}