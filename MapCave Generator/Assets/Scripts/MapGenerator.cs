using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {
	#region Variables
	public int width, height; // Values to store size of our map.
	// values are public so they can be changed in the editor. 

	[Range(0,100)] // Range for the percentage.
	public int rFillPercent; 
	// The percentage of map that will be filled with walls in comparison to amount empty. 


	public int smoothness; // The number of iterations it will run SmoothMap Function

	public int borderSize = 10; //Width of cave border
	int[,] map;
	//
	// 2D Array of Integers, to store locations of our map.
	// Values stored will either be 1 or 0. 0 to deduce that tile location will be empty + 1 will obviously mean its full(or a wall etc).

	public string seed; //A custom seed allowing for the same map to be created multiple times. 
	public bool randSeed; //Boolean allowing the option to randomize the seed value. 

	#endregion


	void Start()
	{
		GenerateNewMap();

	
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			GenerateNewMap();
		}

	}

	void GenerateNewMap()
	{
		map = new int[width,height];
		FillMap ();

		for (int i = 0; i < smoothness ; i++){
			SmoothMap();
		}


		int[,] borderMap = new int[width+borderSize*2,height + borderSize *2];

		for(int x = 0; x<borderMap.GetLength(0); x++){
			for(int y= 0; y<borderMap.GetLength(1); y++){
				if(x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize){
					borderMap[x,y] = map[x-borderSize,y-borderSize];
				}
				else{
					borderMap[x,y] = 1;
				}
			}
		}
	

		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh(borderMap, 1);
	}

	void FillMap()
	{
		if (randSeed) {
			seed = Time.time.ToString();
		}

		System.Random psRand = new System.Random (seed.GetHashCode ());

		for (int x = 0; x < width; x++) {
			for(int y = 0; y < height; y++)			{
				if(x == 0 || x == width - 1 || y == 0 || y == height - 1){
					map[x,y] = 1;
				}
				else{
					map[x,y] = (psRand.Next(0,100) < rFillPercent)? 1: 0;
				}
			}
		}
	}

	void SmoothMap(){
		for(int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				int nWallTiles = GetSurroundingWalls(x,y);
				if(nWallTiles > 4)
					map[x,y] = 1;
				else if(nWallTiles < 4)
					map[x,y] = 0;
			}
		}
	}

	int GetSurroundingWalls(int gridX, int gridY)
	{
		int wallCount = 0;
		for(int neighbourX = gridX -1; neighbourX <= gridX+1; neighbourX++){
			for(int neighbourY = gridY -1; neighbourY <= gridY+1; neighbourY++){
				if(neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY <height){
					if(neighbourX != gridX || neighbourY != gridY){
						wallCount += map[neighbourX,neighbourY];
					}
				}
				else{
					wallCount++;
				}
			}
		}
		return wallCount;

	}
	
}

