using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknameDialog : MonoBehaviour
{
    [Header("Refs")]
    public GameObject panel;           // ��ü �˾� ��Ʈ
    public TMP_InputField input;
    public Button okButton;
    public Button cancelButton;
    public TMP_Text errorText;

    public Action<string> OnConfirm;   // Ȯ�� ������ �� �ݹ�
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

    // ���� ���� ��Ģ: 2~12��, ����/����/�ѱ�/����/����/���
    private string Validate(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "�г����� �Է��� �ּ���.";
        if (s.Length < 2 || s.Length > 12) return "�г����� 2~12�ڷ� ���ּ���.";
        foreach (char c in s)
        {
            if (!(char.IsLetterOrDigit(c) || c == ' ' || c == '_' || c == '-' || IsHangul(c)))
                return "������ ���� ���ڰ� ���ԵǾ� �־��.";
        }
        return null;

        static bool IsHangul(char ch) => (ch >= 0xAC00 && ch <= 0xD7A3) || (ch >= 0x3131 && ch <= 0x318E);
    }
}
