using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static CustomerSpawn;

public class P2Movement : MonoBehaviour
{
	// Constant for movement speed
	const float SPEED = 5.0f;
	
	// Constant for chopping duration
	const float COOLDOWN = 3.0f;
	
	// Current score and time remaining
	int score;
	float timeLeft;
	
	// Areas that can be interacted with
	public GameObject chopBoard;
	public GameObject sparePlate;
	public GameObject trash;
	public GameObject veggie1;
	public GameObject veggie2;
	public GameObject veggie3;
	public GameObject veggie4;
	public GameObject veggie5;
	public GameObject veggie6;
	public GameObject order1;
	public GameObject order2;
	public GameObject order3;
	public GameObject order4;
	public GameObject order5;
	
	// Prefabs for carrying veggies and salads
	public GameObject vLettuce;
	public GameObject vTomato;
	public GameObject vCarrot;
	public GameObject vCheese;
	public GameObject vTurnip;
	public GameObject vCaper;
	public GameObject vSalad;
	
	// Manager for customer interaction
	public GameObject service;
	
	// Variables to manage veggies being held or placed
	bool leftHandOccupied;
	bool rightHandOccupied;
	bool chopBoardOccupied;
	bool sparePlateOccupied;
	GameObject leftHandVeggie;
	GameObject rightHandVeggie;
	GameObject boardVeggie;
	GameObject boardSalad;
	GameObject plateVeggie;
	
	// Controls whether the player can be moved or not
	bool canMove;
	float moveTimer;
	
	// Texture for move timer
	Texture2D barFill;
	
