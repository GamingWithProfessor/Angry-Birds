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
    [SerializeField] private float _ShotForce = 5f;
    [SerializeField] private float _TimeBetweenBirdRespawns = 2f;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea _SlingShotAreaScript;

    [Header("Angry Birds")]
    [SerializeField] private AngryBird _AngryBird;
    [SerializeField] private float _AngryBirdPositionOffset = 2f;

    private AngryBird _SpawnedAngryBird;

    private Vector2 _Direction;
    private Vector2 _DirectionNormalized;


    private Vector2 _SlingShotLinesPosition;
    private bool _ClickedWithInArea = false;
    private bool _BirdOnSlingShot;

    void Awake()
    {
        _LeftLineRenderer.enabled = false;
        _RightLineRenderer.enabled = false;
        SpawnAngryBird();

        //Application.targetFrameRate = 60;
    }
   
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && _SlingShotAreaScript.IsWithinSlingShotArea())
        {
            _ClickedWithInArea = true;
        }

        if (Mouse.current.leftButton.isPressed && _ClickedWithInArea && _BirdOnSlingShot)
        {
            DrawSlingShot();
            PositionAndRotationAngryBird();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _BirdOnSlingShot)
        {
            _ClickedWithInArea = false;

            _SpawnedAngryBird.LaunchBird(_Direction, _ShotForce);
            _BirdOnSlingShot = false;

            SetLines(_CentrePosition.position);

            StartCoroutine(SpawnAngryBirdAfterTime());
        }
    }

    #region SlingShot Methods

    void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        _SlingShotLinesPosition = _CentrePosition.position + Vector3.ClampMagnitude(touchPosition - _CentrePosition.position, _MaxDistance);

        SetLines(_SlingShotLinesPosition);

        _Direction = (Vector2) _CentrePosition.position - _SlingShotLinesPosition;
        _DirectionNormalized = _Direction.normalized;
    }

    void SetLines(Vector2 position)
    {
        if (!_LeftLineRenderer.enabled && !_RightLineRenderer.enabled)
        {
            _LeftLineRenderer.enabled = true;
            _RightLineRenderer.enabled = true;
        }

        _LeftLineRenderer.SetPosition(0, position);
        _LeftLineRenderer.SetPosition(1, _LeftStartPosition.position);

        _RightLineRenderer.SetPosition(0, position);
        _RightLineRenderer.SetPosition(1, _RightStartPosition.position);
    }
    #endregion

    #region AngryBird Methods

    private void SpawnAngryBird()
    {
        SetLines(_IdlePosition.position);

        Vector2 dir = (_CentrePosition.position - _IdlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_IdlePosition.position + dir * _AngryBirdPositionOffset;

        _SpawnedAngryBird = Instantiate(_AngryBird, spawnPosition, Quaternion.identity);
        _SpawnedAngryBird.transform.right = dir;

        _BirdOnSlingShot = true;
    }

    private void PositionAndRotationAngryBird()
    {
        _SpawnedAngryBird.transform.position = _SlingShotLinesPosition + _DirectionNormalized * _AngryBirdPositionOffset;
        _SpawnedAngryBird.transform.right = _DirectionNormalized;
    }
    
    private IEnumerator SpawnAngryBirdAfterTime()
    {
        yield return new WaitForSeconds(_TimeBetweenBirdRespawns);

        SpawnAngryBird();
    }
    #endregion
}