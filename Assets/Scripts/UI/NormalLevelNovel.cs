using TimeSpan = System.TimeSpan;
using Cysharp.Threading.Tasks;

public class NormalLevelNovel : NovelController
{
    public override async UniTask Execute(ChatShower chatShower)
    {
        await chatShower.ShowAsTypeWriter("そしたら本番いくよ～");
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));

        await chatShower.ShowAsTypeWriter("よーい、はじめっ！！");
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }
}
