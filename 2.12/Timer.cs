using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	float minutes=0f;
	float seconds=0f;
	public float timeCounter=0f;
	float startTime=0f;

	public Text timeTextDisplay;

	public bool gameOver=false;
	public bool timerRunning=false;

	void Awake(){
		timeTextDisplay=GetComponent<Text>();
	}

	// Use this for initialization
	public void StartTheTimer () {
		startTime=Time.time;
		timerRunning=true;
		timeTextDisplay.GetComponent<CanvasGroup>().alpha=1;
	}
	
	// Update is called once per frame
	void Update () {
		if(timerRunning==true){
			if(gameOver==false){
				timeCounter=Time.time-startTime;
				minutes=Mathf.Floor(timeCounter/60f);
				seconds=Mathf.Floor(timeCounter % 60f);

				if(seconds<10){
					timeTextDisplay.text=minutes + ":0" + seconds;
				}else{
					timeTextDisplay.text=minutes + ":" + seconds;
				}
			}
		}
	}
}
