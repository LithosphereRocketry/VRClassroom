using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class IonInfo : MonoBehaviour
{
    
	public class Ion {
		public int charge;
		public string abbr;
		
		public Ion(string a, int c) {
			charge = c;
			abbr = a;
		}
		public override string ToString() {
			string output = "";
			foreach(char c in abbr) {
				if(c >= '0' && c <= '9') {
					output += subscript((int) c-'0');
				} else { output += c; }
			}
			
			return output;
		}
	}
	
	public TextAsset ionData;
	protected string[,] ionDataSplit = new string[ANIONS+2, CATIONS+2];
	
	const int CATIONS = 15;
	const int ANIONS = 12;
	
	protected Ion[] cations = new Ion[CATIONS];
	protected Ion[] anions = new Ion[ANIONS];
	protected float[,] solubility = new float[CATIONS, ANIONS];
	
	void Start()
    {
		int x = 0;
		string[] rows = ionData.text.Split('\n');
		foreach(string r in rows) {
			int y = 0;
			string[] elems = r.Split(',');
			foreach(string e in elems) {
				ionDataSplit[x, y] = e;
				y++;
			}
			x++;
		}
		
		for(int i = 0; i < ANIONS; i++) {
			anions[i] = new Ion(ionDataSplit[i+2, 0], int.Parse(ionDataSplit[i+2, 1]));
			for(int j = 0; j < CATIONS; j++) {
				if(i == 0) {
					Debug.Log(ionDataSplit[0, j+2]);
					cations[j] = new Ion(ionDataSplit[0, j+2], int.Parse(ionDataSplit[1, j+2], CultureInfo.InvariantCulture));
					Debug.Log(cations[j]);
				}
				string t = ionDataSplit[i+2, j+2];
				float s;
				if(t == "s") { s = float.PositiveInfinity; }
				else if(float.TryParse(t, out s)) {}
				else { s = 0; }
				solubility[j, i] = s;
			}
		}
		
		Debug.Log(cations[4].abbr);
		Debug.Log(anions[7].abbr);
		Debug.Log(solubility[4, 7]);
    }
	
	static string subscript(int n) {
		char d = (char) (0x2080 + n%10);
		if(n >= 10) {
			return subscript(n/10)+d.ToString();
		} else {
			return d.ToString();
		}
	}
}
