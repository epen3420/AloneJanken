using Debug = UnityEngine.Debug;
using System.Threading;
using Cysharp.Threading.Tasks;

public class RoundManager
{
    private readonly CountDownTimer timer;
    private readonly JankenInputManager inputs;

    public RoundManager(CountDownTimer timer, JankenInputManager inputs)
    {
        this.timer = timer;
        this.inputs = inputs;
    }

    public async UniTask Execute(CancellationToken ctn)
    {
        inputs.Enable();
        timer.Init(3f);
        await timer.Resume(ctn);
        inputs.Disable();

        var resultHands = HandJudger.Judge(inputs.GetCurrentInputHand());
        foreach (var hand in resultHands)
        {
            Debug.Log(hand.ToString());
        }
    }
}
