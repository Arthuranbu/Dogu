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

        protected GameObject[] enemySpawnPoints;
        protected GeneralUse.Stats EnemyStats;
        protected Animator enemyAnims;

        #region Enemy States
        public bool Prepped
        { get; set; }

        private string trimName()
        {
            string objectName = "";
            for (int i = 0; i < this.name.Length; i++)
            {
                if (this.name[i] == '(')
                {
                    for (int j = 0; j < i; j++)
                    {
                        Debug.Log(this.name[j]);
                        objectName += this.name[j];
                    }
                    break;
                }
            }
            return objectName;

        }
        //Make this a coroutine so not all stacked together incase of spawning in same place, low chance but chance.
        public virtual void Prepare()
        {
            Prepped = true;
        }

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
           
            EnemyStats = new GeneralUse.Stats(5,50);

        }
       
        protected virtual void Update()
        {
            if (Prepped)
            {
                EnemyMovement();
                PlayAnimation();
            }
        }

        #region Enemy Movement
        //This will vary from enemy to enemy so will be abstract.
        protected abstract void EnemyMovement();
        
        //this is active for all, scratch that, only ghosts will chase player so only applies to them
        
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
            
               
            enemyAnims.Play(GeneralUse.animStates[currentState]);
            
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