using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotHandler : MonoBehaviour
{
    [Header ("Line Renderers")]
    [SerializeField] private LineRenderer _LeftLineRenderer;
    [SerializeField] private LineRenderer _RightLineRenderer;

    [Header("Transforms")]
    [SerializeField] private Transform _LeftStartPosition;
    [SerializeField] private Transform _RightStartPosition;
    [SerializeField] private Transform _CentrePosition;
    [SerializeField] private Transform _IdlePosition;

    [Header("Slingshot Stats")]
    [SerializeField] private float _MaxDistance = 3.5f;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea _SlingShotAreaScript;


    private Vector2 _SlingShotLinesPosition;
    private bool _ClickedWithInArea = false;

    void Awake()
    {
        _LeftLineRenderer.enabled = false;
        _RightLineRenderer.enabled = false;
    }
   
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && _SlingShotAreaScript.IsWithinSlingShotArea())
        {
            _ClickedWithInArea = true;
        }

        if (Mouse.current.leftButton.isPressed && _ClickedWithInArea)
        {
            DrawSlingShot();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _ClickedWithInArea = false;
        }
    }

    void DrawSlingShot()
    {
        if (!_LeftLineRenderer.enabled && !_RightLineRenderer.enabled)
        {
            _LeftLineRenderer.enabled = true;
            _RightLineRenderer.enabled = true;
        }

        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        _SlingShotLinesPosition = _CentrePosition.position + Vector3.ClampMagnitude(touchPosition - _CentrePosition.position, _MaxDistance);

        SetLines(_SlingShotLinesPosition);
    }

    void SetLines(Vector2 position)
    {
        _LeftLineRenderer.SetPosition(0, position);
        _LeftLineRenderer.SetPosition(1, _LeftStartPosition.position);

        _RightLineRenderer.SetPosition(0, position);
        _RightLineRenderer.SetPosition(1, _RightStartPosition.position);
    }
}