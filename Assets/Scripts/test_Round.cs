using Cysharp.Threading.Tasks;
using UnityEngine;

public class test_Round : MonoBehaviour
{
    [SerializeField]
    private JankenInputManager inputManager;
    [SerializeField]
    private QuestDatabase questDb;

    public void StartRound()
    {
        new RoundManager(new CountDownTimer(null, null), inputManager).Execute(questDb.Quests[0], destroyCancellationToken).Forget();
    }
}
