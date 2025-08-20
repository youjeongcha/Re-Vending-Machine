using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknameDialog : MonoBehaviour
{
    [Header("Refs")]
    public GameObject panel;           // 전체 팝업 루트
    public TMP_InputField input;
    public Button okButton;
    public Button cancelButton;
    public TMP_Text errorText;

    public Action<string> OnConfirm;   // 확인 눌렀을 때 콜백
    public Action OnCancel;

    void Awake()
    {
        Hide();

        okButton.onClick.AddListener(() =>
        {
            var nick = (input?.text ?? "").Trim();
            var err = Validate(nick);
            if (string.IsNullOrEmpty(err))
            {
                OnConfirm?.Invoke(nick);
                Hide();
            }
            else
            {
                if (errorText) errorText.text = err;
            }
        });

        cancelButton.onClick.AddListener(() =>
        {
            OnCancel?.Invoke();
            Hide();
        });
    }

    public void Show(string currentNickname = "")
    {
        if (panel) panel.SetActive(true);
        if (input) { input.text = currentNickname ?? ""; input.Select(); input.ActivateInputField(); }
        if (errorText) errorText.text = "";
    }

    public void Hide()
    {
        if (panel) panel.SetActive(false);
    }

    // 간단 검증 규칙: 2~12자, 영문/숫자/한글/공백/밑줄/대시
    private string Validate(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "닉네임을 입력해 주세요.";
        if (s.Length < 2 || s.Length > 12) return "닉네임은 2~12자로 해주세요.";
        foreach (char c in s)
        {
            if (!(char.IsLetterOrDigit(c) || c == ' ' || c == '_' || c == '-' || IsHangul(c)))
                return "허용되지 않은 문자가 포함되어 있어요.";
        }
        return null;

        static bool IsHangul(char ch) => (ch >= 0xAC00 && ch <= 0xD7A3) || (ch >= 0x3131 && ch <= 0x318E);
    }
}
