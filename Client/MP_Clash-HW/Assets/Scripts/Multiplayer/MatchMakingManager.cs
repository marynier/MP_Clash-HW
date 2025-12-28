using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchMakingManager : MonoBehaviour
{
    [System.Serializable]
    public class Decks
    {
        public string player1ID;
        public string[] player1;
        public string[] player2;
    }

    [SerializeField] private string _gameSceneName = "SampleScene";
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _matchmakingCanvas;
    [SerializeField] private GameObject _cancelButton;

    public void Subscribe()
    {
        MultiplayerManager.Instance.GetReady += GetReady;
        MultiplayerManager.Instance.StartGame += StartGame;
        MultiplayerManager.Instance.CancelStart += CancelStart;
    }

    public void Unsubscribe()
    {
        MultiplayerManager.Instance.GetReady -= GetReady;
        MultiplayerManager.Instance.StartGame -= StartGame;
        MultiplayerManager.Instance.CancelStart -= CancelStart;
    }

    private void GetReady()
    {
        _cancelButton.SetActive(false);
    }

    private void CancelStart()
    {
        _cancelButton.SetActive(true);
    }

    private void StartGame(string jsonDecks)
    {
        Decks decks = JsonUtility.FromJson<Decks>(jsonDecks);
        string[] playerDeck;
        string[] enemyDeck;
        Debug.Log($"{MultiplayerManager.Instance.clientID} || {jsonDecks}");
        if (decks.player1ID == MultiplayerManager.Instance.clientID)
        {
            playerDeck = decks.player1;
            enemyDeck = decks.player2;
        }
        else
        {
            playerDeck = decks.player2;
            enemyDeck = decks.player1;
        }

        CardsInGame.Instance.SetDecks(playerDeck, enemyDeck);

        SceneManager.LoadScene(_gameSceneName);
    }

    public async void FindOpponent()
    {
        _cancelButton.SetActive(false);
        _mainMenuCanvas.SetActive(false);
        _matchmakingCanvas.SetActive(true);

        await MultiplayerManager.Instance.Connect();
        _cancelButton.SetActive(true);
    }

    public void CancelFind()
    {
        _matchmakingCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(true);

        MultiplayerManager.Instance.Leave();
    }
}
