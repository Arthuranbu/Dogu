using UnityEngine;
using System.Collections;
namespace Dogu
{
    public class Ghost : Enemy
    {

        // Use this for initialization

        public override void PrepareEnemy()
        {
            base.PrepareEnemy();
            enemyStats.speedAmp = 10.0f;
   
        }

        protected override void Update()
        {
            base.Update();
        }
        
        protected override void EnemyMovement()
        {
            base.Update();

            float getDistance = CheckDistance();
            bool inVicinity = (getDistance == player.GetComponent<CapsuleCollider>().radius) ? true : false;

            if (!inVicinity && currentState != GeneralUse.CurrentAnimState.ATTACKING)
            {

                /* float direction = 1.0f;
                 if (player.transform.position.x - transform.position.x < 0)
                     direction *= -1;*/
                //float direction = player.transform.position.x - transform.position.x;
                Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 targetPos = new Vector2(player.transform.position.x, player.transform.position.y);

                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * enemyStats.speedAmp);
        
            }
        }
    }
}