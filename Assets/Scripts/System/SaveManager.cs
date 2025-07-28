using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private const string COIN_KEY = "coin";
    private const string BEST_SCORE_KEY = "bestScore";
    private const string CHARACTER_DICT_KEY = "charDict";
    private const string UNLOCKED_THEMES_KEY = "unlockedThemes";

    //public int Coin { get; private set; }


    public int BestScore { get; private set; }
    public Dictionary<string, bool> characterDictionary = new();
    public HashSet<string> unlockedThemes = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //LoadData();
        }
        else Destroy(gameObject);
    }



    /*    public void SaveData(int Coin)
        {
            PlayerPrefs.SetInt(COIN_KEY, Coin);
            PlayerPrefs.SetInt(BEST_SCORE_KEY, BestScore);
            //JsonUtility로 JSON 문자열화
            PlayerPrefs.SetString(CHARACTER_DICT_KEY, JsonUtility.ToJson(new SaveDictWrapper(characterDictionary)));
            PlayerPrefs.SetString(UNLOCKED_THEMES_KEY, string.Join(",", unlockedThemes));
            PlayerPrefs.Save();
        }*/

    /*    public void LoadData()
        {
            CoinManager.Instance.Coin = PlayerPrefs.GetInt(COIN_KEY, 0);
            BestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);

            string dictJson = PlayerPrefs.GetString(CHARACTER_DICT_KEY, "");
            if (!string.IsNullOrEmpty(dictJson))
            {
                characterDictionary = JsonUtility.FromJson<SaveDictWrapper>(dictJson).ToDictionary();
            }

            string themeStr = PlayerPrefs.GetString(UNLOCKED_THEMES_KEY, "");
            // HashSet<string>은 string.Join()으로 문자열로 저장 후 Split()로 복원
            unlockedThemes = new HashSet<string>(themeStr.Split(','));
        }*/

    /*    public void AddCoin(int amount)
        {
            Coin += amount;
            SaveData();
        }*/

    public void LoadData()
    {
        LoadCoin();
        LoadBestScore();
        LoadCharacterDict();
        LoadUnlockedThemes();
        LoadUnlockedThemes();
    }

    // Coin 저장/불러오기
    public void SaveCoin(int coin) => PlayerPrefs.SetInt(COIN_KEY, coin);
    public int LoadCoin() => PlayerPrefs.GetInt(COIN_KEY, 0);



    // BestScore 저장/불러오기
    public void SaveBestScore(int score)
    {
        BestScore = score;
        PlayerPrefs.SetInt(BEST_SCORE_KEY, score);
        PlayerPrefs.Save();
    }
    public int LoadBestScore()
    {
        BestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        return BestScore;
    }



    // 캐릭터 저장/로드
    public void SaveCharacterDict(Dictionary<string, bool> dict)
    {
        PlayerPrefs.SetString(CHARACTER_DICT_KEY, JsonUtility.ToJson(new SaveDictWrapper(dict)));
    }
    public Dictionary<string, bool> LoadCharacterDict()
    {
        string json = PlayerPrefs.GetString(CHARACTER_DICT_KEY, "");
        return string.IsNullOrEmpty(json) ? new() : JsonUtility.FromJson<SaveDictWrapper>(json).ToDictionary();
    }



    // 테마 저장/로드
    public void SaveUnlockedThemes(HashSet<string> themes)
    {
        PlayerPrefs.SetString(UNLOCKED_THEMES_KEY, string.Join(",", themes));
    }

    public HashSet<string> LoadUnlockedThemes()
    {
        string themeStr = PlayerPrefs.GetString(UNLOCKED_THEMES_KEY, "");
        return new HashSet<string>(themeStr.Split(',', System.StringSplitOptions.RemoveEmptyEntries));
    }



    [System.Serializable]
    public class SaveDictWrapper
    {
        public List<string> keys = new();
        public List<bool> values = new();

        public SaveDictWrapper(Dictionary<string, bool> dict)
        {
            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public Dictionary<string, bool> ToDictionary()
        {
            var dict = new Dictionary<string, bool>();
            for (int i = 0; i < keys.Count; i++)
                dict[keys[i]] = values[i];
            return dict;
        }
    }
}
