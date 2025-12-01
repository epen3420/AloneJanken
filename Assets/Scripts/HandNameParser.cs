using System.Collections.Generic;

public static class HandNameParser
{
    private static readonly Dictionary<HandType, string> handTypeNameDict = new Dictionary<HandType, string>
    {
        {HandType.Rock, "グー"},
        {HandType.Scissors, "チョキ"},
        {HandType.Paper, "パー"},
    };

    private static readonly Dictionary<HandPosType, string> handPosTypeNameDict = new Dictionary<HandPosType, string>
    {
        {HandPosType.LeftDown, "左下"},
        {HandPosType.LeftUp, "左上"},
        {HandPosType.RightDown, "右下"},
        {HandPosType.RightUp, "右上"},
    };


    public static string GetHandName(HandType type) => handTypeNameDict[type];

    public static string GetHandPosName(HandPosType type) => handPosTypeNameDict[type];
}
