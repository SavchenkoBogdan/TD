using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuiMock : MonoBehaviour
{

    public Button tapItem;
    public 
	// Use this for initialization
	void Start ()
	{
        tapItem.onClick.AddListener(() =>
        {
            Debug.Log("CLICK");
            //EventAggregator.EnergyRecieved.Publish();
        });
	}


    void OnMouseDown()
    {
        Debug.Log("DA");
        //EventAggregator.EnergyRecieved.Publish();
    }
	void Update ()
	{
        //EventAggregator.EnergyRecieved.Publish();

        //EventAggregator.UnitReachedFinish.Publish();


    }
}
