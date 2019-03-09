using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTiles : MonoBehaviour {

	public List<GameObject> tiles;
	public int columnCount;

	// Use this for initialization
	void Start () {
		ShuffleTheTiles();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShuffleTheTiles(){
		for(int i=tiles.Count-1; i>0; i--){
			int randomPos=Random.Range(0,i);
			GameObject tempValue=tiles[randomPos];
			tiles[randomPos]=tiles[i];
			tiles[i]=tempValue;
		}

		PlaceTheTiles();
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
		//Debug.Log(Mathf.Ceil(tiles.Count/(float)columnCount));
		if((Mathf.Ceil(tiles.Count/(float)columnCount))%2==0){
			//Debug.Log("even rows");
			initialY=(Mathf.Ceil(tiles.Count/(float)columnCount))/2-0.5f;
		//odd rows
		}else{
			//Debug.Log("odd rows");
			initialY=(tiles.Count/columnCount)/2;
		}

		for(var i=0; i<tiles.Count; i++){
			Vector2 tileLocation=new Vector2(initialX+(i%columnCount), initialY-(i/columnCount));
			Instantiate(tiles[i],tileLocation, Quaternion.identity);
		}

	}
}

