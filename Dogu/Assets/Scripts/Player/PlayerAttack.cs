using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class PlayerAttack : MonoBehaviour
    {
        Player player;

        // Use this for initialization
        void Start()
        {
            player = GetComponentInParent<Player>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}