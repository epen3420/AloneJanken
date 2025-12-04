using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameCycleManager : MonoBehaviour
{
    [SerializeField]
    private QuestDatabase questDb;
    [SerializeField]
    private RoundController roundController;
    [SerializeField]
    private VoidEventChannelSO lifeZeroEvent;

    private bool isPlaying = false;
    private CancellationTokenSource cycleStopCts;


    private void OnEnable()
    {
        lifeZeroEvent.OnRaised += GameOver;
    }

    private void OnDisable()
    {
        lifeZeroEvent.OnRaised -= GameOver;
    }

    private void Start()
    {
        cycleStopCts = new CancellationTokenSource();

        GameCycle(cycleStopCts.Token).Forget();
    }

    private async UniTaskVoid GameCycle(CancellationToken ctn)
    {
        if (isPlaying) return;
        isPlaying = true;

        try
        {
            while (!ctn.IsCancellationRequested)
            {
                var targetHand = HandTypeUtil.GetRandomlyHandType();
                var targetHandPos = HandTypeUtil.GetRandomlyHandPosType();
                var questType = questDb.GetQuestTypeRandomly();
                var quest = QuestFactory.GetQuestByType(questType, targetHand, targetHandPos);
                await roundController.StartRound(quest, ctn);
                await UniTask.Delay(3000);
            }
        }
        finally
        {
            isPlaying = false;
        }
    }

    private void GameOver()
    {
        cycleStopCts.Cancel();
        cycleStopCts.Dispose();
        cycleStopCts = null;

        Debug.Log("Game Over");
    }
}
