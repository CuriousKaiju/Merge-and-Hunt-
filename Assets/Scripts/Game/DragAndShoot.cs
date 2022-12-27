using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DragAndShoot : MonoBehaviour
{
    [SerializeField] private AnimalMovement _animalMovement;
    [SerializeField] private DrawTrajectory _drawTrajectory;
    [SerializeField] private Vector3 _lineOffset;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private Transform _rangePoint;
    [SerializeField] private Transform _lineRoot;
    [SerializeField] private float _divideCoeficient;
    [SerializeField] private float _YforceMultiplier;

    [SerializeField] private Transform _predatorPivot;
    [SerializeField] private float _predatorPivotRotationSpeed;

    [SerializeField] private Predator _predator;
    [SerializeField] private Collider _collider;

    [SerializeField] private GameObject _landingPoint;

    public GameObject _jumpParticles;

    private Vector3 _dividedResult;

    private Vector3 _globalForceVector;

    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    private Vector3 secondPoint;

    private Rigidbody rb;

    private bool isShoot;

    public bool _canControl;

    private bool _fingerOnTheScreen;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_canControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePressDownPos = Input.mousePosition;
                _rangePoint.position = new Vector3(0, 0, 0);
                _fingerOnTheScreen = true;
            }

            if (_fingerOnTheScreen)
            {
                mouseReleasePos = Input.mousePosition; //origin
                var force = mousePressDownPos - mouseReleasePos;
                _dividedResult = force;

                if (_dividedResult.magnitude > 90)
                {
                    _landingPoint.SetActive(true);
                    _rangePoint.localPosition = new Vector3(_dividedResult.x, 0, _dividedResult.y);
                    var vectorOfForce = transform.position - new Vector3(_rangePoint.position.x, 0, _rangePoint.position.z);
                    //vectorOfForce = new Vector3(-vectorOfForce.x, Mathf.Abs(vectorOfForce.x), -vectorOfForce.z);
                    vectorOfForce = new Vector3(-vectorOfForce.x, Mathf.Abs(force.y), -vectorOfForce.z);
                    _globalForceVector = vectorOfForce * forceMultiplier;
                    _drawTrajectory.UpdateTrajectory(_globalForceVector, rb, _lineRoot.position);

                    if (Input.GetMouseButtonUp(0))
                    {
                        _predator._readyForAttack = true;
                        _predator.SetJumpAnim();
                        var vectorOfForceButtonUp = _globalForceVector;
                        Shoot(vectorOfForceButtonUp);
                        _drawTrajectory.SetLineStatus(false);
                        _predatorPivot.DOLookAt(_rangePoint.position, _predatorPivotRotationSpeed);
                        _animalMovement.SetInterectibleStatus(false);
                        _landingPoint.transform.parent = null;
                    }
                }
                else
                {

                }
        
            }
        }
    }

    public void OffLandingPoint()
    {
        Destroy(_landingPoint);
    }

    /*
    private void OnMouseDown()
    {
        if(_canControl)
        {
            mousePressDownPos = Input.mousePosition; 
            _rangePoint.position = new Vector3(0, 0, 0);
        }

    }
    private void OnMouseDrag()
    {
        if (_canControl)
        {
            mouseReleasePos = Input.mousePosition; //origin

            var force = mousePressDownPos - mouseReleasePos;
            _dividedResult = force;
            _rangePoint.localPosition = new Vector3(_dividedResult.x, 0, _dividedResult.y);
            var vectorOfForce = transform.position - new Vector3(_rangePoint.position.x, 0, _rangePoint.position.z);
            vectorOfForce = new Vector3(-vectorOfForce.x, Mathf.Abs(vectorOfForce.x), -vectorOfForce.z);
            _globalForceVector = vectorOfForce * forceMultiplier;
            _drawTrajectory.UpdateTrajectory(_globalForceVector, rb, _lineRoot.position);
        }

    }
    private void OnMouseUp()
    {
        if (_canControl)
        {
            _predator._readyForAttack = true;
            _predator.SetJumpAnim();
            var vectorOfForce = _globalForceVector;
            Shoot(vectorOfForce);
            _drawTrajectory.SetLineStatus(false);
            _predatorPivot.DOLookAt(_rangePoint.position, _predatorPivotRotationSpeed);
            _animalMovement.SetInterectibleStatus(false);
        }
    }
    */

    public float forceMultiplier = 3;

    void Shoot(Vector3 Force)
    {
        if (isShoot)
            return;

        if (_animalMovement)
        {
            _animalMovement.enabled = false;
            rb.isKinematic = false;
        }
        rb.AddForce(new Vector3(Force.x, Mathf.Abs(Force.y), Force.z));
        isShoot = true;
        _collider.enabled = true;
    }

}