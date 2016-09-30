using System.Collections.Generic;
using UnityEngine;
//Change name of this script
namespace Dogu 
{
    //Interface that all classes will implement
    //Doing this because though alot of it will be basic just do animation, different implentations will add seperate things.
    public interface Animations
    {
       
        GeneralUse.CurrentAnimState currentState { get; set; }
        
    }
    public interface GameManaging
    {
        //Go to next wave, choose diff target and number to kill, or choose diff item and number of which to collect.
        void nextRound();
        //This with timeLeft argument is only applicable to some, though maybe can make work so will leave as is for now
        void increaseDifficulty(float timeLeft);
        //Will call prepGame, this might not need different implementations.
        void restartGame();
        //Will include choosing target, spawning targets, and setting drops.
        void prepGame();
        //Target number of waves till end, target enemy to kill, target item to loot.
        void setTarget();
    }

    public static class GeneralUse
    {
        //Globally used fields.
        public const float GRAVITY = 30.0f;

        public struct Stats
        {
            public int hp;
            public float speedAmp;
            public int dmg;
            //Default should take over if I don't pass anything in.
            public Stats(int uHP, float uSpeedAmp = 2, int uDmg = 2)
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
        public static Enemy[] allEnemies = new Enemy[2]
        {
           new Boar(),
           new Ghost()
        };
        public static string[] allEnemyNames = new string[2]
        {
            "Boar",
            "Ghost"
        };

        //sTATES ALREADY IN ANIM STATES WILL RENAME LATER
        /*public struct enemyStates
        {
            //This will onyl apply to boar actually, ghosts are forever chasing and birds go into the ground, yeah
            public bool onFloor;
            public bool inRange;
        }*/
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
        public static void playAnim(Animator animator, string stateToPlay)
        {
            animator.SetTrigger(Animator.StringToHash(stateToPlay));
        }
        //Think about what else needs this, and whether I should mov ethis back to just collect items.
        public static Dictionary<Enemy, string> droppedItems = new Dictionary<Enemy, string>
        {
            //Could've sworn this worked before, but even if base enemies the fact their different derivatives
            //means they're not same keys.
            {new Ghost(),"GhostDrop" },
            {new Boar(), "BoarDrop" }
            
        };
    }
}