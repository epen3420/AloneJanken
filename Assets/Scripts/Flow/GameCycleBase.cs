using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SoundSystem;
using UnityEngine;

public abstract class GameCycleBase : MonoBehaviour
{
    [SerializeField]
    protected QuestDatabase questDb;
    [SerializeField]
    protected RoundController roundController;
    [SerializeField]
    protected VoidEventChannelSO endGame;
    [SerializeField]
    protected ScoreManager scoreManager;
    [SerializeField]
    protected NovelController novelController;
    [SerializeField]
    protected ChatShower chatShower;
    [SerializeField]
    protected float waitTimeBeforeTransition = 1f;

    protected bool isPlaying = false;
    protected CancellationTokenSource cycleStopCts;

    protected virtual void OnEnable()
    {
        endGame.OnVoidRaised += GameOver;
    }

    protected virtual void OnDisable()
    {
        endGame.OnVoidRaised -= GameOver;
    }

    protected virtual void Start()
    {
        cycleStopCts = new CancellationTokenSource();
        GameCycle(cycleStopCts.Token).Forget();
    }

    protected async UniTaskVoid GameCycle(CancellationToken ctn)
    {
        if (isPlaying) return;
        isPlaying = true;

        try
        {
            await novelController.Execute(chatShower);

            await SoundPlayer.Instance.PlaySe("start_game", ctn);
            while (!ctn.IsCancellationRequested && CanContinueGame(ctn))
            {
                var targetHand = HandTypeUtil.GetRandomlyHandType();
                int randomNum = Random.Range(0, questDb.UseableHandPosTypes.Length);
                var targetHandPos = questDb.UseableHandPosTypes[randomNum];
                var questType = questDb.GetQuestTypeRandomly();
                var quest = QuestFactory.GetQuestByType(questType, targetHand, targetHandPos);
                chatShower.ShowText(quest.ToString());
                await roundController.StartRound(quest, questDb.UseableHandPosTypes, ctn);

                await UniTask.WaitForEndOfFrame();

                OnRoundFinished();
            }

            OnGameCycleFinished();
        }
        finally
        {
            isPlaying = false;
        }
    }

    protected virtual bool CanContinueGame(CancellationToken ctn)
    {
        return true;
    }

    protected virtual void OnRoundFinished()
    {
    }

    protected virtual void OnGameCycleFinished()
    {
    }

    protected void GameOver()
    {
        SoundPlayer.Instance.PlaySe("end_game");
        AsyncGameOver().Forget();
    }

    private async UniTask AsyncGameOver()
    {
        cycleStopCts.Cancel();
        cycleStopCts.Dispose();
        cycleStopCts = null;

        await PerformGameOverSequence();
    }

    protected abstract UniTask PerformGameOverSequence();
}
