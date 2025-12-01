using UnityEngine;

namespace SoundManagement
{
    [CreateAssetMenu(fileName = "SoundSetting", menuName = "サウンド/音声設定")]
    public class SoundSetting : ScriptableObject
    {
        public SoundSetting(
            float masterVolume = 1.0f,
            float bgmVolume = 1.0f,
            float seVolume = 1.0f,
            float voiceVolume = 1.0f
        )
        {
            SetBgmVolume(bgmVolume);
            SetSeVolume(seVolume);
            SetVoiceVolume(voiceVolume);
            SetMasterVolume(masterVolume);
        }

        public event System.Action OnBgmVolumeChanged;

        [Header("全体音量")]
        [SerializeField]
        float masterVolume = 1.0f;

        [Header("音楽音量")]
        [SerializeField]
        float bgmVolume = 1.0f;

        [Header("効果音量")]
        [SerializeField]
        float seVolume = 1.0f;

        [Header("ボイス音量")]
        [SerializeField]
        float voiceVolume = 1.0f;

        public float MasterVolume => masterVolume;
        public float BgmVolume => bgmVolume;
        public float SeVolume => seVolume;
        public float VoiceVolume => voiceVolume;

        public void SetMasterVolume(float volume)
        {
            masterVolume = ClampValue(volume, "MasterVolume");
            OnBgmVolumeChanged?.Invoke();
        }

        public void SetBgmVolume(float volume)
        {
            bgmVolume = ClampValue(volume, "BgmVolume");
            OnBgmVolumeChanged?.Invoke();
        }

        public void SetSeVolume(float volume)
        {
            seVolume = ClampValue(volume, "SeVolume");
        }

        public void SetVoiceVolume(float volume)
        {
            voiceVolume = ClampValue(volume, "VoiceVolume");
        }

        private float ClampValue(float value, string volumeType)
        {
            if (value < 0 || value > 1)
            {
                Debug.LogWarning($"{volumeType} must be between 0 and 1");
            }
            return Mathf.Clamp(value, 0, 1);
        }
    }
}
