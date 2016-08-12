using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class test : MonoBehaviour {


	void Awake()
	{

	}

	// Use this for initialization
	void Start () {
		EasyHill2DManager.Instance().InitCulling();
	}
	
	// Update is called once per frame
	void Update () {
		EasyHill2DManager.Instance().CullSegments();
		if(Input.GetKey(KeyCode.Space) || Input.touchCount>0) 
		{
			GetComponent<Rigidbody2D>().AddForce(Vector2.up*-100f);
		}

		if(Input.GetKeyUp(KeyCode.Escape))Application.Quit();

		if(Input.touchCount>1)Application.LoadLevel(0);

	}

	void OnGUI()
	{
		GUI.Label (new Rect(25,25,400,200)," Touch screen or press Space to accelerate the ball downwards.");
	}
}
