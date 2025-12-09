using UnityEngine;

namespace SoundSystem
{
    [CreateAssetMenu(fileName = "NewBgmDb", menuName = "Sounds/BgmDatabase")]
    public class BgmSoundDataBase : SoundDataBase<BgmSoundData>
    {
    }

    [System.Serializable]
    // BGMのデータを格納するクラス.
    public class BgmSoundData : SoundData
    {
        /// <summary>
        /// 音楽をループ再生するかどうかを示します。
        /// </summary>
        public bool Loop => loop;

        /// <summary>
        /// ループ再生の開始サンプル位置。
        /// </summary>
        public int LoopStartSample => loopStartSample;

        /// <summary>
        /// ループ再生の終了サンプル位置。
        /// </summary>
        public int LoopEndSample => loopEndSample;

        /// <summary>
        /// 音楽のサンプリング周波数（Hz）。
        /// </summary>
        public int Frequency => frequency;

        [Header("ループ")]
        [SerializeField]
        private bool loop = false;
        [Header("ループ開始サンプル位置")]
        [SerializeField]
        private int loopStartSample = 0;
        [Header("ループ終了サンプル位置")]
        [SerializeField]
        private int loopEndSample = 0;
        [Header("サンプリング周波数 (Hz)")]
        [SerializeField]
        private int frequency = 44100;

        public BgmSoundData(string bgmTitle,
                            AudioClip audioClip,
                            float volume = 1.0f,
                            bool loop = false,
                            int frequency = 0,
                            int loopStartSample = 0,
                            int loopEndSample = 0)
            : base(bgmTitle, audioClip, volume)
        {
            this.frequency = frequency;
            this.loop = loop;
            this.loopStartSample = loopStartSample;
            this.loopEndSample = loopEndSample;
        }
    }
}
