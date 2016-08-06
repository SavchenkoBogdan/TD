using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IGuiView
{
    void AttachEvents();
    void SetHealth(string healthText);
    void SetProgressBarColor(Color color);
    void SetEnergyProgressBarValue(float value);
    void SetSkillProgressBarValue(float value);

}

