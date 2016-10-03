using UnityEngine;
using System.Collections;
namespace Dogu
{
    public class Teleporters : MonoBehaviour
    {

        //Will probably have particles flowing when they enter it
       
        
        
        //i COULD EDIT JUST ONE SYSTEM AND SAVE THE ALL THE NUMBERS AND PROPERTIES OF IT, SO CHANGE KIND OF PARTICLES PROGRAMATICALLY BUT
        //THIS IS EASIER, MIGHT CHANGE LATER AND PARTICLES AREN'T A BIG PRIORITY.

        void Awake()
        {
        }

        //Will make this a coroutine then not instant teleport but just really fast and that will also give time for particle systems to play
        IEnumerator Teleport(Transform teleportee)
        {
          
            if (this.name == "TeleportStart")
            {
           //     activateParticles.Play();
                //for now just instant, I'll do anim for it later.
                teleportee.position = GameObject.Find("TeleportEnd").transform.position;
            }
            yield return new WaitForEndOfFrame();
     

        }
        void Update()
        {
        }
        void OnTriggerEnter(Collider other)
        {

            StartCoroutine(Teleport(other.transform));
        }
    }
}