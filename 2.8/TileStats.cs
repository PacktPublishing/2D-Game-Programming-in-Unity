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
	public CreateField floodFillScript;

	public bool tileFlagged=false;
	public bool flagDisabled=false;

	// Use this for initialization
	void Start () {
		floodFillScript=FindObjectOfType<CreateField>();
	}
	
	// Update is called once per frame
	void Update () {
		
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

	public void OnMouseOver(){
		//tile isn't already revealed
		if(tileRevealed==false){
			if(Input.GetMouseButton(0)){ //left click
				if(tileFlagged==false){
					tileRevealed=true;

					if(isMine==true){
						RevealBomb();
					}else{
						RevealNumber();
					}
				}

			}

			if(flagDisabled==false){
				if(Input.GetMouseButton(1)){ //right click
					if(tileFlagged==false){ //not flagged
						tileFlagged=true;
						GetComponent<SpriteRenderer>().sprite=flagSprite;

					}else{ //already flagged
						tileFlagged=false;
						GetComponent<SpriteRenderer>().sprite=standardSprite;
					}
					flagDisabled=true;
					StartCoroutine(EnableFlags());
				}
			}

		}
	}

	public void RevealBomb(){
		GetComponent<SpriteRenderer>().sprite=mineSprite;
		FindObjectOfType<CreateField>().GameOver();
	}

	public void RevealNumber(){
		GetComponent<SpriteRenderer>().sprite=numberedSprites[mineCount];

		if(mineCount==0){
			floodFillScript.FloodFillAlgorithm(transform.position.x, transform.position.y);
		}
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
