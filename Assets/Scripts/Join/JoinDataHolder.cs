using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinDataHolder : MonoBehaviour
{
    public static JoinDataHolder Instance { get; private set;}
    private Dictionary<GameObject, int> _playerData= new ();
    //参照専用
    
    public IReadOnlyDictionary<GameObject, int> GetPlayerData => _playerData;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerData(GameObject player,int id)
    {
        _playerData.Add(player, id);
    }

    public void RemoveBotData(GameObject bot)
    {
        _playerData.Remove(bot);
    }
}
