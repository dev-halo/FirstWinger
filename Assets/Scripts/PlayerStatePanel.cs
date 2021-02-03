using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatePanel : BasePanel
{
    [SerializeField]
    private Text scoreValue;

    [SerializeField]
    private Gauge HPGauge;

    private Player hero = null;
    public Player Hero
    {
        get
        {
            if (null == hero)
            {
                hero = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().Hero;
            }
            return hero;
        }
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
    
        HPGauge.SetValue(100f, 100f); // 가득찬 상태로 초기화.
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (null != Hero)
        {
            HPGauge.SetValue(Hero.HPCurrent, Hero.HPMax);
        }
    }

    public void SetScore(int value)
    {
        Debug.Log("SetScore value = " + value);

        scoreValue.text = value.ToString();
    }
}
