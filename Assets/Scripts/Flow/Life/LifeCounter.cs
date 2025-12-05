using Debug = UnityEngine.Debug;

public class LifeCounter
{
    private readonly IntEventChannelSO OnChangeLife;
    private readonly VoidEventChannelSO OnLifeZero;

    public int CurrentLife => currentLife;

    private int currentLife;


    public LifeCounter(VoidEventChannelSO lifeZeroEventChannel,
                       IntEventChannelSO changeLifeEventChannel,
                       int maxLife = 3)
    {
        OnChangeLife = changeLifeEventChannel;
        OnLifeZero = lifeZeroEventChannel;

        ChangeLife(maxLife);
    }

    public void IncreaseLife()
    {
        AddLife(1);
    }

    public void DecreaseLife()
    {
        AddLife(-1);
    }

    public void AddLife(int value)
    {
        ChangeLife(currentLife + value);
    }

    private void ChangeLife(int life)
    {
        if (life < 0)
        {
            Debug.LogWarning($"Can not change life to {life}");
            return;
        }

        currentLife = life;
        OnChangeLife?.Raise(currentLife);

        if (currentLife == 0)
        {
            OnLifeZero.Raise();
        }
    }
}
