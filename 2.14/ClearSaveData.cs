using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSaveData : MonoBehaviour {

	public void ClearData(){
		PlayerPrefs.DeleteAll();
	}
}
