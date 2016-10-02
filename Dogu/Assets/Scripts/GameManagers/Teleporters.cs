using UnityEngine;
using System.Collections;
namespace Dogu
{
    public class Teleporters : MonoBehaviour
    {

        //Will probably have particles flowing when they enter it
       
        ParticleSystem idleParticles;
        ParticleSystem enterParticles;
        ParticleSystem activateParticles;
        ParticleSystem leaveParticles;
        
        //i COULD EDIT JUST ONE SYSTEM AND SAVE THE ALL THE NUMBERS AND PROPERTIES OF IT, SO CHANGE KIND OF PARTICLES PROGRAMATICALLY BUT
        //THIS IS EASIER, MIGHT CHANGE LATER AND PARTICLES AREN'T A BIG PRIORITY.

        void Awake()
        {
            idleParticles = GameObject.Find("IdleTeleporter").GetComponent<ParticleSystem>();
        }

        //Will make this a coroutine then not instant teleport but just really fast and that will also give time for particle systems to play
        IEnumerator Teleport(Transform teleportee)
        {
            //Todo: Set up particles for different states of the teleporter.

            //Actually Arthur wanted only to teleport to top, not back down, so I'll just take out top part.
         //   idleParticles.Stop();
          //  enterParticles.Play();
           // yield return new WaitWhile(() => { return enterParticles.isPlaying; });

            //This works.
            if (this.name == "TeleportStart")
            {
           //     activateParticles.Play();
                //for now just instant, I'll do anim for it later.
                teleportee.position = GameObject.Find("TeleportEnd").transform.position;
            }
            yield return new WaitForEndOfFrame();
            //Then after the teleport is done it will desperse to new one, no yeild for this.
           // leaveParticles.Play();

        }
        void Update()
        {
            idleParticles.Play();
        }
        void OnTriggerEnter(Collider other)
        {

            StartCoroutine(Teleport(other.transform));
        }
    }
}