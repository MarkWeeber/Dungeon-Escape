using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected int gems = 2;
    [Header("Define waypoints parent")]
    [SerializeField] protected GameObject waypointsRoot;
    [Header("Or define waypoints manually")]
    [SerializeField] protected List<Waypoint> waypoints;
    [SerializeField] protected float minDistance = 0.45f;
    [Header("Enemy visionining")]
    [SerializeField] EnemyVision enemyVision;

    protected Animator animator;
    protected SpriteRenderer sprite;
    protected Transform target;

    private Waypoint _currentWaypoint = null;
    private float _horizontalDistanceToTarget;
    private float _waypointWaitTimer;
    private Vector3 _movement;
    private bool _attackingTarget;

    #region init
    private void Start()
    {
        Init();
        if (enemyVision != null)
        { 
            enemyVision.OnVisionEnter += OnTargetVisionEnter;
            enemyVision.OnVisionExit += OnTragetVisionExit;
        }
    }

    protected virtual void Init()
    {
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        if (waypointsRoot != null && waypoints != null)
        {
            waypoints = waypointsRoot.GetComponentsInChildren<Waypoint>().ToList();
        }

    }

    private void OnDestroy()
    {
        if (enemyVision != null)
        {
            enemyVision.OnVisionEnter -= OnTargetVisionEnter;
            enemyVision.OnVisionExit -= OnTragetVisionExit;
        }
    }
    #endregion

    #region loop
    private void Update()
    {
        ManageSpriteFlipping();
        ManageAnimator();
        AttackTarget();
        ManageCyclingWaypoints();
        Managemovement();
    }

    protected virtual void AttackTarget()
    {
        _attackingTarget = false;
        if (target == null) return;
        if(ReachTarget(target, minDistance)) // if target is reached, stay and perform attacks
        {
            _attackingTarget = true;
        }
    }

    protected virtual void ManageCyclingWaypoints()
    {
        if (target != null) return;
        if (waypoints == null) return;
        if (_currentWaypoint == null)
        {
            _currentWaypoint = waypoints.GetNextItem(_currentWaypoint);
            if (_currentWaypoint == null) return;
            _waypointWaitTimer = _currentWaypoint.WaitTime;
        }
        if(ReachTarget(_currentWaypoint.transform, minDistance)) // true if target reached, otherwise move to target
        {
            if (_waypointWaitTimer < 0f)
            {
                _currentWaypoint = waypoints.GetNextItem(_currentWaypoint);
                _waypointWaitTimer = _currentWaypoint.WaitTime;
            }
            else _waypointWaitTimer -= Time.deltaTime;
        }
    }

    // returns true if target reached,else returns false and sets movement vector
    private bool ReachTarget(Transform destination, float minimumDistance)
    {
        _movement = Vector3.zero;
        _horizontalDistanceToTarget = destination.position.x - transform.position.x;
        if (Mathf.Abs(_horizontalDistanceToTarget) > minimumDistance)
        {
            _movement.x = (_horizontalDistanceToTarget > 0f) ? speed : -speed;
            return false;
        }
        else return true;
    }

    protected virtual void Managemovement()
    {
        // apply movement
        transform.Translate(_movement * Time.deltaTime);
    }

    protected virtual void ManageAnimator()
    {
        if (Mathf.Abs(_movement.x) > 0.01f)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        animator.SetBool("Attacking", _attackingTarget);
    }

    protected virtual void ManageSpriteFlipping()
    {
        if (_horizontalDistanceToTarget > 0) // look right
        {
            sprite.flipX = false;
            enemyVision.transform.localEulerAngles
                    = new Vector3(
                        enemyVision.transform.localRotation.eulerAngles.x,
                        0f,
                        enemyVision.transform.localRotation.eulerAngles.z
                        );
        }
        else if (_horizontalDistanceToTarget < 0) // look left
        {
            sprite.flipX = true;
            enemyVision.transform.localEulerAngles
                    = new Vector3(
                        enemyVision.transform.localRotation.eulerAngles.x,
                        180f,
                        enemyVision.transform.localRotation.eulerAngles.z
                        );
        }
    }

    #endregion

    #region delegates
    private void OnTargetVisionEnter(Transform target)
    {
        this.target = target;
    }
    private void OnTragetVisionExit()
    {
        target = null;
    }
    #endregion
}
