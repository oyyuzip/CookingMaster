using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaladType : MonoBehaviour
{
	// Variables keep track of veggies in salad
	int numLettuce;
	int numTomato;
	int numCarrot;
	int numCheese;
	int numTurnip;
	int numCaper;
	
	// Keep track of whose salad you are
	bool ownedByP1;
	
    // Awake is called before the first frame update
    void Awake()
    {
        // Initialize ingredients as empty
		numLettuce = 0;
		numTomato = 0;
		numCarrot = 0;
		numCheese = 0;
		numTurnip = 0;
		numCaper = 0;
    }
	
	// Assign salad ownership when created
	public void SetP1Owns(bool player)
	{
		ownedByP1 = player;
	}
	
	// Public method to get the number of ingredients in the salad
	public int GetNumIngredients()
	{
		return numLettuce + numTomato + numCarrot + numCheese + numTurnip + numCaper;
	}
	
	// Public method for adding ingredients to a salad
	public void AddIngredient(int ltc, int tmt, int crt, int chs, int tnp, int cpr)
	{
		// Increase each ingredient by the arguments presented
		numLettuce += ltc;
		numTomato += tmt;
		numCarrot += crt;
		numCheese += chs;
		numTurnip += tnp;
		numCaper += cpr;
	}
	
	// Public method for transferring ingredients from one salad instance to another
	public void MoveSalad(GameObject newSalad)
	{
		// Transfer all ingredient values from this salad into a the new salad
		SaladType s = newSalad.GetComponent<SaladType>();
		s.numLettuce = numLettuce;
		s.numTomato = numTomato;
		s.numCarrot = numCarrot;
		s.numCheese = numCheese;
		s.numTurnip = numTurnip;
		s.numCaper = numCaper;
	}
	
	// Public method to check if salad was prepared correctly
	public bool CheckSalad(int ltc, int tmt, int crt, int chs, int tnp, int cpr)
	{
		// Compare all 6 ingredients, return true if match
		if (numLettuce == ltc && numTomato == tmt && numCarrot == crt && numCheese == chs && numTurnip == tnp && numCaper == cpr)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	// OnGUI is called to draw text for the player
	void OnGUI()
	{
		// Set font style for salad type display
		GUIStyle flavorStyle = new GUIStyle();
		flavorStyle.alignment = TextAnchor.MiddleCenter;
		
		// Create string to interpret flavor
		string flavor = "";
		for (int i = numLettuce; i > 0; i--)
		{
			flavor += "Lt";
		}
		for (int i = numTomato; i > 0; i--)
		{
			flavor += "Tm";
		}
		for (int i = numCarrot; i > 0; i--)
		{
			flavor += "Ct";
		}
		for (int i = numCheese; i > 0; i--)
		{
			flavor += "Ch";
		}
		for (int i = numTurnip; i > 0; i--)
		{
			flavor += "Tr";
		}
		for (int i = numCaper; i > 0; i--)
		{
			flavor += "Cp";
		}
		
		// Output string near cutting board
		if (ownedByP1)
		{
			GUI.Label(new Rect(Screen.width / 3, 9 * Screen.height / 10, Screen.width / 9, Screen.height / 10), flavor, flavorStyle);
		}
		else
		{
			GUI.Label(new Rect(5 * Screen.width / 9, 9 * Screen.height / 10, Screen.width / 9, Screen.height / 10), flavor, flavorStyle);
		}
	}
}
