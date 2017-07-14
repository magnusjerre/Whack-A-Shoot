using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroup : MonoBehaviour {

	public Target targetPrefab;
	[SerializeField]
	private int targetGroupId;
	public int TargetGroupId { get { return targetGroupId; } }
	public float lifetime;

	public int maxScorePerTarget;

	private MeshFilter meshFilter;
	private Mesh mesh;

	void Awake() {
		meshFilter = GetComponent<MeshFilter> ();
		mesh = meshFilter.mesh;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 GetRandomPositionOnMesh() {
		int nTriangles = mesh.triangles.Length / 3;
		int selectedTriangleNumber = Mathf.Max(1, Random.Range (0, nTriangles));
		int firstIndex = (selectedTriangleNumber - 1) * 3;
		Vector3 index0 = mesh.vertices [mesh.triangles [firstIndex]];
		Vector3 index1 = mesh.vertices [mesh.triangles [firstIndex + 1]];
		Vector3 index2 = mesh.vertices [mesh.triangles [firstIndex + 2]];
		float progress01 = Random.value;
		float progress201 = Random.value;
		Vector3 pos01 = Vector3.Lerp (index0, index1, progress01);
		return transform.TransformPoint(Vector3.Lerp (index2, pos01, progress201));
	}
}
