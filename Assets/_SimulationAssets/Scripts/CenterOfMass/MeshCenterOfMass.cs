using UnityEngine;


public class MeshCenterOfMass : MonoBehaviour {

    public Vector3 CenterOfMass { get; set; }

    void Start() {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        CenterOfMass = CenterOfMassOfMesh(mesh);
        string msg = "The center of mass of " + this.gameObject.name + "is at " + CenterOfMass;
        Debug.Log(msg);
    }

    public Vector3 CenterOfMassOfMesh(Mesh mesh) {
        float totalVolume = 0;
        Vector3 weightedCenter = Vector3.zero;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < mesh.triangles.Length; i += 3) {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];

            //calculate the volume of the tetrahedron formed by the triangle and the mesh local "center" (0,0,0)
            float tempVolume = (1.0f / 6.0f) * Vector3.Dot(p1, Vector3.Cross(p2, p3));
            //with uniform density, the amount of mass is tied to the defined temporal volume, so the COM of the tetrahedron is calculated
            Vector3 tempWeightedCenter = tempVolume * (p1 + p2 + p3) / 4;
            //And then accumulated for the COM final formula
            weightedCenter += tempWeightedCenter;
            totalVolume += tempVolume;
        }
        return weightedCenter / totalVolume;
    }
}