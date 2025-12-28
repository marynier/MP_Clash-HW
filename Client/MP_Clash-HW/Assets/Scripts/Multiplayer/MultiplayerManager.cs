using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Colyseus;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    private const string RoomName = "state_handler";
    private const string GetReadyName = "GetReady";
    private const string StartGameName = "Start";
    private const string CancelStartName = "CancelStart";
    private const string SpawnUnitName = "SpawnUnit";

    private ColyseusRoom<State> _room;

    public event Action GetReady;
    public event Action<string> StartGame;
    public event Action CancelStart;

    public event Action<string, Vector3> OnEnemyUnitSpawned;

    public string clientID
    {
        get
        {
            if (_room == null) return "";
            else return _room.SessionId;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        Instance.InitializeClient();
        DontDestroyOnLoad(gameObject);
    }

    public async Task Connect()
    {
        _room = await Instance.client.JoinOrCreate<State>(RoomName, new Dictionary<string, object> { { "id", UserInfo.Instance.ID } });
        _room.OnMessage<object>(GetReadyName, (empty) => GetReady?.Invoke());
        _room.OnMessage<string>(StartGameName, (jsonDecks) => StartGame?.Invoke(jsonDecks));
        _room.OnMessage<object>(CancelStartName, (empty) => CancelStart?.Invoke());

        _room.OnMessage<string>(SpawnUnitName, (jsonMessage) =>
        {
            SpawnUnitMessage message = JsonUtility.FromJson<SpawnUnitMessage>(jsonMessage);
            Vector3 position = new Vector3(message.posX, 0, message.posZ);
            OnEnemyUnitSpawned?.Invoke(message.cardId, position);
        });
    }

    public void SendUnitSpawn(string cardId, Vector3 position)
    {
        if (_room == null) return;

        SpawnUnitMessage message = new SpawnUnitMessage
        {
            cardId = cardId,
            posX = position.x,            
            posZ = position.z
        };

        string json = JsonUtility.ToJson(message);
        _room.Send(SpawnUnitName, json);
    }


    public void Leave()
    {
        _room?.Leave();
        _room = null;
    }
}

[System.Serializable]
public class SpawnUnitMessage
{
    public string cardId;
    public float posX;
    public float posZ;
}
