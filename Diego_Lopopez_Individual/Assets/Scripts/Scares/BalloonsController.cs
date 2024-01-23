using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsController : MonoBehaviour
{

    public GameObject[] balloons;
    public BoxCollider trigger;

    public AudioClip clip;

    public float timeToExplodeMin;
    public float timeToExplodeMax;

    private void Start()
    {
        for (int i = 0; i < balloons.Length; i++)
        {
            balloons[i].gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trigger.enabled = false;
            for (int i = 0; i < balloons.Length; i++)
            {
                float time = Random.Range(timeToExplodeMin, timeToExplodeMax);
                StartCoroutine(ExplodeIE(i, time));
            }
        }
    }

    IEnumerator ExplodeIE(int number, float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("aaaaa");
        AudioSource audio = balloons[number].GetComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
        balloons[number].gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
