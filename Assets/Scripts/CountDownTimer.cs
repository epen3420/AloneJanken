using System.Threading;
using Cysharp.Threading.Tasks;
using MiniGame.Event;
using Time = UnityEngine.Time;
using Debug = UnityEngine.Debug;

namespace MiniGame.Core
{
    /// <summary>
    /// CountDownします
    /// </summary>
    public class CountDownTimer
    {
        private readonly FloatEventChannelSO OnChangeTime;
        public readonly VoidEventChannelSO OnTimeZero;

        public float InitTime => initTime;
        public float CurrentTime => currentTime;

        private float initTime;
        private float currentTime;
        private bool isCounting = false;
        private bool zeroFired = false;
        private CancellationTokenSource resetCts;


        public CountDownTimer(FloatEventChannelSO changeTimeEventChannel,
                              VoidEventChannelSO timeZeroEventChannel)
        {
            OnChangeTime = changeTimeEventChannel;
            OnTimeZero = timeZeroEventChannel;
        }

        public void Init(float countTime)
        {
            StopAndReset();

            if (countTime <= 0)
            {
                Debug.LogWarning($"Can not set {countTime} as timer");
                return;
            }

            initTime = countTime;
            ChangeTime(countTime);
        }

        public async UniTask Resume(CancellationToken ctn)
        {
            if (resetCts == null ||
                resetCts.IsCancellationRequested)
            {
                resetCts = new CancellationTokenSource();
                CountTime(resetCts.Token).Forget();
            }

            isCounting = true;

            await UniTask.WaitUntil(() => !isCounting, cancellationToken: ctn);
        }

        public void StopAndReset()
        {
            isCounting = false;
            zeroFired = false;

            resetCts?.Cancel();
            resetCts?.Dispose();
            resetCts = null;

            initTime = 0f;
            ChangeTime(0f);
        }

        private void ChangeTime(float time)
        {
            currentTime = time;
            OnChangeTime?.Raise(currentTime);

            if (currentTime <= 0f &&
                !zeroFired)
            {
                zeroFired = true;
                OnTimeZero?.Raise();
            }
        }

        private async UniTaskVoid CountTime(CancellationToken ctn)
        {
            try
            {
                while (currentTime > 0f)
                {
                    if (isCounting)
                    {
                        ChangeTime(currentTime - Time.deltaTime);
                    }

                    await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ctn);
                }

                if (currentTime < 0f)
                {
                    ChangeTime(0f);
                }
            }
            finally
            {
                StopAndReset();
            }
        }
    }
}
