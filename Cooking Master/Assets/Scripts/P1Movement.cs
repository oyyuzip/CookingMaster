using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Movement : MonoBehaviour
{
	// Constant for movement speed
	const float speed = 5.0f;
	
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
	
    // Start is called before the first frame update
    void Start()
    {
		// Initialize hands and counter as not holding anything
        leftHandOccupied = false;
		rightHandOccupied = false;
		chopBoardOccupied = false;
		sparePlateOccupied = false;
		
		// Initialize player as being able to move
		canMove = true;
		moveTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
		// Get and store current position of player
		Vector3 pos = this.transform.position;
		
		// Update when player is able to move
		if (!canMove)
		{
			moveTimer -= Time.deltaTime;
			if (moveTimer <= 0.0)
			{
				AddVeggie(boardVeggie);
				Destroy(boardVeggie);
				canMove = true;
			}
		}
		else
		{
			// WASD Movement for Player 1, stay in bounds, don't move when you shouldn't
        	if (Input.GetKey(KeyCode.A))
			{
				pos.x -= speed * Time.deltaTime;
				if (pos.x < -6.4)
				{
					pos.x = -6.4f;
				}
				this.transform.position = pos;
			}
			if (Input.GetKey(KeyCode.D))
			{
				pos.x += speed * Time.deltaTime;
				if (pos.x > 6.4)
				{
					pos.x = 6.4f;
				}
				this.transform.position = pos;
			}
			if (Input.GetKey(KeyCode.S))
			{
				pos.y -= speed * Time.deltaTime;
				if (pos.y < -2.4)
				{
					pos.y = -2.4f;
				}
				this.transform.position = pos;
			}
			if (Input.GetKey(KeyCode.W))
			{
				pos.y += speed * Time.deltaTime;
				if (pos.y > 2.4)
				{
					pos.y = 2.4f;
				}
				this.transform.position = pos;
			}
			
			// Press Space to interact with nearby area
			if (Input.GetKeyDown(KeyCode.Space))
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
					}
					leftHandOccupied = true;
					rightHandOccupied = false;
					Destroy(rightHandVeggie);
				}
				
				// Start timer for chopping veggie
				canMove = false;
				moveTimer = 3.0f;
			}
			else if (chopBoardOccupied)
			{
				if (!leftHandOccupied)
				{
					leftHandVeggie = Instantiate(boardSalad) as GameObject;
					boardSalad.GetComponent<SaladType>().MoveSalad(leftHandVeggie);
					leftHandOccupied = true;
					chopBoardOccupied = false;
					Destroy(boardSalad);
				}
				else if (!rightHandOccupied)
				{
					rightHandVeggie = Instantiate(boardSalad) as GameObject;
					boardSalad.GetComponent<SaladType>().MoveSalad(rightHandVeggie);
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
				leftHandOccupied = false;
				Destroy(leftHandVeggie);
				
				// Move right hand veggie to occupy left hand for next interaction
				if (rightHandOccupied)
				{
					leftHandVeggie = Instantiate(rightHandVeggie) as GameObject;
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
	}
	
	// Adds a veggie to a salad once it has finished being chopped
	void AddVeggie(GameObject veggie)
	{
		// Make new salad for board if necessary
		if (!chopBoardOccupied)
		{
			boardSalad = Instantiate(vSalad) as GameObject;
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
}
