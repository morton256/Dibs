using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(MeshFilter))]
public class TDChunk : MonoBehaviour {
	#region ----------------------------VARIABLES----------------------------------
	public int[,] map; // 2D Array to hold coordinates of tiles within a chunk. 
	//private List<TDChunk> chunkList; // List of chunks
	public Vector2 worldPos;
	int width,height;
	#endregion

	#region ----------------------------Getters/Setters----------------------------

	//-------------------------------Getting and setting values about the world from the world class-------
	public static int worldHeight
	{
		get{return World.curWorld.worldHeight;} 
	}
	public static int worldWidth
	{
		get{return World.curWorld.worldWidth;}
	}
	public string worldSeed
	{
		get{return World.curWorld.worldSeed;}
	}
	public Vector2 _wPos
	{
		get{return World.curWorld.worldPos;}
	}
	public int smoothness
	{
		get{return World.curWorld.smoothness;}
	}
	//------------------------------------------------------------------------------------------------------
	
    #endregion

	void Start(){

		width = worldWidth;
		height = worldHeight;

		//chunkList = new List<TDChunk>(); 
		map = new int[width,height];
		worldPos = _wPos;
		FillMap();
	}

	void FillMap(){
		for (int x = 0; x < width; x++){
			for (int y =0; y < worldHeight; y++){
				int noiseVal = GetNoiseValue(x,y,worldSeed,smoothness);
				map[x,y] = noiseVal;
			}
		}


	}

	//Create a Chunk, needs to take in current world position.
	public void CreateChunk(){
		MeshGenerator meshGen = GetComponent<MeshGenerator>();

		meshGen.GenerateMesh(map,1);

	}


	public int ChooseChunkType(){
		//int noise = GetNoiseValue();

		return 0;
	}

	public int GetNoiseValue(int x, int y, string worldSeed, int smoothness){
		int noiseVal =0;
		int worldS = worldSeed.GetHashCode();
		noiseVal = ((int)Mathf.Pow((Mathf.PerlinNoise(x,y)*worldS),smoothness));



		return noiseVal; 



	}
}
