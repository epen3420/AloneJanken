using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ChatShower : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private float duration = 0.5f;
    [SerializeField]
    private float waitTimeAfterAllVisible = 1.0f;


    public void ShowText(string sentence)
    {
        text.SetText(sentence);
    }

    public async UniTask ShowAsTypeWriter(string sentence)
    {
        text.maxVisibleCharacters = 0;
        text.SetText(sentence);

        for (int i = 0; i < sentence.Length; i++)
        {
            text.maxVisibleCharacters++;
            await UniTask.Delay((int)(duration * 1000));
        }

        await UniTask.Delay((int)(waitTimeAfterAllVisible * 1000));
    }

    public async UniTask ShowAsTypeWriter(IEnumerable<string> sentences)
    {
        foreach (var sentence in sentences)
        {
            await ShowAsTypeWriter(sentence);
        }
    }
}
