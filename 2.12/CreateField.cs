using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateField : MonoBehaviour {
	public Camera theCamera;
	public GameObject minesweeperTile;

	[System.Serializable]
	public class LevelsAttributes{
		public int columns;
		public int rows;
		public int mines;
	}
	public LevelsAttributes[] levelsArray;

	public int difficultyLevel;
	int columnCount;
	int rowCount;
	int mineCount;
	int totalTiles;

	List<GameObject> tiles=new List<GameObject>();
	public List<Vector2> tilePositions;

	//public Sprite mineSprite;

	int totalTilesRevealed;

	//for mine text
	public int minesLeft;
	public Text minesText;

	//for popup panels
	public CanvasGroup difficultyPanel;
	public CanvasGroup gameOverPanel;
	public CanvasGroup gameWonPanel;

	//for score
	public float score;
	public Text scoreText;

	// Use this for initialization
	public void SetTheDifficulty (int levelSelected) {
		difficultyLevel=levelSelected;

		//hide the panel
		difficultyPanel.alpha=0;
		difficultyPanel.interactable=false;
		difficultyPanel.blocksRaycasts=false;

		//now set the values from the button selection
		columnCount=levelsArray[difficultyLevel].columns;
		rowCount=levelsArray[difficultyLevel].rows;
		mineCount=levelsArray[difficultyLevel].mines;
		totalTiles=columnCount*rowCount;
		minesLeft=mineCount;

		//resize the camera
		theCamera.orthographicSize=(float)rowCount/2+2;

		PlaceTheTiles();
		FindObjectOfType<Timer>().StartTheTimer();

		minesText.GetComponent<CanvasGroup>().alpha=1;
	}
	
	// Update is called once per frame
	void Update () {
		minesText.text="Mines: "+minesLeft;
	}

	public void PlaceTheTiles(){
		float initialX=0;
		//even columns
		if(columnCount%2==0){
			initialX=-(columnCount/2)+0.5f;

		//odd columns
		}else{
			initialX=-(columnCount/2);
		}

		float initialY=0;
		//even rows
		if(rowCount%2==0){
			//Debug.Log("even rows");
			initialY=rowCount/2-0.5f;
		//odd rows
		}else{
			//Debug.Log("odd rows");
			initialY=rowCount/2;
		}

		for(var i=0; i<totalTiles; i++){
			Vector2 tileLocation=new Vector2(initialX+(i%columnCount), initialY-(i/columnCount));
			var theTile=Instantiate(minesweeperTile,tileLocation, Quaternion.identity);
			tiles.Add(theTile);
		}

		ShuffleTheTiles();
	}

	public void ShuffleTheTiles(){
		for(int i=tiles.Count-1; i>0; i--){
			int randomPos=Random.Range(0,i);
			GameObject tempValue=tiles[randomPos];
			tiles[randomPos]=tiles[i];
			tiles[i]=tempValue;
		}

		//needed for the flood fill algorithm
		for(int i=0; i<tiles.Count; i++){
			tilePositions.Add(new Vector2(tiles[i].transform.position.x,tiles[i].transform.position.y));
		}

		DetermineTheMines();
	}

	public void DetermineTheMines(){
		for(int i=0; i<mineCount; i++){
			//tiles[i].GetComponent<SpriteRenderer>().sprite=mineSprite;
			tiles[i].GetComponent<TileStats>().isMine=true;
			tiles[i].GetComponent<TileStats>().DetermineSurroundingNumbers(tiles);
		}
	}

	public void FloodFillAlgorithm(float xStart, float yStart){
		GameObject tileToCheck=new GameObject();
		int index;
		int mineCount;
		bool floodPerformed;

		//make sure the position being checked is in range of the field, if not leave the function
		if(!tilePositions.Contains(new Vector2(xStart,yStart))){
			return;
		}else{
			index=tilePositions.IndexOf(new Vector2(xStart,yStart));
			tileToCheck=tiles[index];
			mineCount=tileToCheck.GetComponent<TileStats>().mineCount;
			floodPerformed=tileToCheck.GetComponent<TileStats>().floodPerformed;
		}

		if(tileToCheck!=null){
			if(floodPerformed==true){
				return;
			}

			tileToCheck.GetComponent<TileStats>().floodPerformed=true;
			tileToCheck.GetComponent<TileStats>().tileRevealed=true;
			tileToCheck.GetComponent<SpriteRenderer>().sprite=tileToCheck.GetComponent<TileStats>().numberedSprites[mineCount];

			//if the tile is empty, reveal the ones adjacent to it
			//run the flood fill check on those
			if(mineCount==0){
				//left
				FloodFillAlgorithm(tileToCheck.transform.position.x-1, tileToCheck.transform.position.y);
				//right
				FloodFillAlgorithm(tileToCheck.transform.position.x+1, tileToCheck.transform.position.y);
				//bottom
				FloodFillAlgorithm(tileToCheck.transform.position.x, tileToCheck.transform.position.y-1);
				//top
				FloodFillAlgorithm(tileToCheck.transform.position.x, tileToCheck.transform.position.y+1);
				//left bottom
				FloodFillAlgorithm(tileToCheck.transform.position.x-1, tileToCheck.transform.position.y-1);
				//left top
				FloodFillAlgorithm(tileToCheck.transform.position.x-1, tileToCheck.transform.position.y+1);
				//right bottom
				FloodFillAlgorithm(tileToCheck.transform.position.x+1, tileToCheck.transform.position.y-1);
				//right top
				FloodFillAlgorithm(tileToCheck.transform.position.x+1, tileToCheck.transform.position.y+1);
			}
		}

		HaveAllBeenRevealed();
	}

	public void GameOver(){
		Debug.Log("You Lost!!!");
		foreach (GameObject tile in tiles){
			tile.GetComponent<TileStats>().Explode();
		}
		FindObjectOfType<Timer>().gameOver=true;

		StartCoroutine(ShowGameOver());
	}

	public IEnumerator ShowGameOver(){
		yield return new WaitForSeconds(0.5f);
		//show the panel
		gameOverPanel.alpha=1;
		gameOverPanel.interactable=true;
		gameOverPanel.blocksRaycasts=true;
	}

	public void HaveAllBeenRevealed(){
		foreach(GameObject tile in tiles){
			if(tile.GetComponent<TileStats>().tileRevealed==true){
				totalTilesRevealed++;
			}
		}

		if(totalTilesRevealed==totalTiles-mineCount){
			GameWon();
		}else{
			totalTilesRevealed=0;
		}
	}

	public void GameWon(){
		Debug.Log("You Won!!!");
		FindObjectOfType<Timer>().gameOver=true;

		score=FindObjectOfType<Timer>().timeCounter;
		scoreText.text="Time: "+FindObjectOfType<Timer>().timeTextDisplay.text;
		StartCoroutine(ShowGameWon());
	}

	public IEnumerator ShowGameWon(){
		yield return new WaitForSeconds(0.5f);
		//show the panel
		gameWonPanel.alpha=1;
		gameWonPanel.interactable=true;
		gameWonPanel.blocksRaycasts=true;
	}
}