    // Start is called before the first frame update
    void Start()
    {
		// Initialize score and timer
		score = 0;
		timeLeft = 180.0f;
		
		// Initialize hands and counter as not holding anything
        leftHandOccupied = false;
		rightHandOccupied = false;
		chopBoardOccupied = false;
		sparePlateOccupied = false;
		
		// Initialize player as being able to move
		canMove = true;
		moveTimer = 0.0f;
		
		// Initialize texture for chopping board timer
		barFill = new Texture2D(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
		// Update time remaining
		if (timeLeft > 0.0)
		{
			timeLeft -= Time.deltaTime;
		}
		else
		{
			canMove = false;
		}
		
		// Get and store current position of player
		Vector3 pos = this.transform.position;
		
		// Update when player is able to move
		if (!canMove)
		{
			if (timeLeft > 0.0)
			{
				moveTimer -= Time.deltaTime;
				if (moveTimer <= 0.0)
				{
					AddVeggie(boardVeggie);
					Destroy(boardVeggie);
					canMove = true;
				}
			}
		}
		else
		{
			// Arrow key movement for Player 2, stay in bounds, don't move when you shouldn't
        	if (Input.GetKey(KeyCode.LeftArrow))
			{
				pos.x -= SPEED * Time.deltaTime;
				if (pos.x < -6.4)
				{
					pos.x = -6.4f;
				}
				this.transform.position = pos;
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				pos.x += SPEED * Time.deltaTime;
				if (pos.x > 6.4)
				{
					pos.x = 6.4f;
				}
				this.transform.position = pos;
			}
			if (Input.GetKey(KeyCode.DownArrow))
			{
				pos.y -= SPEED * Time.deltaTime;
				if (pos.y < -2.4)
				{
					pos.y = -2.4f;
				}
				this.transform.position = pos;
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				pos.y += SPEED * Time.deltaTime;
				if (pos.y > 2.4)
				{
					pos.y = 2.4f;
				}
				this.transform.position = pos;
			}
			
			// Press Enter to interact with nearby area
			if (Input.GetKeyDown(KeyCode.Return))
			{
				CheckObjects(pos);
			}
		}
		
		// Update veggie positions to match player's position if they are being held
		if (leftHandOccupied)
		{
			pos.x -= 0.5f;
			leftHandVeggie.transform.position = pos;
			if (rightHandOccupied)
			{
				pos.x += 1.0f;
				rightHandVeggie.transform.position = pos;
			}
		}
    }
	
	// Checks proximity with stations and interacts with them
	void CheckObjects(Vector3 pos)
	{
		// Cutting board: hold still and chop a vegetable if player is holding one
		if (Vector3.Distance(pos, chopBoard.transform.position) <= 1.5)
		{
			if (leftHandOccupied && leftHandVeggie.tag != "Salad")
			{
				boardVeggie = Instantiate(leftHandVeggie) as GameObject;
				Vector3 surface = chopBoard.transform.position;
				surface.x -= 0.25f;
				surface.z -= 0.75f;
				boardVeggie.transform.position = surface;
				leftHandOccupied = false;
				Destroy(leftHandVeggie);
				
				// Move right hand veggie to occupy left hand for next interaction
				if (rightHandOccupied)
				{
					leftHandVeggie = Instantiate(rightHandVeggie) as GameObject;
					if (rightHandVeggie.tag == "Salad")
					{
						rightHandVeggie.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
						leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					leftHandOccupied = true;
					rightHandOccupied = false;
					Destroy(rightHandVeggie);
				}
				
				// Start timer for chopping veggie
				canMove = false;
				moveTimer = COOLDOWN;
			}
			else if (chopBoardOccupied)
			{
				if (!leftHandOccupied)
				{
					leftHandVeggie = Instantiate(boardSalad) as GameObject;
					boardSalad.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
					leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					leftHandOccupied = true;
					chopBoardOccupied = false;
					Destroy(boardSalad);
				}
				else if (!rightHandOccupied)
				{
					rightHandVeggie = Instantiate(boardSalad) as GameObject;
					boardSalad.GetComponent<SaladType>().MoveSalad(rightHandVeggie);
					rightHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					rightHandOccupied = true;
					chopBoardOccupied = false;
					Destroy(boardSalad);
				}
			}
		}
		
		// Spare plate: leave a veggie you're holding if empty, pick veggie up if full
		if (Vector3.Distance(pos, sparePlate.transform.position) <= 1.4)
		{
			if (leftHandOccupied && !sparePlateOccupied)
			{
				plateVeggie = Instantiate(leftHandVeggie) as GameObject;
				if (leftHandVeggie.tag == "Salad")
				{
					leftHandVeggie.GetComponent<SaladType>().MoveSalad(plateVeggie);
					plateVeggie.GetComponent<SaladType>().SetP1Owns(false);
				}
				Vector3 surface = sparePlate.transform.position;
				surface.z -= 0.25f;
				plateVeggie.transform.position = surface;
				sparePlateOccupied = true;
				leftHandOccupied = false;
				Destroy(leftHandVeggie);
				
				// Move right hand veggie to occupy left hand for next interaction
				if (rightHandOccupied)
				{
					leftHandVeggie = Instantiate(rightHandVeggie) as GameObject;
					if (rightHandVeggie.tag == "Salad")
					{
						rightHandVeggie.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
						leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					leftHandOccupied = true;
					rightHandOccupied = false;
					Destroy(rightHandVeggie);
				}
			}
			else if (sparePlateOccupied)
			{
				if (!leftHandOccupied)
				{
					leftHandVeggie = Instantiate(plateVeggie) as GameObject;
					if (plateVeggie.tag == "Salad")
					{
						plateVeggie.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
						leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					leftHandOccupied = true;
					sparePlateOccupied = false;
					Destroy(plateVeggie);
				}
				else if (!rightHandOccupied)
				{
					rightHandVeggie = Instantiate(plateVeggie) as GameObject;
					if (plateVeggie.tag == "Salad")
					{
						plateVeggie.GetComponent<SaladType>().MoveSalad(rightHandVeggie);
						rightHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					rightHandOccupied = true;
					sparePlateOccupied = false;
					Destroy(plateVeggie);
				}
			}
		}
		
		// Trash can: discard whatever you're holding in your left hand
		if (Vector3.Distance(pos, trash.transform.position) <= 1.35)
		{
			if (leftHandOccupied)
			{
				if (leftHandVeggie.tag == "Salad")
				{
					score -= leftHandVeggie.GetComponent<SaladType>().GetNumIngredients() * 5;
				}
				else
				{
					score--;
				}
				leftHandOccupied = false;
				Destroy(leftHandVeggie);
				
				// Move right hand veggie to occupy left hand for next interaction
				if (rightHandOccupied)
				{
					leftHandVeggie = Instantiate(rightHandVeggie) as GameObject;
					if (rightHandVeggie.tag == "Salad")
					{
						rightHandVeggie.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
						leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					leftHandOccupied = true;
					rightHandOccupied = false;
					Destroy(rightHandVeggie);
				}
			}
		}
		
		// Veggies: grab an instance of the veggie you're in front of if you can carry it
		if (Vector3.Distance(pos, veggie1.transform.position) <= 1.25)
		{
			if (!leftHandOccupied)
			{
				leftHandVeggie = Instantiate(vLettuce) as GameObject;
				leftHandOccupied = true;
			}
			else if (!rightHandOccupied)
			{
				rightHandVeggie = Instantiate(vLettuce) as GameObject;
				rightHandOccupied = true;
			}
		}
		if (Vector3.Distance(pos, veggie2.transform.position) <= 1.25)
		{
			if (!leftHandOccupied)
			{
				leftHandVeggie = Instantiate(vTomato) as GameObject;
				leftHandOccupied = true;
			}
			else if (!rightHandOccupied)
			{
				rightHandVeggie = Instantiate(vTomato) as GameObject;
				rightHandOccupied = true;
			}
		}
		if (Vector3.Distance(pos, veggie3.transform.position) <= 1.25)
		{
			if (!leftHandOccupied)
			{
				leftHandVeggie = Instantiate(vCarrot) as GameObject;
				leftHandOccupied = true;
			}
			else if (!rightHandOccupied)
			{
				rightHandVeggie = Instantiate(vCarrot) as GameObject;
				rightHandOccupied = true;
			}
		}
		if (Vector3.Distance(pos, veggie4.transform.position) <= 1.25)
		{
			if (!leftHandOccupied)
			{
				leftHandVeggie = Instantiate(vCheese) as GameObject;
				leftHandOccupied = true;
			}
			else if (!rightHandOccupied)
			{
				rightHandVeggie = Instantiate(vCheese) as GameObject;
				rightHandOccupied = true;
			}
		}
		if (Vector3.Distance(pos, veggie5.transform.position) <= 1.25)
		{
			if (!leftHandOccupied)
			{
				leftHandVeggie = Instantiate(vTurnip) as GameObject;
				leftHandOccupied = true;
			}
			else if (!rightHandOccupied)
			{
				rightHandVeggie = Instantiate(vTurnip) as GameObject;
				rightHandOccupied = true;
			}
		}
		if (Vector3.Distance(pos, veggie6.transform.position) <= 1.25)
		{
			if (!leftHandOccupied)
			{
				leftHandVeggie = Instantiate(vCaper) as GameObject;
				leftHandOccupied = true;
			}
			else if (!rightHandOccupied)
			{
				rightHandVeggie = Instantiate(vCaper) as GameObject;
				rightHandOccupied = true;
			}
		}
		
		// Customers: give salad to customer if both are present, compare salad and order to see if they match
		if (Vector3.Distance(pos, order1.transform.position) <= 1.4)
		{
			if (leftHandOccupied && leftHandVeggie.tag == "Salad" && service.GetComponent<CustomerSpawn>().GetOccupied1())
			{
				GameObject customer = GameObject.FindWithTag("Cust1");
				int ltc = customer.GetComponent<CustomerOrder>().GetNumLettuce();
				int tmt = customer.GetComponent<CustomerOrder>().GetNumTomato();
				int crt = customer.GetComponent<CustomerOrder>().GetNumCarrot();
				int chs = customer.GetComponent<CustomerOrder>().GetNumCheese();
				int tnp = customer.GetComponent<CustomerOrder>().GetNumTurnip();
				int cpr = customer.GetComponent<CustomerOrder>().GetNumCaper();
				if (leftHandVeggie.GetComponent<SaladType>().CheckSalad(ltc, tmt, crt, chs, tnp, cpr))
				{
					CustomerSpawn.SetOccupied(1, false);
					score += leftHandVeggie.GetComponent<SaladType>().GetNumIngredients() * 10;
					Destroy(customer);
				}
				else
				{
					customer.GetComponent<CustomerOrder>().MakeAngry();
				}
				leftHandOccupied = false;
				Destroy (leftHandVeggie);
				
				// Move right hand veggie to occupy left hand for next interaction
				if (rightHandOccupied)
				{
					leftHandVeggie = Instantiate(rightHandVeggie) as GameObject;
					if (rightHandVeggie.tag == "Salad")
					{
						rightHandVeggie.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
						leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					leftHandOccupied = true;
					rightHandOccupied = false;
					Destroy(rightHandVeggie);
				}
			}
		}
		if (Vector3.Distance(pos, order2.transform.position) <= 1.4)
		{
			if (leftHandOccupied && leftHandVeggie.tag == "Salad" && service.GetComponent<CustomerSpawn>().GetOccupied2())
			{
				GameObject customer = GameObject.FindWithTag("Cust2");
				int ltc = customer.GetComponent<CustomerOrder>().GetNumLettuce();
				int tmt = customer.GetComponent<CustomerOrder>().GetNumTomato();
				int crt = customer.GetComponent<CustomerOrder>().GetNumCarrot();
				int chs = customer.GetComponent<CustomerOrder>().GetNumCheese();
				int tnp = customer.GetComponent<CustomerOrder>().GetNumTurnip();
				int cpr = customer.GetComponent<CustomerOrder>().GetNumCaper();
				if (leftHandVeggie.GetComponent<SaladType>().CheckSalad(ltc, tmt, crt, chs, tnp, cpr))
				{
					CustomerSpawn.SetOccupied(2, false);
					score += leftHandVeggie.GetComponent<SaladType>().GetNumIngredients() * 10;
					Destroy(customer);
				}
				else
				{
					customer.GetComponent<CustomerOrder>().MakeAngry();
				}
				leftHandOccupied = false;
				Destroy (leftHandVeggie);
				
				// Move right hand veggie to occupy left hand for next interaction
				if (rightHandOccupied)
				{
					leftHandVeggie = Instantiate(rightHandVeggie) as GameObject;
					if (rightHandVeggie.tag == "Salad")
					{
						rightHandVeggie.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
						leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					leftHandOccupied = true;
					rightHandOccupied = false;
					Destroy(rightHandVeggie);
				}
			}
		}
		if (Vector3.Distance(pos, order3.transform.position) <= 1.4)
		{
			if (leftHandOccupied && leftHandVeggie.tag == "Salad" && service.GetComponent<CustomerSpawn>().GetOccupied3())
			{
				GameObject customer = GameObject.FindWithTag("Cust3");
				int ltc = customer.GetComponent<CustomerOrder>().GetNumLettuce();
				int tmt = customer.GetComponent<CustomerOrder>().GetNumTomato();
				int crt = customer.GetComponent<CustomerOrder>().GetNumCarrot();
				int chs = customer.GetComponent<CustomerOrder>().GetNumCheese();
				int tnp = customer.GetComponent<CustomerOrder>().GetNumTurnip();
				int cpr = customer.GetComponent<CustomerOrder>().GetNumCaper();
				if (leftHandVeggie.GetComponent<SaladType>().CheckSalad(ltc, tmt, crt, chs, tnp, cpr))
				{
					CustomerSpawn.SetOccupied(3, false);
					score += leftHandVeggie.GetComponent<SaladType>().GetNumIngredients() * 10;
					Destroy(customer);
				}
				else
				{
					customer.GetComponent<CustomerOrder>().MakeAngry();
				}
				leftHandOccupied = false;
				Destroy (leftHandVeggie);
				
				// Move right hand veggie to occupy left hand for next interaction
				if (rightHandOccupied)
				{
					leftHandVeggie = Instantiate(rightHandVeggie) as GameObject;
					if (rightHandVeggie.tag == "Salad")
					{
						rightHandVeggie.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
						leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					leftHandOccupied = true;
					rightHandOccupied = false;
					Destroy(rightHandVeggie);
				}
			}
		}
		if (Vector3.Distance(pos, order4.transform.position) <= 1.4)
		{
			if (leftHandOccupied && leftHandVeggie.tag == "Salad" && service.GetComponent<CustomerSpawn>().GetOccupied4())
			{
				GameObject customer = GameObject.FindWithTag("Cust4");
				int ltc = customer.GetComponent<CustomerOrder>().GetNumLettuce();
				int tmt = customer.GetComponent<CustomerOrder>().GetNumTomato();
				int crt = customer.GetComponent<CustomerOrder>().GetNumCarrot();
				int chs = customer.GetComponent<CustomerOrder>().GetNumCheese();
				int tnp = customer.GetComponent<CustomerOrder>().GetNumTurnip();
				int cpr = customer.GetComponent<CustomerOrder>().GetNumCaper();
				if (leftHandVeggie.GetComponent<SaladType>().CheckSalad(ltc, tmt, crt, chs, tnp, cpr))
				{
					CustomerSpawn.SetOccupied(4, false);
					score += leftHandVeggie.GetComponent<SaladType>().GetNumIngredients() * 10;
					Destroy(customer);
				}
				else
				{
					customer.GetComponent<CustomerOrder>().MakeAngry();
				}
				leftHandOccupied = false;
				Destroy (leftHandVeggie);
				
				// Move right hand veggie to occupy left hand for next interaction
				if (rightHandOccupied)
				{
					leftHandVeggie = Instantiate(rightHandVeggie) as GameObject;
					if (rightHandVeggie.tag == "Salad")
					{
						rightHandVeggie.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
						leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					leftHandOccupied = true;
					rightHandOccupied = false;
					Destroy(rightHandVeggie);
				}
			}
		}
		if (Vector3.Distance(pos, order5.transform.position) <= 1.4)
		{
			if (leftHandOccupied && leftHandVeggie.tag == "Salad" && service.GetComponent<CustomerSpawn>().GetOccupied5())
			{
				GameObject customer = GameObject.FindWithTag("Cust5");
				int ltc = customer.GetComponent<CustomerOrder>().GetNumLettuce();
				int tmt = customer.GetComponent<CustomerOrder>().GetNumTomato();
				int crt = customer.GetComponent<CustomerOrder>().GetNumCarrot();
				int chs = customer.GetComponent<CustomerOrder>().GetNumCheese();
				int tnp = customer.GetComponent<CustomerOrder>().GetNumTurnip();
				int cpr = customer.GetComponent<CustomerOrder>().GetNumCaper();
				if (leftHandVeggie.GetComponent<SaladType>().CheckSalad(ltc, tmt, crt, chs, tnp, cpr))
				{
					CustomerSpawn.SetOccupied(5, false);
					score += leftHandVeggie.GetComponent<SaladType>().GetNumIngredients() * 10;
					Destroy(customer);
				}
				else
				{
					customer.GetComponent<CustomerOrder>().MakeAngry();
				}
				leftHandOccupied = false;
				Destroy (leftHandVeggie);
				
				// Move right hand veggie to occupy left hand for next interaction
				if (rightHandOccupied)
				{
					leftHandVeggie = Instantiate(rightHandVeggie) as GameObject;
					if (rightHandVeggie.tag == "Salad")
					{
						rightHandVeggie.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
						leftHandVeggie.GetComponent<SaladType>().SetP1Owns(false);
					}
					leftHandOccupied = true;
					rightHandOccupied = false;
					Destroy(rightHandVeggie);
				}
			}
		}
	}
	
	// Adds a veggie to a salad once it has finished being chopped
	void AddVeggie(GameObject veggie)
	{
		// Make new salad for board if necessary
		if (!chopBoardOccupied)
		{
			boardSalad = Instantiate(vSalad) as GameObject;
			boardSalad.GetComponent<SaladType>().SetP1Owns(false);
			Vector3 surface = chopBoard.transform.position;
			surface.x += 0.25f;
			surface.z -= 0.5f;
			boardSalad.transform.position = surface;
			chopBoardOccupied = true;
		}
		
		// Add ingredient to salad on board
		if (veggie.tag == "Lettuce")
		{
			boardSalad.GetComponent<SaladType>().AddIngredient(1, 0, 0, 0, 0, 0);
		}
		else if (veggie.tag == "Tomato")
		{
			boardSalad.GetComponent<SaladType>().AddIngredient(0, 1, 0, 0, 0, 0);
		}
		else if (veggie.tag == "Carrot")
		{
			boardSalad.GetComponent<SaladType>().AddIngredient(0, 0, 1, 0, 0, 0);
		}
		else if (veggie.tag == "Cheese")
		{
			boardSalad.GetComponent<SaladType>().AddIngredient(0, 0, 0, 1, 0, 0);
		}
		else if (veggie.tag == "Turnip")
		{
			boardSalad.GetComponent<SaladType>().AddIngredient(0, 0, 0, 0, 1, 0);
		}
		else if (veggie.tag == "Caper")
		{
			boardSalad.GetComponent<SaladType>().AddIngredient(0, 0, 0, 0, 0, 1);
		}
	}
	
	// OnGUI is called to draw text for the player
	void OnGUI()
	{
		// Set font style for stat display
		GUIStyle scoreStyle = new GUIStyle();
		scoreStyle.alignment = TextAnchor.MiddleRight;
		scoreStyle.normal.textColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
		
		// Determine time remaining to display
		int minLeft = (int)(timeLeft / 60);
		int secLeft = (int)(timeLeft % 60);
		string timeDisplay;
		if (secLeft < 10)
		{
			timeDisplay = "Time: " + minLeft.ToString() + ":0" + secLeft.ToString();
		}
		else
		{
			timeDisplay = "Time: " + minLeft.ToString() + ":" + secLeft.ToString();
		}
		
		// Output player's score and time in bottom left corner
		GUI.Label(new Rect(15 * Screen.width / 18, 8 * Screen.height / 10, Screen.width / 9, Screen.height / 10), "Score: " + score.ToString(), scoreStyle);
		GUI.Label(new Rect(15 * Screen.width / 18, 17 * Screen.height / 20, Screen.width / 9, Screen.height / 10), timeDisplay, scoreStyle);
		
		// Draw cooldown bar only when using the cutting board
		if (moveTimer > 0.0)
		{
			// Set style for time meter display
			GUIStyle barStyle = new GUIStyle();
			barFill.SetPixel(0, 0, new Color(0.0f, 0.0f, 1.0f, 1.0f));
			barFill.Apply();
			barStyle.normal.background = barFill;
			
			// Draw bar indicating time remaining
			GUI.Box(new Rect(5 * Screen.width / 9, 9 * Screen.height / 10, (Screen.width / 9) - ((moveTimer / COOLDOWN) * Screen.width / 9), Screen.height / 20), GUIContent.none, barStyle);
		}
	}
}
