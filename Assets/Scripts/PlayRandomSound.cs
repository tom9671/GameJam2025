using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayRandomSound : MonoBehaviour
{
    public float frequency = 15;
    public float randomness = 3;

    public AudioClip[] allClips;

    AudioSource source;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine("PlayClips");
    }

    IEnumerator PlayClips()
    {
        while (true)
        {
            yield return new WaitForSeconds(frequency + Random.Range(-randomness, randomness));
            source.PlayOneShot(allClips[Random.Range(0, allClips.Length)]);
        }
    }
}
