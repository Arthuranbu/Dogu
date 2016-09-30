using UnityEngine;
using System.Collections;
namespace Dogu
{
    public class Items : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject gameManager = GameObject.Find("")
            }
        }
    }
}