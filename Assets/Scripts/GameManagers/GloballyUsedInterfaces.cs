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

        public enum CurrentAnimState
        {
            IDLE,
            MOVING,
            STOPPING,
            ATTACKING,
            HIT,
            DYING,
            SHOOTING
        }
        //Should I make it flags? enums take less time than strings, but going to have to use strings regardless, so basicall same as below.
        //So is it better If I just use strings straight up then whatever it randomizes to I save that instead of assigning something completely
        //diff cause again it's same logic of using magic value that has no real purpose other than to be used as a trigger to pass in another 
        //value, if it's trigger to do an event then it's fine it has a purpose but it's trigger to assign the real trigger to event.
        public static string[] enemyTypes =
        {
            "Ghost",
            "Bird",
            "Boar"
        };
       //Will play states directly, will save triggers and params like that for more specificity.
        ///0:Idle,1:Move,2:Attack,3:Hit,4:Die,5:Shoot
        public static Dictionary<CurrentAnimState,string> animStates = new Dictionary<CurrentAnimState, string>()
         {
            {CurrentAnimState.IDLE, "Idle" },
            {CurrentAnimState.MOVING, "Move" },
            {CurrentAnimState.STOPPING,"Stop" },
            {CurrentAnimState.ATTACKING, "Attack" },
            {CurrentAnimState.HIT, "Hit" },
            {CurrentAnimState.DYING, "Die"},
            {CurrentAnimState.SHOOTING, "Shoot" }
         };
    }
}