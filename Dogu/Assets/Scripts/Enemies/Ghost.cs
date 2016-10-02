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
            itemDropName = "GhostDrop";

            enemyStats.speedAmp = 20.0f;
   
        }

        protected override void Update()
        {
            base.Update();
        }
        
        protected override void EnemyMovement()
        {


            float getDistance = CheckDistance();
            bool inVicinity = (getDistance == player.GetComponent<CapsuleCollider>().radius) ? true : false;

            if (!inVicinity && currentState != GeneralUse.CurrentAnimState.ATTACKING)
            {
                Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 targetPos = new Vector2(player.transform.position.x, player.transform.position.y);

                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * enemyStats.speedAmp);
        
            }
        }
    }
}