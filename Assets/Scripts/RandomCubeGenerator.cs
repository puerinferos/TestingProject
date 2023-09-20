using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]
public class RandomCubeGenerator : MonoBehaviour
{
	[Min(.2f)][SerializeField] private float size;
	[SerializeField] private Vector2 minMaxVerticeDisplacementX;
	[SerializeField] private Vector2 minMaxVerticeDisplacementY;
	[SerializeField] private Vector2 minMaxVerticeDisplacementZ;

	void Start()
	{
		CreateCube();
	}

	private Vector3 RandomVerticeDisplaceMent()
	{
		float x = Random.Range(minMaxVerticeDisplacementX.x, minMaxVerticeDisplacementX.y);
		float y = Random.Range(minMaxVerticeDisplacementY.x, minMaxVerticeDisplacementY.y);
		float z = Random.Range(minMaxVerticeDisplacementZ.x, minMaxVerticeDisplacementZ.y);

		return new Vector3(x, y, z);
	}
    
    	private void CreateCube () 
	    {
		    
    		Vector3[] vertices = 
		    {
    			new Vector3 (0, 0, 0) * size + RandomVerticeDisplaceMent(),
    			new Vector3 (1, 0, 0) * size + RandomVerticeDisplaceMent(),
    			new Vector3 (1, 1, 0) * size + RandomVerticeDisplaceMent(),
    			new Vector3 (0, 1, 0) * size + RandomVerticeDisplaceMent(),
    			new Vector3 (0, 1, 1) * size + RandomVerticeDisplaceMent(),
    			new Vector3 (1, 1, 1) * size + RandomVerticeDisplaceMent(),
    			new Vector3 (1, 0, 1) * size + RandomVerticeDisplaceMent(),
    			new Vector3 (0, 0, 1) * size + RandomVerticeDisplaceMent(),
    		};
    
    		int[] triangles = {
    			0, 2, 1, //face front
    			0, 3, 2,
    			2, 3, 4, //face top
    			2, 4, 5,
    			1, 2, 5, //face right
    			1, 5, 6,
    			0, 7, 4, //face left
    			0, 4, 3,
    			5, 4, 7, //face back
    			5, 7, 6,
    			0, 6, 7, //face bottom
    			0, 1, 6
    		};
		    
		    Vector3 centroid = Vector3.zero;

		    // Calculate the centroid
		    foreach (Vector3 vertex in vertices)
		    {
			    centroid += vertex;
		    }
		    centroid /= vertices.Length;

		    // Offset the vertices by the negative of the centroid's position
		    for (int i = 0; i < vertices.Length; i++)
		    {
			    vertices[i] -= centroid;
		    }

    		Mesh mesh = GetComponent<MeshFilter> ().mesh;
    		mesh.Clear ();
    		mesh.vertices = vertices;
    		mesh.triangles = triangles;
    		//mesh.Optimize ();
    		mesh.RecalculateNormals ();
		    mesh.RecalculateBounds();

		    gameObject.AddComponent<MeshCollider>();
	    }
}