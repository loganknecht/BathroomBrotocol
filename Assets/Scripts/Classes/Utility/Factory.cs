using UnityEngine;

public class Factory
{
	//SINGLETON CONFIGURATION INFORMATION GOES HERE
	private static volatile Factory _instance;
	private static object _lock = new object();

	//Stops the lock being created ahead of time if it's not necessary
	static Factory() {
	}

	public static Factory Instance {
		get {
			if (_instance == null) {
				lock(_lock) {
					if (_instance == null)
						_instance = new Factory();
				}
			}
			return _instance;
		}
	}

	private Factory() {
	}

	// Use this for initialization
	void Awake() {
		//There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
		//the script, which is assumedly attached to some GameObject. This in turn allows the instance
		//to be assigned when a game object is given this script in the scene view.
		//This also allows the pre-configured lazy instantiation to occur when the script is referenced from
		//another call to it, so that you don't need to worry if it exists or not.
		_instance = this;
	}
	//END OF SINGLETON CODE CONFIGURATION

	//-------------------------------------------------------------------------
	// Generation Code
	//-------------------------------------------------------------------------
	public GameObject GenerateBathroomObject(BathroomObjectType typeOfBathroomObjectToGenerate) {
		GameObject newBathroomObject = null;

		switch(typeOfBathroomObjectToGenerate) {
		case BathroomObjectType.None:
			break;
		case BathroomObjectType.Exit:
			newBathroomObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BathroomObjects/Exit") as GameObject);
			break;
		case BathroomObjectType.Sink:
			newBathroomObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BathroomObjects/Sink") as GameObject);
			break;
		case BathroomObjectType.Stall:
			newBathroomObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BathroomObjects/Stall") as GameObject);
			break;
		case BathroomObjectType.Urinal:
			newBathroomObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BathroomObjects/Urinal") as GameObject);
			break;
		default:
			Debug.Log("Bummer, something went wrong in the method: \"GenerateBathroomObjectTypeOnBorderTilesRandomly()\"");
			break;
		}

		return newBathroomObject;
	}

	public GameObject GenerateBroGameObject(BroType broTypeToGenerate, Vector3 startingPosition) {
		GameObject newBro = GenerateBroGameObject(broTypeToGenerate);

		if(newBro != null) {
			newBro.transform.position = new Vector3(startingPosition.x, startingPosition.y, newBro.transform.position.z);
		}

		return newBro;
	}

  public GameObject GenerateBroGameObject(BroType broTypeToGenerate) {
    GameObject newBro = null;
    switch(broTypeToGenerate) {
      case(BroType.None):
        Debug.Log("There was a none type sent to the Factory for a bro. If this was not intended please check the logic.");
      break;
      case(BroType.BluetoothBro):
        newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/BluetoothBro1")) as GameObject;
      break;
      case(BroType.ChattyBro):
        newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/ChattyBro1")) as GameObject;
      break;
      case(BroType.CuttingBro):
        newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/CuttingBro1")) as GameObject;
      break;
      case(BroType.DrunkBro):
        newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/DrunkBro1")) as GameObject;
        // newBro.GetComponent<DrunkBro>().vomitTimerThreshold = Random.Range(10f, 20f);
      break;
      case(BroType.GassyBro):
        newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/GassyBro1")) as GameObject;
      break;
      case(BroType.GenericBro):
          newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/GenericBro1")) as GameObject;
      break;
      case(BroType.RichBro):
          newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/RichBro1")) as GameObject;
      break;
      case(BroType.ShyBro):
          newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/ShyBro1")) as GameObject;
      break;
      case(BroType.SlobBro):
          newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/SlobBro1")) as GameObject;
      break;
      case(BroType.TimeWasterBro):
        newBro = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Bro/TimeWasterBro1")) as GameObject;
      break;
      default:
        Debug.Log("Something broke in the \"GenerateBroGameObject\" method in the \"Factory\"");
      break;
    }

    return newBro;
  }

