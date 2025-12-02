using Cysharp.Threading.Tasks;
using UnityEngine;

public class test_Round : MonoBehaviour
{
    [SerializeField]
    private RoundManager roundManager;


    public void StartRound()
    {
        roundManager.GameCycle(destroyCancellationToken).Forget();
    }
}
