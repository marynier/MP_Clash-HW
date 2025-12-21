using System.Collections.Generic;
using UnityEngine;

public class DeckSaver : MonoBehaviour
{
    [SerializeField] private CardSelector _cardSelector; 
    private int[] _idsForSaving;

    public void Save()
    {
        _idsForSaving = _cardSelector.GetSelectedCardIds();
        DeckSaveData saveData = new DeckSaveData
        {
            userID = /*UserInfo.Instance.ID.ToString()*/ "7",
            selectedIDs = _idsForSaving
        };

        string json = JsonUtility.ToJson(saveData);

        Network.Instance.Post(
            URLLibrary.MAIN + URLLibrary.SAVEDECK,
            new Dictionary<string, string>
            {
                { "data", json }
            },
            OnSuccessSave,
            OnErrorSave
        );        
    }

    private void OnSuccessSave(string response)
    {
        if (response != "OK")
        {
            OnErrorSave(response);
            return;
        }
        _cardSelector.OnSaveButton();
        Debug.Log("Deck saved: " + response);
    }

    private void OnErrorSave(string error)
    {
        _cardSelector.OnCancelButton();
        Debug.LogError("Save error: " + error);
    }
}

[System.Serializable]
public class DeckSaveData
{
    public string userID;
    public int[] selectedIDs;
}