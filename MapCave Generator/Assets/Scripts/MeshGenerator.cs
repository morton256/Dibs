using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour {
	public SquareGrid sqGrid;
	public MeshFilter walls; 
	List<Vector3> vertices;
	List<int> triangles; 

	Dictionary<int,List<Triangle>> triDictionary = new Dictionary<int, List<Triangle>>();

	List<List<int>> outlines = new List<List<int>>();
	HashSet<int> checkedVertices = new HashSet<int>();

	public void GenerateMesh(int[,] map, float sqSize){
		triDictionary.Clear ();
		outlines.Clear ();
		checkedVertices.Clear ();


		sqGrid = new SquareGrid(map,sqSize);

		vertices = new List<Vector3>();
		triangles = new List<int>();

		for(int x = 0; x < sqGrid.squares.GetLength(0); x++){
			for(int y = 0; y < sqGrid.squares.GetLength(1); y++){
				TriangulateSquare(sqGrid.squares[x,y]);
			}
		}
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();

		CreateWalls();
	}

	void CreateWalls()
	{
		CalculateMeshOutlines();
		List<Vector3> wallVerts = new List<Vector3>();
		List<int> wallTris = new List<int>();
		Mesh wallMesh = new Mesh();
		float wallHeight = 5;

		foreach(List<int> outline in outlines){
			for(int i = 0; i<outline.Count-1; i++)
			{
				int startIndex = wallVerts.Count;
				wallVerts.Add (vertices[outline[i]]);
				wallVerts.Add (vertices[outline[i+1]]);
				wallVerts.Add (vertices[outline[i]] - Vector3.up * wallHeight);
				wallVerts.Add (vertices[outline[i+1]] - Vector3.up * wallHeight);

				wallTris.Add (startIndex + 0);
				wallTris.Add (startIndex + 2);
				wallTris.Add (startIndex + 3);

				wallTris.Add (startIndex + 3);
				wallTris.Add (startIndex + 1);
				wallTris.Add (startIndex + 0);
			}
		}
		wallMesh.vertices = wallVerts.ToArray();
		wallMesh.triangles = wallTris.ToArray();
		walls.mesh = wallMesh;
	}

	void TriangulateSquare(Square square)
	{
		switch (square.config){
		case 0:
			break;

		//1 Point Configs:
		case 1:
			MeshFromPoints(square.cLeft, square.cBottom, square.bLeft);
			break;
		case 2:
			MeshFromPoints(square.bRight, square.cBottom,square.cRight);
			break;
		case 4:
			MeshFromPoints(square.tRight,square.cRight,square.cTop);
			break;
		case 8:
			MeshFromPoints(square.tLeft, square.cTop, square.cLeft);
			break;

		//2 Point configs:
		case 3:
			MeshFromPoints(square.cRight,square.bRight,square.bLeft,square.cLeft);
			break;
		case 6:
			MeshFromPoints(square.tRight,square.bRight,square.cBottom,square.cTop);
			break;
		case 9:
			MeshFromPoints(square.cTop,square.cBottom,square.bLeft,square.tLeft);
			break;
		case 12:
			MeshFromPoints(square.tRight,square.cRight,square.cLeft,square.tLeft);
			break;
		case 5:
			MeshFromPoints(square.tRight,square.cRight,square.cBottom,square.bLeft,square.cLeft,square.cTop);
			break;
		case 10:
			MeshFromPoints(square.cRight,square.bRight,square.cBottom,square.cLeft,square.tLeft,square.cTop);
			break;

		//3 point configs:
		case 7:
			MeshFromPoints (square.tRight,square.bRight,square.bLeft,square.cLeft,square.cTop);
			break;
		case 11: 
			MeshFromPoints (square.cRight,square.bRight,square.bLeft,square.tLeft,square.cTop);
			break;
		case 13:
			MeshFromPoints (square.tRight,square.cRight,square.cBottom,square.bLeft,square.tLeft);
			break;
		case 14:
			MeshFromPoints(square.tRight,square.bRight,square.cBottom,square.cLeft,square.tLeft);
			break;


		//4 point configs:
		case 15:
			MeshFromPoints(square.tLeft, square.tRight, square.bRight, square.bLeft);
			checkedVertices.Add (square.tLeft.vertexIndex);
			checkedVertices.Add (square.tRight.vertexIndex);
			checkedVertices.Add (square.bLeft.vertexIndex);
			checkedVertices.Add (square.bRight.vertexIndex);
			break;
		}

	}

	void MeshFromPoints(params Node[] points)
	{
		AssignVertices(points);

		if(points.Length >= 3)
			CreateTris(points[0], points[1], points[2]);
		if(points.Length >= 4)
			CreateTris(points[0], points[2], points[3]);
		if(points.Length >= 5)
			CreateTris(points[0], points[3], points[4]);
		if(points.Length >= 6)
			CreateTris(points[0], points[4],points[5]);



	}

	void AssignVertices(Node[] points)
	{
		for(int i = 0; i < points.Length; i++)
		{
			if(points[i].vertexIndex == -1)
			{
				points[i].vertexIndex = vertices.Count;
				vertices.Add (points[i].position);
			}
		}
	}

	void CreateTris(Node a, Node b, Node c)
	{
		triangles.Add(a.vertexIndex);
		triangles.Add(b.vertexIndex);
		triangles.Add(c.vertexIndex);

		Triangle triangle = new Triangle (a.vertexIndex, b.vertexIndex, c.vertexIndex);
		AddTriToDict(triangle.vertexIndexA,triangle);
		AddTriToDict(triangle.vertexIndexB,triangle);
		AddTriToDict(triangle.vertexIndexC,triangle);
	}

	void AddTriToDict(int vertexIKey, Triangle triangle){
		if(triDictionary.ContainsKey(vertexIKey)){
			triDictionary[vertexIKey].Add (triangle);
		}
		else{
			List<Triangle> triangleList = new List<Triangle>();
			triangleList.Add (triangle);
			triDictionary.Add (vertexIKey,triangleList);
		}
	}

	void CalculateMeshOutlines(){
		for (int vertIndex = 0; vertIndex < vertices.Count; vertIndex++){
			if(!checkedVertices.Contains(vertIndex)){
				int newOutlineVert = GetConnectedOutlineVertex(vertIndex);
				if(newOutlineVert != -1){
					checkedVertices.Add (vertIndex);

					List<int> newOutline = new List<int>();
					newOutline.Add (vertIndex);
					outlines.Add (newOutline);
					FollowOutline(newOutlineVert,outlines.Count-1);
					outlines[outlines.Count-1].Add (vertIndex);
				}
			}
		}
	}

	void FollowOutline(int vertIndex, int outlineIndex)
	{
		outlines[outlineIndex].Add (vertIndex);
		checkedVertices.Add (vertIndex);
		int nextVertIndex = GetConnectedOutlineVertex(vertIndex);

		if(nextVertIndex != -1){
			FollowOutline(nextVertIndex,outlineIndex);
		}
	}

	int GetConnectedOutlineVertex(int vertIndex){
		List<Triangle> triConVertex = triDictionary[vertIndex];

		for(int i = 0; i < triConVertex.Count; i++){
			Triangle triangle = triConVertex[i];

			for(int j = 0; j < 3; j++){
				int vertexB = triangle[j];
				if(vertexB != vertIndex && !checkedVertices.Contains(vertexB)){
					if(IsOutlineEdge(vertIndex, vertexB)){
						return vertexB;
					}
				}
			}
		}
		return -1;
	}

	bool IsOutlineEdge(int vertexA, int vertexB){
		List<Triangle> vertATris = triDictionary[vertexA];
		int shareTriCount = 0;

		for(int i = 0; i < vertATris.Count; i++){
			if(vertATris[i].Contains(vertexB)){
				shareTriCount++;
				if(shareTriCount > 1)
				{
					break;
				}
			}
		}
		return shareTriCount == 1;
	}


	struct Triangle{
		public int vertexIndexA;
		public int vertexIndexB;
		public int vertexIndexC;
		int[] vertices;


		public Triangle(int a, int b, int c){
			vertexIndexA = a;
			vertexIndexB = b;
			vertexIndexC = c;

			vertices = new int[3];
			vertices[0] = a;
			vertices[1] = b;
			vertices[2] = c;

		}

		public int this[int i]{
			get{
				return vertices[i];
			}
		}

		public bool Contains(int vertIndex){
			return vertIndex == vertexIndexA || vertIndex == vertexIndexB || vertIndex == vertexIndexC;
		}
	}

	public class SquareGrid{
		public Square[,] squares;

		public SquareGrid(int[,] map, float sqSize){
			int nodeCountX = map.GetLength(0);
			int nodeCountY = map.GetLength(1);

			float mapWidth = nodeCountX * sqSize;
			float mapHeight = nodeCountY * sqSize;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

			for(int x = 0; x < nodeCountX; x++){
				for(int y = 0; y < nodeCountY; y++){
					Vector3 pos = new Vector3(-mapWidth/2 + x * sqSize + sqSize/2, 0, -mapHeight/2 + y * sqSize + sqSize/2);
					controlNodes[x,y] = new ControlNode(pos,map[x,y] == 1, sqSize);
				}
			}
			squares = new Square[nodeCountX -1, nodeCountY -1];
			for(int x = 0; x < nodeCountX-1; x++){
				for(int y = 0; y < nodeCountY-1; y++){
					squares[x,y] = new Square(controlNodes[x,y+1], controlNodes[x+1,y+1], controlNodes[x+1,y], controlNodes[x,y]);
				}
			}
		}

	}

	public class Square{
		public ControlNode tLeft, tRight, bRight, bLeft;
		public Node cTop,cRight,cBottom,cLeft;
		public int config;

		public Square(ControlNode _tLeft,ControlNode _tRight,ControlNode _bRight,ControlNode _bLeft)
		{
			tLeft = _tLeft;
			tRight = _tRight;
			bLeft = _bLeft;
			bRight = _bRight;

			cTop = tLeft.right;
			cRight = bRight.above;
			cBottom = bLeft.right;
			cLeft = bLeft.above;

			if(tLeft.active)
				config += 8;
			if(tRight.active)
				config += 4;
			if(bRight.active)
				config += 2;
			if(bLeft.active)
				config += 1; 
		}
	}

	public class Node{
		public Vector3 position; //Keep Track of mesh world position
		public int vertexIndex = -1; //Count vertexIndex

		public Node(Vector3 _pos){
			position = _pos;
		}
	}

	public class ControlNode : Node {

		public bool active; //If active node is a wall, else it isn't a wall
		public Node above, right; 

		public ControlNode(Vector3 _pos, bool _active, float sqSize) : base(_pos)
		{
			active = _active;
			above = new Node(position + Vector3.forward * sqSize/2f);
			right = new Node(position + Vector3.right * sqSize/2f);
		}

	}
}
