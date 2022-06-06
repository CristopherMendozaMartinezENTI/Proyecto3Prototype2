using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudMechanicsManager : MonoBehaviour
{
    public enum FadeDirection
    {
        In,
        Out
    }

    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private GameObject titleLine;
    [SerializeField] private GameObject stairs;
    [SerializeField] private GameObject portals;
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject pad;
    [SerializeField] private GameObject _switch;
    [SerializeField] private GameObject amff;
    [SerializeField] private GameObject moving;

    private float fadeSpeed = 2f;
    private List<RawImage> imagesToFade;

    public string[] stringArray;

    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnWords;

    int counter = 0;

    private void Start()
    {
        imagesToFade = new List<RawImage>();
        imagesToFade.Add(titleLine.GetComponent<RawImage>());
        imagesToFade.Add(stairs.GetComponent<RawImage>());
        imagesToFade.Add(portals.GetComponent<RawImage>());
        imagesToFade.Add(cube.GetComponent<RawImage>());
        imagesToFade.Add(pad.GetComponent<RawImage>());
        imagesToFade.Add(_switch.GetComponent<RawImage>());
        imagesToFade.Add(amff.GetComponent<RawImage>());
        imagesToFade.Add(moving.GetComponent<RawImage>());
    }

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

        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        Show();
        yield return new WaitForSeconds(5);
        Hide();
    }

    void Show()
    {
       StartCoroutine(Fade(FadeDirection.In));
    }

    void Hide()
    {
       StartCoroutine(Fade(FadeDirection.Out));
    }

    private IEnumerator Fade(FadeDirection fadeDirection)
    {
        float alpha = (fadeDirection == FadeDirection.Out) ? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.Out) ? 0 : 1;
        if (fadeDirection == FadeDirection.Out)
        {
            while (alpha >= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
            foreach (RawImage fadeOutUIImage in imagesToFade)
            {
                fadeOutUIImage.enabled = false;
                roomName.text = " ";
            }
        }
        else
        {
            foreach (RawImage fadeOutUIImage in imagesToFade)
            {
                fadeOutUIImage.enabled = true;
            }
            while (alpha <= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
        }
    }

    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        foreach (RawImage fadeOutUIImage in imagesToFade)
        {
            fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
        }
        alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
    }
}
