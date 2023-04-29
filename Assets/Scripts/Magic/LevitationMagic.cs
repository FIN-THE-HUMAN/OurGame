using UnityEngine;
using UnityEngine.Events;

public class LevitationMagic : MonoBehaviour
{
<<<<<<< Updated upstream
=======
    [SerializeField] private KeyCode GrabButton = KeyCode.E;
    [SerializeField] private KeyCode ThrowButton = KeyCode.Mouse1;
>>>>>>> Stashed changes
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxGrabDistance;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _lerpSpeed;
<<<<<<< Updated upstream
    [SerializeField] private Transform _objectHolder;
=======
    [SerializeField] private Transform _objectHolderPoint;
    [SerializeField] private Transform _rayStartPoint;
>>>>>>> Stashed changes

    private Rigidbody _grabbed;
    private Transform _root;

    public Rigidbody Grabbed => _grabbed;
<<<<<<< Updated upstream
=======
    public Transform RayStartPoint => _rayStartPoint;
>>>>>>> Stashed changes
    public UnityEvent<Rigidbody> OnGrabbed;
    public UnityEvent<Rigidbody> OnDrop;
    public UnityEvent<Rigidbody> OnThrow;

    public void PowerForce(float forceModifier)
    {
        _throwForce *= forceModifier;
    }

    void Update()
    {
        if (_grabbed)
        {
            ExplosiveCharged explosive;
            if (_grabbed.TryGetComponent(out explosive)) explosive.Charged = false;

<<<<<<< Updated upstream
            _grabbed.MovePosition(Vector3.Lerp(_grabbed.position, _objectHolder.transform.position, Time.deltaTime * _lerpSpeed));
=======
            _grabbed.MovePosition(Vector3.Lerp(_grabbed.position, _objectHolderPoint.transform.position, Time.deltaTime * _lerpSpeed));
>>>>>>> Stashed changes
            _grabbed.rotation = transform.rotation;

            //_root.position = Vector3.Lerp(_grabbed.position, _objectHolder.transform.position, Time.deltaTime * _lerpSpeed);

<<<<<<< Updated upstream
            if (Input.GetMouseButtonDown(1))
            {
                _grabbed.isKinematic = false;
                _grabbed.AddForce(_camera.transform.forward * _throwForce, ForceMode.VelocityChange);
                OnThrow.Invoke(_grabbed);

                if (_grabbed.TryGetComponent(out explosive)) explosive.Charged = true;

                _grabbed = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_grabbed)
            {
                _grabbed.isKinematic = false;
                OnDrop.Invoke(_grabbed);
                _grabbed = null;
=======
            if (Input.GetKeyDown(ThrowButton))
            {
                Throw(explosive);
            }
        }

        if (Input.GetKeyDown(GrabButton))
        {
            if (_grabbed)
            {
                Drop();
>>>>>>> Stashed changes
            }
            else
            {
                RaycastHit hit;
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                if (Physics.Raycast(ray, out hit, _maxGrabDistance))
                {
                    _grabbed = hit.collider.GetComponent<Rigidbody>();
                    if (_grabbed)
                    {
                        _root = hit.collider.transform.root;

                        _grabbed.isKinematic = true;
                        OnGrabbed.Invoke(_grabbed);
                    }
                }
            }
        }
<<<<<<< Updated upstream
=======

    }

    private void Drop()
    {
        _grabbed.isKinematic = false;
        OnDrop.Invoke(_grabbed);
        _grabbed = null;
    }

    private void Throw(ExplosiveCharged explosive)
    {
        _grabbed.isKinematic = false;
        _grabbed.AddForce(_camera.transform.forward * _throwForce, ForceMode.VelocityChange);
        OnThrow.Invoke(_grabbed);

        if (_grabbed.TryGetComponent(out explosive)) explosive.Charged = true;

        _grabbed = null;
    }

    private void OnDisable()
    {
        Drop();
>>>>>>> Stashed changes
    }
}
