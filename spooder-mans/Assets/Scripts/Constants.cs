using UnityEngine;
using System.Collections;

public static class Constants {

	public static float FALL_SPEED = 30f;

	public static Vector2 In2D(this Vector3 u)
	{
		return new Vector2( u.x, u.y );
	}

	public static bool IsWithin(this float f, float target, float epsilon)
	{
		return Mathf.Abs(f - target)< epsilon;
	}

	public static Vector2 GetFallVector(Vector2 currVel)
	{
		return new Vector2( currVel.x, currVel.y - FALL_SPEED );
	}

	// Returns a new vector stepped towards the new target vector by a magnitude scaled to time
	public static Vector2 GetSteppedVector (Vector2 oldVec, Vector2 newVec, float magnitude)
	{
		return Vector2.MoveTowards(oldVec, newVec, magnitude * Time.fixedDeltaTime);
	}

	public static float Angle(this Vector3 u)
	{
		u = u.normalized;
		return Mathf.Atan2( u.y, u.x) * Mathf.Rad2Deg;
	}

    public static float Angle( this Vector2 u )
    {
        u = u.normalized;
        return Mathf.Atan2( u.y, u.x ) * Mathf.Rad2Deg;
    }

    public static Vector2 ToVector2( this float angle )
    {
        return new Vector2( Mathf.Cos( angle * Mathf.Deg2Rad ), Mathf.Sin( angle * Mathf.Deg2Rad ) );
    }

}
