using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoCachableEffect : MonoBehaviour
{
    public string FilePath { get; set; }

    private void OnEnable()
    {
        StartCoroutine(nameof(CheckIfAlive));
    }

    private IEnumerator CheckIfAlive()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (!GetComponent<ParticleSystem>().IsAlive(true))
            {
                SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectManager.RemoveEffect(this);
                break;
            }
        }
    }
}
