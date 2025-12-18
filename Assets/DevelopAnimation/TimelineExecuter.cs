using UnityEngine;
using UnityEngine.Playables;

public class TimelineExecuter : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector timeline;
    [SerializeField]
    private float speed = 1.0f;
    [Header("これをオンにすると上のSpeedは無効になります。")]
    [SerializeField]
    private bool useBpm = false;
    [SerializeField]
    private int bpm = 60;

    private void Start()
    {
        float timelineSeed = useBpm ? bpm / 60.0f : speed;
        timeline.RebuildGraph();
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(timelineSeed);
        timeline.Play();
    }
}
