using UnityEngine;
using System.Collections;

public class Destruction : MonoBehaviour {
//	bool destroyToggle=false;

	public ParticleSystem par;

	void Start () {
		EasyHill2DManager.Instance().getAllHill2DNodes();
	}
	
	// Update is called once per frame
	void Update () {
				if(Input.GetMouseButtonDown(0))
				{
					Vector3 p = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
					EasyHill2DManager.Instance().DestructSegmentCircle(p.x,p.y, 5, 0.2f);
					ParticleSystem.Instantiate(par, p, Quaternion.identity);
				}
		
		if(Input.touchCount>0 && Input.GetTouch(0).phase==TouchPhase.Began)
				{
					Vector3 p = GetComponent<Camera>().ScreenToWorldPoint(Input.touches[0].position);
					EasyHill2DManager.Instance().DestructSegmentCircle(p.x,p.y, 5, 0.2f);
			ParticleSystem a=	(ParticleSystem)ParticleSystem.Instantiate(par, p, Quaternion.identity);
			a.gameObject.SetActive(true);
				}

		if(Input.GetKeyUp(KeyCode.Escape))Application.Quit();
	}

	void OnGUI()
	{
		GUI.Label (new Rect(25,25,400,200), "Click or Touch on the terrain to destroy it.");
	}
}
