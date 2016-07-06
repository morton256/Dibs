using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


    public Transform camTarget;
    public float m_Speed = 0.1f; 
    Camera myCam; 

	// Use this for initialization
	void Start () {
        myCam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        myCam.orthographicSize = (Screen.height / 100f) / 1f;

        if (camTarget)
        {
            transform.position = Vector3.Lerp(transform.position, camTarget.position, m_Speed) + new Vector3(0,0,-10);
        }
	}
}
