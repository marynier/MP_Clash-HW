using UnityEngine;


public class MenuSubscriber : MonoBehaviour
{
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private SelectedDeckUI _selectedDeckUI;
    [SerializeField] private SelectedDeckUI _selectedDeckUI2;
    [SerializeField] private SelectedDeckUI _selectedDeckUIMatchMaking;
    [SerializeField] private AvailableDeckUI _availableDeckUI;
    [SerializeField] private MatchMakingManager _matchMakingManager;

    private void Start()
    {
        _deckManager.UpdateSelected += _selectedDeckUI.UpdateCardsList;
        _deckManager.UpdateSelected += _selectedDeckUI2.UpdateCardsList;
        _deckManager.UpdateSelected += _selectedDeckUIMatchMaking.UpdateCardsList;
        _deckManager.UpdateAvailable += _availableDeckUI.UpdateCardsList;

        _matchMakingManager.Subscribe();
    }

    private void OnDestroy()
    {
        _deckManager.UpdateSelected -= _selectedDeckUI.UpdateCardsList;
        _deckManager.UpdateSelected -= _selectedDeckUI2.UpdateCardsList;
        _deckManager.UpdateSelected -= _selectedDeckUIMatchMaking.UpdateCardsList;
        _deckManager.UpdateAvailable -= _availableDeckUI.UpdateCardsList;

        _matchMakingManager.Unsubscribe();

    }
}
