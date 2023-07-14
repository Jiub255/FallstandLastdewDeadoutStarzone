using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
	public static bool Contains(this LayerMask layerMask, int layer)
    {
        return ((layerMask & (1 << layer)) != 0);
    }


    // Get intersection point of ray and XZ-plane. 
    private static Vector3 XZPlaneNormal = new Vector3(0f, 1f, 0f);
    public static bool IntersectsXZPlane(this Ray ray, out Vector3 intersectionPoint)
    {
        float a = Vector3.Dot(XZPlaneNormal, ray.origin);
        float b = Vector3.Dot(XZPlaneNormal, ray.direction);

        if (a == 0f)
        {
            // Origin is on xz-plane, so that's the intersection. 
            Debug.LogWarning($"Ray's origin on xz-plane. ");
            intersectionPoint = ray.origin;
            // Return false even though there is an intersection because don't want the camera level with the ground ever. 
            // Code in CameraMoveRotate doesn't let it rotate that far down so it shouldn't happen. 
            return false;
        }
        else if (Mathf.Sign(a) == Mathf.Sign(b))
        {
            // Ray intersects xz-plane. 
            float lambda = a / b;
            intersectionPoint = lambda * ray.direction;
            return true;
        }
        else
        {
            // No intersection. 
            Debug.LogWarning($"Ray doesn't intersect xz-plane. ");
            intersectionPoint = Vector3.zero;
            return false;
        }

/*        // Origin is above (or below) xz-plane (not sure how unity does angles).
        if (a < 0f)
        {
            // Ray points down (or up).
            if (b < 0f)
            {
                // Ray intersects xz-plane. 
                float lambda = a / b;
                intersectionPoint = lambda * ray.direction;
                return true;
            }
            // Ray is parallel (with no intersection) or points up (or down). 
            else
            {
                // No intersection. 
                Debug.LogWarning($"Ray doesn't intersect xz-plane. ");
                intersectionPoint = Vector3.zero;
                return false;
            }
        }
        // Origin is below (or above) xz-plane (not sure how unity does angles).
        else if (a > 0f)
        {
            // Ray points up (or down).
            if (b > 0f)
            {
                // Ray intersects xz-plane. 
                float lambda = a / b;
                intersectionPoint = lambda * ray.direction;
                return true;
            }
            // Ray is parallel (no intersection) or points down (or up). 
            else
            {
                // No intersection. 
                Debug.LogWarning($"Ray doesn't intersect xz-plane. ");
                intersectionPoint = Vector3.zero;
                return false;
            }
        }
        // Origin is on xz-plane, so that's the intersection. 
        else // if (a == 0f); 
        {
            Debug.LogWarning($"Ray's origin on xz-plane. ");
            intersectionPoint = ray.origin;
            // Return false even though there is an intersection because don't want the camera level with the ground ever. 
            // Code in CameraMoveRotate doesn't let it rotate that far down so it shouldn't happen. 
            return false;
        }*/
    }


    public static RaycastHit[] ConeCastAll(this Physics physics, Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle)
    {
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();

        if (sphereCastHits.Length > 0)
        {
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                Vector3 hitPoint = sphereCastHits[i].point;
                Vector3 directionToHit = hitPoint - origin;
                float angleToHit = Vector3.Angle(direction, directionToHit);

                if (angleToHit < coneAngle)
                {
                    coneCastHitList.Add(sphereCastHits[i]);
                }
            }
        }

        RaycastHit[] coneCastHits = new RaycastHit[coneCastHitList.Count];
        coneCastHits = coneCastHitList.ToArray();

        return coneCastHits;
    }

    // Might not work with nonalloc since you can't get collision point from overlapboxnonalloc. 
    // TODO - Maybe use a lower number? Look into it, especially before final build. 
/*    private static Collider[] _colliders = new Collider[256]; 
    public static bool OverlapCone(Vector3 tip, Vector3 baseCenter, float baseRadius, out Collider[] colliders)
    {
        Vector3 center = (tip + baseCenter) / 2f;

        float boxHalfDepth = (baseCenter - tip).magnitude / 2f;

        Vector3 halfExtents = new Vector3(
            baseRadius,
            baseRadius,
            boxHalfDepth);

        Quaternion orientation = Quaternion.LookRotation(baseCenter - tip, Vector3.up);

        int maxIndex = Physics.OverlapBoxNonAlloc(
            center,
            halfExtents,
            _colliders)
            // Subtract 1 to get max index from count. 
            - 1;

        for (int i = 0; i < maxIndex; i++)
        {
            _colliders[i].
        }
    }*/
}