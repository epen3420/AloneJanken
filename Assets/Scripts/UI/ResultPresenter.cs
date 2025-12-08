using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResultPresenter : MonoBehaviour
{
    [SerializeField]
    private ResultViewer resultViewer;


    private void Start()
    {
        resultViewer.CountScore(ScoreManager.Instance.GetCurrentScore(), 1).Forget();
    }
}
