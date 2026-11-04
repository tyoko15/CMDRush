using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource[] sources;
    AudioClip[] clips;
    void Start()
    {
        Instance = this;

        sources = new AudioSource[transform.childCount];
        clips = new AudioClip[sources.Length];
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i] = transform.GetChild(i).GetComponent<AudioSource>();
            clips[i] = sources[i].clip;   
        }
    }

    void Update()
    {
        
    }


    public void PlaySE(int i)
    {
        sources[i].PlayOneShot(clips[i]);
    }
}
