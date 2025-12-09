using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string Title => title;
    public AudioClip AudioClip => audioClip;
    public float Volume => volume;
    public float ClipLength => audioClip.length;

    [Header("タイトル")]
    [SerializeField]
    private string title;
    [Header("クリップ")]
    [SerializeField]
    private AudioClip audioClip;
    [Header("音量")]
    [SerializeField]
    private float volume = 0.75f;

    public SoundData(string title, AudioClip audioClip, float volume)
    {
        if (title == null)
        {
            throw new System.ArgumentException("title must not be null");
        }
        this.title = title;

        if (audioClip == null)
        {
            throw new System.ArgumentException("audioClip must not be null");
        }

        this.audioClip = audioClip;
        this.volume = volume;
    }
}
