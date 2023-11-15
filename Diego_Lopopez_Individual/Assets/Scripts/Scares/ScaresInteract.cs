using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ScaresInteract : MonoBehaviour
{
    public GameObject cube;
    public Camera cam;
    public MeshRenderer mesh;
    Plane[] cameraFrustum;
    public Collider col;
    public AudioSource audioSource;
    public float fadeDuration;
    private float startVol;

    private bool insideBox;
    private bool used;

    private void Start()
    {
        startVol = audioSource.volume;
        cube.SetActive(false);
        used = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        cam = other.GetComponentInChildren<Camera>();
        if (cam)
        {
            cube.SetActive(true);
            audioSource.Play(0);
            insideBox = true;
            audioSource.volume = startVol;
            used = true;
        }
        else
        {
            insideBox = false;
        }
    }


    private void Update()
    {
        if (insideBox)
        {
            var bounds = col.bounds;
            cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
            {
                mesh.material.color = Color.green;
                StartCoroutine(FadeAudio());
            }
            else
            {
                mesh.material.color = Color.red;
                

            }
        }
    }

    public IEnumerator FadeAudio()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, 0, time / fadeDuration);
            yield return null;
        }
        yield break;
    }



}
