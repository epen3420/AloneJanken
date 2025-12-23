using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TutorialCycle : MonoBehaviour
{
    [SerializeField]
    private QuestDatabase questDb;
    [SerializeField]
    private RoundController roundController;
    [SerializeField]
    private VoidEventChannelSO endGame;
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private NovelController novelController;
    [SerializeField]
    private ChatShower chatShower;
    [SerializeField]
    private float waitTimeBeforeTransition = 1f;
    [SerializeField]
    private int maxCycleCount = 10;

    private bool isPlaying = false;
    private CancellationTokenSource cycleStopCts;


    private void OnEnable()
    {
        endGame.OnVoidRaised += GameOver;
    }

    private void OnDisable()
    {
        endGame.OnVoidRaised -= GameOver;
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
            await novelController.Execute(chatShower);

            while (!ctn.IsCancellationRequested && scoreManager.GetCurrentWinCount() < maxCycleCount)
            {
                var targetHand = HandTypeUtil.GetRandomlyHandType();
                int randomNum = Random.Range(0, questDb.UseableHandPotTypes.Length);
                var targetHandPos = questDb.UseableHandPotTypes[randomNum];
                var questType = questDb.GetQuestTypeRandomly();
                var quest = QuestFactory.GetQuestByType(questType, targetHand, targetHandPos);
                chatShower.ShowText(quest.ToString());
                await roundController.StartRound(quest, questDb.UseableHandPotTypes, ctn);

                await UniTask.WaitForEndOfFrame();
                Debug.Log(scoreManager.GetCurrentWinCount());
            }

            GameOver();
        }
        finally
        {

            isPlaying = false;
        }
    }

    private void GameOver()
    {
        AsyncGameOver().Forget();
    }

    private async UniTask AsyncGameOver()
    {
        cycleStopCts.Cancel();
        cycleStopCts.Dispose();
        cycleStopCts = null;

        Debug.Log($"{scoreManager.GetCurrentScore()}");

        int winCount = scoreManager.GetCurrentWinCount();
        if (winCount < maxCycleCount)
        {
            await chatShower.ShowAsTypeWriter("Game Over");
        }
        else
        {
            await chatShower.ShowAsTypeWriter($"{winCount}回クリアー！");
        }

        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTimeBeforeTransition));
        SceneController.LoadScene("Result");
    }
}
