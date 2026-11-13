using System;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    [SerializeField] public Fade fade;

    [SerializeField] Texture maskTexture;
    [SerializeField, Range(0, 1)] float fadeRange;

    void Awake()
    {
        if (FadeManager.Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        fade = GetComponent<Fade>();
    }

    void OnValidate()
    {
        fade.maskTexture = maskTexture;
        fade.Range = fadeRange;
    }
}
