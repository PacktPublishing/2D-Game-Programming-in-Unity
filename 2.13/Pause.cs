using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {
	public CanvasGroup pausePanel;
	CanvasGroup pauseButton;

	public bool gamePaused=false;

	// Use this for initialization
	void Start () {
		pauseButton=GetComponent<CanvasGroup>();
	}
	
	public void PauseTheGame(){
		gamePaused=true;
		Time.timeScale=0;

		//show the panel
		pausePanel.alpha=1;
		pausePanel.interactable=true;
		pausePanel.blocksRaycasts=true;

		//hide the pause button
		pauseButton.alpha=0;
		pauseButton.interactable=false;
		pauseButton.blocksRaycasts=false;
	}

	public void UnPauseTheGame(){
		gamePaused=false;
		Time.timeScale=1;

		//hide the panel
		pausePanel.alpha=0;
		pausePanel.interactable=false;
		pausePanel.blocksRaycasts=false;

		//show the pause button
		pauseButton.alpha=1;
		pauseButton.interactable=true;
		pauseButton.blocksRaycasts=true;
	}
}
