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
        bool _singleJumping = false;
        bool _doubleJumping = false;
        #endregion

        Transform firePoint;
        SpriteRenderer doguSprite;
        //Should maintain same gravity for all, just amps will vary.
        static GeneralUse.Stats initPlayerStats;
        GameObject blastPrefab;
        GameObject blastInstance;
        //Should be inside the struct, but enemies aren't jumping right now so won't.
        //Public soley for camera pan
        public float direction;
        float jumpHeight = 15.0f;

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
            GeneralUse.playAnim(playerAnims, GeneralUse.animStates[currentState]);
            if (Input.GetAxis("Attack") > 0)
                Attack();
            if (Input.GetAxis("RangedAtk") > 0 && !_isShooting)
                StartCoroutine(rangedAttack());
            if (!_isDead)
                playerMovement();
            //for debugging.
            
            if (!OnFloor && _doneJumping)
            {
                transform.Translate(-transform.up * (GeneralUse.GRAVITY * 2) * Time.deltaTime);
                if (transform.position.y == GameObject.Find("FloorStart").transform.position.y)
                {
                    OnFloor = true;
                    _doubleJumping = false;
                    _singleJumping = false;
                }
            }
                
        }
        #region Player Actions
        void playerMovement()
        {

            //Moving this block to a function later.
            bool jumped = Input.GetKeyDown(KeyCode.Space);
            direction = Input.GetAxis("Horizontal");

            currentState = GeneralUse.CurrentAnimState.MOVING;
            transform.Translate(transform.right * direction * Time.deltaTime * playerStats.speedAmp);
            if (direction != 0)
            {
                
                firePoint.localPosition *= direction;
                if (direction < 0)
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
                
            }
            else
                    currentState = GeneralUse.CurrentAnimState.STOPPING;
            if (jumped)
            {
                if (_doneJumping && _onFloor)
                {
                    _singleJumping = true;
                    StartCoroutine(Jump());
                }
                else if (!_doneJumping)

                    StartCoroutine(Jump());
            }
            

            
        }
        IEnumerator Jump()
        {
            OnFloor = false;
            float jumpAmp = 1.0f;

            if (_doneJumping)
                _doneJumping = false;
            else if (!_doneJumping && _singleJumping)
            {
                Debug.Log("I'm double jumping! I'm doing it!");
                _doubleJumping = true;
                jumpAmp += 5.0f;
            }
            Vector3 initPos = transform.position;

            //Jump up loop
            do
            {
                transform.Translate(transform.up * Time.deltaTime * GeneralUse.GRAVITY);
                yield return new WaitForEndOfFrame();
                if (_doubleJumping)
                {
                    _doneJumping = false;
                  
                }
            } while (transform.position.y <= initPos.y + (jumpHeight + jumpAmp));


            //Starts to go down from here, like life.

            
            _doneJumping = true;
        }

        

        void Attack()
        {
            currentState = GeneralUse.CurrentAnimState.ATTACKING;
            
        }
        IEnumerator rangedAttack()
        {
            _isShooting = true;
            currentState = GeneralUse.CurrentAnimState.SHOOTING;
            

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

       
       
        void PlayHUDAnimation(GameObject UIElement,string state)
        {
            UIElement.GetComponent<Animator>().Play(state);
            
        }
        void Die()
        {
            Dead = true;
            currentState = GeneralUse.CurrentAnimState.DYING;
           
        }

        
        //Move this to 
        public void Spawn()
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