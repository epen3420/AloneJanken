using TimeSpan = System.TimeSpan;
using Cysharp.Threading.Tasks;

public class TutorialNovel : NovelController
{
    public override async UniTask Execute(ChatShower chatShower)
    {
        await chatShower.ShowAsTypeWriter("まずは練習から～");
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
    }
}
