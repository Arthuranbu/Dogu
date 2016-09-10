using UnityEngine;
using System.Collections;

namespace Dogu
{
    /// <summary>
    /// Base won't have attack interval and will only attack if player in vicinity for attacking.
    /// Spearmen will have attack interval.
    /// </summary>
    public class Enemy : MonoBehaviour, Animations
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
        private void EnemyMovement()
        {
            bool inVicinity = CheckDistance();
          
            if (!inVicinity && currentState != GeneralUse.CurrentAnimState.ATTACKING)
            {

                /* float direction = 1.0f;
                 if (player.transform.position.x - transform.position.x < 0)
                     direction *= -1;*/
                //float direction = player.transform.position.x - transform.position.x;
                Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 targetPos = new Vector2(player.transform.position.x,player.transform.position.y);
             
                //Stupid but this works if 3d object and moving in 2d, BUT now attacking doesn't work, wtf;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * EnemyStats.speedAmp);
                currentState = GeneralUse.CurrentAnimState.MOVING;
                   // enemyAnims.Play(GeneralUse.AnimStates[currentState]);
            }
        }
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

                currentState = GeneralUse.CurrentAnimState.ATTACKING;
                if (!doingPostAction)
                {
                    StartCoroutine(PostAnimActions());
                }

            }
        }
        //Virtual because spearmen will probably run instead of keep attacking to stay ranged
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
            get;//Going to add more to this too 
        }
        public void PlayAnimation()
        {
            //Difference in this implentation will basically be adding all the extra checks I have cluttered in there and put in here
            
               
            enemyAnims.Play(GeneralUse.AnimStates[currentState]);
            
        }
        IEnumerator PostAnimActions()
        {
            //Speed is equal to whatever current state is. Would have get stateinfo if 
            //had multiple errors but it auto sees only one and sees that as default layer
            //and gets that.
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
                      
                        if (stillInRange)
                        {
                            player.DecreasePlayerHP();
                        }
                    }
                    break;
                case GeneralUse.CurrentAnimState.DYING:
                    {
                        if (prevState == GeneralUse.CurrentAnimState.ATTACKING)
                            yield return new WaitForSeconds(enemyAnims.speed);
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