using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dogu
{
    public class Player : MonoBehaviour, Animations
    {
       
        Animator playerAnims;
        bool isAttacking;
        #region PlayerHealth Vars 
        public GeneralUse.Stats playerStats;
        GameObject[] healthMeters;
        IEnumerator currentHeart;
        float invincibilityFrames = 1.0f;
        float leftInvincible;
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
        float jumpHeight = 20.0f;

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
            leftInvincible = invincibilityFrames;
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
            Debug.Log(currentState);

            
            if (Input.GetAxis("Attack") > 0)
                Attack();
            else if (Input.GetAxis("RangedAtk") > 0 && !_isShooting)
                StartCoroutine(rangedAttack());
            else if (playerAnims.GetCurrentAnimatorStateInfo(0).IsName("Dogu" + GeneralUse.animStates[GeneralUse.CurrentAnimState.IDLE]))
                currentState = GeneralUse.CurrentAnimState.IDLE;
            if (!_isDead && !isAttacking)
                playerMovement();
            GeneralUse.playAnim(playerAnims, GeneralUse.animStates[currentState]);
            //for debugging.
            if (leftInvincible > 0)
            {
                leftInvincible -= Time.deltaTime;
            }
            if (!OnFloor && _doneJumping)
            {
                transform.Translate(-transform.up * (GeneralUse.GRAVITY * 2) * Time.deltaTime);
              
            }
                
        }
        #region Player Actions
        void playerMovement()
        {

            //Moving this block to a function later.
            bool jumped = Input.GetKeyDown(KeyCode.Space);
            direction = Input.GetAxis("Horizontal");

            transform.Translate(transform.right * direction * Time.deltaTime * playerStats.speedAmp);
            if (direction != 0)
            {
                currentState = GeneralUse.CurrentAnimState.MOVING;


                firePoint.localPosition *= direction;
                if (direction < 0)
                {
                    firePoint.localPosition -= new Vector3(3.0f, 0, 0);
                    doguSprite.flipX = true;
                    if (blastInstance != null)
                        blastInstance.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    firePoint.localPosition += new Vector3(3.0f, 0, 0);
                    doguSprite.flipX = false;
                    if (blastInstance != null)
                        blastInstance.GetComponent<SpriteRenderer>().flipX = false;
                }

            }
            
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
            {
                _doneJumping = false;
                currentState = GeneralUse.CurrentAnimState.JUMP;
            }
            else if (!_doneJumping && _singleJumping)
            {
                _doubleJumping = true;
                currentState = GeneralUse.CurrentAnimState.DOUBLEJUMP;

                jumpAmp += 7.0f;
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
           // GeneralUse.playAnim(playerAnims, GeneralUse.animStates[currentState]);

        }
        void playAttackAnim()
        {
            playerAnims.SetTrigger(GeneralUse.animStates[currentState]);
        }
        IEnumerator rangedAttack()
        {
            _isShooting = true;
           


            if (blastInstance == null)
                blastInstance = Instantiate(blastPrefab);
         

            //Changed so doesn't control it, seems pointless in small area now, but if need to keep jst move inside 
            //do block.

            

            
            currentState = GeneralUse.CurrentAnimState.SHOOTING;
            blastInstance.SetActive(false);
            //  GeneralUse.playAnim(playerAnims, GeneralUse.animStates[currentState]);
            yield return new WaitForSeconds(playerAnims.speed / 2);
            blastInstance.transform.position = firePoint.position;
          //  blastInstance.transform.rotation = firePoint.rotation;
            blastInstance.SetActive(true);
            float blastDirection = (blastInstance.GetComponent<SpriteRenderer>().flipX) ? -1.0f : 1.0f;
            float blastLife = 0;
            //either use blastlife as or statement to kill blast over time, OR since going to have levelInteractions script, could just let death area handle it.
            do
            {
               blastInstance.transform.Translate(transform.right * blastDirection * Time.deltaTime * 100);
                yield return new WaitForEndOfFrame();
                blastLife += Time.deltaTime;
                if (blastLife > 2.0f) 
                    blastInstance.SetActive(false);
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
                if (leftInvincible <= 0)
                {
                    PlayHUDAnimation(currentHeart.Current as GameObject, "HPLoss");
                    leftInvincible = invincibilityFrames;
                    if (!currentHeart.MoveNext())
                    {
                        Die();
                        Debug.Log("Player is dead,stop hitting me");
                    }
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
            //GeneralUse.playAnim(playerAnims, GeneralUse.animStates[currentState]);


        }


        //Move this to 
        public void Spawn()
        {
            //Have to get these references here, because call when come back from mainmenu, even though new instance, and thus should call start again and get these references, for whatever reason it doesn't soo yeah.
            healthMeters = GameObject.FindGameObjectsWithTag("PlayerHealth");
            currentHeart = healthMeters.GetEnumerator();
            currentHeart.MoveNext();
            foreach (GameObject heart in healthMeters)
            {
                PlayHUDAnimation(heart, "HPGain");
            }
            //Sweet perfectly in sync naturally, I don't have to put any delay/speed anything up.
            if (Dead)
            {
                currentState = GeneralUse.CurrentAnimState.IDLE;
                GeneralUse.playAnim(playerAnims, GeneralUse.animStates[currentState]);

            }
            Dead = false;

        }
        
        public GeneralUse.CurrentAnimState currentState
        {
            set; get;

        }
        #endregion

            #region Collision Events
        void HurtEnemy(GameObject enemyRef)
        {

            Enemy enemyAttacked = enemyRef.GetComponent<Enemy>();
            enemyAttacked.Dead = true;
            StartCoroutine(enemyAttacked.Die());
            

        }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Threshhold") && currentState == GeneralUse.CurrentAnimState.MOVING)
            {
                CameraManager moveCamera = GameObject.Find("GameManager").GetComponent<CameraManager>();

                StartCoroutine(moveCamera.MoveCamera(direction));
            }
            //Might make weapon script, and have it handle there on the cone collider I was thinking about, but for now just to make it playable.

            if (other.CompareTag("Floor"))
            {
                _onFloor = true;

            }
            if (other.CompareTag("Enemy"))
            {
                if (currentState == GeneralUse.CurrentAnimState.ATTACKING)
                    HurtEnemy(other.gameObject);
            }
        }
        void OnTriggerStay(Collider other)
        {
            
            if (other.CompareTag("Floor"))
            {
                _onFloor = true;

            }
            //Hate copy and past but fuck.
            if (other.CompareTag("Enemy"))
            {

                if (currentState == GeneralUse.CurrentAnimState.ATTACKING && (other.gameObject.GetComponent<Enemy>().Dead == false))
                    HurtEnemy(other.gameObject);
            }
            
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Threshhold") && currentState == GeneralUse.CurrentAnimState.MOVING)
            {
                CameraManager moveCamera = GameObject.Find("GameManager").GetComponent<CameraManager>();
                StartCoroutine(moveCamera.MoveCamera(direction));
            }
            if (other.CompareTag("Floor"))
                _onFloor = false;
            
        }
        #endregion
    }
}