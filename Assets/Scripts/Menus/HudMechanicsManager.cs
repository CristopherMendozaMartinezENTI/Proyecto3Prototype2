using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudMechanicsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private GameObject stairs;
    [SerializeField] private GameObject portals;
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject pad;
    [SerializeField] private GameObject _switch;
    [SerializeField] private GameObject amff;
    [SerializeField] private GameObject moving;

    public void SetActives(string _roomName, bool _stairs, bool _portals, bool _cube, bool _pad, bool __switch, bool _amff, bool _moving)
    {
        roomName.text = _roomName;
        stairs.SetActive(_stairs);
        portals.SetActive(_portals);
        cube.SetActive(_cube);
        pad.SetActive(_pad);
        _switch.SetActive(__switch);
        amff.SetActive(_amff);
        moving.SetActive(_moving);

        StartAnimation();
    }

    IEnumerator StartAnimation()
    {
        Show();
        //timer
        Hide();

        yield return null;
    }

    void Show()
    {
        
    }

    void Hide()
    {

    }

}
