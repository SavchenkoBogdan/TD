using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

    //private static Camera
    private static float originalScale;
    private static float minScale;
    private static float maxScale;

    public GameObject map;
	// Use this for initialization
    void OnEnable()
    {
        originalScale = Camera.main.orthographicSize;
        //minScale = originalScale;
        maxScale = originalScale * 0.5f;
        //RectTransform mapTransform = map.GetComponent<RectTransform>();
        //outerLeft = mapTransform.rect.left;
        //outerRight = mapTransform.rect.right;

    }

    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public bool cameraDragging = true;

    public float outerLeft = -184f;
    public float outerRight = 184f;


    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            if (Camera.main.orthographicSize < originalScale)
                Camera.main.orthographicSize += 0.1f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            if (Camera.main.orthographicSize > maxScale)
                Camera.main.orthographicSize -= 0.1f;
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    dragOrigin = Input.mousePosition;
        //    return;
        //}

        //if (!Input.GetMouseButton(0)) return;

        //Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
        //Debug.Log(move);
        //Vector3 newPos = new Vector3(Camera.main.transform.position.x - move.x, Camera.main.transform.position.y - move.z, Camera.main.transform.position.z);
        //RectTransform mapTransform = map.GetComponent<RectTransform>();
        ////if (newPos.x - Camera.main.rect.width / 2 >= mapTransform.rect.left &&
        ////    newPos.x + Camera.main.rect.width / 2 <= mapTransform.rect.right &&
        ////    newPos.y - Camera.main.rect.height / 2 >= mapTransform.rect.top &&
        ////    newPos.y + Camera.main.rect.height / 2 <= mapTransform.rect.bottom)
        ////    Camera.main.transform.position = newPos;
        //Camera.main.transform.Translate(move, Space.World);

        ////transform.Translate(move, Space.World);


        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        float left = Screen.width*0.2f;
        float right = Screen.width - (Screen.width*0.2f);

        if (mousePosition.x < left)
        {
            cameraDragging = true;
        }
        else if (mousePosition.x > right)
        {
            cameraDragging = true;
        }






        if (cameraDragging)
        {

            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(0)) return;

            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x*dragSpeed, 0, 0);

            if (move.x > 0f)
            {
                if (this.transform.position.x < outerRight)
                {
                    transform.Translate(move, Space.World);
                }
            }
            else
            {
                if (this.transform.position.x > outerLeft)
                {
                    transform.Translate(move, Space.World);
                }
            }

        }
    }
}
