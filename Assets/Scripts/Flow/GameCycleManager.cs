using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SoundSystem;
using UnityEngine;

public class GameCycleManager : MonoBehaviour
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

            await SoundPlayer.Instance.PlaySe("start_game", ctn);
            while (!ctn.IsCancellationRequested)
            {
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
        SoundPlayer.Instance.PlaySe("end_game");
        AsyncGameOver().Forget();
    }

    private async UniTask AsyncGameOver()
    {
        cycleStopCts.Cancel();
        cycleStopCts.Dispose();
        cycleStopCts = null;

        Debug.Log($"{scoreManager.GetCurrentScore()}");

        await chatShower.ShowAsTypeWriter("GAME OVER");
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTimeBeforeTransition));
        Debug.Log("Game Over");
        SceneController.LoadScene("Result");
    }
}