  public GameObject GenerateBathroomTileBlockerObject(BathroomTileBlockerType typeOfBathroomTileBlockerObjectToGenerate) {
    GameObject newBathroomTileBlockerObject = null;

    switch(typeOfBathroomTileBlockerObjectToGenerate) {
      case(BathroomTileBlockerType.None):
        break;
      case(BathroomTileBlockerType.Fart):
        newBathroomTileBlockerObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BathroomTileBlockers/Fart") as GameObject);
        break;
      case(BathroomTileBlockerType.Vomit):
        newBathroomTileBlockerObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/BathroomTileBlockers/Vomit") as GameObject);
        break;
      default:
        Debug.Log("Bummer, something went wrong in the 'Factory.cs' method: \"GenerateBathroomTileBlockerObject()\"");
        break;
    }

    return newBathroomTileBlockerObject;
  }
	//-------------------------------------------------------------------------
	// Selection Code
	//-------------------------------------------------------------------------
	public BathroomWall SelectRandomBathroomWall() {
		int wallSelected = Random.Range(0,4);

		BathroomWall wallToReturn = BathroomWall.None;

		switch(wallSelected)
		{
		case 0:
			wallToReturn = BathroomWall.Top;
			break;
		case 1:
			wallToReturn = BathroomWall.Right;
			break;
		case 2:
			wallToReturn = BathroomWall.Bottom;
			break;
		case 3:
			wallToReturn = BathroomWall.Left;
			break;
		default:
			wallToReturn = BathroomWall.None;
			break;
		}

		return wallToReturn;
	}

	public BroType SelectRandomBroType() {
		BroType broType = BroType.None;

		int broNumber = Random.Range(0, 5);

		switch(broNumber) {
		case(0):
			broType = BroType.GenericBro;
			break;
		case(1):
			broType = BroType.DrunkBro;
			break;
		case(2):
			broType = BroType.ChattyBro;
			break;
		case(3):
			broType = BroType.BluetoothBro;
			break;
		case(4):
			broType = BroType.GassyBro;
			break;
		default:
			broType = BroType.None;
			break;
		}

		return broType;
	}

	public ReliefRequired SelectRandomReliefRequired() {
		ReliefRequired reliefRequired = ReliefRequired.None;

		int reliefNumber = Random.Range(0, 2);

		switch(reliefNumber) {
  		case(0):
  			reliefRequired = ReliefRequired.Pee;
  			break;
  		case(1):
  			reliefRequired = ReliefRequired.Poop;
  			break;
  		default:
  			reliefRequired = ReliefRequired.None;
  			break;
		}

		return reliefRequired;
	}

  public ReliefRequired SelectRandomReliefType(params ReliefRequired[] itemsToChooseFrom) {
    if(itemsToChooseFrom.Length == 0) {
      Debug.Log("THE 'SelectRandomReliefType()' METHOD IN THE FACTORY WAS CALLED WITH ZERO PARAMETERS, PLEASE FIND AND RECTIFY THIS ISSSUE");

      return ReliefRequired.None;
    }

    int selectedIndex = Random.Range(0, itemsToChooseFrom.Length);
    // Debug.Log("Selected the item: " + itemsToChooseFrom[selectedIndex].ToString());
    return itemsToChooseFrom[selectedIndex];
  }

  public BathroomObjectType SelectRandomBathroomObjectType(params BathroomObjectType[] itemsToChooseFrom) {
    if(itemsToChooseFrom.Length == 0) {
      Debug.Log("THE 'SelectRandomReliefType()' METHOD IN THE FACTORY WAS CALLED WITH ZERO PARAMETERS, PLEASE FIND AND RECTIFY THIS ISSSUE");

      return BathroomObjectType.None;
    }

    int selectedIndex = Random.Range(0, itemsToChooseFrom.Length);
    // Debug.Log("Selected the item: " + itemsToChooseFrom[selectedIndex].ToString());
    return itemsToChooseFrom[selectedIndex];
  }
	//-------------------------------------------------------------------------
}
