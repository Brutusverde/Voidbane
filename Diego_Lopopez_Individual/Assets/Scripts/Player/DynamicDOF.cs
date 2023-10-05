using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DynamicDOF : MonoBehaviour
{
    private DepthOfField testDoF;
    public Volume volume;
    public float hitDist;
    public float maxDist = 100f;
    public Transform cam;
    public float focusSpeed;

    void Start()
    {
        volume.profile.TryGet<DepthOfField>(out testDoF);

        // Near blur
        //testDoF.nearFocusStart.value = 0f;
        //testDoF.nearFocusEnd.value = 0f;

        //// Far blur
        //testDoF.farFocusStart.value = 10f;
        testDoF.farFocusEnd.value = 10f;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDist))
        {
            hitDist = Vector3.Distance(transform.position, hit.point);
            //Debug.Log("hit" + hitDist);
            //testDoF.nearFocusEnd.value = hitDist;
            //testDoF.farFocusEnd.value = hitDist;
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

            

            //testDoF.farFocusEnd.value = hitDist + 10f;
            //testDoF.nearFocusStart.value = hitDist - 25f;
            //testDoF.nearFocusEnd.value = hitDist - 15f;

            //testDoF.farFocusStart.value = hitDist;
            //testDoF.focusDistance.value = hitDist;
            //Debug.Log(testDoF.farFocusEnd.value);
        }
    }
}
