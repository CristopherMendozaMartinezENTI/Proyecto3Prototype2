using UnityEngine;

public static class CameraUtility {
    static readonly Vector3[] cubeCornerOffsets = {
        new Vector3 (1, 1, 1),
        new Vector3 (-1, 1, 1),
        new Vector3 (-1, -1, 1),
        new Vector3 (-1, -1, -1),
        new Vector3 (-1, 1, -1),
        new Vector3 (1, -1, -1),
        new Vector3 (1, 1, -1),
        new Vector3 (1, -1, 1),
    };

    public static bool VisibleFromCamera (Renderer renderer, Camera camera) {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes (camera);
        return GeometryUtility.TestPlanesAABB (frustumPlanes, renderer.bounds);
    }

    public static bool BoundsOverlap (MeshFilter nearObject, MeshFilter farObject, Camera camera) {

        MinMax3D near = GetScreenRectFromBounds (nearObject, camera);
        MinMax3D far = GetScreenRectFromBounds (farObject, camera);

        if (far.zMax > near.zMin) {
            if (far.xMax < near.xMin || far.xMin > near.xMax) {
                return false;
            }
            if (far.yMax < near.yMin || far.yMin > near.yMax) {
                return false;
            }
            return true;
        }
        return false;
    }

    public static MinMax3D GetScreenRectFromBounds (MeshFilter renderer, Camera mainCamera) {
        MinMax3D minMax = new MinMax3D (float.MaxValue, float.MinValue);

        Vector3[] screenBoundsExtents = new Vector3[8];
        Bounds localBounds = renderer.sharedMesh.bounds;
        bool anyPointIsInFrontOfCamera = false;

        for (int i = 0; i < 8; i++) {
            Vector3 localSpaceCorner = localBounds.center + Vector3.Scale (localBounds.extents, cubeCornerOffsets[i]);
            Vector3 worldSpaceCorner = renderer.transform.TransformPoint (localSpaceCorner);
            Vector3 viewportSpaceCorner = mainCamera.WorldToViewportPoint (worldSpaceCorner);

            if (viewportSpaceCorner.z > 0) {
                anyPointIsInFrontOfCamera = true;
            } else {
                viewportSpaceCorner.x = (viewportSpaceCorner.x <= 0.5f) ? 1 : 0;
                viewportSpaceCorner.y = (viewportSpaceCorner.y <= 0.5f) ? 1 : 0;
            }

            minMax.AddPoint (viewportSpaceCorner);
        }

        if (!anyPointIsInFrontOfCamera) {
            return new MinMax3D ();
        }

        return minMax;
    }

    public struct MinMax3D {
        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;
        public float zMin;
        public float zMax;

        public MinMax3D (float min, float max) {
            this.xMin = min;
            this.xMax = max;
            this.yMin = min;
            this.yMax = max;
            this.zMin = min;
            this.zMax = max;
        }

        public void AddPoint (Vector3 point) {
            xMin = Mathf.Min (xMin, point.x);
            xMax = Mathf.Max (xMax, point.x);
            yMin = Mathf.Min (yMin, point.y);
            yMax = Mathf.Max (yMax, point.y);
            zMin = Mathf.Min (zMin, point.z);
            zMax = Mathf.Max (zMax, point.z);
        }
    }
}