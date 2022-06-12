using UnityEngine;
using System.Collections;

public class EnemyPlane : MonoBehaviour
{
    [SerializeField] GameObject[] _planes;

    System.Action<EnemyPlane> _requestNewRoute;
    Transform _target;
    float _speed;

    void Start()
    {
        _planes[Random.Range(0, _planes.Length)].SetActive(true);
    }

    void Update()
    {
        // Move our position a step closer to the target.
        var step = _speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
        transform.rotation = Quaternion.LookRotation(_target.position.normalized);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, _target.position) < 10f)
        {
            _requestNewRoute.Invoke(this);
        }
    }

    public void SetupNewRouteCallback(System.Action<EnemyPlane> newRouteCallback)
    {
        _requestNewRoute = newRouteCallback;
    }

    public void SetupRoute(Transform source, Transform target, float speed, int speedSeed)
    {
        var random = new System.Random(speedSeed);
        _speed = (1 + (float)random.NextDouble()) * _speed;

        transform.position = source.position;
        _target = target;
        _speed = speed;
    }
}
