using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameCycleManager : GameCycleBase
{
    protected override async UniTask PerformGameOverSequence()
    {
        Debug.Log($"{scoreManager.GetCurrentScore()}");

        await chatShower.ShowAsTypeWriter("GAME OVER");
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitTimeBeforeTransition));
        Debug.Log("Game Over");
        SceneController.LoadScene("Result");
    }
}
