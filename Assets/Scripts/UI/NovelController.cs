using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class NovelController : MonoBehaviour
{
    public abstract UniTask Execute(ChatShower chatShower);
}
