using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DynamicDOF : MonoBehaviour
{
    public bool isEnabled;
    private DepthOfField testDoF;
    public Volume volume;
    public float hitDist;
    public float maxDist = 100f;
    public Transform cam;
    public float focusSpeed;

    void Start()
    {
        volume.profile.TryGet<DepthOfField>(out testDoF);
        testDoF.farFocusEnd.value = 10f;
    }

    private void Update()
    {
        if (!isEnabled)
        {
            volume.gameObject.SetActive(false);
        }

        if (!isEnabled) return;
        volume.gameObject.SetActive(true);

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDist))
        {
            hitDist = Vector3.Distance(transform.position, hit.point);
            testDoF.farFocusStart.value = Mathf.Lerp(testDoF.farFocusStart.value, hitDist, Time.deltaTime * focusSpeed);

            if(hitDist <= 10)
            {
                testDoF.farFocusEnd.value = hitDist * 4f;
                testDoF.nearFocusStart.value = hitDist * 2f;
                testDoF.nearFocusEnd.value = hitDist / 5f;
            }
            else
            {
                testDoF.farFocusEnd.value = hitDist * 4f;
                testDoF.nearFocusStart.value = hitDist / 15f;
                testDoF.nearFocusEnd.value = hitDist / 7f;
            }
        }
    }
}
