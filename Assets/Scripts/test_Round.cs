using Cysharp.Threading.Tasks;
using UnityEngine;

public class test_Round : MonoBehaviour
{
    [SerializeField]
    private JankenInputManager inputManager;

    public void StartRound()
    {
        new RoundManager(new CountDownTimer(null, null), inputManager).Execute(destroyCancellationToken).Forget();
    }
}
