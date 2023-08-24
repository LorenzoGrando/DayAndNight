using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    #region Component References

    private BoxCollider2D _collider;

    #endregion

    #region Raycast Information

    private RaycastOrigins _raycastOrigins;
    public CollisionInfo Collisions;
    public LayerMask CollisionMask;
    public Color DefaultRaycastColor;

    [SerializeField]
    [Range(2,100)]
    private int numHorizontalRays = 2;
    [SerializeField]
    [Range(2,100)]
    private int numVerticalRays = 2;
    private float _horizontalRaySpacing;
    private float _verticalRaySpacing;

    private const float skinWidth = 0.015f;
    private const float _maxSlopeAngle = 80;
    private const float _maxDescendSlopeAngle = 75;
    #endregion


    #region Unity Methods

    private void Start()
    {
        if(_collider == null) {
            _collider = GetComponent<BoxCollider2D>();
        }
        CalculateRaySpacing();
    }

    #endregion

    #region Core Methods
    private void UpdateRaycastOrigins() {
        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidth *- 2);

        _raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySpacing() {
        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidth *- 2);

        _horizontalRaySpacing = bounds.size.y/(numHorizontalRays - 1);
        _verticalRaySpacing = bounds.size.x/(numVerticalRays - 1);
    }

    public void Move(Vector2 velocity) {
        UpdateRaycastOrigins();
        Collisions.Reset();
        Collisions.oldVelocity = velocity;
        
        if(velocity.y < 0) {
            DescendSlope(ref velocity);
        }
        if(velocity.x != 0) {
            HorizontalCollisions(ref velocity);
        }
        if(velocity.y != 0) {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    private void VerticalCollisions(ref Vector2 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLenght = Mathf.Abs(velocity.y) + skinWidth;
        for(int i = 0; i < numVerticalRays; i++) {
            Vector2 rayOrigin = (directionY == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (_verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, CollisionMask);

            if(hit && !hit.collider.isTrigger) {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLenght = hit.distance;

                if(Collisions.climbingSlope) {
                    velocity.x = velocity.y / Mathf.Tan(Collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                Collisions.below  = directionY == -1;
                Collisions.above = directionY == 1;
                if(Time.time > Collisions.lastTimeOnGround && Collisions.below) {
                    Collisions.lastTimeOnGround = Time.time;
                }
            }
            Vector2 offset = directionY * rayLenght * Vector2.up;
            Debug.DrawRay(rayOrigin + offset, offset, DefaultRaycastColor);
        }

        if(Collisions.climbingSlope) {
            float directionX = Mathf.Sign(velocity.x);
            rayLenght = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, CollisionMask);

            if(hit && !hit.collider.isTrigger) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != Collisions.slopeAngle) {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    Collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    private void HorizontalCollisions(ref Vector2 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float rayLenght = Mathf.Abs(velocity.x) + skinWidth;
        for(int i = 0; i < numHorizontalRays; i++) {
            Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (_horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, CollisionMask);

            if(hit && !hit.collider.isTrigger) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if(i == 0 && slopeAngle <= _maxSlopeAngle) {
                    if(Collisions.descendingSlope) {
                        Collisions.descendingSlope = false;
                        velocity = Collisions.oldVelocity;
                    }
                    float distanceToSlope = 0;
                    if(slopeAngle != Collisions.slopeAnglePrevious) {
                        distanceToSlope = hit.distance - skinWidth;
                        velocity.x -= distanceToSlope * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlope * directionX;
                }
                if(!Collisions.climbingSlope || slopeAngle > _maxSlopeAngle) {
                    velocity.x = Mathf.Min(Mathf.Abs(velocity.x), hit.distance - skinWidth) * directionX;
                    rayLenght = Mathf.Min(Mathf.Abs(velocity.x) + skinWidth, hit.distance);

                    if(Collisions.climbingSlope) {
                        velocity.y = Mathf.Tan(Collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    Collisions.left  = directionX == -1;
                    Collisions.right = directionX == 1;
                }
            }
            Vector2 offset = directionX * rayLenght * Vector2.right;
            Debug.DrawRay(rayOrigin + offset, offset, DefaultRaycastColor);
        }
    }

    private void ClimbSlope(ref Vector2 velocity, float slopeAngle) {
        float distance = Mathf.Abs(velocity.x);
        float yClimbVelocity = distance * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);

        if(velocity.y <= yClimbVelocity) {
            velocity.y = yClimbVelocity;
            velocity.x = distance * Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);

            Collisions.slopeAngle = slopeAngle;
            Collisions.below = true;
            Collisions.climbingSlope = true;
            if(Time.time > Collisions.lastTimeOnGround) {
                    Collisions.lastTimeOnGround = Time.time;
            }
        }
        
    }

    private void DescendSlope(ref Vector2 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, CollisionMask);

        if(hit && !hit.collider.isTrigger) {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= _maxDescendSlopeAngle) {
                if(Mathf.Sign(hit.normal.x) == directionX) {
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) {
                        float distance = Mathf.Abs(velocity.x);
                        float ydescendVelocity = distance * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
                        velocity.x = distance * Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                        velocity.y -= ydescendVelocity;

                        Collisions.slopeAngle = slopeAngle;
                        Collisions.descendingSlope = true;
                        Collisions.below = true;
                        if(Time.time > Collisions.lastTimeOnGround) {
                            Collisions.lastTimeOnGround = Time.time;
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Data Structs

    struct RaycastOrigins {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above, below, left, right;

        public bool climbingSlope, descendingSlope;
        public float slopeAngle, slopeAnglePrevious;
        public Vector2 oldVelocity;
        public float lastTimeOnGround;

        public void Reset() {
            above = below = left = right = false;
            slopeAnglePrevious = slopeAngle;
            climbingSlope = descendingSlope = false;
        }
    }

    #endregion
}
