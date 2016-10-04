using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class CameraManager : MonoBehaviour
    {
        Camera mainMenuCamera;
        Camera mainSceneCamera;
        Vector3 mainSceneCameraInitPos;
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
            mainSceneCameraInitPos = mainSceneCamera.transform.position;

        }
        public void resetCameraPositions()
        {
            mainSceneCamera.transform.position = mainSceneCameraInitPos;
        }

        void Awake()
        {
            mainMenuCamera = GameObject.Find("MainMenuCamera").GetComponent<Camera>();
            mainSceneCamera = GameObject.Find("MainSceneCamera").GetComponent<Camera>();
        }

        public IEnumerator MoveCamera(float direction)
        {
            bool hitMaxDis;
     
            Vector3 initPos = mainSceneCamera.transform.localPosition;
            do
            {
                mainSceneCamera.transform.localPosition += direction * transform.right * Time.deltaTime * 20;
                hitMaxDis = (direction > 0) ? mainSceneCamera.transform.localPosition.x > initPos.x + 15.0f : (direction < 0) ? mainSceneCamera.transform.localPosition.x < initPos.x - 15.0f : false;
                yield return new WaitForEndOfFrame();
            }
            while (hitMaxDis == false);
           
        }
       
    }
}