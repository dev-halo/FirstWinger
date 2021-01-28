using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    private static Dictionary<Type, BasePanel> Panels = new Dictionary<Type, BasePanel>();

    public static bool RegistPanel(Type panelClassType, BasePanel basePanel)
    {
        if (Panels.ContainsKey(panelClassType))
        {
            Debug.LogError("RegistPanel Error! Already exist Type! PanelClassType = " + panelClassType.ToString());
            return false;
        }

        Debug.Log("RegistPanel is called! Type = " + panelClassType.ToString() + ", basePanel = " + basePanel.name);

        Panels.Add(panelClassType, basePanel);
        return true;
    }

    public static bool UnregistPanel(Type panelClassType)
    {
        if (!Panels.ContainsKey(panelClassType))
        {
            Debug.LogError("UnregistPanel Error! Can't Find Type! PanelClassType = " + panelClassType.ToString());
            return false;
        }

        Panels.Remove(panelClassType);
        return true;
    }

    public static BasePanel GetPanel(Type panelClassType)
    {
        if (!Panels.ContainsKey(panelClassType))
        {
            Debug.LogError("GetPanel Error! Can't Find Type! PanelClassType = " + panelClassType.ToString());
            return null;
        }

        return Panels[panelClassType];
    }
}
