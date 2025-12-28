using TMPro;
using UnityEngine;

public class GameStartController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private TextMeshProUGUI _countdownText;

    private void Start()
    {
        _countdownPanel.SetActive(true);
        
        if (MultiplayerManager.Instance != null)
        {
            MultiplayerManager.Instance.OnCountdownStart += StartCountdown;
            MultiplayerManager.Instance.OnCountdownUpdate += UpdateCountdown;
        }
    }

    private void OnDestroy()
    {
        if (MultiplayerManager.Instance != null)
        {
            MultiplayerManager.Instance.OnCountdownStart -= StartCountdown;
            MultiplayerManager.Instance.OnCountdownUpdate -= UpdateCountdown;
        }
    }

    private void StartCountdown(int seconds)
    {
        _countdownPanel.SetActive(true);
        UpdateCountdown(seconds);
    }

    private void UpdateCountdown(int seconds)
    {
        if (seconds > 0)
        {
            _countdownText.text = seconds.ToString();
        }
        else
        {
            _countdownText.text = "НАЧАЛИ!";
            Invoke("EnableGameplay", 1f);
        }
    }

    private void EnableGameplay()
    {
        _countdownPanel.SetActive(false);

        Debug.Log("Игра началась!");
    }
}
