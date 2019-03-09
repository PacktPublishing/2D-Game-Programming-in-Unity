using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStats : MonoBehaviour {
	public Sprite standardSprite;
	public Sprite mineSprite;
	public Sprite flagSprite;
	public List<Sprite> numberedSprites;

	public bool isMine=false;
	public int mineCount=0;

	public bool tileRevealed=false;

	public bool floodPerformed=false;
	public CreateField fieldScript;

	public bool tileFlagged=false;
	public bool flagDisabled=false;

	Pause pauseScript;

	// Use this for initialization
	void Start () {
		fieldScript=FindObjectOfType<CreateField>();
		pauseScript=FindObjectOfType<Pause>();
	}
	
	// Update is called once per frame
	void Update () {
		if(pauseScript.gamePaused==true){
			GetComponent<SpriteRenderer>().enabled=false;
		}else{
			GetComponent<SpriteRenderer>().enabled=true;
		}
	}

	public void DetermineSurroundingNumbers(List<GameObject> allTiles){
		foreach(GameObject tile in allTiles){
			if(tile.transform.position.x==transform.position.x || tile.transform.position.x==transform.position.x+1 || tile.transform.position.x==transform.position.x-1){
				if(tile.transform.position.y==transform.position.y || tile.transform.position.y==transform.position.y+1 || tile.transform.position.y==transform.position.y-1){
					if(tile.GetComponent<TileStats>().isMine==false){
						tile.GetComponent<TileStats>().mineCount=tile.GetComponent<TileStats>().mineCount+1;
						//tile.GetComponent<SpriteRenderer>().sprite=numberedSprites[tile.GetComponent<TileStats>().mineCount];
					}
				}
			}
		}
	}

	public void MouseSelect(){
		//tile isn't already revealed
		if(tileRevealed==false && pauseScript.gamePaused==false){
			if(Input.GetMouseButton(0)){ //left click

				//the flag button isn't selected
				if(fieldScript.flagging==false){
					if(tileFlagged==false){
						tileRevealed=true;

						if(isMine==true){
							RevealBomb();
						}else{
							RevealNumber();
						}
					}
				}else{
					if(tileFlagged==false){ //not flagged
						tileFlagged=true;
						GetComponent<SpriteRenderer>().sprite=flagSprite;
						fieldScript.minesLeft--;

					}else{ //already flagged
						tileFlagged=false;
						GetComponent<SpriteRenderer>().sprite=standardSprite;
						fieldScript.minesLeft++;
					}
				}

			}

			if(flagDisabled==false){
				if(Input.GetMouseButton(1)){ //right click
					if(tileFlagged==false){ //not flagged
						tileFlagged=true;
						GetComponent<SpriteRenderer>().sprite=flagSprite;
						fieldScript.minesLeft--;

					}else{ //already flagged
						tileFlagged=false;
						GetComponent<SpriteRenderer>().sprite=standardSprite;
						fieldScript.minesLeft++;
					}
					flagDisabled=true;
					StartCoroutine(EnableFlags());
				}
			}
		}
	}

	public void RevealBomb(){
		GetComponent<SpriteRenderer>().sprite=mineSprite;
		fieldScript.GameOver();
	}

	public void RevealNumber(){
		GetComponent<SpriteRenderer>().sprite=numberedSprites[mineCount];

		if(mineCount==0){
			fieldScript.FloodFillAlgorithm(transform.position.x, transform.position.y);
		}

		fieldScript.HaveAllBeenRevealed();
	}

	public void Explode(){
		tileRevealed=true;
		if(isMine==true){
			GetComponent<SpriteRenderer>().sprite=mineSprite;
		}else{
			GetComponent<SpriteRenderer>().sprite=numberedSprites[mineCount];
		}
	}

	public IEnumerator EnableFlags(){
		yield return new WaitForSeconds(0.1f);
		flagDisabled=false;
	}
}
