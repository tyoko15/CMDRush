using System.Collections;
using UnityEngine;
using System;

public class Fade : UnityEngine.UI.Graphic
{
    public static Fade Instance;
    [HideInInspector] public Texture maskTexture;
    float fadeRange;
    public float Range
    {
        get { return fadeRange; }
        set
        {
            fadeRange = value;
            UpdateFade(fadeRange);
        }
    }

    protected override void Awake()
    {
        if (Fade.Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        enabled = true;
        SetTexture(maskTexture);
    }

    public void SetTexture(Texture texture)
    {
        material.SetTexture("_MaskTex", texture);
        material.SetColor("_Color", color);
    }

    void UpdateFade(float range)
    {
        material.SetFloat("_Range", 1 - range);
    }

    public Coroutine StartFadeIn(float time, Action action = null)
    {
        StopAllCoroutines();
        return StartCoroutine(FadeInCoroutine(time, action));
    }
    public Coroutine StartFadeOut(float time, Action action = null)
    {
        StopAllCoroutines();
        return StartCoroutine(FadeOutCoroutine(time, action));
    }

    IEnumerator FadeInCoroutine(float time, Action action)
    {
        float timer = 0;
        float a = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            a = Mathf.Lerp(0f, 1f, timer / time);
            UpdateFade(a);
            yield return null;
        }
        UpdateFade(1f);
        if (action != null) action();
    }
    IEnumerator FadeOutCoroutine(float time, Action action = null)
    {
        float timer = 0;
        float a = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            a = Mathf.Lerp(1f, 0f, timer / time);
            UpdateFade(a);
            yield return null;
        }
        UpdateFade(0f);
        if (action != null) action();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateFade(Range);
        SetTexture(maskTexture);
    }
#endif
}
