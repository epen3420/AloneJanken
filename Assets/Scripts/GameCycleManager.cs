using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameCycleManager : MonoBehaviour
{
    [SerializeField]
    private QuestDatabase questDb;
    [SerializeField]
    private RoundController roundController;

    private bool isPlaying = false;


    private void Start()
    {
        GameCycle(destroyCancellationToken).Forget();
    }

    private async UniTaskVoid GameCycle(CancellationToken ctn)
    {
        if (isPlaying) return;
        isPlaying = true;

        int totalRounds = questDb.Quests.Length;
        try
        {
            for (int roundCount = 0; roundCount < totalRounds; roundCount++)
            {
                int randomInt = Random.Range(0, totalRounds);
                await roundController.StartRound(questDb.Quests[randomInt], ctn);
                await UniTask.Delay(3000);
            }
        }
        finally
        {
            isPlaying = false;
        }
    }
}
