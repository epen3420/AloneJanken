using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultHekatonView : MonoBehaviour
{
    [System.Serializable]
    private struct SceneHekatonMapByScore
    {
        [System.Serializable]
        public struct SpriteScoreMap
        {
            public int score;
            public Sprite sprite;
            public string text;
        }

        public string SceneName;
        public bool useScoreLimit;
        public string text;
        public Sprite sprite; // useScoreLimit == true
        public SpriteScoreMap[] scores; // useScoreLimit == false
    }


    [SerializeField]
    private SceneHekatonMapByScore[] hekatonMapByScores;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text text;


    private void Start()
    {
        var currentSceneMap = hekatonMapByScores.FirstOrDefault(map => map.SceneName == SceneController.PreviousSceneName);

        if (!currentSceneMap.useScoreLimit)
        {
            image.sprite = currentSceneMap.sprite;
            text.SetText(currentSceneMap.text);
            return;
        }

        var spriteScoreMaps = currentSceneMap.scores.ToList();

        spriteScoreMaps.Sort((a, b) => b.score.CompareTo(a.score));

        var score = ScoreManager.Instance.GetCurrentScore();
        foreach (var spriteScoreMap in spriteScoreMaps)
        {
            if (score >= spriteScoreMap.score)
            {
                image.sprite = spriteScoreMap.sprite;
                text.SetText(spriteScoreMap.text);

                break;
            }
        }
    }
}
