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

    private Rigidbody _rigidbody;
    private bool _moving = false;
    private Vector3[] _corners;        
    private int _index;
    private Vector3 _destination;

    private float _stoppingDistanceSquared { get { return _stoppingDistance * _stoppingDistance; } }

    public float StoppingDistance { get { return _stoppingDistance; } }
    public bool Moving { get { return _moving;} }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StopMoving() 
    {
        _moving = false;
    }

    public void TravelPath(Vector3 destination)
    {
        _destination = destination;
 
        GetCorners();

        // Set to 1 if first corner is starting position, 0 if not. 
        _index = 1;
        _moving = true;
    }

    private void GetCorners()
    {
        NavMeshPath path = new();
        // How to get this to use NavMeshObstacles in the way and calculate path around them? 
        NavMesh.CalculatePath(transform.position, _destination, NavMesh.AllAreas, path);
        _corners = path.corners;
    }
    
    private void FixedUpdate()
    {
        if (_moving)
        {
            if (_index < _corners.Length)
            {
/*                Debug.Log($"Position: {_rigidbody.position}, Next corner: {_corners[_index]}," +
                    $" Distance: {(_corners[_index] - _rigidbody.position).sqrMagnitude}, Stopping distance: {_stoppingDistanceSquared}");*/
                if ((_corners[_index] - _rigidbody.position).sqrMagnitude > _stoppingDistanceSquared)
                {
                    // Recalculate path when blocked. 
                    // Raycast from character to next waypoint. If it hits something, recalculate path and return out of this fixed update. 
                    RaycastHit[] hits = Physics.RaycastAll(
                        new Vector3(_rigidbody.position.x, 1f, _rigidbody.position.z),
                        _corners[_index] - _rigidbody.position);
                    if (hits.Length > 0)
                    {
                        TravelPath(_destination);
                        return;
                    }
                    
                    // Move rigidbody toward next corner. 
                    _rigidbody.MovePosition(_rigidbody.position + ((_corners[_index] - _rigidbody.position).normalized * _speed * Time.fixedDeltaTime));

                    // Rotate to look forward. 
                    Quaternion rotation = Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(_corners[_index] - _rigidbody.position), _turnSpeed * Time.fixedDeltaTime);

                    _rigidbody.rotation = rotation;
//                    _rigidbody.MoveRotation(rotation);
                }
                else
                {
                    _index++;
                }
            }
            else
            {
                _moving = false;
            }            
        }
    }
}