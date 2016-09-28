using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class CameraManager : MonoBehaviour
    {
        Camera mainMenuCamera;
        Camera mainSceneCamera;

        public void switchCameras()
        {
            if (mainMenuCamera.targetDisplay == 0)
            {
                mainMenuCamera.targetDisplay = 1;
                mainSceneCamera.targetDisplay = 0;
            }
            else if (mainSceneCamera.targetDisplay == 0)
            {
                mainSceneCamera.targetDisplay = 1;
                mainMenuCamera.targetDisplay = 0;
            }

        }

        void Awake()
        {
            mainMenuCamera = GameObject.Find("MainMenuCamera").GetComponent<Camera>();
            mainSceneCamera = GameObject.Find("MainSceneCamera").GetComponent<Camera>();
        }


        //ToDo: Move panning to separate function and change to do while yield every frame, play with numbers for how much I have to pan it.
        void OnTriggerStay(Collider other)
        {     //This will pan the camera in either direction when player hits border
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<Player>().currentState == GeneralUse.CurrentAnimState.MOVING)
                //Will get bg reference and when position reacges certain point then do this.
                {

                    //It's always only doing left.//My dumb ass logic, of course it's other way around holy shit. player is terminal and threshhold is initial.
                    //Hate having direction just for this but meh,
                    if (other.GetComponent<Player>().direction > 0)
                        mainSceneCamera.transform.Translate(transform.right * Time.deltaTime * 20);
                    else if (other.GetComponent<Player>().direction < 0)
                        mainSceneCamera.transform.Translate(-transform.right * Time.deltaTime * 20);
                }
            }
        }
    }
}