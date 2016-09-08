using UnityEngine;
using System.Collections;

public class DoguAnimations : MonoBehaviour {
	Animator dogu;
	void Awake (){
        dogu = GetComponent<Animator>();

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            
            dogu.SetTrigger(Animator.StringToHash("ToMove"));
        }
        if (Input.GetKeyDown(KeyCode.S))
            dogu.SetTrigger(Animator.StringToHash("ToStop"));

    }
}
