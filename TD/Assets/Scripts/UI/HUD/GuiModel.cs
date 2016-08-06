using UnityEngine;
using System.Collections;
using System;

public class GuiModel : IGuiModel
{
    private IGuiView _view;

    public GuiModel(IGuiView view)
    {
        _view = view;
	    EventAggregator.UnitReachedFinish.Subscribe(OnUnitReachedFinish);
        EventAggregator.EnergyRecieved.Subscribe(OnEnergyRecieved);
	}

    private void OnUnitReachedFinish()
    {
        if (LevelManager.Instance.gameHealth < 0)
            return;
        _view.SetHealth(LevelManager.Instance.gameHealth.ToString());
        float healthPercent = LevelManager.Instance.gameHealth / (float)LevelManager.Instance.maxHealth;
        float green = healthPercent;
        float red = (1 - healthPercent);
        Color color = new Color(red, green, 0);
        _view.SetProgressBarColor(color);
    }


    private void OnEnergyRecieved()
    {
        float energyPercent = LevelManager.Instance.gameEnergy / (float) LevelManager.Instance.maxEnergy;
        _view.SetEnergyProgressBarValue(energyPercent);
    }


    public void OnPauseClick()
    {
        GameManager.TogglePause();
    }
}
