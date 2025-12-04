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

        try
        {
            while (true)
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
}
