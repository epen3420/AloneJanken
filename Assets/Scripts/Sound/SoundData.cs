using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string title;
    public AudioClip audioClip;
    public float volume;

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
