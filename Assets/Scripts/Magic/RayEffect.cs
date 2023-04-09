using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(LevitationMagic))]
public class RayEffect : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private LevitationMagic levitationMagic;

    void Start()
    {
        levitationMagic = GetComponent<LevitationMagic>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levitationMagic.Grabbed)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPositions(new Vector3[] { transform.position, levitationMagic.Grabbed.transform.position });
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}
