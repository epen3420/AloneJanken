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
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private string[] sentencesBeforeStart;
    [SerializeField]
    private ChatShower chatShower;

    private bool isPlaying = false;
    private CancellationTokenSource cycleStopCts;


    private void OnEnable()
    {
        lifeZeroEvent.OnVoidRaised += GameOver;
    }

    private void OnDisable()
    {
        lifeZeroEvent.OnVoidRaised -= GameOver;
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
            await chatShower.ShowAsTypeWriter(sentencesBeforeStart);

            while (!ctn.IsCancellationRequested)
            {
                // await UniTask.Delay(3000);
                var targetHand = HandTypeUtil.GetRandomlyHandType();
                int randomNum = Random.Range(0, questDb.UseableHandPotTypes.Length);
                var targetHandPos = questDb.UseableHandPotTypes[randomNum];
                var questType = questDb.GetQuestTypeRandomly();
                var quest = QuestFactory.GetQuestByType(questType, targetHand, targetHandPos);
                chatShower.ShowText(quest.ToString());
                await roundController.StartRound(quest, questDb.UseableHandPotTypes, ctn);

                await UniTask.WaitForEndOfFrame();
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

        Debug.Log($"{scoreManager.GetCurrentScore()}");

        Debug.Log("Game Over");
    }
}
