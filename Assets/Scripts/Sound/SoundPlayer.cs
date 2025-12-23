using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SoundSystem
{
    public class SoundPlayer : MonoBehaviour
    {
        public static SoundPlayer Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
                Initialize();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }


        [Header("BGMのAudioSource")]
        [SerializeField]
        private AudioSource bgmAudioSource;

        [Header("BGMのクリップのデータリスト")]
        [SerializeField]
        private BgmSoundDataBase bgmSoundDatas;

        [Header("SEのクリップのデータリスト")]
        [SerializeField]
        private SeDataBase seDatas;

        [Header("Voiceのクリップのデータリスト")]
        [SerializeField]
        private VoiceSoundDataBase voiceSoundDatas;

        [Header("音声設定")]
        public SoundSetting SoundSetting;

        [Header("ミュート")]
        [SerializeField]
        private bool isMute = false;
        public bool IsMute => isMute;

        private AudioSource seAudioSourcePrefab;
        private AudioSource voiceAudioSourcePrefab;
        private BgmSoundData currentBgmData;
        private CancellationTokenSource bgmCts;


        /// <summary>
        /// 全カテゴリのミュートを設定します。
        /// </summary>
        public void MuteSound(bool isMute)
        {
            this.isMute = isMute;
            bgmAudioSource.mute = isMute;
        }

        public async UniTask PlayBgm(string bgmTitle, float fadeTime = 0, CancellationToken ctn = default)
        {
            // 再生処理中ならキャンセル
            if (bgmCts != null)
            {
                bgmCts.Cancel();
                bgmCts.Dispose();
            }
            bgmCts = CancellationTokenSource.CreateLinkedTokenSource(ctn);

            if (bgmAudioSource.isPlaying)
            {
                await StopBgmInternal(fadeTime, bgmCts.Token);
            }

            currentBgmData = bgmSoundDatas.GetSoundData(bgmTitle);
            if (currentBgmData == null) return;

            bgmAudioSource.clip = currentBgmData.AudioClip;
            VolumeApply(bgmAudioSource, SoundSetting.BgmVolume, currentBgmData.Volume);
            bgmAudioSource.loop = !currentBgmData.Loop; // ループポイント指定がない場合はUnityのloop機能を使う
            bgmAudioSource.timeSamples = 0;
            bgmAudioSource.Play();

            if (currentBgmData.Loop)
            {
                await CustomBgmLoop(bgmCts.Token);
            }
            else
            {
                await UniTask.WaitUntil(() => !bgmAudioSource.isPlaying, cancellationToken: bgmCts.Token);
            }

            if (bgmCts.Token.IsCancellationRequested) return;

            // 一応ストップ処理
            StopBgm(0);
        }

        /// <summary>
        /// BGMを停止します
        /// </summary>
        /// <param name="fadeTime"></param>
        public void StopBgm(float fadeTime = 0)
        {
            StopBgmInternal(fadeTime, CancellationToken.None).Forget();
        }

        /// <summary>
        /// SEを再生し、再生後AudioSourceを破棄します。
        /// </summary>
        public void PlaySe(string seTitle)
        {
            InternalPlay(seTitle, true, destroyCancellationToken).Forget();
        }

        /// <summary>
        /// Voiceを再生し、再生後AudioSourceを破棄します。
        /// </summary>
        public void PlayVoice(string voiceTitle)
        {
            InternalPlay(voiceTitle, false, destroyCancellationToken).Forget();
        }

        public async UniTask PlaySe(string seTitle, CancellationToken ctn)
        {
            await InternalPlay(seTitle, true, ctn);
        }

        public async UniTask PlayVoice(string voiceTitle, CancellationToken ctn)
        {
            await InternalPlay(voiceTitle, false, ctn);
        }

        private async UniTask InternalPlay(string title, bool isSE, CancellationToken ctn)
        {
            SoundDataBase<SoundData> datas = isSE ? seDatas : voiceSoundDatas;
            var data = datas.GetSoundData(title);
            if (data == null) return;

            AudioSource tempSource = Instantiate(voiceAudioSourcePrefab, transform);
            tempSource.enabled = true; // テンプレートを有効化

            tempSource.clip = data.AudioClip;
            var typeVolume = isSE ? SoundSetting.SeVolume : SoundSetting.VoiceVolume;
            VolumeApply(tempSource, typeVolume, data.Volume);
            tempSource.mute = isMute; // ミュート状態を適用

            tempSource.Play();

            await UniTask.Delay(System.TimeSpan.FromSeconds(tempSource.clip.length), cancellationToken: ctn);
            // 再生が終わったら自動で破棄する
            Destroy(tempSource.gameObject);
        }

        public void PauseBgm()
        {
            if (bgmAudioSource.isPlaying)
            {
                bgmAudioSource.Pause();
                Debug.Log($"BGM paused position: {bgmAudioSource.time:F0}s");
            }
        }

        public void UnPauseBgm()
        {
            if (!bgmAudioSource.isPlaying && bgmAudioSource.clip != null)
            {
                Debug.Log($"BGM unpaused position: {bgmAudioSource.time}s");
                bgmAudioSource.UnPause();
            }
        }

        /// <summary>
        /// 実際のAudioSourceに音量を適用します。
        /// </summary>
        private void VolumeApply(
            AudioSource audioSource,
            float categoryVolume,
            float dataVolume
        )
        {
            // ミュート状態も考慮
            float master = isMute ? 0f : SoundSetting.MasterVolume;
            audioSource.volume = master * categoryVolume * dataVolume;
        }

        private async UniTask CustomBgmLoop(CancellationToken ctn)
        {
            while (bgmAudioSource.isPlaying && !ctn.IsCancellationRequested)
            {
                if (bgmAudioSource.timeSamples >= CorrectFrequency(currentBgmData.LoopEndSample))
                {
                    bgmAudioSource.timeSamples -= CorrectFrequency(currentBgmData.LoopEndSample - currentBgmData.LoopStartSample);
                }
                await UniTask.WaitForEndOfFrame(cancellationToken: ctn);
            }
        }

        private int CorrectFrequency(long n)
        {
            if (bgmAudioSource.clip == null) return 0;
            // 整数キャスト前に double/float で計算
            return (int)(n * (double)bgmAudioSource.clip.frequency / currentBgmData.Frequency);
        }

        /// <summary>
        /// 内部用のBGM停止処理
        /// </summary>
        private async UniTask StopBgmInternal(float fadeTime, CancellationToken ctn)
        {
            if (bgmCts != null)
            {
                bgmCts.Cancel();
                bgmCts.Dispose();
                bgmCts = null;
            }

            if (fadeTime > 0 && bgmAudioSource.isPlaying)
            {
                float startVolume = bgmAudioSource.volume;
                float duration = 0f;
                while (duration < fadeTime && !ctn.IsCancellationRequested)
                {
                    bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, duration / fadeTime);
                    duration += Time.deltaTime;
                    await UniTask.Yield(ctn);
                }
            }

            // 状態リセット
            bgmAudioSource.Stop();
            bgmAudioSource.clip = null;
            bgmAudioSource.volume = 0f;
            bgmAudioSource.timeSamples = 0;
            currentBgmData = null;
        }

        /// <summary>
        /// SE/Voiceの再生に必要なAudioSourceを生成します。
        /// </summary>
        private void Initialize()
        {
            if (bgmAudioSource == null)
            {
                bgmAudioSource = gameObject.AddComponent<AudioSource>();
            }

            seAudioSourcePrefab = CreateAudioSourceTemplate("SE_Template");
            voiceAudioSourcePrefab = CreateAudioSourceTemplate("Voice_Template");

            SoundSetting.OnBgmVolumeChanged += () =>
            {
                // BGMの音量を再適用
                if (currentBgmData != null)
                {
                    VolumeApply(bgmAudioSource, SoundSetting.BgmVolume, currentBgmData.Volume);
                }
            };
        }

        /// <summary>
        /// AudioSourceのテンプレートを生成するヘルパーメソッド
        /// </summary>
        private AudioSource CreateAudioSourceTemplate(string name)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(transform);
            AudioSource source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            // テンプレートなので無効化しておく
            source.enabled = false;
            return source;
        }

        private void OnDestroy()
        {
            bgmCts?.Cancel();
            bgmCts?.Dispose();
            bgmCts = null;
        }
    }
}
