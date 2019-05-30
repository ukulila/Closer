using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trail_Behaviour : MonoBehaviour
{
    public bool isTrailActivated = false;
    public bool isTrailStillVisible = true;
    public TrailRenderer trailRenderer;


    public static trail_Behaviour Instance;


    private void Awake()
    {
        Instance = this;

        if (trailRenderer == null)
            trailRenderer = gameObject.GetComponent<TrailRenderer>();
    }

    public void ActivateTrail()
    {
        if (!isTrailActivated && isTrailStillVisible)
        {
            isTrailActivated = true;
            trailRenderer.time = 1.65f;
            trailRenderer.enabled = true;
        }
    }

    public void DeactivateTrail()
    {
        if (isTrailActivated)
        {
            isTrailActivated = false;
            trailRenderer.time = 0;
            trailRenderer.enabled = false;
        }
    }
}
