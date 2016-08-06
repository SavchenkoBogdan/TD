using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxEnergy;

    private int _gameHealth;
    private int _gameEnergy;
    public int maxHealth
    {
        get { return _maxHealth; }
    }
    public int gameHealth
    {
        get { return _gameHealth;}
        set { _gameHealth = value; }
    }
    public int gameEnergy
    {
        get { return _gameEnergy; }
        set { _gameEnergy = value; }
    }
    public int maxEnergy
    {
        get { return _maxEnergy; }
    }

    private static LevelManager _instance;

    public static LevelManager Instance
    {
        set { _instance = value; }
        get { return _instance; }
    }

    

    void OnEnable()
    {
        _instance = this;
        EventAggregator.UnitReachedFinish.Subscribe(OnUnitReachedFinish);
        EventAggregator.EnergyRecieved.Subscribe(OnEnergyRecieved);
        _gameHealth = _maxHealth;
        _gameEnergy = 0;
        //StartCoroutine(EnergyConsumingProcess());
    }

    private IEnumerator EnergyConsumingProcess()
    {
        while (true)
        {
            if (_gameEnergy > 0)
            {
                _gameEnergy--;
                yield return new WaitForSeconds(1);
            }
        }
    }

    private void OnEnergyRecieved()
    {
        if (_gameEnergy != _maxEnergy)
            _gameEnergy++;
    }

    private void OnUnitReachedFinish()
    {
        _gameHealth--;
    }
    // Update is called once per frame
    void Update () {
	
	}
}
