using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class WaveGameType : GameManager
    {

        int Waves
        {
            set { _amount += value; }
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
            //Amount of waves, this will vary probably
            _amount = 5;
        }
    }
}