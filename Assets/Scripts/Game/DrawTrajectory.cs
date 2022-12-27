using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _landingPoint;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField]
    [Range(3, 300)]
    private int _lineSegmentCount = 20;
    private Vector3 _lastHitPosition;

    private List<Vector3> _linePoints = new List<Vector3>();

    void Start()
    {
        
    }

    void Update()
    {

    }
    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidBody, Vector3 startingPoint)
    {
        Vector3 velocity = (forceVector / rigidBody.mass) * Time.fixedDeltaTime;

        float FlightDuration = (2 * velocity.y) / Physics.gravity.y;

        float stepTime = FlightDuration / _lineSegmentCount;

        _linePoints.Clear();
        _linePoints.Add(startingPoint);

        for (int i = 1; i <= _lineSegmentCount; i++)
        {
            float stepTimePassed = stepTime * i;

            Vector3 MovementVector = new Vector3(
                        velocity.x * stepTimePassed,
                        velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                        velocity.z * stepTimePassed
                                                );
            Vector3 newPointOfLine = -MovementVector + startingPoint;

            RaycastHit hit;

            _linePoints.Add(newPointOfLine);

            
            if (Physics.Raycast(_linePoints[i - 1], newPointOfLine - _linePoints[i - 1], out hit, (newPointOfLine - _linePoints[i - 1]).magnitude, _groundLayer))
            {
                _linePoints.Add(hit.point);
                break;
            }
            
        }
  
        _landingPoint.position = _linePoints[_linePoints.Count - 1] + new Vector3(0, 0.1f, 0);

        _lineRenderer.positionCount = _linePoints.Count;

        _lineRenderer.SetPositions(_linePoints.ToArray());
    }
    public void SetLineStatus(bool status)
    {
        _lineRenderer.gameObject.SetActive(status);
    }
       
}
