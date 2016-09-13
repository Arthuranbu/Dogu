using UnityEngine;
using System.Collections;

public class LevelInteraction : MonoBehaviour
{
    //This will handle the camera panning and the teleports.
    //Will have teleport and camera pan be diff scripts, and then just put it all in level management/game manager folder, so instead of getting camera refs directly inside GameManager, will instead get ref to scripts that will handle all of the camera switching.
    //Might do in game manager since that already has camera ref, but we'll see.
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
