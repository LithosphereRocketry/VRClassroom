using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorGraph : MonoBehaviour
{
	float x(float t) { return Mathf.Sin(t); }
	float y(float t) { return Mathf.Cos(t); }
	float z(float t) { return t; }
	
	public float startT;
	public float endT;
	public int nPoints;
	float dt;
	Vector3 dvdt;
	Vector3 d2vdt2;
	Vector3 cross;
	List<Vector3> points = new List<Vector3>();
	
	public Color firstDerivColor;
	public Color secondDerivColor;
	public Color crossColor;
	
	LineRenderer graph, lineDv, lineD2v, lineCross;
    void Start() {
		graph = gameObject.GetComponent<LineRenderer>();
		graph.positionCount = nPoints;
		dt = (endT - startT) / (float) nPoints;
		for(int i = 0; i < nPoints; i++) {
			float t = i*dt;
			points.Add(new Vector3(x(t), y(t), z(t)));
		}
		setupLine(lineDv, firstDerivColor);
		setupLine(lineD2v, secondDerivColor);
		setupLine(lineCross, crossColor);
		
		graph.SetPositions(points.ToArray());
		
		GenerateVectors(Random.Range(1, nPoints-2));
    }
	
	void GenerateVectors(int i) {
		if(i < 1) { i = 1; }
		if(i >= nPoints-1) { i = nPoints-2; }
		dvdt = (points[i]-points[i-1]) / dt;
		Vector3 dvstep = (points[i+1]-points[i]) / dt;
		d2vdt2 = (dvstep - dvdt) / dt;
		cross = Vector3.Cross(d2vdt2, dvdt);
		
		lineDv.SetPosition(0, points[i]);
		lineDv.SetPosition(1, points[i] + dvdt);
	}
	
	void setupLine(LineRenderer r, Color c) {
		r = new GameObject().AddComponent<LineRenderer>();
		r.positionCount = 2;
		r.material = new Material(Shader.Find("Sprites/Default"));
		r.positionCount = 2;
		r.loop = false;
		r.startWidth = graph.startWidth;
		r.useWorldSpace = false;
		r.gameObject.transform.position = transform.position;
		r.startColor = c;
		r.endColor = c;
	}
	
    void Update() {
        
    }
}
