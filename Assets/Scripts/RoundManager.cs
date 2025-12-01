using Debug = UnityEngine.Debug;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Linq;

public class RoundManager
{
    private readonly CountDownTimer timer;
    private readonly JankenInputManager inputs;

    public RoundManager(CountDownTimer timer, JankenInputManager inputs)
    {
        this.timer = timer;
        this.inputs = inputs;
    }

    public async UniTask Execute(JankenQuestBase quest, CancellationToken ctn)
    {
        inputs.Enable();
        timer.Init(3f);
        await timer.Resume(ctn);
        inputs.Disable();

        var inputResult = inputs.GetCurrentInputHand();
        bool isWin = false;
        if (inputResult.Count() > 0)
        {
            var resultHands = HandJudger.Judge(inputResult);
            isWin = quest.Judge(resultHands);

            foreach (var hand in resultHands)
            {
                Debug.Log(hand.ToString());
            }
        }

        Debug.Log(isWin);
    }
}
