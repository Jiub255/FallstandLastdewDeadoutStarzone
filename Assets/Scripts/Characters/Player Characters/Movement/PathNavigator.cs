using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Uses the NavMeshPath from NavMesh.CalculatePath() (get from _stateMachine.GetPath()), and moves the 
// rigidbody along the path instead of using NavMeshAgent. 
public class PathNavigator : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _stoppingDistance = 0.5f;
    [SerializeField]
    private float _turnSpeed = 180f;
    [SerializeField]
    private LayerMask _obstaclesLayerMask;

    private Rigidbody _rigidbody;
    private bool _moving = false;
    private Vector3[] _corners;        
    private int _index;
    private Vector3 _destination;
    private Collider _destinationCollider;
    private Transform _destinationTransform;
    private Vector3 _lastPositionChecked;
//    private AIPath _aiPath;
    private NavMeshObstacle _navMeshObstacle;
    private Seeker _seeker;

    private float _stoppingDistanceSquared { get { return _stoppingDistance * _stoppingDistance; } }

    public float StoppingDistance { get { return _stoppingDistance; } }
    public bool Moving { get { return _moving;} }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
//        _aiPath = GetComponent<AIPath>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _seeker = GetComponent<Seeker>();
    }

    public void StopMoving() 
    {
        _moving = false;
    }

    public void TravelPath(Vector3 destination, Collider destinationCollider)
    {
        _destination = destination;
        _destinationCollider = destinationCollider;

        if (destinationCollider != null)
        {
            _destinationTransform = destinationCollider.transform;
            _lastPositionChecked = _destinationTransform.position;
        }
        else
        {
            _destinationTransform = null;
            _lastPositionChecked = Vector3.zero;
        }

//        Debug.Log("TravelPath called. ");
        _seeker.StartPath(_rigidbody.position, _destination, OnPathComplete);
/*        ABPath path = ABPath.Construct(_rigidbody.position, _destination, OnPathComplete);
        AstarPath.StartPath(path);*/
//        _aiPath.destination = _destination;

//        StartCoroutine(TemporarilyDisableObstacle());

//        GetCorners();
    }

    public void OnPathComplete(Path p)
    {
        // We got our path back
        if (p.error)
        {
            // Nooo, a valid path couldn't be found
            Debug.LogWarning("No valid path found. ");
        }
        else
        {
            // Yay, now we can get a Vector3 representation of the path
            // from p.vectorPath
            _corners = p.vectorPath.ToArray();
//            Debug.Log($"{gameObject.name}'s corner count: {_corners.Length}");
/*            foreach (Vector3 corner in _corners)
            {
                Debug.Log(corner);
            }*/

            // Set to 1 if first corner is starting position, 0 if not. 
            _index = 1;
            _moving = true;
        }
    }

    private void GetCorners()
    {

        NavMeshPath path = new();
        // How to get this to use NavMeshObstacles in the way and calculate path around them? 
        NavMesh.CalculatePath(transform.position, _destination, NavMesh.AllAreas, path);
        _corners = path.corners;

        // Is it okay to turn back on here? Or should I wait a frame?
    }

    private IEnumerator TemporarilyDisableObstacle()
    {
        // Disable navmeshobstacle while calculating path, otherwise it blocks itself and thinks it can't move. 
        _navMeshObstacle.enabled = false;

        yield return null;

        GetCorners();

        yield return null;

        _navMeshObstacle.enabled = true;

        // Set to 1 if first corner is starting position, 0 if not. 
        _index = 1;
        _moving = true;
    }

    private void FixedUpdate()
    {
        if (_moving)
        {
            // If character hasn't reached the last corner yet, 
            if (_index < _corners.Length)
            {
                // If character hasn't reached the next corner yet, 
/*                Debug.Log($"Position: {_rigidbody.position}, Next corner: {_corners[_index]}," +
                    $" Distance: {(_corners[_index] - _rigidbody.position).sqrMagnitude}, Stopping distance: {_stoppingDistanceSquared}");*/
                if ((_corners[_index] - _rigidbody.position).sqrMagnitude > _stoppingDistanceSquared)
                {
                    // Recalculate path when blocked. 

                    // Raycast from character to next waypoint. If it hits something, recalculate path and return out of this fixed update. 
                    // TODO - Do boxcast instead? Raycasts could easily miss something that is in the way. 
                    Vector3 directionVector = _corners[_index] - _rigidbody.position;
                    Debug.DrawRay(
                        new Vector3(_rigidbody.position.x, 1f, _rigidbody.position.z),
                        new Vector3(directionVector.x, 0f, directionVector.z),
                        Color.red,
                        0.5f);
                    RaycastHit[] hits = Physics.RaycastAll(
                        new Vector3(_rigidbody.position.x, 1f, _rigidbody.position.z),
                        new Vector3(directionVector.x, 0f, directionVector.z),
                        Vector3.Distance(_rigidbody.position, _corners[_index]),
                        _obstaclesLayerMask); ;
                    if (hits.Length > 0)
                    {
                        Debug.Log($"Hits length: {hits.Length}");
                        Debug.Log($"Destination collider name: {_destinationCollider.name}");
                        // If raycast only hits destination colllider, don't do anything. 
                        // Is comparing colliders working? Should I compare something else? InstanceID? Transform? GameObject? 
                        if (hits.Length == 1 && hits[0].collider == _destinationCollider)
                        {
                            return;
                        }

                        foreach (RaycastHit hit in hits)
                        {
                            Debug.Log($"Hit: {hit.transform.name}");
                        }
                        TravelPath(_destination, _destinationCollider);
                        return;
                    }

                    // Recalculate path if target has moved too much.
                    if (_destinationTransform != null)
                    {
                        if (((_destinationTransform.position - _lastPositionChecked).sqrMagnitude > 1f) ||
                            // or target is within y units of you, 
                            ((_destinationTransform.position - _rigidbody.position).sqrMagnitude < 1f))
                        {
                            _lastPositionChecked = _destinationTransform.position;
                            TravelPath(_destinationTransform.position, _destinationCollider);
                            //            _navMeshAgent.SetDestination(_target.position);
                            return;
                        }
                    }

                    // Move rigidbody toward next corner. 
                    _rigidbody.MovePosition(_rigidbody.position + ((_corners[_index] - _rigidbody.position).normalized * _speed * Time.fixedDeltaTime));

                    // Rotate to look forward. 
                    Quaternion rotation = Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(_corners[_index] - _rigidbody.position), _turnSpeed * Time.fixedDeltaTime);

                    _rigidbody.rotation = rotation;
//                    _rigidbody.MoveRotation(rotation);
                }
                // Else character HAS reached the next corner, so increment the corner index. 
                else
                {
                    _index++;
                }
            }
            // Else character HAS reached the last corner, so stop moving. 
            else
            {
                _moving = false;
            }
        }
    }
}