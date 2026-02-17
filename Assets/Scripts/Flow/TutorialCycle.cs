using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TutorialCycle : GameCycleBase
{
    [SerializeField]
    private int maxCycleCount = 10;

    protected override bool CanContinueGame(CancellationToken ctn)
    {
        return scoreManager.GetCurrentWinCount() < maxCycleCount;
    }

    protected override void OnGameCycleFinished()
    {
        GameOver();
    }

    protected override void OnRoundFinished()
    {
        Debug.Log(scoreManager.GetCurrentWinCount());
    }

    protected override async UniTask PerformGameOverSequence()
    {
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
