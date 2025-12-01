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

    private static readonly HandPosType[] handPosTypes = (HandPosType[])System.Enum.GetValues(typeof(HandPosType));

    private static readonly HandPosType[] handPosTypesWithoutNone = handPosTypes.Where(pos => pos != HandPosType.None).ToArray();


    public static int HandPosCount => handPosTypes.Length - 1; // Noneの文を引く
    public static HandPosType[] GetHandPosTypes() => handPosTypesWithoutNone;

    public static string GetHandName(HandType type) => handTypeNameDict[type];

    public static string GetHandPosName(HandPosType type) => handPosTypeNameDict[type];
}
