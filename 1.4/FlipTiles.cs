using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTiles : MonoBehaviour {
	public bool flipUp=false;
	public bool flipDown=false;
	Vector3 downAngle;
	Vector3 flippedAngle;
	float timeToRotate=0.75f;
	float startTime;

	// Use this for initialization
	void Start () {
		downAngle=transform.eulerAngles;
		flippedAngle=downAngle+180f*Vector3.up;
	}
	
	// Update is called once per frame
	void Update () {
		if(flipUp==true){
			transform.eulerAngles=Vector3.Lerp(transform.eulerAngles, flippedAngle, (Time.time-startTime)/timeToRotate);
			if(transform.eulerAngles==flippedAngle){
				flipUp=false;
			}
		}

		if(flipDown==true){
			transform.eulerAngles=Vector3.Lerp(transform.eulerAngles, downAngle, (Time.time-startTime)/timeToRotate);
			if(transform.eulerAngles==downAngle){
				flipDown=false;
			}
		}
		
	}

	void OnMouseDown(){
		//Debug.Log("press");
		startTime=Time.time;
		if(transform.eulerAngles==downAngle){
			flipUp=true;
		}

		if(transform.eulerAngles==flippedAngle){
			flipDown=true;
		}
	}

	void OnMouseUp(){
		//Debug.Log("release");
		//transform.Rotate(0,180,0);
	}
}
