using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GuiView : MonoBehaviour, IGuiView
{

    [SerializeField] private Button btnPause;
    [SerializeField] private Image healthProgressBar;
    [SerializeField] private Text healthField;
    [SerializeField] private Image energyProgressBar;
    [SerializeField] private Image skillProgressBar;
    private IGuiModel model;
    private int towerIndex;
    //private Tower.TowerType type;

    private static GuiView instance;
    public static GuiView Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    public void Init()
    {
    }

    // Use this for initialization
    void OnEnable()
    {
        model = new GuiModel(this);
        AttachEvents();
    }

    private int tmp = 0;
	// Update is called once per frame
	void Update ()
	{
	    tmp++;
	    float perc = tmp/1000f;
        SetSkillProgressBarValue(perc);
	}

    public void AttachEvents()
    {
        btnPause.onClick.AddListener(model.OnPauseClick);
    }

    public void SetHealth(string healthText)
    {
        healthField.text = healthText;
    }

    public void SetProgressBarColor(Color color)
    {
        healthProgressBar.color = color;
    }

    public void SetEnergyProgressBarValue(float value)
    {
        energyProgressBar.fillAmount = value;
    }

    public void SetSkillProgressBarValue(float value)
    {
        skillProgressBar.fillAmount = value;
    }
}
