using System.Collections.Generic;
using UnityEngine;

public class test_Janken : MonoBehaviour
{
    [SerializeField]
    private HandInfo[] handInfos;

    [System.Serializable]
    private struct HandInfo
    {
        public string ownerName;
        public HandType handType;
    }

    public void StartJudge()
    {
        var hands = new List<Hand>();
        foreach (var handInfo in handInfos)
        {
            hands.Add(new Hand(handInfo.handType, handInfo.ownerName));
        }

        var results = HandJudger.Judge(hands);
        foreach (var result in results)
        {
            Debug.Log(result.ToString());
        }
    }
}
