using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int maxTowerLevel = 3;
    public int towerLevel = 0;
    public int towerType = 0;

    public GameObject rangeZone;
    private BaseTowerController currentTower;

    public void OnEnable()
    {
        _rangeZoneActive = false;
        //SetTowerType("T" + towerType + towerLevel);
    }

    public void SetTowerType(string towerName)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            bool goActive = child.name == towerName;
            child.gameObject.SetActive(goActive);
            if (goActive && towerType != 0)
            {
                currentTower = child.gameObject.GetComponent<BaseTowerController>();
                currentTower.bulletPrefab = bulletPrefab;
                currentTower.bulletType = towerType.ToString() + towerLevel.ToString();
            }
        }
    }

    private bool _rangeZoneActive;
    private void OnMouseEnter()
    {
        if (_rangeZoneActive || towerType == 0)
            return;
        rangeZone.SetActive(true);
        rangeZone.transform.position = transform.position;
        rangeZone.transform.localScale += new Vector3(currentTower.attackRange * 2,
                                                      currentTower.attackRange * 2);
        _rangeZoneActive = true;
    }

    private void OnMouseExit()
    {
        if (!_rangeZoneActive)
            return;
        //Debug.Log("DeActivate RANGE_ZONE");
        rangeZone.SetActive(false);
        rangeZone.transform.localScale = new Vector3(1, 1, 1);
        _rangeZoneActive = false;
    }

    private void OnMouseDown()
    {
        towerType = 1;
        LevelUp();
        string towerName = "T" + towerType + towerLevel;
        SetTowerType(towerName);
        OnMouseExit();
        OnMouseEnter();
    }

    public void SetType(string newTowerType)
    {

    }

    public void LevelUp()
    {
        if (towerLevel >= maxTowerLevel)
            return;
        towerLevel++;
        //SetTowerType();
    }

	
    public void Init()
    {
        SetupTower();

    }

    private void SetupTower()
    {
   
    }
}
