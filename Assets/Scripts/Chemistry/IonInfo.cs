using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class IonInfo : MonoBehaviour
{
    
	public class Ion {
		public int charge;
		public string abbr;
		public float mass;
		
		public Ion(string a, int c, float m) {
			charge = c;
			abbr = a;
			mass = m;
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
	protected string[,] ionDataSplit = new string[ANIONS+3, CATIONS+3];
	
	const int CATIONS = 15;
	const int ANIONS = 12;
	
	protected Ion[] cations = new Ion[CATIONS];
	protected Ion[] anions = new Ion[ANIONS];
	protected float[,] solubility = new float[CATIONS, ANIONS];
	
	protected void Start()
    {
		string[] rows = ionData.text.Split('\n');
		for(int i = 0; i < rows.Length; i++) {
			string[] elems = rows[i].Split(',');
			for(int j = 0; j < elems.Length; j++) {
				ionDataSplit[i, j] = elems[j];
			}
		}
		
		for(int i = 0; i < ANIONS; i++) {
			anions[i] = new Ion(ionDataSplit[i+3, 0], int.Parse(ionDataSplit[i+3, 1]), float.Parse(ionDataSplit[i+3, 2]));
			for(int j = 0; j < CATIONS; j++) {
				if(i == 0) {
					cations[j] = new Ion(ionDataSplit[0, j+3], int.Parse(ionDataSplit[1, j+3]), float.Parse(ionDataSplit[2, i+3]));
				}
				string t = ionDataSplit[i+3, j+3];
				float s;
				if(t == "s") { s = float.PositiveInfinity; }
				else if(float.TryParse(t, out s)) {}
				else { s = 0; }
				solubility[j, i] = s;
			}
		}
    }
	
	static string subscript(int n) {
		char d = (char) (0x2080 + n%10);
		if(n >= 10) {
			return subscript(n/10)+d.ToString();
		} else {
			return d.ToString();
		}
	}
	static int gcd(int a, int b) {
		while(a!=0 && b!=0) {
			if(a > b) { a %= b; }
			else { b %= a; }
		}
		return a | b;
	}
	protected float getSolubility(int cationIndex, int anionIndex) {
		return solubility[cationIndex, anionIndex];
	}
	protected float getMass(Ion cation, Ion anion) {
		int nc = -anion.charge;
		int na = cation.charge;
		int scalar = gcd(nc, na);
		nc /= scalar;
		na /= scalar;
		return cation.mass*nc + anion.mass*na;
	}

}
