using UnityEngine;
using System.Collections;
namespace Dogu
{
    public class Ghost : Enemy
    {

        // Use this for initialization

        public override void Prepare()
        {
            base.Prepare();
   
        }

        protected override void Update()
        {
            base.Update();
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
        protected override void EnemyMovement()
        {

            Debug.Log("Ghost Moving");
            bool inVicinity = CheckDistance();

            if (!inVicinity && currentState != GeneralUse.CurrentAnimState.ATTACKING)
            {

                /* float direction = 1.0f;
                 if (player.transform.position.x - transform.position.x < 0)
                     direction *= -1;*/
                //float direction = player.transform.position.x - transform.position.x;
                Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 targetPos = new Vector2(player.transform.position.x, player.transform.position.y);

                //Stupid but this works if 3d object and moving in 2d, BUT now attacking doesn't work, wtf;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * EnemyStats.speedAmp);
                currentState = GeneralUse.CurrentAnimState.MOVING;
                // enemyAnims.Play(GeneralUse.AnimStates[currentState]);
            }
        }
    }
}