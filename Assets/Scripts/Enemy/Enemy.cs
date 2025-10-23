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
    [SerializeField] protected float waypointMinDistance = 0.45f;
    [Header("Enemy visionining")]
    [SerializeField] EnemyVision enemyVision;
    [SerializeField] protected float targetAttackMinDistance = 0.45f;

    protected Animator animator;
    protected float waitingTimer = 0f;
    protected bool alive = true;

    #region private vars
    private SpriteRenderer sprite;
    private Transform target;
    private Waypoint _currentWaypoint = null;
    private float _horizontalDistanceToTarget;
    private float _waypointWaitTimer;
    private Vector3 _movement;
    private bool _attackingTarget;
    #endregion

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
        // if dead then perform no actions
        if (!alive) return;
        // if enemy is dormant by some time then skip other actions
        if (waitingTimer > 0)
        {
            waitingTimer -= Time.deltaTime;
            ManageSpriteFlipping();
            return;
        }
        ManageSpriteFlipping();
        ManageAttackingTarget();
        ManagePatroling();
        Managemovement();
        ManageAnimator();
    }

    private void ManageAttackingTarget()
    {
        _attackingTarget = false;
        if (target == null) return;
        if(ReachTarget(target, targetAttackMinDistance)) // if target is reached, stay and perform attacks
        {
            _attackingTarget = true;
        }
    }

    private void ManagePatroling()
    {
        if (target != null) return;
        if (waypoints == null) return;
        if (_currentWaypoint == null)
        {
            _currentWaypoint = waypoints.GetNextItem(_currentWaypoint);
            if (_currentWaypoint == null) return;
            _waypointWaitTimer = _currentWaypoint.WaitTime;
        }
        if(ReachTarget(_currentWaypoint.transform, waypointMinDistance)) // true if target reached, otherwise move to target
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

    private void Managemovement()
    {
        // apply movement
        transform.Translate(_movement * Time.deltaTime);
    }

    private void ManageAnimator()
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

    private void ManageSpriteFlipping()
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

    // if enemy was hit whilst not yet seen player - check behind
    protected void CheckBehindIfNotYetAlerted()
    {
        if (target == null)
        {
            _horizontalDistanceToTarget *= -1;
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
