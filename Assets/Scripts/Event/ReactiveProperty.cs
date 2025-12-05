using System.Collections.Generic;

[System.Serializable]
public class ReactiveProperty<T>
{
    [UnityEngine.SerializeField]
    private EventChannelSO<T> observer;
    private T value;

    public T Value
    {
        get { return this.value; }
        set
        {
            if (EqualityComparer<T>.Default.Equals(this.value, value))
            {
                return;
            }
            this.value = value;

            observer.Raise(this.value);
        }
    }
}
