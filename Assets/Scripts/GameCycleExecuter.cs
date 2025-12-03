using UnityEngine;

public class GameCycleExecuter : MonoBehaviour
{
    [SerializeField]
    private GameCycleManager cycleManager;


    private void Start()
    {
        cycleManager.GameCycle(destroyCancellationToken).Forget();
    }
}
