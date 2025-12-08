using UnityEngine;
using UnityEngine.UI;

public class LifePresenter : MonoBehaviour
{
    [SerializeField]
    private IntEventChannelSO changeLifeEvent;
    [SerializeField]
    private LifeViewer[] lifeViews;


    private void OnEnable()
    {
        changeLifeEvent.OnRaised += SetView;
    }

    // スクリプトが無効になったときに、イベントの購読を解除 (メモリリーク防止)
    private void OnDisable()
    {
        changeLifeEvent.OnRaised -= SetView;
    }


    /// <summary>
    /// 現在のライフ数に応じてUIのImageの表示を更新します。
    /// </summary>
    /// <param name="lifeCount">現在のライフ数</param>
    private void SetView(int lifeCount)
    {
        for (int i = 0; i < lifeViews.Length; i++)
        {
            bool isLifeRemaining = i < lifeCount;
            lifeViews[i].SetSprite(isLifeRemaining);
        }
    }
}
