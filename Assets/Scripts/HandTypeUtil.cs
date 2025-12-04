using System.Collections.Generic;
using System.Linq;

public static class HandTypeUtil
{
    private static readonly Dictionary<HandType, string> handTypeNameDict = new Dictionary<HandType, string>
    {
        {HandType.Rock, "グー"},
        {HandType.Scissors, "チョキ"},
        {HandType.Paper, "パー"},
    };

    private static readonly Dictionary<HandPosType, string> handPosTypeNameDict = new Dictionary<HandPosType, string>
    {
        {HandPosType.None, "None"},
        {HandPosType.LeftDown, "左下"},
        {HandPosType.LeftUp, "左上"},
        {HandPosType.RightDown, "右下"},
        {HandPosType.RightUp, "右上"},
    };

    private static readonly HandType[] handTypes = (HandType[])System.Enum.GetValues(typeof(HandType));
    private static readonly HandType[] handTypesWithoutNone = handTypes.Where(pos => pos != HandType.None && pos != HandType.Strange).ToArray();
    private static readonly HandPosType[] handPosTypes = (HandPosType[])System.Enum.GetValues(typeof(HandPosType));
    private static readonly HandPosType[] handPosTypesWithoutNone = handPosTypes.Where(pos => pos != HandPosType.None).ToArray();


    public static HandType GetRandomlyHandType()
    {
        int randomInt = UnityEngine.Random.Range(0, handTypesWithoutNone.Length);
        return handTypesWithoutNone[randomInt];
    }

    public static HandPosType GetRandomlyHandPosType()
    {
        int randomInt = UnityEngine.Random.Range(0, handPosTypesWithoutNone.Length);
        return handPosTypesWithoutNone[randomInt];
    }

    public static HandType[] HandTypes => handTypesWithoutNone;
    public static int HandCount => handTypes.Length - 1; // Noneの文を引く

    public static HandPosType[] HandPosTypes => handPosTypesWithoutNone;
    public static int HandPosCount => handPosTypes.Length - 1; // Noneの文を引く

    public static string GetHandName(HandType type) => handTypeNameDict[type];

    public static string GetHandPosName(HandPosType type) => handPosTypeNameDict[type];
}
