/* using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

public class RoundManager
{
    private readonly CountDownTimer timer;

    public RoundManager(CountDownTimer timer, InputActionAsset inputs)
    {
        this.timer = timer;
        this.inputs = inputs;
    }

    public async UniTask Execute(CancellationToken ctn)
    {
        timer.Init(3f);
        await timer.Resume(ctn);

        inputs.Enable();
    }
}
 */
