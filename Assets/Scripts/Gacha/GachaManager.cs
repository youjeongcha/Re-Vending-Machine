using System.Collections.Generic;
using UnityEngine;



public class GachaManager : MonoBehaviour
{
    public List<CharacterData> characterPool;
    public GameObject gachaResultPopup;
    public GachaTable gachaTable;
    /*Assets > Create > Game > GachaTable로 에셋 생성s
    entries 리스트에 CharacterData와 weight, rarity 입력
    GachaManager에서 다음처럼 호출:
    */

    public void Roll()
    {
        if (CoinManager.Instance.UseCoins(100))
        {
            /*int index = Random.Range(0, characterPool.Count);
            CharacterData drawn = characterPool[index];*/
    CharacterData drawn = gachaTable.GetRandomCharacter();
            CollectionManager.Instance.Register(drawn);
            UIManager.Instance.ShowGachaResult(drawn);
        }
    }
}