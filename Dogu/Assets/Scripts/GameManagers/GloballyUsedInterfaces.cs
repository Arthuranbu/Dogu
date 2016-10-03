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
            JUMP,
            DOUBLEJUMP,
            STOPPING,
            ATTACKING,
            HIT,
            DYING,
            SHOOTING
        }
       
     
        public static string[] enemyNames = new string[2]
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
<<<<<<< HEAD
        public const Dictionary<CurrentAnimState,string> ANIMSTATES = new Dictionary<CurrentAnimState, string>()
=======
        public static Dictionary<CurrentAnimState,string> animStates = new Dictionary<CurrentAnimState, string>()
>>>>>>> c34bbb8ddecd18b1a6a8dbdc98551eecf764dcee
         {
            {CurrentAnimState.IDLE, "Idle" },
            {CurrentAnimState.MOVING, "Move" },
            {CurrentAnimState.STOPPING,"Stop" },
            {CurrentAnimState.ATTACKING, "Attack" },
            {CurrentAnimState.JUMP,"Jump" },
            {CurrentAnimState.DOUBLEJUMP,"DoubleJump" },
            {CurrentAnimState.HIT, "Hit" },
            {CurrentAnimState.DYING, "Die"},
            {CurrentAnimState.SHOOTING, "Shoot" }
         };
        public void playAnim(Animator animator, string stateToPlay)
        {
            animator.SetTrigger(stateToPlay);
        }
<<<<<<< HEAD
        //Think about what else needs this, and whether I should mov ethis back to just collect items.
        public  const Dictionary<Enemy, string> DROPPEDITEMS = new Dictionary<Enemy, string>
        {
            "GhostDrop",
            "BoarDrop",
            "BirdDrop"
        };
=======
     
>>>>>>> c34bbb8ddecd18b1a6a8dbdc98551eecf764dcee
    }
}