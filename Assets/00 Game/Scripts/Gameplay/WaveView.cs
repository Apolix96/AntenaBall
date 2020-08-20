using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;

    public LayerMask obstacleMask;
    public MeshFilter viewMeshFilter;

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;
    public float maskCutawayDst = .1f;

    private Mesh viewMesh;
    private List<Vector3> viewPoints = new List<Vector3>();
    private Vector3 edgeMinPoint, edgeMaxPoint;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    private void LateUpdate()
    {
        //draw field of view
        var stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        var stepAngleSize = viewAngle / stepCount;
        viewPoints.Clear();
        var oldViewCast = new ViewCastInfo();
        for (var i = 0; i <= stepCount; i++)
        {
            var angle = -transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            var newViewCast = ViewCast(angle);

            if (i > 0)
            {
                var edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit ||
                    (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    FindEdge(oldViewCast, newViewCast, out edgeMinPoint, out edgeMaxPoint);
                    if (edgeMinPoint != Vector3.zero)
                        viewPoints.Add(edgeMinPoint);
                    if (edgeMaxPoint != Vector3.zero)
                        viewPoints.Add(edgeMaxPoint);
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        var vertexCount = viewPoints.Count + 1;
        var vertices = new Vector3[vertexCount];
        var triangles = new int[(vertexCount - 2) * 3];
        var uv = new Vector2[vertexCount];

        vertices[0] = Vector3.zero;
        uv[0] = Vector2.zero;

        for (var i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.up * maskCutawayDst;

            if (i >= vertexCount - 2) continue;
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        var xUv = 0f;
        
        for (var i = 1; i < vertices.Length; i ++)
        {
            var distance = Vector2.Distance(vertices[0], vertices[i]);
            uv[i] = new Vector2(xUv, distance/viewRadius);

            if (xUv == 0f)
                xUv = 1f;
            else
                xUv = 0f;
            
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.uv = uv;
        viewMesh.RecalculateNormals();
        viewMesh.Optimize();
    }

    private void FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast, out Vector3 minPoint,
        out Vector3 maxPoint)
    {
        var minAngle = minViewCast.angle;
        var maxAngle = maxViewCast.angle;
        minPoint = Vector3.zero;
        maxPoint = Vector3.zero;

        for (var i = 0; i < edgeResolveIterations; i++)
        {
            var angle = (minAngle + maxAngle) / 2;
            var newViewCast = ViewCast(angle);

            var edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        var dir = DirFromAngle(globalAngle);
        var hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);

        return hit
            ? new ViewCastInfo(true, hit.point, hit.distance, globalAngle)
            : new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
}