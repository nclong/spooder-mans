﻿using UnityEngine;
using System.Collections;

public static class Constants {

	public static Vector2 In2D(this Vector3 u)
	{
		return new Vector2( u.x, u.y );
	}

}
