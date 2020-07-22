using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
	// Customer game object being spawned
	public GameObject customer;
	
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

    // Update is called once per frame
    void Update()
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
					spawnPos = Random.Range(1, 5);
					switch (spawnPos)
					{
						case 1:
							if (!isOccupied1)
							{
								vacancyFound = true;
								GameObject nextCust = Instantiate(customer) as GameObject;
								nextCust.transform.position = custPos1;
								nextCust.GetComponent<CustomerOrder>().SetIDNumber(1);
								isOccupied1 = true;
							}
							break;
						case 2:
							if (!isOccupied2)
							{
								vacancyFound = true;
								GameObject nextCust = Instantiate(customer) as GameObject;
								nextCust.transform.position = custPos2;
								nextCust.GetComponent<CustomerOrder>().SetIDNumber(2);
								isOccupied2 = true;
							}
							break;
						case 3:
							if (!isOccupied3)
							{
								vacancyFound = true;
								GameObject nextCust = Instantiate(customer) as GameObject;
								nextCust.transform.position = custPos3;
								nextCust.GetComponent<CustomerOrder>().SetIDNumber(3);
								isOccupied3 = true;
							}
							break;
						case 4:
							if (!isOccupied4)
							{
								vacancyFound = true;
								GameObject nextCust = Instantiate(customer) as GameObject;
								nextCust.transform.position = custPos4;
								nextCust.GetComponent<CustomerOrder>().SetIDNumber(4);
								isOccupied4 = true;
							}
							break;
						case 5:
							if (!isOccupied5)
							{
								vacancyFound = true;
								GameObject nextCust = Instantiate(customer) as GameObject;
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
			spawnTimer = Random.Range(10.0f, 30.0f);
		}
    }
}
