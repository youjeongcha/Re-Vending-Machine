using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;
    public List<CharacterData> ownedCharacters = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Register(CharacterData data)
    {
        if (!ownedCharacters.Contains(data))
        {
            ownedCharacters.Add(data);
            // Save to PlayerPrefs or file
        }
    }
}