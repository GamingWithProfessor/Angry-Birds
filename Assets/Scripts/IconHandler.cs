using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] _Icons;
    [SerializeField] private Color _UsedColour;

    public void UsedShot(int ShotNumber)
    {
        for (int i = 0; i < _Icons.Length; i++)
        {
            if (ShotNumber == i + 1)
            {
                _Icons[i].color = _UsedColour;
                return;
            }
        }
    }
}
