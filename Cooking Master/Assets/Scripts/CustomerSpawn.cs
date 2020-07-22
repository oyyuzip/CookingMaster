using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
	// Customer game objects being spawned
	public GameObject customer1;
	public GameObject customer2;
	public GameObject customer3;
	public GameObject customer4;
	public GameObject customer5;
	
	// Determine vacancy of customer counter
	static bool isOccupied1;
	static bool isOccupied2;
	static bool isOccupied3;
	static bool isOccupied4;
	static bool isOccupied5;
	
	// Cooldown timer between customer arrivals
	float spawnTimer;
	
	// Spawn locations for different positions
	Vector3 custPos1 = new Vector3(-4.0f, 4.5f, -0.5f);
	Vector3 custPos2 = new Vector3(-2.0f, 4.5f, -0.5f);
	Vector3 custPos3 = new Vector3(0.0f, 4.5f, -0.5f);
	Vector3 custPos4 = new Vector3(2.0f, 4.5f, -0.5f);
	Vector3 custPos5 = new Vector3(4.0f, 4.5f, -0.5f);
	
	// Booleans determine when the game is over
	bool p1Done;
	bool p2Done;
	static bool gameOver;
	
	// Keep track of players' scores to determine winner
	int p1Score;
	int p2Score;
	string result;
	
    // Start is called before the first frame update
    void Start()
    {
        // Counter is initially empty when restaurant opens
		isOccupied1 = false;
		isOccupied2 = false;
		isOccupied3 = false;
		isOccupied4 = false;
		isOccupied5 = false;
		
		// First customer should arrive after a few seconds
		spawnTimer = 3.0f;
		
		// Both players are active when game starts
		p1Done = false;
		p2Done = false;
		gameOver = false;
    }
	
	// Public get methods indicating which seats have customers at them
	public bool GetOccupied1()
	{
		return isOccupied1;
	}
	
	public bool GetOccupied2()
	{
		return isOccupied2;
	}
	
	public bool GetOccupied3()
	{
		return isOccupied3;
	}
	
	public bool GetOccupied4()
	{
		return isOccupied4;
	}
	
	public bool GetOccupied5()
	{
		return isOccupied5;
	}
	
	// Blanket set method changing the vacancy of a specified seat
	public static void SetOccupied(int seatNum, bool occupation)
	{
		switch (seatNum)
		{
			case 1:
				isOccupied1 = occupation;
				break;
			case 2:
				isOccupied2 = occupation;
				break;
			case 3:
				isOccupied3 = occupation;
				break;
			case 4:
				isOccupied4 = occupation;
				break;
			case 5:
				isOccupied5 = occupation;
				break;
			default:
				break;
		}
	}
	
	// Other classes can check if the game is over
	public static bool GameIsOver()
	{
		return gameOver;
	}
	
	// Players assign when they have run out of time
	public void PlayerFinished(int pNum, int score)
	{
		if (pNum == 1)
		{
			p1Done = true;
			p1Score = score;
		}
		if (pNum == 2)
		{
			p2Done = true;
			p2Score = score;
		}
	}

    // Update is called once per frame
    void Update()
    {
		// Only progress customer spawning if game has not finished
		if (!gameOver)
		{
	        // Update the spawn timer if nothing else
			spawnTimer -= Time.deltaTime;
			
			// Spawn new customer when timer expires and space is available
			if (spawnTimer <= 0.0)
			{
				if (!(isOccupied1 && isOccupied2 && isOccupied3 && isOccupied4 && isOccupied5))
				{
					// Look for an open seat at the counter
					int spawnPos;
					bool vacancyFound = false;
					do
					{
						spawnPos = Random.Range(1, 6);
						switch (spawnPos)
						{
							case 1:
								if (!isOccupied1)
								{
									vacancyFound = true;
									GameObject nextCust = Instantiate(customer1) as GameObject;
									nextCust.transform.position = custPos1;
									nextCust.GetComponent<CustomerOrder>().SetIDNumber(1);
									isOccupied1 = true;
								}
								break;
							case 2:
								if (!isOccupied2)
								{
									vacancyFound = true;
									GameObject nextCust = Instantiate(customer2) as GameObject;
									nextCust.transform.position = custPos2;
									nextCust.GetComponent<CustomerOrder>().SetIDNumber(2);
									isOccupied2 = true;
								}
								break;
							case 3:
								if (!isOccupied3)
								{
									vacancyFound = true;
									GameObject nextCust = Instantiate(customer3) as GameObject;
									nextCust.transform.position = custPos3;
									nextCust.GetComponent<CustomerOrder>().SetIDNumber(3);
									isOccupied3 = true;
								}
								break;
							case 4:
								if (!isOccupied4)
								{
									vacancyFound = true;
									GameObject nextCust = Instantiate(customer4) as GameObject;
									nextCust.transform.position = custPos4;
									nextCust.GetComponent<CustomerOrder>().SetIDNumber(4);
									isOccupied4 = true;
								}
								break;
							case 5:
								if (!isOccupied5)
								{
									vacancyFound = true;
									GameObject nextCust = Instantiate(customer5) as GameObject;
									nextCust.transform.position = custPos5;
									nextCust.GetComponent<CustomerOrder>().SetIDNumber(5);
									isOccupied5 = true;
								}
								break;
							default:
								break;
						}
					} while (!vacancyFound);
				}
				
				// Reset timer for next customer spawn
				spawnTimer = Random.Range(5.0f, 20.0f);
			}
			
			// Check players to see if game is over
			if (p1Done && p2Done)
			{
				if (p1Score > p2Score)
				{
					result = "RED CHEF WINS!";
				}
				else if (p1Score < p2Score)
				{
					result = "BLUE CHEF WINS!";
				}
				else
				{
					result = "IT'S A TIE!";
				}
				gameOver = true;
			}
		}
    }
	
	// OnGUI is called to draw text for the player
	void OnGUI()
	{
		// Set font style for ingredient display
		GUIStyle orderStyle = new GUIStyle();
		orderStyle.alignment = TextAnchor.MiddleCenter;
		
		// Label each ingredient appropriately
		GUI.Label(new Rect(-Screen.width / 36, 3 * Screen.height / 10, Screen.width / 9, Screen.height / 10), "Lt", orderStyle);
		GUI.Label(new Rect(-Screen.width / 36, 9 * Screen.height / 20, Screen.width / 9, Screen.height / 10), "Tm", orderStyle);
		GUI.Label(new Rect(-Screen.width / 36, 3 * Screen.height / 5, Screen.width / 9, Screen.height / 10), "Ct", orderStyle);
		GUI.Label(new Rect(33 * Screen.width / 36, 3 * Screen.height / 10, Screen.width / 9, Screen.height / 10), "Ch", orderStyle);
		GUI.Label(new Rect(33 * Screen.width / 36, 9 * Screen.height / 20, Screen.width / 9, Screen.height / 10), "Tr", orderStyle);
		GUI.Label(new Rect(33 * Screen.width / 36, 3 * Screen.height / 5, Screen.width / 9, Screen.height / 10), "Cp", orderStyle);
		
		// Display victory message when game is over
		if (gameOver)
		{
			GUI.Label(new Rect(4 * Screen.width / 9, 9 * Screen.height / 20, Screen.width / 9, Screen.height / 10), result, orderStyle);
		}
	}
}
