using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class IonInfo : MonoBehaviour
{
    
	public class Ion { // Ion utility class
		public int charge; // charge of ion (positive or negative)
		public string abbr; // parse-friendly abbreviation ('X' 'x' reserved, numbers not subscripted)
		public float mass; // molar mass in g/mol
		
		public Ion(string a, int c, float m) {
			charge = c;
			abbr = a.Replace("\r", "");
			mass = m;
		}
		public override string ToString() { // produce human-readable ion abbreviation
			string output = "";
			foreach(char c in abbr) {
				if(c >= '0' && c <= '9') {
					output += subscript((int) c-'0'); // subscripts for polyatomic ions
				} else if(c != 'x' && c != 'X') { output += c; } // X is reserved for multiple charges of the same ion
			}
			
			return output;
		}
	}
	
	public TextAsset ionData;
	protected string[,] ionDataSplit = new string[ANIONS+3, CATIONS+3]; // indexable form of raw text
	
	const int CATIONS = 15; // IMPORTANT
	const int ANIONS = 12;  // Change these when adding additional ions to the matrix
	
	protected Ion[] cations = new Ion[CATIONS]; // database of all cations
	protected Ion[] anions = new Ion[ANIONS]; // database of all anions
	protected float[,] solubility = new float[CATIONS, ANIONS]; // solubility matrix
	
	protected void Start() { // Read data from solubility & info matrix, typically located at Scripts/Chemistry/ions.csv
		string[] rows = ionData.text.Split('\n');
		for(int i = 0; i < rows.Length; i++) { // split raw data into 2D CSV array
			string[] elems = rows[i].Split(',');
			for(int j = 0; j < elems.Length; j++) {
				ionDataSplit[i, j] = elems[j];
			}
		}
		
		for(int i = 0; i < ANIONS; i++) { // parse the array to various database arrays
			anions[i] = new Ion(ionDataSplit[i+3, 0], int.Parse(ionDataSplit[i+3, 1]), float.Parse(ionDataSplit[i+3, 2])); // read anion from beginning of row
			for(int j = 0; j < CATIONS; j++) {
				if(i == 0) { // once, read cations from beginning of each column
					cations[j] = new Ion(ionDataSplit[0, j+3], int.Parse(ionDataSplit[1, j+3]), float.Parse(ionDataSplit[2, j+3]));
				}
				string t = ionDataSplit[i+3, j+3]; // read each solubility cell
				float s; // final parsed solubility (g/mol)
				if(t == "s") { s = 99999; } // if it's 's'/soluble, set s to ~infinity
				else if(float.TryParse(t, out s)) {} // otherwise try to read as a number
				else { s = 0; } // failing all else, return 0
				solubility[j, i] = s;
			}
		}
    }
	
	static string subscript(int n) { // unicode subscript utility
		char d = (char) (0x2080 + n%10); // first digit (0x2080 = subscript 0)
		if(n >= 10) { // if multi-digit:
			return subscript(n/10)+d.ToString(); // recursively subscript the next digit
		} else {
			return d.ToString(); // otherwise just return the first digit
		}
	}
	static int gcd(int a, int b) { // Euler GCD utility
		while(a!=0 && b!=0) {
			if(a > b) { a %= b; }
			else { b %= a; }
		}
		return a | b;
	}
	protected float getSolubility(int cationIndex, int anionIndex) { // literally just reading the solubility matrix
		Debug.Log(cationIndex +" "+ anionIndex+" "+solubility[cationIndex, anionIndex]);
		return solubility[cationIndex, anionIndex];
	}
	protected float getMass(Ion cation, Ion anion) { // calculate compound molar mass
		int nc = -anion.charge; // calculate empirical formula via GCD
		int na = cation.charge;
		int scalar = gcd(nc, na);
		nc /= scalar;
		na /= scalar;
		return cation.mass*nc + anion.mass*na; // use ion DB masses
	}
	static string numParen(string ion, int n) { // readable ion-multiples utility
		if(n > 1) { // if we even need a subscript...
			bool parens = false; // do we need parentheses?
			byte cap = 0; // number of capitals
			foreach(char c in ion) {
				if(c >= 'A' && c <= 'Z' && c != 'x' && c != 'X') { cap++; } // if multiple characters are capitals, it's polyatomic
				if((c >= '0' && c <= '9') || cap >= 2) { // if any characters are numbers, it's polyatomic
					parens = true;
					break;
				}
			}
			if(parens) { ion = "("+ion+")"; } // add parentheses
			ion += subscript(n); // add subscript
		}
		return ion;
	}
	protected string getReadable(Ion cation, Ion anion) { // readable ionic compound formula
		int nc = -anion.charge; // calculate empirical formula
		int na = cation.charge;
		int scalar = gcd(nc, na);
		nc /= scalar;
		na /= scalar;
		return numParen(cation.ToString(), nc) + numParen(anion.ToString(), na); // stick together all the previous string utils
	}
}