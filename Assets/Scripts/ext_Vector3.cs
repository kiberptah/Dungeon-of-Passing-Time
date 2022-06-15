using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ext_Vector3
{
    public static Vector3 Grounded(this Vector3 vector3)
    {
        vector3.y = 0;
        return vector3;
    }
    public static Vector3 GroundedTo(this Vector3 vector3, float y)
    {
        vector3.y = y;
        return vector3;
    }

    public static Vector3 Offset(this Vector3 vector3, float x, float y, float z)
    {
        vector3.x += x;
        vector3.y += y;
        vector3.z += z;
        return vector3;
    }
    public static Vector3 Override(this Vector3 vector3, float x, float y, float z)
    {
        vector3.x = x;
        vector3.y = y;
        vector3.z = z;
        return vector3;
    }
    public static Vector3 OverrideX(this Vector3 vector3, float x)
    {
        vector3.x = x;
        return vector3;
    }

    public static Vector3 OverrideY(this Vector3 vector3, float y)
    {
        vector3.y = y;
        return vector3;
    }
    public static Vector3 OverrideZ(this Vector3 vector3, float z)
    {
        vector3.z = z;
        return vector3;
    }
}
