using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchCounter : MonoBehaviour {
	public int matchesRemaining;
	public Text matchesTextBox;
	public CanvasGroup replayButton;

	public void InitializeMatches(int initialMatches){
		matchesRemaining=initialMatches;
		matchesTextBox.text="Matches Remaining="+matchesRemaining.ToString();
	}

	public void UpdateMatches(){
		matchesRemaining=matchesRemaining-1;
		matchesTextBox.text="Matches Remaining="+matchesRemaining.ToString();
		//check for game over
		if(matchesRemaining==0){
			Debug.Log("Win");
			GameOver();
		}
	}

	public void GameOver(){
		replayButton.alpha=1;
		replayButton.interactable=true;
		replayButton.blocksRaycasts=true;
	}
}
