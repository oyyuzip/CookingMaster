using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CustomerSpawn;

public class CustomerOrder : MonoBehaviour
{
	// Keeps track of customer's position at counter
	int custID;
	
	// Variables to determine the customer's order
	int numIngredients;
	int lettuceCount;
	int tomatoCount;
	int carrotCount;
	int cheeseCount;
	int turnipCount;
	int caperCount;
	
	// Determine how much time the customer will stick around before leaving
	float timer;
	float anger;
	
	// Constants determining ranges and scaling
	const int MIN_ORDER_SIZE = 1;
	const int MAX_ORDER_SIZE = 3;
	const float BASE_TIMER = 15.0f;
	
    // Start is called before the first frame update
    void Start()
    {
		// Start with an empty order
		lettuceCount = 0;
		tomatoCount = 0;
		carrotCount = 0;
		cheeseCount = 0;
		turnipCount = 0;
		caperCount = 0;
				
        // Initialize the desired order randomly
		numIngredients = Random.Range(MIN_ORDER_SIZE, MAX_ORDER_SIZE);
		for (int i = 0; i < numIngredients; i++)
		{
			int nextIngredient = Random.Range(1, 6);
			switch (nextIngredient)
			{
				case 1:
					lettuceCount++;
					break;
				case 2:
					tomatoCount++;
					break;
				case 3:
					carrotCount++;
					break;
				case 4:
					cheeseCount++;
					break;
				case 5:
					turnipCount++;
					break;
				case 6:
					caperCount++;
					break;
				default:
					lettuceCount++;
					break;
			}
		}
		
		// Timer is based on order size
		timer = BASE_TIMER * numIngredients;
		
		// Customer starts off happy, multiplier is standard
		anger = 1.0f;
    }
	
	// Assigns the customer an order number from the spawn script
	public void SetIDNumber(int newID)
	{
		custID = newID;
	}

    // Update is called once per frame
    void Update()
    {
		// Decrease timer at rate determined by customer satisfaction
        timer -= Time.deltaTime * anger;
		
		// When timer elapses, customer leaves
		if (timer <= 0.0)
		{
			CustomerSpawn.SetOccupied(custID, false);
			Destroy(gameObject);
		}
    }
	
	// OnGUI is called to draw text for the player
	void OnGUI()
	{
		// Set font style for order display
		GUIStyle orderStyle = new GUIStyle();
		orderStyle.alignment = TextAnchor.MiddleCenter;
		
		// Create string to interpret order
		string order = "";
		for (int i = lettuceCount; i > 0; i--)
		{
			order += "Lt";
		}
		for (int i = tomatoCount; i > 0; i--)
		{
			order += "Tm";
		}
		for (int i = carrotCount; i > 0; i--)
		{
			order += "Ct";
		}
		for (int i = cheeseCount; i > 0; i--)
		{
			order += "Ch";
		}
		for (int i = turnipCount; i > 0; i--)
		{
			order += "Tr";
		}
		for (int i = caperCount; i > 0; i--)
		{
			order += "Cp";
		}
		
		// Output string onto customer's plate
		GUI.Label(new Rect((2 + (2 * custID)) * Screen.width / 18, Screen.height / 10, Screen.width / 9, Screen.height / 10), order, orderStyle);
	}
}
