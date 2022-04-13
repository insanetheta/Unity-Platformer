using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
	public class Projectile : KinematicObject
	{
		public float MaxSpeed = 2f;

		protected Vector2 _initialVelocity;

		public void Initialize(Vector2 initialVelocity)
		{
			_initialVelocity = initialVelocity;
			gravityModifier = 0f;
			ComputeVelocity();
		}

		protected override void ComputeVelocity()
		{
			targetVelocity = _initialVelocity * MaxSpeed;
		}

		protected override void FixedUpdate()
		{
			//if already falling, fall faster than the jump speed, otherwise use normal gravity.
			velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;


			velocity.x = targetVelocity.x;

			IsGrounded = false;

			var deltaPosition = velocity * Time.deltaTime;

            var move = deltaPosition;

			PerformMovement(move, false);

			move = Vector2.up * deltaPosition.y;

			PerformMovement(move, true);

		}

        void PerformMovement(Vector2 move, bool yMovement)
        {
            var distance = move.magnitude;

            if (distance > minMoveDistance)
            {
                //check if we hit anything in current direction of travel
                var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                for (var i = 0; i < count; i++)
                {
                    var currentNormal = hitBuffer[i].normal;

                    //is this surface flat enough to land on?
                    if (currentNormal.y > minGroundNormalY)
                    {
                        IsGrounded = true;
                        // if moving up, change the groundNormal to new surface normal.
                        if (yMovement)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    if (IsGrounded)
                    {
                        //how much of our velocity aligns with surface normal?
                        var projection = Vector2.Dot(velocity, currentNormal);
                        if (projection < 0)
                        {
                            //slower velocity if moving against the normal (up a hill).
                            velocity = velocity - projection * currentNormal;
                        }
                    }
                    else
                    {
                        //We are airborne, but hit something, so cancel vertical up and horizontal velocity.
                        velocity.x *= 0;
                        velocity.y = Mathf.Min(velocity.y, 0);
                    }
                    //remove shellDistance from actual move distance.
                    var modifiedDistance = hitBuffer[i].distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }

				if (IsGrounded)
				{

				}
            }
            body.position = body.position + move.normalized * distance;
        }
    }
}
