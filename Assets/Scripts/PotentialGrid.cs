using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialGrid : MonoBehaviour
{
    public static int width;
	public static int height;
	public float cellSize;
	public Transform root;
	
	protected float[,] values = new float[width, height];
	protected LineRenderer[] columns = new LineRenderer[width+1];
	protected LineRenderer[] rows = new LineRenderer[height+1];
	
	// Start is called before the first frame update
    void Start() {
		for(int i = 0; i <= width; i++) {
			GameObject c = new GameObject("col_"+i);
			columns[i] = c.AddComponent<LineRenderer>();
		}
		for(int i = 0; i <= height; i++) {
			GameObject c = new GameObject("row_"+i);
			rows[i] = c.AddComponent<LineRenderer>();
		}
    }

    void Update() {
        for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				
			}
		}
    }
}
