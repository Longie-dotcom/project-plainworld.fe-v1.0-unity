using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class EmojiData
{
    public string id;        // sad
    public Sprite icon;
    public string unicode;   // :sad:
}

[CreateAssetMenu(fileName = "EmojiCatalog", menuName = "Emoji/EmojiCatalog")]
public class EmojiCatalog : ScriptableObject
{
    public List<EmojiData> emojis;

    private Dictionary<string, string> unicodeToSpriteTag;

    private void OnEnable()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        unicodeToSpriteTag = new Dictionary<string, string>();

        foreach (var emoji in emojis)
        {
            if (!string.IsNullOrEmpty(emoji.unicode))
            {
                unicodeToSpriteTag[emoji.unicode] =
                    $"<sprite name=\"{emoji.id}\">";
            }
        }
    }

    public string ParseToSpriteTags(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        if (unicodeToSpriteTag == null)
            BuildLookup();

        StringBuilder result = new StringBuilder(input);

        foreach (var pair in unicodeToSpriteTag)
        {
            result.Replace(pair.Key, pair.Value);
        }

        return result.ToString();
    }

    public string ParseToUnicode(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        foreach (var emoji in emojis)
        {
            string tag = $"<sprite name=\"{emoji.id}\">";
            input = input.Replace(tag, emoji.unicode);
        }

        return input;
    }
}
