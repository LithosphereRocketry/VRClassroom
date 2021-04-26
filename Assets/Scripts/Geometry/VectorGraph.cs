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
	public Color firstDerivColor;
	public ObjectToggleButton firstDerivButton;
	public Color secondDerivColor;
	public ObjectToggleButton secondDerivButton;
	public Color crossColor;
	public ObjectToggleButton crossButton;
	public GameObject targetPoint;
	public int pathRate;
	
	float dt;
	Vector3 dvdt;
	Vector3 d2vdt2;
	Vector3 cross;
	List<Vector3> points = new List<Vector3>();
	
	LineRenderer graph, lineDv, lineD2v, lineCross;
    void Start() {
		targetPoint.transform.SetParent(transform);
		graph = gameObject.GetComponent<LineRenderer>();
		graph.positionCount = nPoints;
		dt = (endT - startT) / (float) nPoints;
		for(int i = 0; i < nPoints; i++) {
			float t = i*dt;
			points.Add(new Vector3(x(t), y(t), z(t)));
		}
		lineDv = setupLine(firstDerivColor, firstDerivButton);
		lineD2v = setupLine(secondDerivColor, secondDerivButton);
		lineCross = setupLine(crossColor, crossButton);
		
		graph.SetPositions(points.ToArray());
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
		
		lineD2v.SetPosition(0, points[i] + dvdt);
		lineD2v.SetPosition(1, points[i] + dvdt + d2vdt2);
		
		lineCross.SetPosition(0, points[i] + cross);
		lineCross.SetPosition(1, points[i]  - cross);
	}
	
	LineRenderer setupLine(Color c, ObjectToggleButton toggle) {
		GameObject g = new GameObject();
		g.transform.SetParent(transform);
		g.transform.localPosition = Vector3.zero;
		g.transform.localRotation = Quaternion.identity;
		g.transform.localScale = Vector3.one;
		if(toggle) {
			toggle.target = g;
			g.SetActive(false);
		}
		LineRenderer r = g.AddComponent<LineRenderer>();
		r.positionCount = 2;
		r.material = new Material(Shader.Find("Sprites/Default"));
		r.positionCount = 2;
		r.loop = false;
		r.startWidth = graph.startWidth;
		r.useWorldSpace = false;
		Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c, 0.0f), new GradientColorKey(c, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
        );
        r.colorGradient = gradient;
		return r;
	}
	
    void Update() {
		int i = (int) ( Time.time*pathRate % nPoints );
        GenerateVectors(i);
		targetPoint.transform.localPosition = points[i];
    }
}
