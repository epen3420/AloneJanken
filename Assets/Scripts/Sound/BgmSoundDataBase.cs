using UnityEngine;
using System.Collections.Generic;

namespace SoundManagement
{
    [CreateAssetMenu(menuName = "サウンド/BGMデータベース")]
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
        public bool loop;

        /// <summary>
        /// ループ再生の開始サンプル位置。
        /// </summary>
        public int loopStartSample;

        /// <summary>
        /// ループ再生の終了サンプル位置。
        /// </summary>
        public int loopEndSample;

        /// <summary>
        /// 音楽のサンプリング周波数（Hz）。
        /// </summary>
        public int frequency;

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
