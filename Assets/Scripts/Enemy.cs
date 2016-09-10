using UnityEngine;
using System.Collections;

namespace Dogu
{
    /// <summary>
    /// Base won't have attack interval and will only attack if player in vicinity for attacking.
    /// Spearmen will have attack interval.
    /// </summary>
    public abstract class Enemy : MonoBehaviour, Animations
    {
        //Might need to add delay a bit
        //I should have a handler class that handles this, but will write that later, that's more of a polish.
        protected Player player;

        private bool stillInRange;
        protected bool doingPostAction;

        GeneralUse.Stats EnemyStats;
        protected Animator enemyAnims;
        int directionMovement;


        #region Enemy States
        public bool Prepped
        { get; set; }

        public bool Dead
        { get; set; }

        public void Die()
        {
            currentState = GeneralUse.CurrentAnimState.DYING;
            PlayAnimation();
            if (!doingPostAction)
                StartCoroutine(PostAnimActions());
            
        }
        
        #endregion

        void Awake()
        {
            enemyAnims = GetComponentInChildren<Animator>();
        }

        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            Dead = false;
            Prepped = false;
           
            //I don't understand why default value doesn't work.
            EnemyStats = new GeneralUse.Stats(5,50);

        }
       
        protected void Update()
        {
            if (Prepped && !Dead && !player.Dead)
            {
                
                if (player.OnFloor)
                    EnemyMovement();
                //Will turn off loop for idle and just always call update so it loops via calling, that way won't have to repeat calling playanimation
                PlayAnimation();
            }
        }

        #region Enemy Movement
        protected abstract void EnemyMovement();
        /*{
            //This will differ from enemy to enemy.
            bool inVicinity = CheckDistance();
          
            if (!inVicinity && currentState != GeneralUse.CurrentAnimState.ATTACKING)
            {

                Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 targetPos = new Vector2(player.transform.position.x,player.transform.position.y);
             
                //Stupid but this works if 3d object and moving in 2d, BUT now attacking doesn't work, wtf;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * EnemyStats.speedAmp);
                currentState = GeneralUse.CurrentAnimState.MOVING;
                   // enemyAnims.Play(GeneralUse.AnimStates[currentState]);
            }
        }*/
        //This is used by all derivative of Enemy.
        bool CheckDistance()
        {
            bool inVicinity;
            float vicinity = player.GetComponent<CapsuleCollider>().radius;
            float dis = player.transform.position.x - transform.position.x;
            
            //Checks if distance is same as distance between player and its hitbox.
            if (dis != vicinity)
            {
                inVicinity = false;
                if (dis < 0)
                {
                    GetComponentInChildren<SpriteRenderer>().flipX = false;
                }
                else
                {
                    GetComponentInChildren<SpriteRenderer>().flipX = true;
                }

            }
            else
                inVicinity = true;

            return inVicinity;
        }
        #endregion

        #region On Collision Events
        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !Dead && player.OnFloor)
            {
                stillInRange = true;
                //Is in range of player so start attacking anim 
                currentState = GeneralUse.CurrentAnimState.ATTACKING;
                if (!doingPostAction)
                {
                    StartCoroutine(PostAnimActions());
                }
            }
        }
       
        //This is pretty general too.
        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && !Dead && player.OnFloor)
            {
                stillInRange = true;
                currentState = GeneralUse.CurrentAnimState.ATTACKING;
                
                if (!doingPostAction)
                {
                    
                    StartCoroutine(PostAnimActions());
                }
            }
        }
        //Virtual because spearmen will start attacking when out of vicinity

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !Dead)
            {
                stillInRange = false;
                if (!doingPostAction)
                    StartCoroutine(PostAnimActions());
            }
        }
        //So I'll put all in this and what happens after the yield all depends on argument I pass into here, this can work. Prob currentState

        #endregion

        #region Methods relating to Animations. 

        public GeneralUse.CurrentAnimState currentState
        {
            set;
            get;
        }
        public void PlayAnimation()
        {
            //Difference in this implentation will basically be adding all the extra checks I have cluttered in there and put in here
            enemyAnims.Play(GeneralUse.AnimStates[currentState]);
            
        }
        IEnumerator PostAnimActions()
        {
            doingPostAction = true;
            GeneralUse.CurrentAnimState prevState = currentState;
            if (Dead)
                Debug.Log(currentState);

            if (currentState != GeneralUse.CurrentAnimState.DYING)
                yield return new WaitForSeconds(enemyAnims.speed / 2);
            else
                yield return new WaitForSeconds(enemyAnims.speed);

            switch (currentState)
            {
                case GeneralUse.CurrentAnimState.ATTACKING:
                    {
                      
                        //This remains the same.
                        if (stillInRange)
                        {
                            player.DecreasePlayerHP();
                        }
                    }
                    break;
                case GeneralUse.CurrentAnimState.DYING:
                    {
                        //This is fine, delay is so attack anim goes through before dying, maybe just force the death
                        /*if (prevState == GeneralUse.CurrentAnimState.ATTACKING)
                            yield return new WaitForSeconds(enemyAnims.speed);*/
                            //leave in but commented out
                        Destroy(gameObject);
                    }
                        break;
            }
            //Post anim will always revert back to idle state, inputs will change it to diff states
            if (!stillInRange)
                currentState = GeneralUse.CurrentAnimState.IDLE;
            
            doingPostAction = false;
        }
       
           
        #endregion
    }
}