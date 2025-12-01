using System.Threading;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private int jankenBpm = 60;
    [SerializeField]
    private int beatsNum = 7;
    [SerializeField]
    private JankenInputManager inputs;
    [SerializeField]
    private FloatEventChannelSO changeTimeEvent;
    [SerializeField]
    private QuestDatabase questDb;

    private CountDownTimer timer;
    private bool isPlaying = false;


    private void Awake()
    {
        timer = new CountDownTimer(changeTimeEvent, null);
    }

    public async UniTask Execute(CancellationToken ctn)
    {
        if (isPlaying) return;
        isPlaying = true;

        int roundCount = 0;
        while (questDb.Quests.Count() > roundCount)
        {
            inputs.Enable();
            timer.Init(jankenBpm / 60 * beatsNum);
            await timer.Resume(ctn);
            inputs.Disable();

            var inputResult = inputs.GetCurrentInputHand();
            bool isWin = false;
            if (inputResult.Count() > 0)
            {
                var resultHands = HandJudger.Judge(inputResult);
                isWin = questDb.Quests[roundCount].Judge(resultHands);

                foreach (var hand in resultHands)
                {
                    Debug.Log(hand.ToString());
                }
            }

            Debug.Log(isWin);
            roundCount++;
        }
    }
}
