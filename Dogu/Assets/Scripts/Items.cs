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
                GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                if (this.gameObject.name == gameManager.currentGameType.targetName + "(Clone)")
                    Debug.Log("dfd");
            }
        }
    }
}