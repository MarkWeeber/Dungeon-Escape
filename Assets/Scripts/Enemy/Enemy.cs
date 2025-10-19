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

    protected Animator animator;
    protected SpriteRenderer sprite;

    private Waypoint _currentWaypoint = null;
    private float _horizontalDistanceToWaypoint;
    private float _waypointWaitTimer;
    private Vector3 _movement;

    private void Start()
    {
        Init();
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

    private void Update()
    {
        ManageMovement();
        ManageSpriteFlipping();
        ManageAnimator();
    }

    protected virtual void ManageMovement()
    {
        if (waypoints == null) return;
        if (_currentWaypoint == null)
        {
            _currentWaypoint = waypoints.GetNextItem(_currentWaypoint);
            if (_currentWaypoint == null) return;
            _waypointWaitTimer = _currentWaypoint.WaitTime;
        }
        _movement = Vector3.zero;
        _horizontalDistanceToWaypoint = _currentWaypoint.transform.position.x - transform.position.x;
        // not yet reached target - horizontal moving
        if (Mathf.Abs(_horizontalDistanceToWaypoint) > minDistance)
        {
            _movement.x = (_horizontalDistanceToWaypoint > 0f) ? speed : -speed;
        }
        // target reached - waiting, idling
        else
        {
            if (_waypointWaitTimer < 0f)
            {
                _currentWaypoint = waypoints.GetNextItem(_currentWaypoint);
                _waypointWaitTimer = _currentWaypoint.WaitTime;
            }
            else _waypointWaitTimer -= Time.deltaTime;
        }
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
    }

    protected virtual void ManageSpriteFlipping()
    {
        if (_movement.x > 0)
        {
            sprite.flipX = false;
        }
        else if (_movement.x < 0)
        {
            sprite.flipX = true;
        }
    }

}
