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

<<<<<<< Updated upstream
    // Update is called once per frame
=======
>>>>>>> Stashed changes
    void Update()
    {
        if (levitationMagic.Grabbed)
        {
            _lineRenderer.enabled = true;
<<<<<<< Updated upstream
            _lineRenderer.SetPositions(new Vector3[] { transform.position, levitationMagic.Grabbed.transform.position });
=======
            _lineRenderer.SetPositions(new Vector3[] { levitationMagic.RayStartPoint.position, levitationMagic.Grabbed.transform.position });
>>>>>>> Stashed changes
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}
