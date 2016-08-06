using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    public enum Scene
    {
        None = 0,
        Preloader = 1,
        Menu = 2,
        Garage = 3,
        Trip = 4,
        TripEndless = 5,
        Fortune = 6,
        Comics,
        FinalComics
    }

    public enum GameState
    {
        None,
        Preloader,
        Menu,
        Comics,
        Garage,
        Trip,
        PausedRace,
        PreRace,
        Race,
        Finish,
        Fortune,
        PrevState,
        FinalComics
    }

    public static Action<GameState> OnGameStateChanged = delegate { };

    private static GameState _currentState;
    public static GameState currentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }
    private static GameState _previousState;
    public static GameState previousState
    {
        get { return _previousState; }
        private set { _previousState = value; }
    }

    public static bool TogglePause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            return false;
        }
        Time.timeScale = 1;
        return true;
    }

    public static bool TogglePause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            return false;
        }
        Time.timeScale = 0;
        return true;
    }

    public static void OnStateChange(GameState gs)
    {
        if (gs == GameState.PrevState)
        {
            // if you need to change to previus state, just swap them 
            gs = _currentState;
            _currentState = _previousState;
            _previousState = gs;
        }
        else
        {
            _previousState = _currentState;
            _currentState = gs;
        }

        switch (_currentState)
        {
            case GameState.None:
                break;
            case GameState.Preloader:
                TogglePause(false);
                break;
            case GameState.Menu:
                TogglePause(false);
                break;
            case GameState.Comics:
                break;
            case GameState.Garage:
                TogglePause(false);
                break;
            case GameState.Trip:
                break;
            case GameState.PausedRace:
                TogglePause(true);
                break;
            case GameState.PreRace:
                TogglePause(true);
                break;
            case GameState.Race:
                TogglePause(false);
                //				RaceLogic.instance.StartRace();
                break;
            case GameState.Finish:
                TogglePause(true);
                break;
            case GameState.Fortune:
                break;
            case GameState.FinalComics:
                TogglePause(true);
                break;
            default:
                Debug.Log("wtf??!");
                break;
        }
        OnGameStateChanged(gs);

        //if (previousState == GameState.Garage || previousState == GameState.Finish)
            //SaveProgressToServer();
    }

    void OnEnable()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //StartCoroutine(loadGui());
    }

    void Start()
    {
        
    }

    private IEnumerator loadGui()
    {
        Application.LoadLevelAdditive("GUI");
        yield return 0;
    }


    
}
