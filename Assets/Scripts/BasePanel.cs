using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    private void Awake()
    {
        InitializePanel();
    }

    private void Update()
    {
        UpdatePanel();
    }

    private void OnDestroy()
    {
        DestroyPanel();
    }

    public virtual void InitializePanel()
    {
        PanelManager.RegistPanel(GetType(), this);
    }

    public virtual void UpdatePanel()
    {

    }

    public virtual void DestroyPanel()
    {
        PanelManager.UnregistPanel(GetType());
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
