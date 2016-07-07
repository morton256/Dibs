using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Allows me to access Lists



public class World : MonoBehaviour {
	#region ----------------------------VARIABLES----------------------------------
	public static World curWorld; // Allows access to world information from other classes
	public TDChunk chunkTile; // Create a prefab with an attached Chunk script. 

	public int worldHeight; // The maximum height of the world (for mountain sizes etc)
	public int worldWidth; // The length of the world before reaching the start point
	public int _chunkSizeX; // How big to make each chunk
	public int _chunkSizeY; // How big to make each chunk
	public int smoothness; 

	public string worldSeed; //Allows me to create a set string which the world be generated from. 
	public bool randSeed; //Bool to check if seed should be random or not.

	private List<TDChunk> chunkList; // A list that holds all of our chunks

	private int _totalChunks; // Number of chunks required to create World. 
	private int _curChunks; // Current Chunks Generated. 
	private bool _worldGenerated; // To check if world is generated. 

	public Vector2 worldPos; //Holds the current Position in the world; 

	#endregion

	#region ----------------------------GETTERS/SETTERS----------------------------

		
	

	#endregion

	// Use this for initialization
	void Start () {
		//Initializing variables, preventing any errors from null values. 
		if(worldSeed == null || randSeed == true){
			worldSeed = Time.time.ToString();} //If the seed is empty or randomSeed is set to true then generate random seed.                                      
		if(_chunkSizeX == null || _chunkSizeX < 8){
			_chunkSizeX = 8;}
		if(_chunkSizeY == null || _chunkSizeY < 8){
			_chunkSizeY = 8;}
		if(worldHeight < 64 || worldHeight == null){
			worldHeight = 64;}//If the height is less than 64 or null, make it 64. 
		if(worldWidth < 128 || worldWidth == null){
			worldWidth = 128;}//If the width is less than 128 or null, make it 128.
		if(smoothness == 0 || smoothness == null){
			smoothness = Random.Range(0,100);
		}
		worldPos = new Vector2(0,0);

		_totalChunks = _chunkSizeX * _chunkSizeY;
		_curChunks = 0;

	
		chunkList = new List<TDChunk>();
		_worldGenerated = false;

		GenerateNewWorld();
	}

	void Update(){
		if(Input.GetMouseButtonDown(0)){
			_totalChunks = 0; 
			_curChunks = 0; 
			_worldGenerated = false; 

			GenerateNewWorld();
			chunkList.Clear ();
		
		}

	}
	
	void GenerateNewWorld(){
		//Create the chunks. 
		for(int x = 0; x <= (worldWidth/_chunkSizeX); x++){ 
			for(int y = 0; y<=(worldHeight/_chunkSizeY); y++){
				chunkTile = (TDChunk)Instantiate(chunkTile, new Vector3(x+_chunkSizeX,y+_chunkSizeY,0), Quaternion.identity);
				chunkTile.transform.parent = transform;
				_curChunks++;

				chunkList.Add(chunkTile); //adding chunkTile to a list of chunks.

			}

		}


	}
}
	

