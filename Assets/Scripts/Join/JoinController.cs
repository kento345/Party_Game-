using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class JoinController : MonoBehaviour
{
    [Header("参加設定")]
    [SerializeField] private InputAction _joinAction = default;
    [SerializeField] private InputAction _startAction = default;
    [SerializeField] private GameObject _playerPrefab = default;
    private int _maxPlayer = 4;
    //-----Text-----
    [SerializeField] private TextMeshProUGUI _p1text;
    [SerializeField] private TextMeshProUGUI _p2text;
    [SerializeField] private TextMeshProUGUI _p3text;
    [SerializeField] private TextMeshProUGUI _p4text;
    private Dictionary<InputDevice, int> _playerMap = new();
    [SerializeField] private Transform[] _pos = default;

    private void Awake()
    {
        //参加InputActionの有効化
        _joinAction.Enable();
        _joinAction.performed += OnJoin;
        _startAction.Enable();
        _startAction.performed += OnGameStart;

        //UpdateText(false);
    }

    private void OnDestroy()
    {
        _joinAction.performed -= OnJoin;
        _joinAction.Disable();
        _startAction.performed -= OnGameStart;
        _startAction.Disable();
    }

    /// <summary>
    /// 参加処理
    /// </summary>
    /// <param name="context"></param>
    void OnJoin(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        //既に参加済み,4人以上参加してたらreturn
        if (_playerMap.ContainsKey(device) || _playerMap.Count >= _maxPlayer) { return; }

        int playerID = _playerMap.Count;
        _playerMap.Add(device, playerID);

        UpdateText(true);
        CreatePlayer(device, playerID);
    }

    /// <summary>
    /// Textの更新
    /// </summary>
    void UpdateText(bool a)
    {
        TextMeshProUGUI[] texts = {_p1text,_p2text, _p3text, _p4text};
        if (!a)
        {
            foreach (var p in _playerMap)
            {
                var device = p.Key;
                var playerID = p.Value;

                texts[playerID].text = $"Push A To Join";
            }
        }
        if (a)
        {
            foreach (var p in _playerMap)
            {
                var device = p.Key;
                var playerID = p.Value;

                texts[playerID].text = $"Player {playerID + 1}\nJoined";
            }
        }
    }

    void CreatePlayer(InputDevice device,int id)
    {
        if(device == null) { return; }
        var obj = PlayerInput.Instantiate(
            prefab: _playerPrefab,
            playerIndex: id,
            pairWithDevice: device);

        obj.transform.position = _pos[id].transform.position;
        obj.transform.rotation = _pos[id].transform.rotation;
        var input = obj.GetComponent<PlayerInputController>();
        if(input != null) { input.OnMoveStop(false); }

        JoinDataHolder.Instance.SetPlayerData(obj.gameObject,id + 1);
        DontDestroyOnLoad(obj);
    }

    public void OnGameStart(InputAction.CallbackContext context)
    {
        if(_playerMap.Count <= 0) { return; }
        _startAction.Disable();
        _joinAction.Disable();

        foreach(var p in JoinDataHolder.Instance.GetPlayerData.Keys)
        {
            foreach (var child in p.GetComponentsInChildren<Transform>())
            {
                child.gameObject.SetActive(false);
            }
        }

        SceneManager.LoadScene("MainGame");
    }
}