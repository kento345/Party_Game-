using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour
{
    private List<Material> _tankColors = new();

    PlayerInputController _inputCon; 
    BotController _botCon;

    Image _image;

    private void OnEnable()
    {
        _inputCon = GetComponentInParent<PlayerInputController>();
        _botCon = GetComponentInParent<BotController>();

        if(_inputCon != null)
        {
            _inputCon.OnMoveStop(true);
        }
        if (_botCon != null)
        {
            _botCon.OnMoveStop(true);
        }
    }

    private void OnDisable()
    {
        if(_inputCon != null)
        {
            _inputCon.OnMoveStop(false);
        }
        if (_botCon != null)
        {
            _botCon.OnMoveStop(false);
        }
    }

    private void Start()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        _image = GetComponent<Image>();
        if(JoinDataHolder.instance != null)
        {
            var id = JoinDataHolder.instance.GetPlayerData[transform.root.gameObject];
            if (renderers != null)
            {
                foreach (var renderer in renderers)
                {
                    foreach (var material in renderer.materials)
                    {
                        if (material.name.Contains("TankColor"))
                        {
                            _tankColors.Add(material);
                        }
                    }
                }

                switch (id)
                {
                    case 1:
                        SetColor(Color.red);
                        break;
                    case 2:
                        SetColor(Color.blue);
                        break;
                    case 3:
                        SetColor(Color.green);
                        break;

                    case 4:
                        SetColor(Color.yellow);
                        break;
                }
            }

            if (_image != null)
            {
                switch (id)
                {
                    case 1:
                        SetImageColor(Color.red);
                        break;
                    case 2:
                        SetImageColor(Color.blue);
                        break;
                    case 3:
                        SetImageColor(Color.green);
                        break;

                    case 4:
                        SetImageColor(Color.yellow);
                        break;
                }
            }
        }
       
    }

    void SetColor(Color color)
    {
        foreach (var material in _tankColors)
        {
            material.color = color;
        }
    }
    void SetImageColor(Color color)
    {
        color.a = _image.color.a;
        _image.color = color;
    }
}
