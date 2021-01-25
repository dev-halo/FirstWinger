using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGScrollData
{
    public Renderer RenderForScroll;
    public float Speed;
    public float OffsetX;
}

public class BGScroller : MonoBehaviour
{
    [SerializeField]
    BGScrollData[] ScrollDatas;

    private void Update()
    {
        UpdateScroll();
    }

    private void UpdateScroll()
    {
        for (int i = 0; i < ScrollDatas.Length; ++i)
        {
            SetTextureOffset(ScrollDatas[i]);
        }
    }

    private void SetTextureOffset(BGScrollData scrollData)
    {
        scrollData.OffsetX += scrollData.Speed * Time.deltaTime;
        if (1f < scrollData.OffsetX)
        {
            scrollData.OffsetX %= 1f;
        }

        Vector2 offset = new Vector2(scrollData.OffsetX, 0f);

        scrollData.RenderForScroll.material.SetTextureOffset("_MainTex", offset);
    }
}
