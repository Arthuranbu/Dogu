using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dogu
{
    public class Player : MonoBehaviour, Animations
    {
       
        Animator playerAnims;

        #region PlayerHealth Vars 
        public GeneralUse.Stats playerStats;
        GameObject[] healthMeters;
        IEnumerator currentHeart;
        #endregion
        #region Player States
        bool _isShooting;
        bool _isDead;
        bool _onFloor;
        bool _doneJumping = true;
        #endregion

        Transform firePoint;
        SpriteRenderer doguSprite;
        float gravity = 30.0f;
        static GeneralUse.Stats initPlayerStats;
        GameObject blastPrefab;
        GameObject blastInstance;
        //Should be inside the struct, but enemies aren't jumping right now so won't.
        float jumpHeight = 6.0f;

        public bool Dead
        {
            get { return _isDead; }
            private set { _isDead = value; }
        }

        public bool OnFloor
        {
            private set { _onFloor = value; }
            get { return _onFloor; }
        }

        
        void Start()
        {
            doguSprite = GameObject.Find("DoguSprite").GetComponent<SpriteRenderer>();
            firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();
            blastPrefab = Resources.Load("Prefabs/DoguRangedAttack") as GameObject;
            
            healthMeters = new GameObject[5];
            healthMeters = GameObject.FindGameObjectsWithTag("PlayerHealth");
            playerAnims = GetComponentInChildren<Animator>();
            currentHeart = healthMeters.GetEnumerator();
            currentHeart.MoveNext();
            //onFloor = true;
            //Hp stat useless now with heart system in place, will could put them in sync and play only half animation depending on hp, but eh hmm.
            playerStats.hp = 10;
            playerStats.speedAmp = 100;
            playerStats.dmg = 4;
            //Saves initial values, prob better way to make this more modular since using in enemies too, only thing could think of is making them ALL under base class livingThing or something. I could do that. would be best, so I'll keep off doing stats for enemies and just handle spawning and moving first.
            initPlayerStats = new GeneralUse.Stats(playerStats.hp, playerStats.speedAmp, playerStats.dmg);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Attack") > 0)
                Attack();
            if (Input.GetAxis("RangedAtk") > 0 && !_isShooting)
                StartCoroutine(rangedAttack());
            if (!_isDead)
                playerMovement();
            //for debugging.
            
            if (!OnFloor && _doneJumping)
            {
                transform.Translate(-transform.up * gravity * Time.deltaTime);
                if (transform.position.y == GameObject.Find("FloorStart").transform.position.y)
                    OnFloor = true;
            }
                
        }
        #region Player Actions
        void playerMovement()
        {
            
            //Moving this block to a function later.
            float jumpVal = Input.GetAxis("Jump");
            float horizontalVal = Input.GetAxis("Horizontal");


            transform.Translate(transform.right * horizontalVal * Time.deltaTime * playerStats.speedAmp);
            if (horizontalVal != 0)
            {
                
                firePoint.localPosition *= horizontalVal;
                if (horizontalVal < 0)
                {
                    firePoint.eulerAngles = new Vector3(0, 180.0f, 0);
                    firePoint.localPosition -= new Vector3(3.0f, 0, 0);
                    doguSprite.flipX = true;
                }
                else
                {
                    firePoint.eulerAngles = Vector3.zero;
                    firePoint.localPosition += new Vector3(3.0f,0,0);
                    doguSprite.flipX = false;
                }
                currentState = GeneralUse.CurrentAnimState.MOVING;
            }
            else
                    currentState = GeneralUse.CurrentAnimState.STOPPING;
            if (jumpVal > 0 && _onFloor && _doneJumping)
            {
                StartCoroutine(Jump());
            }
            PlayAnimation();
            
        }
        IEnumerator Jump()
        {
            _doneJumping = false;
            Vector3 initPos = transform.position;


            //Jump up loop
            do
            {
                transform.Translate(transform.up * Time.deltaTime * gravity/2);
                yield return new WaitForEndOfFrame();
            }
            while (transform.position.y <= initPos.y + jumpHeight);
            OnFloor = false;
            if (Input.GetAxis("Jump") > 0 )
            {
                do
                {
                    Debug.Log("double jump");
                    transform.Translate(transform.up * Time.deltaTime * gravity / 2);
                    yield return new WaitForEndOfFrame();
                } while (transform.position.y <= initPos.y + (jumpHeight * 2));
            }
              
            //Starts to go down from here, like life.
            _doneJumping = true;
        }

        void Attack()
        {
            currentState = GeneralUse.CurrentAnimState.ATTACKING;
            PlayAnimation();
        }
        IEnumerator rangedAttack()
        {
            _isShooting = true;
            currentState = GeneralUse.CurrentAnimState.SHOOTING;
            
            PlayAnimation();

            if (blastInstance == null)
                blastInstance = Instantiate(blastPrefab);
         

            //Changed so doesn't control it, seems pointless in small area now, but if need to keep jst move inside 
            //do block.

            float directionShot;
            if (doguSprite.flipX)
            {
                directionShot = 1.0f;
                blastInstance.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                directionShot = -1.0f;
                blastInstance.GetComponent<SpriteRenderer>().flipX = true;
            }
            yield return new WaitForSeconds(playerAnims.speed);
            

            blastInstance.transform.position = firePoint.position;
            blastInstance.transform.rotation = firePoint.rotation;
            blastInstance.SetActive(true);
            float blastLife = 0;
            //either use blastlife as or statement to kill blast over time, OR since going to have levelInteractions script, could just let death area handle it.
            do
            {
               blastInstance.transform.Translate(transform.right * directionShot * Time.deltaTime * 20);
                yield return new WaitForEndOfFrame();

            } while (blastInstance.activeInHierarchy);
            
            //Destroy(tempInstance);
            _isShooting = false;
        }
        #endregion

        #region AnimationMethods
        public void DecreasePlayerHP()
        {
            if (!Dead)
            {
                Debug.Log(((GameObject)currentHeart.Current).name);
                PlayHUDAnimation(currentHeart.Current as GameObject, "HPLoss");
                if (!currentHeart.MoveNext())
                {
                    Die();
                    Debug.Log("Player is dead,stop hitting me");
                }
            }
        }

       
        public void PlayAnimation()
        {
            playerAnims.SetTrigger(Animator.StringToHash(GeneralUse.AnimStates[currentState]));
        }
        void PlayHUDAnimation(GameObject UIElement,string state)
        {
            UIElement.GetComponent<Animator>().Play(state);
            
        }
        void Die()
        {
            Dead = true;
            currentState = GeneralUse.CurrentAnimState.DYING;
            PlayAnimation();
           
        }

        //This should honestly be in gameManager, I could put a function in there that calls this
        //But eh, fuck it just call directly.
        public void Respawn()
        {
            Dead = false;
            //Have to get these references here, because call when come back from mainmenu, even though new instance, and thus should call start again and get these references, for whatever reason it doesn't soo yeah.
            healthMeters = GameObject.FindGameObjectsWithTag("PlayerHealth");
            currentHeart = healthMeters.GetEnumerator();
            currentHeart.MoveNext();
            foreach (GameObject heart in healthMeters)
            {
                PlayHUDAnimation(heart, "HPGain");
            }
            //Sweet perfectly in sync naturally, I don't have to put any delay/speed anything up.

        }
        
        public GeneralUse.CurrentAnimState currentState
        {
            set; get;
        }


        #endregion

        #region Collision Events
        void HurtEnemy(GameObject enemyRef)
        {

            if (currentState == GeneralUse.CurrentAnimState.ATTACKING)
            {
                Enemy enemyAttacked = enemyRef.GetComponent<Enemy>();
                if (!enemyAttacked.Dead)
                {
                    enemyAttacked.Dead = true;
                    enemyAttacked.Die();
                }
            }

        }
        void OnTriggerEnter(Collider other)
        {
            //Might make weapon script, and have it handle there on the cone collider I was thinking about, but for now just to make it playable.

            if (other.CompareTag("Floor"))
            {
                _onFloor = true;

            }
            if (other.CompareTag("Enemy"))
            {
                //  currentState = GeneralUse.CurrentAnimState.ATTACKING;
                
                    HurtEnemy(other.gameObject);
            }
        }
        void OnTriggerStay(Collider other)
        {
            //Hate copy and past but fuck.
            if (other.CompareTag("Enemy"))
            {
                HurtEnemy(other.gameObject);
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Floor"))
                _onFloor = false;
        }
        #endregion
    }
}