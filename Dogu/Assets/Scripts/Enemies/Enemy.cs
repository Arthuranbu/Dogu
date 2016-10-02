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
        //  I should have a handler class that handles this, but will write that later, that's more of a polish.
        protected Player player;
        protected IGameType updateProgress;

        //Okay so no Enemy ref in game manager so not circularly dependant, so even in rush did atleast that right.
        protected GameManager gameManager;

        private bool stillInRange;
        protected bool doingPostAction;

        protected GameObject[] enemySpawnPoints;
        protected GeneralUse.Stats enemyStats;
        //Only one need right now.
        protected float baseSpeed;
        
        protected Animator enemyAnims;

        #region Enemy States

        protected GameObject itemToDrop;

        public string itemDropName
        { set; get; }
        
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
                        objectName += this.name[j];
                    }
                    break;
                }
            }
            return objectName;

        }
        //Make this a coroutine so not all stacked together incase of spawning in same place, low chance but chance.
        public virtual void PrepareEnemy()
        {
            itemToDrop = Resources.Load<GameObject>("Prefabs/" + itemDropName);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            Prepped = true;
            enemyAnims = GetComponentInChildren<Animator>();
        }

        public bool Dead
        { get; set; }

        public IEnumerator Die()
        {

            currentState = GeneralUse.CurrentAnimState.DYING;
            GeneralUse.playAnim(enemyAnims, GeneralUse.animStates[currentState]);

            if (gameManager.currentGameType is CollectItems)
            {
                gameManager.currentGameType.dropItem(this.gameObject);
            }

            if (gameManager.currentGameType.targetName + "(Clone)" == gameObject.name)
            {
                gameManager.currentGameType.GoalAmount--;
                Debug.Log(gameManager.currentGameType.GoalAmount);
                gameManager.UpdateUI();
            }
            Dead = true;
            yield return new WaitForSeconds(enemyAnims.speed * 2);
            if (!doingPostAction)
                PostAnimActions();
            
        }

        #endregion


        void Awake()
        {

            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        // Use this for initialization
        void Start()
        {
           

            Dead = false;
            Prepped = false;
           

        }
       
        protected virtual void Update()
        {
            if (Prepped && !Dead)
            {
                EnemyMovement();
                GeneralUse.playAnim(enemyAnims,GeneralUse.animStates[currentState]);
            }
        }


        #region Enemy Movement
        protected float CheckDistance()
        {
          
            float dis = player.transform.position.x - transform.position.x;


            //Checks if distance is same as distance between player and its hitbox.
          
                if (dis < 0)
                {
                    GetComponentInChildren<SpriteRenderer>().flipX = false;
                }
                else
                {
                    GetComponentInChildren<SpriteRenderer>().flipX = true;
                }
            

            return dis;
        }
        //This will vary from enemy to enemy so will be abstract.
        protected virtual void EnemyMovement()
        {
            currentState = GeneralUse.CurrentAnimState.MOVING;
        }
        
        //this is active for all, scratch that, only ghosts will chase player so only applies to them
        
        #endregion

        #region On Collision Events
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !Dead)
            {
                stillInRange = true;

                currentState = GeneralUse.CurrentAnimState.ATTACKING;

                if (!doingPostAction)
                {
                    PostAnimActions();
                }
            }
        }
        //Virtual because spearmen will probably run instead of keep attacking to stay ranged
        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && !Dead)
            {
                stillInRange = true;
                currentState = GeneralUse.CurrentAnimState.ATTACKING;
                if (!doingPostAction)
                {

                    PostAnimActions();
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
                    PostAnimActions();
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
   
        void PostAnimActions()
        {
            //Speed is equal to whatever current state is. Would have get stateinfo if 
            //had multiple errors but it auto sees only one and sees that as default layer
            //and gets that.
            doingPostAction = true;
            GeneralUse.CurrentAnimState prevState = currentState;
            
            switch (currentState)
            {
                case GeneralUse.CurrentAnimState.ATTACKING:
                    {
                        if (stillInRange)
                        {
                            player.DecreasePlayerHP();
                        }
                        else
                            currentState = GeneralUse.CurrentAnimState.MOVING;
                    }
                    break;
                case GeneralUse.CurrentAnimState.DYING:
                    {
                        if (this != null)
                         gameObject.SetActive(false);
                    }
                        break;
            }
            //Post anim will always revert back to idle state, inputs will change it to diff states
            if (!stillInRange)
                currentState = GeneralUse.CurrentAnimState.IDLE;
            
            doingPostAction = false;
        }
       
           
        #endregion
        void dropItem()
        {

            
            GameObject droppedItem = Instantiate(Resources.Load<GameObject>("Prefabs/Items/" + itemDropName));
            //Spawns the instantiated dropped item to where the enemy just killed was.
            droppedItem.transform.position = transform.position;
        }

    }
}