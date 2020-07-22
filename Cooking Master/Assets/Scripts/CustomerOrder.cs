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
	
	// Texture for timer bar
	Texture2D barFill;
	
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
		numIngredients = Random.Range(MIN_ORDER_SIZE, MAX_ORDER_SIZE + 1);
		for (int i = 0; i < numIngredients; i++)
		{
			int nextIngredient = Random.Range(1, 7);
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
		
		// Initialize texture for timer display
		barFill = new Texture2D(1, 1);
		
		// Customer starts off happy, multiplier is standard
		anger = 1.0f;
    }
	
	// Get methods for desired ingredients
	public int GetNumLettuce()
	{
		return lettuceCount;
	}
	
	public int GetNumTomato()
	{
		return tomatoCount;
	}
	
	public int GetNumCarrot()
	{
		return carrotCount;
	}
	
	public int GetNumCheese()
	{
		return cheeseCount;
	}
	
	public int GetNumTurnip()
	{
		return turnipCount;
	}
	
	public int GetNumCaper()
	{
		return caperCount;
	}
	
	// Assigns the customer an order number from the spawn script
	public void SetIDNumber(int newID)
	{
		custID = newID;
	}
	
	// Call this method when the customer is dissatisfied
	public void MakeAngry()
	{
		anger += 1.0f;
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
		GUI.Label(new Rect((2 + (2 * custID)) * Screen.width / 18, 3 * Screen.height / 40, Screen.width / 9, Screen.height / 10), order, orderStyle);
		
		// Set style for time meter display
		GUIStyle barStyle = new GUIStyle();
		barFill.SetPixel(0, 0, new Color(0.5f, 1.0f, 0.5f, 1.0f));
		barFill.Apply();
		barStyle.normal.background = barFill;
		
		// Draw bar indicating time remaining
		GUI.Box(new Rect((5 + (4 * custID)) * Screen.width / 36, 3 * Screen.height / 20, (timer / (BASE_TIMER * numIngredients)) * Screen.width / 18, Screen.height / 20), GUIContent.none, barStyle);
	}
}
