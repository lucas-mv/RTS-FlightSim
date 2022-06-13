using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPlaneController : MonoBehaviour
{
    [SerializeField] float _enemyPlaneBaseSpeed;
    [SerializeField] int _numberOfEnemyPlanes;
    [SerializeField] EnemyPlane _enemyPlanePrefab;
    [SerializeField] Transform[] _sourceRoutingPoints;
    [SerializeField] Transform[] _targetRoutingPoints;
    [SerializeField] GameObject _enemyPlanesContainer;

    int _speedSeed = 0;
    List<EnemyPlane> _enemyPlanes = new List<EnemyPlane>();

    private void Start()
    {
        for(int i=0; i<_numberOfEnemyPlanes; i++)
        {
            EnemyPlane enemyPlane = Instantiate<EnemyPlane>(_enemyPlanePrefab);
            enemyPlane.transform.SetParent(_enemyPlanesContainer.transform);
            enemyPlane.SetupNewRouteCallback(BuildRoute);
            BuildRoute(enemyPlane);
            _enemyPlanes.Add(enemyPlane);
        }
    }

    public float CalculateClosestDistance(Vector3 referencePosition, float referenceDistance)
    {
        float smallestDistance = referenceDistance;
        foreach(var enemyPlane in _enemyPlanes)
        {
            float distance = Vector3.Distance(enemyPlane.gameObject.transform.position, referencePosition);
            if (distance < smallestDistance) smallestDistance = distance;
        }
        return smallestDistance;
    }

    private void BuildRoute(EnemyPlane enemyPlane)
    {
        Transform source = _sourceRoutingPoints[Random.Range(0, _sourceRoutingPoints.Length)];
        Transform target = _targetRoutingPoints[Random.Range(0, _targetRoutingPoints.Length)];
        enemyPlane.SetupRoute(source, target, _enemyPlaneBaseSpeed, _speedSeed);
        _speedSeed++;
    }
}
