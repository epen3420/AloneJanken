using SoundSystem;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void PlaySe(string name)
    {
        SoundPlayer.Instance?.PlaySe(name);
    }
}
