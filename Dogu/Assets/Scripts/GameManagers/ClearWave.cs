﻿using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class ClearWave : IGameType
    {
        public override void increaseDifficulty()
        {

        }
        
        public override void prepareGame()
        {
            targetName = "ClearWaves";
          
        }
        void Start()
        {
        }
        void Update()
        {
            //Sweet so theory works.
        }
    }
}