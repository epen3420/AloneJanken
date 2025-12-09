using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannelSO endJanken;
    [SerializeField]
    private IntEventChannelSO changeLifeEvent;
    [SerializeField]
    private VoidEventChannelSO lifeZeroEvent;
    [SerializeField]
    private int maxLife = 3;

    private LifeCounter lifeCounter;

    private void Awake()
    {
        lifeCounter = new LifeCounter(lifeZeroEvent, changeLifeEvent, maxLife);
    }

    private void OnEnable()
    {
        endJanken.OnRaised += SetLife;
    }

    private void OnDisable()
    {
        endJanken.OnRaised -= SetLife;
    }

    private void SetLife(bool isWin)
    {
        if (!isWin)
        {
            lifeCounter.DecreaseLife();
        }
    }
}
