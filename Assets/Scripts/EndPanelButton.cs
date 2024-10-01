using System.Collections;
using UnityEngine;

public class EndPanelButton : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("soundVolume");
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.5f);
        while (transform.localScale.x < 0.75f)
        {
            yield return null;
        }
        audioSource.Play();
    }
}
