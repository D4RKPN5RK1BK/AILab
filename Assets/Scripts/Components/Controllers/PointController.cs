using System;
using System.Linq;
using Core;
using TMPro;
using UnityEngine;

/// <summary>
/// Контроллер который реализует поведение точек на доске
/// Является вспомогательным компонентом
/// </summary>
public class PointController : NavigateComponent
{
    private Renderer _renderer;
    private TextMeshPro _textMeshPro;
    private Camera _camera;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        _camera = FindObjectsOfType<Camera>().First();
    }

    private void Update()
    {
        _textMeshPro.transform.rotation = _camera.transform.rotation;
    }
    
    public void UpdateCell(CellType type, string additionalData = null)
    {
        if (!string.IsNullOrEmpty(additionalData))
            _textMeshPro.text = additionalData;

        switch (type)
        { 
            case CellType.Empty:
            {
                _textMeshPro.gameObject.SetActive(false);
                _renderer.material.color = Color.white;
                transform.localScale = Vector3.zero;
                break;
            }
            case CellType.Brick:
            {
                _textMeshPro.gameObject.SetActive(false);
                _renderer.material.color = Color.white;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            }
            case CellType.Start:
            {
                _renderer.material.color = new Color(0.7f, 0.7f, 1.0f);
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                break;
            }
            case CellType.Finish:
            {
                _renderer.material.color = new Color(0.3f, 0.3f, 0.9f);
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                break;
            }
            case CellType.Marked:
            {
                _renderer.material.color = Color.yellow;
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                break;
            }
            case CellType.Observed:
            {
                _textMeshPro.gameObject.SetActive(true);
                _renderer.material.color = Color.green;
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                break;
            }
            case CellType.Closed:
            {
                _renderer.material.color = Color.red;
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        } 
    }
}
