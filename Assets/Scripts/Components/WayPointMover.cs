using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[DisallowMultipleComponent]
[AddComponentMenu("TDCore/WayPointMover")]
public class WayPointMover : MonoBehaviour
{
    [SerializeField] 
    private float speed = 2f;
  
    private bool _moving = true;
    
    private Transform[] _waypoints;
    private int _currentWaypointIndx = 0;
   
    private Rigidbody2D _rigidbody;

    public void Awake()
    {
        _rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        //Очень просто потому не дальновидно - найдём все WayPoint на сцене, по его имени определим индекс
        var allWaypoints = GameObject.FindGameObjectsWithTag("WayPoint");
        _waypoints = new Transform[allWaypoints.Length];
        foreach (var waypoint in allWaypoints)
        {
            var index = int.Parse(waypoint.name.Split('_')[1]);
            _waypoints[index] = waypoint.transform;
        }
    }

    /// <summary>
    /// Обработка события смерти - пекращаем перемещение
    /// </summary>
    public void HaveHitPointIsDead()
    {
        _moving = false;
    }

    public void Update()
    {
        if (!_moving) return;
        if (_currentWaypointIndx < _waypoints.Length)
            MoveToNextWaypoint();
        else
            _moving = false;
    }

    public void MoveToNextWaypoint()
    {
        var nextWayPoint = new Vector3(_waypoints[_currentWaypointIndx].position.x, _waypoints[_currentWaypointIndx].position.y);
        var distance = nextWayPoint - transform.position;
        transform.position += distance.normalized * speed * Time.deltaTime;
        //Проверим, достигли ли текущего waypoint и если да, то перейдём к следующему
        if (distance.magnitude < 0.2f)
            _currentWaypointIndx++;
    }
}
