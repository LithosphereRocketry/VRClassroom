using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PotentialGrid : MonoBehaviour
{
    private const float k = 8.987552e9f;
	private List<GameObject> trackedObjects = new List<GameObject>();
	private Stopwatch clock;
	public static int width = 81;
	public static int height = 61;
	public float frameTimeTarget = 10f;
	public float lineWidth;
	public float cellSize;
	public float verticalScale;
	public float alpha;
	public Color positiveColor;
	public Color neutralColor;
	public Color negativeColor;
	
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
			
			columns[i].startWidth = lineWidth;
		}
		for(int i = 0; i < height; i++) {
			GameObject c = new GameObject("row_"+i);
			rows[i] = c.AddComponent<LineRenderer>();
			rows[i].positionCount = width;
			
			rows[i].material = new Material(Shader.Find("Sprites/Default"));
			rows[i].positionCount = width;
			rows[i].loop = false;
			
			rows[i].startWidth = lineWidth;
		}
		clock = new Stopwatch();
		
		StartCoroutine("UpdateGrid");
    }
	
	float GetPotentialContribution(Vector3 point, GameObject obj) {
		PotentialItem stats = (PotentialItem) obj.GetComponent("PotentialItem");
		Vector3 targetpoint = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
		if(stats) {
			if(stats.type == "point") {
				return k*stats.pointPotential/Vector3.Distance(point, targetpoint);
			} else {
				return 0;
			}
		} else {
			return 0;
		}
	}
	void UpdateRenders() {
		for(int i = 0; i < width; i++) { // for each row:
			Gradient g = new Gradient(); // set up the gradient
			int[] gradientSamples = new int[8];
			GradientColorKey[] c = new GradientColorKey[8];
			GradientAlphaKey[] a = new GradientAlphaKey[8];
			Vector3[] points = new Vector3[height]; // set up the line points
			int currentSample = 0; // track how many of our 8 keys we've used
			for(int j = 0; j < height; j++) {
				points[j] = new Vector3(transform.position.x+i*cellSize, transform.position.y+values[i, j]*verticalScale, transform.position.z+j*cellSize); // set the line point to the correct height
				if( // Which points should be keys?
				   j == 0 // first endpoint
				|| j == height-1 // last endpoint
				|| (currentSample < 7 // make sure there's room for the last endpoint...
				&& (values[i, j-1]-values[i, j])*(values[i, j+1]-values[i, j]) > 0 // ...then corner points
				)) {
					gradientSamples[currentSample] = j;
					currentSample++;
				}
			}
			columns[i].SetPositions(points); // set physical shape
			
			for(int s = 0; s < 8; s++) { // for each gradient keypoint
				int j = gradientSamples[s]; // find the associated vertex
				a[s] = new GradientAlphaKey(alpha, ((float) j)/(float) height); // fixed opacity for now
				c[s].time = ((float) j)/(float) height; // set color time
				if(values[i, j] >= 0) { // if it's positive:
					c[s].color = Color.Lerp(neutralColor, positiveColor, values[i, j]); // set it according to the positive gradient
				} else { // if it's negative:
					c[s].color = Color.Lerp(neutralColor, negativeColor, -values[i, j]); // set it according to the negative gradient
				}
			}
			g.SetKeys(c, a); columns[i].colorGradient = g; // apply the gradient
		}
		for(int j = 0; j < height; j++) {
			Gradient g = new Gradient(); // set up the gradient
			int[] gradientSamples = new int[8];
			GradientColorKey[] c = new GradientColorKey[8];
			GradientAlphaKey[] a = new GradientAlphaKey[8];
			Vector3[] points = new Vector3[width]; // set up the line points
			int currentSample = 0; // track how many of our 8 keys we've used
			for(int i = 0; i < width; i++) {
				points[i] = new Vector3(transform.position.x+i*cellSize, transform.position.y+values[i, j]*verticalScale, transform.position.z+j*cellSize); // set the line point to the correct height
				if( // Which points should be keys?
				   i == 0 // first endpoint
				|| i == width-1 // last endpoint
				|| (currentSample < 7 // make sure there's room for the last endpoint...
				&& (values[i-1, j]-values[i, j])*(values[i+1, j]-values[i, j]) > 0 // ...then corner points
				)) {
					gradientSamples[currentSample] = i;
					currentSample++;
				}
			}
			rows[j].SetPositions(points);
			
			for(int s = 0; s < 8; s++) { // for each gradient keypoint
				int i = gradientSamples[s]; // find the associated vertex
				a[s] = new GradientAlphaKey(alpha, ((float) i)/(float) width); // fixed opacity for now
				c[s].time = ((float) i)/(float) width; // set color time
				if(values[i, j] >= 0) { // if it's positive:
					c[s].color = Color.Lerp(neutralColor, positiveColor, values[i, j]); // set it according to the positive gradient
				} else { // if it's negative:
					c[s].color = Color.Lerp(neutralColor, negativeColor, -values[i, j]); // set it according to the negative gradient
				}
			}
			g.SetKeys(c, a); rows[j].colorGradient = g; // apply the gradient
		}
	}
	IEnumerator UpdateGrid() {
		while(true) {
			for(int i = 0; i < width; i++) {
				for(int j = 0; j < height; j++) {
					values[i, j] = 0;
				}
			}
			clock.Start();
			for(int i = 0; i < width; i++) { // x loop
				for(int j = 0; j < height; j++) { // y loop
					Vector3 pos = new Vector3(transform.position.x+i*cellSize, 0, transform.position.z+j*cellSize);
					foreach(GameObject g in trackedObjects) { // object loop
						if(g) { values[i, j] += GetPotentialContribution(pos, g); }
					}
				}
				if(clock.ElapsedMilliseconds < frameTimeTarget) {
					clock.Reset();
					yield return null;
					clock.Start();
				}
			}
			clock.Reset();
			yield return null;
			UpdateRenders();
		}
	}
	void UpdatePotentials(GameObject g) {
		trackedObjects.Add(g);
	}
}
