using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public List<CharacterData> characterPool;
    public GameObject gachaResultPopup;

    public void Roll()
    {
        if (CoinManager.Instance.UseCoins(100))
        {
            int index = Random.Range(0, characterPool.Count);
            CharacterData drawn = characterPool[index];
            CollectionManager.Instance.Register(drawn);
            UIManager.Instance.ShowGachaResult(drawn);
        }
    }
}