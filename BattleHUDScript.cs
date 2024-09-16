using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BattleHUD : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text levelText;
    public Slider hpSlider;

    public void SetHUD(UnitScript unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "" + unit.unitLevel;
        hpSlider.maxValue = unit.maxHp;
        hpSlider.value = unit.currentHp;
    }

    public void SetHp(int hp)
    {
        hpSlider.value = hp;
    }
}
