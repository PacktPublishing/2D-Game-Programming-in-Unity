using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {

	float currentBestTime;
	Text bestText;

	void Awake(){
		bestText=GetComponent<Text>();
	}

	public void CheckBestScore(int difficultyLevel, float score){
		if(PlayerPrefs.HasKey("BestTime"+difficultyLevel.ToString())){
			Debug.Log("best time exists");
			currentBestTime=PlayerPrefs.GetFloat("BestTime"+difficultyLevel.ToString());
		}else{
			Debug.Log("no best time exists");
			currentBestTime=0;
		}

		if(currentBestTime!=0){
			if(score<currentBestTime){
				Debug.Log("new best time!");
				currentBestTime=score;
				PlayerPrefs.SetFloat("BestTime"+difficultyLevel.ToString(),currentBestTime);
			}
		}else{
			Debug.Log("initialize best time");
			currentBestTime=score;
			PlayerPrefs.SetFloat("BestTime"+difficultyLevel.ToString(),currentBestTime);
		}

		float minutes=Mathf.Floor(currentBestTime/60f);
		float seconds=Mathf.Floor(currentBestTime % 60f);

		if(seconds<10){
			bestText.text="Best Time: "+ minutes + ":0" + seconds;
		}else{
			bestText.text="Best Time: "+ minutes + ":" + seconds;
		}
	}
}
