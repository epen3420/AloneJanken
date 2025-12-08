using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatShower : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private Image textboxImage;
    [SerializeField]
    private Sprite chatTextboxSprite;
    [SerializeField]
    private Sprite questTextboxSprite;


    public void ShowText(string sentence)
    {
        text.maxVisibleCharacters = sentence.Length;
        textboxImage.sprite = questTextboxSprite;
        text.SetText(sentence);
    }

    public async UniTask ShowAsTypeWriter(string sentence, float duration = 0.1f)
    {
        textboxImage.sprite = chatTextboxSprite;
        text.maxVisibleCharacters = 0;
        text.SetText(sentence);

        for (int i = 0; i < sentence.Length; i++)
        {
            text.maxVisibleCharacters++;
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }

    public async UniTask ShowAsTypeWriter(IEnumerable<string> sentences)
    {
        foreach (var sentence in sentences)
        {
            await ShowAsTypeWriter(sentence);
        }
    }
}
