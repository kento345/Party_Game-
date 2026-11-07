using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject _botPreefab = default;
    [SerializeField] private Transform[] _pos = default;
    private GameObject _joinObj;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TextMeshProUGUI _resultText;

    private Dictionary<GameObject,int> _ActivePlayers = new();
    private List<GameObject> _playerList = new();
    private List<GameObject> _botList = new();

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (JoinDataHolder.Instance == null) { return; }
        //DontDestoryObjectを取得
        _joinObj = JoinDataHolder.Instance.gameObject;
        
        //インスタンスで保持しているPlayer情報を取得
        var players = JoinDataHolder.Instance.GetPlayerData;

        foreach (var player in players)
        {
            player.Key.transform.position = _pos[player.Value - 1].position;
            player.Key.transform.rotation = _pos[player.Value - 1].rotation;
            foreach (var child in player.Key.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.SetActive(true);
            }
        }
        int i = players.Count;
        while(i < _pos.Length)
        {
            var bot = Instantiate(_botPreefab, _pos[i].position, _pos[i].rotation);
            JoinDataHolder.Instance.SetPlayerData(bot, i + 1);
            _botList.Add(bot);
            i++;
        }

        _ActivePlayers = new Dictionary<GameObject,int>(JoinDataHolder.Instance.GetPlayerData);
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        List<GameObject> deadPlayers = new();
        Debug.Log(Time.timeScale);
        foreach (var p in _ActivePlayers)
        {
            var life = p.Key.GetComponent<HitController>();

            if (life != null && !life.IsAlive)
            {
                deadPlayers.Add(p.Key);
            }
        }

        foreach (var player in deadPlayers)
        {
            _ActivePlayers.Remove(player);
        }

        if (_ActivePlayers.Count == 1)
        {
            foreach (var p in _ActivePlayers)
            {
                _resultText.text = $"Win: {p.Key.name.Replace("(Clone)", "")}{p.Value}";
                _resultPanel.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
    }

    public void OnReset()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        if (JoinDataHolder.Instance != null)
        {
            foreach (var bot in _botList)
            {
                JoinDataHolder.Instance.RemoveBotData(bot);
            }
        }
        SceneManager.LoadScene(currentScene);
    }

    public void OnTitle()
    {
        foreach(var p in JoinDataHolder.Instance.GetPlayerData)
        {
            Destroy(p.Key);
        }
        Destroy(_joinObj);
        SceneManager.LoadScene("JoinScene");
    }
}
