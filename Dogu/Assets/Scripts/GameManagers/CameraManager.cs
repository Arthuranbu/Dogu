using UnityEngine;
using System.Collections;

namespace Dogu
{
    public class CameraManager : MonoBehaviour
    {
        Camera mainMenuCamera;
        public Camera mainSceneCamera;
        public bool CameraMoving
        { set; get; }
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

        public IEnumerator MoveCamera(float direction)
        {
            CameraMoving = true;
            bool hitMaxDis;
     
            Vector3 initPos = mainSceneCamera.transform.localPosition;
            do
            {
                mainSceneCamera.transform.localPosition += direction * transform.right * Time.deltaTime * 20;
                hitMaxDis = (direction > 0) ? mainSceneCamera.transform.localPosition.x > initPos.x + 20.0f : (direction < 0) ? mainSceneCamera.transform.localPosition.x < initPos.x - 20.0f : false;
                yield return new WaitForEndOfFrame();
              

            }
            while (hitMaxDis == false);
            CameraMoving = false;
           
        }
       
    }
}