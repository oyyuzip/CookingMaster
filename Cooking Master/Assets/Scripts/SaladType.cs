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
	
    // Start is called before the first frame update
    void Start()
    {
        // Initialize ingredients as empty
		numLettuce = 0;
		numTomato = 0;
		numCarrot = 0;
		numCheese = 0;
		numTurnip = 0;
		numCaper = 0;
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
}
