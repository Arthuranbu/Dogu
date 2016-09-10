using UnityEngine;
using System.Collections;
namespace Dogu
{
    public class CollectGameType : GameManager
    {

        int ItemsLeftToCollect
        {
            set { _amount -= value; }
            get { return _amount; }
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        protected override void PrepGame()
        {
            _amount = Random.Range(1, 6);
            //it will vary from enemy to enemy.
        }
    }
}