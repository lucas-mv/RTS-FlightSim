using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPlaneController : MonoBehaviour
{
    [SerializeField] float _enemyPlaneBaseSpeed;
    [SerializeField] int _numberOfEnemyPlanes;
    [SerializeField] EnemyPlane _enemyPlanePrefab;
    [SerializeField] Transform[] _routingPoints;
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
        Transform source = _routingPoints[Random.Range(0, _routingPoints.Length)];
        Transform[] targets = _routingPoints.Where(x => x != source).ToArray();
        Transform target = targets[Random.Range(0, targets.Length)];
        enemyPlane.SetupRoute(source, target, _enemyPlaneBaseSpeed, _speedSeed);
        _speedSeed++;
    }
}
