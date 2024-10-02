using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int _MaxNumberOfShorts = 3;   

    private int _UsedNumberOfShorts;

    [SerializeField] private IconHandler _IconHandler;
    [SerializeField] private float _SecondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject _RestartScreen;
    [SerializeField] private SlingShotHandler _SlingShotHandler;

    [SerializeField] private List<PiggyScript> _Piggy = new List<PiggyScript>();

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PiggyScript[] Piggy = FindObjectsOfType<PiggyScript>();

        for (int i = 0; i < Piggy.Length; i++)
        {
            _Piggy.Add(Piggy[i]);
        }
    }
    public void UseShorts()
    {
        _UsedNumberOfShorts++;
        _IconHandler.UsedShot(_UsedNumberOfShorts);

        CheckForLastShot();
    }

    public bool HasEnoughShorts()
    {
        if (_UsedNumberOfShorts < _MaxNumberOfShorts)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForLastShot()
    {
        if (_UsedNumberOfShorts == _MaxNumberOfShorts)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(_SecondsToWaitBeforeDeathCheck);

        if (_Piggy.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }
    }

    public void RemovePiggy(PiggyScript piggy)
    {
        _Piggy.Remove(piggy);
        CheckForAllDeadPiggys();
    }

    void CheckForAllDeadPiggys()
    {
        if ( _Piggy.Count == 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        _RestartScreen.SetActive(true);
        _SlingShotHandler.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}