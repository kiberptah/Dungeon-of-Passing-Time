using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    public int raysAmount = 8;
    float viewAngle = 360;
    public float stepAngleSize;

    public float viewRadius = 1;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public int edgeResolveIterations = 10;
    //public float edgeDistanceThreshold;

    //public float maskCutawayDst = 0.1f;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;  
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }


    void DrawFieldOfView()
    {
        stepAngleSize = viewAngle / raysAmount;

        //Vector3 endOfRayOffset = new Vector2(0, viewRadius);

        List<Vector3> viewPoints = new List<Vector3>();
        List<RaycastHit2D> raycastPoints = new List<RaycastHit2D>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= raysAmount; i++)
        {
            float angle = i * stepAngleSize;

            /*
            Vector3 offset = new Vector2();
            offset.x = endOfRayOffset.x * Mathf.Cos(angle * Mathf.Deg2Rad) - endOfRayOffset.y * Mathf.Sin(angle * Mathf.Deg2Rad);
            offset.y = endOfRayOffset.x * Mathf.Sin(angle * Mathf.Deg2Rad) + endOfRayOffset.y * Mathf.Cos(angle * Mathf.Deg2Rad);
            */
            
            Vector3 offset = AngleToDirection(angle) * viewRadius;

            Debug.DrawLine(transform.position, transform.position + offset, Color.magenta);
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                //bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDistanceThreshold;
                //if (oldViewCast.hit != newViewCast.hit || oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded)
                if (oldViewCast.hit != newViewCast.hit)
                {
                    //Debug.Log("EDGE DETECT");
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                        raycastPoints.Add(edge.raycasthit);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                        raycastPoints.Add(edge.raycasthit);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            raycastPoints.Add(newViewCast.raycasthit);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] verticies = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        verticies[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; ++i)
        {
            //Vector3 meshOffset = -raycastPoints[i].normal * maskCutawayDst;

            verticies[i + 1] = transform.InverseTransformPoint(viewPoints[i]);// + meshOffset;
            //verticies[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = verticies;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        int sweepDirection = 1;
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        ViewCastInfo newViewCast = new ViewCastInfo();

        for (int i = 0; i < edgeResolveIterations; ++i)
        {
            float angle = (minAngle + maxAngle) / 2;
            newViewCast = ViewCast(angle);

            if (sweepDirection == 1)
            {
                //bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDistanceThreshold;
                //if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
                if (newViewCast.hit == minViewCast.hit)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;

                    sweepDirection = -sweepDirection;
                }
            }
            else
            {
                if (newViewCast.hit == maxViewCast.hit)
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
                else
                {
                    break;
                }
            }
        }
        return new EdgeInfo(minPoint, maxPoint, newViewCast.raycasthit);
    }

    ViewCastInfo ViewCast(float angle)
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, AngleToDirection(angle), viewRadius, LayerMask.GetMask("Geometry"));

        if (hit.collider != null)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, angle, hit);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + AngleToDirection(angle) * viewRadius, viewRadius, angle, new RaycastHit2D());
        }
    }
    Vector3 AngleToDirection(float angle)
    {
        Vector3 endOfRayOffset = new Vector2(0, viewRadius); // first ray direction is UP
        Vector3 direction = new Vector2();

        direction.x = endOfRayOffset.x * Mathf.Cos(angle * Mathf.Deg2Rad) - endOfRayOffset.y * Mathf.Sin(angle * Mathf.Deg2Rad);
        direction.y = endOfRayOffset.x * Mathf.Sin(angle * Mathf.Deg2Rad) + endOfRayOffset.y * Mathf.Cos(angle * Mathf.Deg2Rad);

        direction = direction.normalized;

        return direction;
    }
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;
        public RaycastHit2D raycasthit;
        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle, RaycastHit2D _raycasthit)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;

            raycasthit = _raycasthit;
        }
    }
    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;
        public RaycastHit2D raycasthit;
        public EdgeInfo(Vector3 _pointA, Vector3 _pointB, RaycastHit2D _raycasthit)
        {
            pointA = _pointA;
            pointB = _pointB;

            raycasthit = _raycasthit;
        }
    }
}
