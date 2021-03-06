using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
	[SerializeField] private List<RawImage> fadeOutUIImages;
	[SerializeField] private float fadeSpeed = 0.8f;

	public enum FadeDirection
	{
		In,
		Out
	}

	void OnEnable()
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
			foreach(RawImage fadeOutUIImage in fadeOutUIImages)
            {
				fadeOutUIImage.enabled = false;
            }
		}
		else
		{
			foreach (RawImage fadeOutUIImage in fadeOutUIImages)
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

	public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection, string sceneToLoad)
	{
		yield return Fade(fadeDirection);
		SceneManager.LoadScene(sceneToLoad);
	}

	private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
	{
		foreach (RawImage fadeOutUIImage in fadeOutUIImages)
		{
			fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
		}
		alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
	}
}
