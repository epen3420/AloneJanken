using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResultHekatonView : MonoBehaviour
{
    [System.Serializable]
    private struct SceneHekatonMap
    {
        public string SceneName;
        public Sprite sprite;
    }
    [System.Serializable]
    private struct SceneHekatonMapByScore
    {
        [System.Serializable]
        public struct SpriteScoreMap
        {
            public int score;
            public Sprite sprite;
        }

        public string SceneName;
        public SpriteScoreMap[] scores;
    }

    [SerializeField]
    private SceneHekatonMap[] sceneHekatonMaps;
    [SerializeField]
    private SceneHekatonMapByScore[] hekatonMapByScores;
    [SerializeField]
    private Image image;


    private void Start()
    {
        List<SceneHekatonMapByScore.SpriteScoreMap> spriteScoreMaps =
            hekatonMapByScores.FirstOrDefault(map => map.SceneName == SceneController.PreviousSceneName)
            .scores
            .ToList();

        spriteScoreMaps.Sort((a, b) =>
        {
            if (a.score < b.score) return 1;
            else return -1;
        });

        var score = ScoreManager.Instance.GetCurrentScore();
        foreach (var spriteScoreMap in spriteScoreMaps)
        {
            if (score < spriteScoreMap.score)
            {
                image.sprite = spriteScoreMap.sprite;
            }
        }
    }
}
