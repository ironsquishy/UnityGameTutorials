using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public class BoardManager : MonoBehaviour {
	[Serializable]
	public class Count {
		public int minimum;
		public int maximium;

		public Count(int min, int max){
			minimum = min;
			maximium = max;
		}
	}

	public int rows = 8;
	public int columns = 8;

	public Count wallCount = new Count (5, 9);
	public Count foodCount = new Count (1, 5);

	public GameObject exit;

	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	private Transform boardHolder;
	private List <Vector3> gridPostions = new List<Vector3>();

	void InitializeList(){
		gridPostions.Clear ();

		for (int x = 0; x < columns - 1; x++) {
			for (int y = 0; y < rows - 1; y++) gridPostions.Add (new Vector3 (x, y, 0f));
		}
	}

	void BoardSetup(){
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < columns + 1; y++) {

				GameObject toInstiate = null;
				if (x == -1 || x == columns || y == -1 || y == rows)
					toInstiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				else 
					toInstiate = floorTiles [Random.Range (0, floorTiles.Length)];

				GameObject instance = Instantiate (toInstiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);

			}
		}
	}

	Vector3 RandomPostion(){
		int randomIndex = Random.Range (0, gridPostions.Count - 1);
		Vector3 randomPostion = gridPostions [randomIndex];
		gridPostions.RemoveAt (randomIndex);
		return randomPostion;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
		int objectCount = Random.Range (minimum, maximum + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPostion ();
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity); 
		}
	}

	public void SetupScene(int level){
		BoardSetup ();
		InitializeList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximium);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximium);

		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}
}
