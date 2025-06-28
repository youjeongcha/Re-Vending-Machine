using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGachaTable", menuName = "Game/GachaTable")]
public class GachaTable : ScriptableObject
{
    public List<GachaEntry> entries;

    public CharacterData GetRandomCharacter()
    {
        float totalWeight = 0f;
        foreach (var entry in entries)
            totalWeight += entry.weight;

        float random = Random.Range(0f, totalWeight);
        float current = 0f;

        foreach (var entry in entries)
        {
            current += entry.weight;
            if (random <= current)
                return entry.character;
        }

        return null;
    }
}
