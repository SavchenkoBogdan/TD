using UnityEngine;
using System.Collections;

public class CameraFollow2D : MonoBehaviour {
		
	public Transform target;
	public float delay=10000f;
	public float distance=40;
	public float maxX, minX;
	float height,width;
	Camera cam;
//	bool destroyToggle=false;
	// Use this for initialization
	void Start () {
		minX=EasyHill2DManager.Instance().getMinX();
		maxX=EasyHill2DManager.Instance().getMaxX();

		cam = Camera.main;
		height = 2f * cam.orthographicSize;
		width = height * cam.aspect;

//		EasyHill2DManager.Instance().InitCulling();
	}

	void Update()
	{
//		if(Input.GetMouseButtonDown(0))
//		{
//			Vector3 p = cam.ScreenToWorldPoint(Input.mousePosition);
//			EasyHill2DManager.Instance().DestructSegmentCircle(p.x,p.y, 10, 0.2f);
//		//Debug.Log("Mouse " + (Input.mousePosition+Camera.main.transform.position).ToString());
//		}
//
//		if(Input.touchCount>0 && !destroyToggle)
//		{
//			destroyToggle=true;
//			Vector3 p = cam.ScreenToWorldPoint(Input.touches[0].position);
//			EasyHill2DManager.Instance().DestructSegmentCircle(p.x,p.y, 10, 0.2f);
//		}
//		else
//			destroyToggle=false;
	}

	// Update is called once per frame
	void FixedUpdate () {

		transform.position = Vector3.Lerp(transform.position, target.position+new Vector3(0f,0f,-distance),Time.deltaTime*delay);

		if(transform.position.y<height/2)transform.position=new Vector3(transform.position.x, GetComponent<Camera>().orthographicSize, transform.position.z);
		if(transform.position.x<minX+width/2)
			transform.position=new Vector3(minX+width/2, transform.position.y, transform.position.z);

		if(transform.position.x>maxX-width/2)
			transform.position=new Vector3(maxX-width/2, transform.position.y, transform.position.z);



	}
}
