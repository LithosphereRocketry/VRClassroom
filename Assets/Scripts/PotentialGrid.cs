using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialGrid : MonoBehaviour
{
    private const float k = 8.987552e9f;
	public static int width = 80;
	public static int height = 60;
	public Transform root;
	public float lineWidth;
	public float cellSize;
	public float verticalScale;
	public Color color;
	public GameObject orb;
	
	
	protected float[,] values = new float[width, height];
	protected LineRenderer[] columns = new LineRenderer[width];
	protected LineRenderer[] rows = new LineRenderer[height];
	
	// Start is called before the first frame update
    void Start() {
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				values[i, j] = 0;
			}
		}
		for(int i = 0; i < width; i++) {
			GameObject c = new GameObject("col_"+i);
			columns[i] = c.AddComponent<LineRenderer>();
			
			columns[i].material = new Material(Shader.Find("Sprites/Default"));
			columns[i].positionCount = height;
			columns[i].loop = false;
			
			Gradient g = new Gradient();
			float alpha = 1.0f;
			g.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) });
			columns[i].colorGradient = g;
			columns[i].startWidth = lineWidth;
		}
		for(int i = 0; i < height; i++) {
			GameObject c = new GameObject("row_"+i);
			rows[i] = c.AddComponent<LineRenderer>();
			rows[i].positionCount = width;
			
			rows[i].material = new Material(Shader.Find("Sprites/Default"));
			rows[i].positionCount = width;
			rows[i].loop = false;
			
			Gradient g = new Gradient();
			float alpha = 1.0f;
			g.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) });
			rows[i].colorGradient = g;
			rows[i].startWidth = lineWidth;
		}
		UpdateRenders();
    }

	void UpdateRenders() {
		for(int i = 0; i < width; i++) {
			Vector3[] points = new Vector3[height];
			for(int j = 0; j < height; j++) {
				points[j] = new Vector3(transform.position.x+i*cellSize, transform.position.y+values[i, j]*verticalScale, transform.position.z+j*cellSize);
			}
			columns[i].SetPositions(points);
		}
		for(int j = 0; j < height; j++) {
			Vector3[] points = new Vector3[width];
			for(int i = 0; i < width; i++) {
				points[i] = new Vector3(transform.position.x+i*cellSize, transform.position.y+values[i, j]*verticalScale, transform.position.z+j*cellSize);
			}
			rows[j].SetPositions(points);
		}
	}
    void Update() {
        for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				values[i,j] = GetPotentialContribution(new Vector3(transform.position.x+i*cellSize, 0, transform.position.z+j*cellSize), orb);
			}
		}
		UpdateRenders();
    }
	float GetPotentialContribution(Vector3 point, GameObject obj) {
		PotentialItem stats = (PotentialItem) obj.GetComponent("PotentialItem");
		Vector3 targetpoint = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
		if(stats) {
			if(stats.type == "point") {
				return k*stats.potential/Vector3.Distance(point, targetpoint);
			} else {
				return 0;
			}
		} else {
			return 0;
		}
	}
}
