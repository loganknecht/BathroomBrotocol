using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarManager : MonoBehaviour {

    //These should be considered the de facto closed nodes. Reason being is that additional nodes can be added as closed nodes, but these should be considered permanently closed nodes for the sake of having this covered.... it sounded right in my head
	public List<GameObject> closedNodes = new List<GameObject>();

	//BEGINNING OF SINGLETON CODE CONFIGURATION
	private static volatile AStarManager _instance;
	private static object _lock = new object();

    public bool debugShowLastPathCalculated = false;
    public List<GameObject> debugLastPathNodes = new List<GameObject>();

	//Stops the lock being created ahead of time if it's not necessary
	static AStarManager() {
	}

	public static AStarManager Instance {
		get {
			if(_instance == null) {
				lock(_lock) {
					if (_instance == null) {
						GameObject aStarManagerGameObject = new GameObject("AStarManagerGameObject");
						_instance = (aStarManagerGameObject.AddComponent<AStarManager>()).GetComponent<AStarManager>();
					}
				}
			}
			return _instance;
		}
	}

	private AStarManager() {
	}

	public void Awake() {
		//There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
		//the script, which is assumedly attached to some GameObject. This in turn allows the instance
		//to be assigned when a game object is given this script in the scene view.
		//This also allows the pre-configured lazy instantiation to occur when the script is referenced from
		//another call to it, so that you don't need to worry if it exists or not.
		_instance = this;
	}
	//END OF SINGLETON CODE CONFIGURATION

	// Use this for initialization
	void Start () {
        // ConfigurePermantleyClosedNodes();
	}

	// Update is called once per frame
    void Update () {
        // if(!debugShowLastPathCalculated) {
        //     foreach(GameObject gameObj in debugLastPathNodes) {
        //       // removeDebugTiles.Add(gameObj);
        //       Destroy(gameObj);
        //     }
        //     debugLastPathNodes.Clear();
        // }
    }

  public void ConfigurePermantleyClosedNodes() {
    // List<GameObject> untraversableTiles = BathroomTileMap.Instance.GetAllUntraversableTiles();
    // foreach(GameObject untraversableTile in untraversableTiles) {
    //   if(!closedNodes.Contains(untraversableTile)) {
    //     closedNodes.Add(untraversableTile);
    //   }
    // }
  }
//-----------------------------
	public List<Vector2> CalculateAStarPath(List<GameObject> openNodes, List<GameObject> closedNodes, BathroomTile startBathroomTile, BathroomTile endBathroomTile) {
//     // Debug.Log("Start tile X: " + startBathroomTile.tileX + " Y: " + startBathroomTile.tileY);
//     // Debug.Log("End tile X: " + endBathroomTile.tileX + " Y: " + endBathroomTile.tileY);
// 		foreach(GameObject gameObj in BathroomTileMap.Instance.tiles) {
// 			gameObj.GetComponent<BathroomTile>().ResetAStarValues();
// 		}

// 		foreach(GameObject permClosedNode in AStarManager.Instance.closedNodes) {
//       GameObject referenceCopy = permClosedNode;
// 			closedNodes.Add(referenceCopy);
// 			//Debug.Log("Added permanently closed node: " + permClosedNode);
// 		}

// 		List<Vector2> bathroomTilePositionsForPath = new List<Vector2>();
// 		if(startBathroomTile.tileX == endBathroomTile.tileX
// 		   && startBathroomTile.tileY == endBathroomTile.tileY) {
// 			// Debug.Log("Start tile and end tile are the same, returned empty new list.");
// 			return bathroomTilePositionsForPath;
// 		}

// 		BathroomTile currentNode = null;
// 		BathroomTile previousNode = null;

// 		bool isEndBathroomTileInOpenNodeList = false;

// 		int xOffsetToCheck = 0;
// 		int yOffsetToCheck = 0;

// 		try {
// 			//Adds the first node to the starting list to kick it off
// 			//1) Add the starting square (or node) to the open list.
// 			openNodes.Add(startBathroomTile.gameObject);
// 			//openNodes.Remove(endBathroomTile.gameObject);


// 			//2) Repeat the following:
// 			while(!isEndBathroomTileInOpenNodeList) {
// 				//a) Look for the lowest F cost square on the open list. We refer to this as the current square.
// 				BathroomTile lowestCost = null;
// 				foreach(GameObject gameObj in openNodes) {
// 					BathroomTile openNode = gameObj.GetComponent<BathroomTile>();
// //					openNode.heuristicValue = GetManhattanDistance(openNode.tileX, openNode.tileY, endBathroomTile.tileX, endBathroomTile.tileY);
// 					if(lowestCost == null) {
// 						lowestCost = openNode;
// 					}
// 					else if(openNode.gValue + openNode.heuristicValue < lowestCost.gValue + lowestCost.heuristicValue) {
// 						lowestCost = openNode;
// 					}
// 				}

// 				previousNode = currentNode;
// 				currentNode = lowestCost;

// //				Debug.Log("----------------------------------------------------");
// //				Debug.Log("Current Node is: X: " + ((currentNode != null) ? currentNode.tileX.ToString() : "Null current node") + " Y: " + ((currentNode != null) ? currentNode.tileY.ToString() : "Null current node"));
// //				Debug.Log("Current Node Parent is: " + ((currentNode != null) ? ((currentNode.parentAStarNode != null) ? currentNode.parentAStarNode.ToString() : "Null parent node") : "Null current node"));
// //				Debug.Log("----------------------------------------------------");
// //				Debug.Log("Performing Top Left A Star Tile Check");
// 				xOffsetToCheck = -1;
// 				yOffsetToCheck = 1;
// 				PerformAStarCalculation(currentNode, endBathroomTile.tileX, endBathroomTile.tileY, openNodes, closedNodes, xOffsetToCheck, yOffsetToCheck);
// //				Debug.Log("----------------------------------------------------");
// //				Debug.Log("Performing Top A Star Tile Check");
// 				xOffsetToCheck = 0;
// 				yOffsetToCheck = 1;
// 				PerformAStarCalculation(currentNode, endBathroomTile.tileX, endBathroomTile.tileY, openNodes, closedNodes, xOffsetToCheck, yOffsetToCheck);
// //				Debug.Log("----------------------------------------------------");
// //				Debug.Log("Performing Top Right A Star Tile Check");
// 				xOffsetToCheck = 1;
// 				yOffsetToCheck = 1;
// 				PerformAStarCalculation(currentNode, endBathroomTile.tileX, endBathroomTile.tileY, openNodes, closedNodes, xOffsetToCheck, yOffsetToCheck);

// //				Debug.Log("----------------------------------------------------");
// //				Debug.Log("Performing Left A Star Tile Check");
// 				xOffsetToCheck = -1;
// 				yOffsetToCheck = 0;
// 				PerformAStarCalculation(currentNode, endBathroomTile.tileX, endBathroomTile.tileY, openNodes, closedNodes, xOffsetToCheck, yOffsetToCheck);
// //				Debug.Log("----------------------------------------------------");
// //				Debug.Log("Performing Right A Star Tile Check");
// 				xOffsetToCheck = 1;
// 				yOffsetToCheck = 0;
// 				PerformAStarCalculation(currentNode, endBathroomTile.tileX, endBathroomTile.tileY, openNodes, closedNodes, xOffsetToCheck, yOffsetToCheck);

// //				Debug.Log("----------------------------------------------------");
// //				Debug.Log("Performing Bottom Left A Star Tile Check");
// 				xOffsetToCheck = -1;
// 				yOffsetToCheck = -1;
// 				PerformAStarCalculation(currentNode, endBathroomTile.tileX, endBathroomTile.tileY, openNodes, closedNodes, xOffsetToCheck, yOffsetToCheck);
// //				Debug.Log("----------------------------------------------------");
// //				Debug.Log("Performing Bottom A Star Tile Check");
// 				xOffsetToCheck = 0;
// 				yOffsetToCheck = -1;
// 				PerformAStarCalculation(currentNode, endBathroomTile.tileX, endBathroomTile.tileY, openNodes, closedNodes, xOffsetToCheck, yOffsetToCheck);
// //				Debug.Log("----------------------------------------------------");
// //				Debug.Log("Performing Bottom Right A Star Tile Check");
// 				xOffsetToCheck = 1;
// 				yOffsetToCheck = -1;
// 				PerformAStarCalculation(currentNode, endBathroomTile.tileX, endBathroomTile.tileY, openNodes, closedNodes, xOffsetToCheck, yOffsetToCheck);
// //				Debug.Log("----------------------------------------------------");

// //				Debug.Log("Size of Open Nodes After Calculation: " + openNodes.Count);
// //				Debug.Log("Size of Closed Nodes After Calculation: " + closedNodes.Count);
// //				Debug.Log("-----------End of Surrounding Tile Check------------");

// 				if(openNodes.Contains(currentNode.gameObject)) {
// 					openNodes.Remove(currentNode.gameObject);
// 				}
// 				if(!closedNodes.Contains(currentNode.gameObject)) {
// 					closedNodes.Add(currentNode.gameObject);
// 				}

//         bool endTileDetectedInOpenNodeList = CheckIfBathroomTileListContainsTile(openNodes, endBathroomTile.tileX, endBathroomTile.tileY);
//         //bool endTileDetectedInClosedNodeList = CheckIfBathroomTileListContainsTile(closedNodes, endBathroomTile.tileX, endBathroomTile.tileY);

//         if(endTileDetectedInOpenNodeList
//           || openNodes.Count == 0) {
//           //Debug.Log("End tile detected in open nodes");
//           isEndBathroomTileInOpenNodeList = true;
//         }
// 			}

// 			//BathroomTile currentBathroomTile = RetrieveTileByXandY(openNodes, endBathroomTile.tileX, endBathroomTile.tileY);
//       // GameObject currentBathroomTileObject = BathroomTileMap.Instance.GetTileByXandY(currentNode.tileX, currentNode.tileY);
//       GameObject currentBathroomTileObject = null;
//       if(openNodes.Count == 0) {
//         currentBathroomTileObject = BathroomTileMap.Instance.GetTileByXandY(currentNode.tileX, currentNode.tileY);
//       }
//       else {
//         foreach(GameObject gameObj in openNodes) {
//           if(gameObj.GetComponent<BathroomTile>()
//              && (gameObj.GetComponent<BathroomTile>().tileX == endBathroomTile.tileX && gameObj.GetComponent<BathroomTile>().tileY == endBathroomTile.tileY)) {
//             currentBathroomTileObject = gameObj;
//           }
//         }
//       }

// 			BathroomTile currentBathroomTile = null;
// 			if(currentBathroomTileObject != null) {
// 				currentBathroomTile = currentBathroomTileObject.GetComponent<BathroomTile>();
// 			}
// 			while(currentBathroomTile != null) {
//         // Debug.Log(currentBathroomTile);
// //				Debug.Log("Adding tile (" + currentBathroomTile.tileX + "," + currentBathroomTile.tileY + ") to the movement node list");
// 				bathroomTilePositionsForPath.Add(new Vector2(currentBathroomTile.gameObject.transform.position.x, currentBathroomTile.gameObject.transform.position.y));
// 				currentBathroomTile = currentBathroomTile.parentAStarNode;
// 			}
// 			//adds the first node, otherwise it's left out
// 			//bathroomTilePositionsForPath.Add(new Vector2(startBathroomTile.transform.position.x, startBathroomTile.transform.position.y));
// 			bathroomTilePositionsForPath.Reverse();

//       if(debugShowLastPathCalculated) {
//         // List<GameObject> removeDebugTiles = new List<GameObject>();
//         foreach(GameObject gameObj in debugLastPathNodes) {
//           // removeDebugTiles.Add(gameObj);
//           Destroy(gameObj);
//         }
//         // foreach(GameObject gameObj in removeDebugTiles) {
//         //   // debugLastPathNodes.Remove(gameObj);
//         //   Destroy(gameObj);
//         // }
//         debugLastPathNodes.Clear();
//         int colorTracker = 0;
//         foreach(Vector2 vect2 in bathroomTilePositionsForPath) {
//           GameObject debugNode = null;
//           switch(colorTracker) {
//             case 0:
//               debugNode = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Debug/DebugPinkTile") as GameObject);
//             break;
//             case 1:
//               debugNode = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Debug/DebugRedTile") as GameObject);
//             break;
//             case 2:
//               debugNode = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Debug/DebugOrangeTile") as GameObject);
//             break;
//             case 3:
//               debugNode = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Debug/DebugYellowTile") as GameObject);
//             break;
//             case 4:
//               debugNode = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Debug/DebugGreenTile") as GameObject);
//             break;
//             case 5:
//               debugNode = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Debug/DebugBlueTile") as GameObject);
//             break;
//             case 6:
//               debugNode = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Debug/DebugIndigoTile") as GameObject);
//             break;
//             case 7:
//               debugNode = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Debug/DebugVioletTile") as GameObject);
//               colorTracker = 0;
//             break;
//           }

//           debugNode.transform.position = new Vector3(vect2.x, vect2.y, debugNode.transform.position.z);
//           debugLastPathNodes.Add(debugNode);
//           colorTracker++;
//         }
//       }

// 			return bathroomTilePositionsForPath;
// 		}
// 		catch(System.Exception e) {
// 			Debug.Log(e.StackTrace);
// 			//------------------------------------------------
// 			Debug.Log("-------------------------------------------------------------");
// 			Debug.Log("Custom Log");
// 			Debug.Log("-------------------------------------------------------------");
// 			Debug.Log("There was an issue with the Calculate AStar Path method. Please validate with the information provided in order to fix the issue. - Love your past self");
// 			Debug.Log("Number of open nodes: " + openNodes.Count);
// 			Debug.Log("Number of closed nodes: " + closedNodes.Count);
// 			Debug.Log("Start Bathroom Tile: " + startBathroomTile.name);
// 			Debug.Log("End Bathroom Tile: " + endBathroomTile.name);
// 			Debug.Log("Previous Node X: " + previousNode.tileX + " Y: " + previousNode.tileY);
// 			Debug.Log("Current Node: " + currentNode);
// 			Debug.Log("xOffsetToCheck: " + xOffsetToCheck);
// 			Debug.Log("yOffsetToCheck: " + yOffsetToCheck);
// 			Debug.Log("End bathroom tile is in the open list: " + isEndBathroomTileInOpenNodeList);
// 			if(currentNode != null) {
// 				Debug.Log("Tile X Being checked: " + currentNode.tileX);
// 				Debug.Log("Tile Y Being checked: " + currentNode.tileX);
// 			}
// 			else {
// 				Debug.Log("Current node is null, so Tile X and Y being checked are null as well.");
// 			}
// 			//return bathroomTilePositionsForPath;
// 			return new List<Vector2>();
// 		}
        return null;
	}

	public void PerformAStarCalculation(BathroomTile currentNode, int endTileX, int endTileY, List<GameObject> openNodes, List<GameObject> closedNodes, int xTileOffsetToPerformCalculationOn, int yTileOffsetToPerformCalculationOn) {
// //		if(currentNode == null) {
// //			Debug.Log("X TileOffset: " + xTileOffsetToPerformCalculationOn);
// //			Debug.Log("Y TileOffset: " + yTileOffsetToPerformCalculationOn);
// //		}

// 		int tileXBeingChecked = currentNode.tileX + xTileOffsetToPerformCalculationOn;
// 		int tileYBeingChecked = currentNode.tileY + yTileOffsetToPerformCalculationOn;
// 		int gValueOffset = 0;

// 		GameObject bathroomTileBeingCheckedObject = BathroomTileMap.Instance.GetTileByXandY(tileXBeingChecked, tileYBeingChecked);
// 		BathroomTile tileBeingChecked = null;
// 		if(bathroomTileBeingCheckedObject != null) {
// 			tileBeingChecked = bathroomTileBeingCheckedObject.GetComponent<BathroomTile>();
// 		}
// //		Debug.Log("Tile Being Checked X: " + tileXBeingChecked + " Y: " + tileYBeingChecked);

// 		if(tileBeingChecked != null) {
// //			if(tileBeingChecked.tileX == endTileX
// //			   && tileBeingChecked.tileY == endTileY) {
// //				Debug.Log("LOCATED END TILE");
// //			}
// 			//left
// 			if(xTileOffsetToPerformCalculationOn < 0) {
// 				//top left
// 				if(yTileOffsetToPerformCalculationOn > 0) {
// 					gValueOffset = 14;
// 				}
// 				//bottom left
// 				else if(yTileOffsetToPerformCalculationOn < 0) {
// 					gValueOffset = 14;
// 				}
// 				//left
// 				else {
// 					gValueOffset = 10;
// 				}
// 			}
// 			//right
// 			else if(xTileOffsetToPerformCalculationOn > 0) {
// 				//top right
// 				if(yTileOffsetToPerformCalculationOn > 0) {
// 					gValueOffset = 14;
// 				}
// 				//bottom right
// 				else if(yTileOffsetToPerformCalculationOn < 0) {
// 					gValueOffset = 14;
// 				}
// 				//right
// 				else {
// 					gValueOffset = 10;
// 				}
// 			}
// 			else {
// 				//top
// 				if(yTileOffsetToPerformCalculationOn > 0) {
// 					gValueOffset = 10;
// 				}
// 				//bottom
// 				else if(yTileOffsetToPerformCalculationOn < 0) {
// 					gValueOffset = 10;
// 				}
// 				//middle
// 				else {
// 					//not sure when this would happen?
// 					gValueOffset = 0;
// 				}
// 			}
// 			// Debug.Log("Current Node X: " + currentNode.tileX + " Y: " + currentNode.tileY);

// 			bool openNodesContainTileBeingChecked = CheckIfBathroomTileListContainsTile(openNodes, tileXBeingChecked, tileYBeingChecked);
// 			bool closedNodesContainTileBeingChecked = CheckIfBathroomTileListContainsTile(closedNodes, tileXBeingChecked, tileYBeingChecked);
// 			//BathroomTile tileBeingChecked = RetrieveTileByXandY(openNodes, tileXBeingChecked, tileYBeingChecked);

// 			//If it is not walkable or if it is on the closed list, ignore it. Otherwise do the following.

// 			if(!closedNodesContainTileBeingChecked
// 			   || (closedNodesContainTileBeingChecked
// 			    	&& (tileXBeingChecked == endTileX) && (tileYBeingChecked == endTileY))) {
// 				//Debug.Log("Not in closed nodes");
// 				//If it isn’t on the open list, add it to the open list. Make the current square the parent of this square. Record the F, G, and H costs of the square.
// 				if(!openNodesContainTileBeingChecked) {
// 					//openNodes.Add(currentNode.gameObject);
// 					openNodes.Add(tileBeingChecked.gameObject);
// 					tileBeingChecked.parentAStarNode = currentNode;
// 					tileBeingChecked.gValue = currentNode.gValue + gValueOffset;
// 					tileBeingChecked.heuristicValue = GetManhattanDistance(tileXBeingChecked, tileYBeingChecked, endTileX, endTileY);
// //					Debug.Log("Added node to open node list");
// 				}
// 				//If it is on the open list already, check to see if this path to that square is better, using G cost as the measure.
// 				else {
// 					//A lower G cost means that this is a better path. If so, change the parent of the square to the current square,
// 					//and recalculate the G and F scores of the square. If you are keeping your open list sorted by F score, you may
// 					//need to resort the list to account for the change.
// 					if(tileBeingChecked.gValue < currentNode.gValue) {
// 						tileBeingChecked.parentAStarNode = currentNode;
// 						tileBeingChecked.gValue = currentNode.gValue + gValueOffset;
// 					}
// 				}
// 			}
// //			else {
// //				Debug.Log("ENCOUNTERED CLOSED NODE AND SKIPPED IT");
// //				Debug.Log(tileBeingChecked);
// //			}
// 		}
	}

	public  BathroomTile RetrieveTileByXandY(List<GameObject> listToRetrieveFrom, int tileXToSelect, int tileYToSelect) {
		// foreach(GameObject gameObj in listToRetrieveFrom) {
		// 	BathroomTile node = gameObj.GetComponent<BathroomTile>();
		// 	if(node != null
		// 	   && node.tileX == tileXToSelect
		// 	   && node.tileY == tileYToSelect) {
		// 		return node;
		// 	}
		// }
		return null;
	}

	public bool CheckIfBathroomTileListContainsTile(List<GameObject> listToCheck, int tileXToCheckFor, int tileYToCheckFor) {
		// bool listContainsNodeSpecified = false;
		// foreach(GameObject gameObj in listToCheck) {
		// 	BathroomTile node = gameObj.GetComponent<BathroomTile>();
		// 	if(node != null
		// 	   && node.tileX == tileXToCheckFor
		// 	   && node.tileY == tileYToCheckFor) {
		// 		listContainsNodeSpecified =  true;
		// 	}
		// }
		// return listContainsNodeSpecified;
        return false;
	}

	public float GetManhattanDistance(float startTileX, float startTileY, float endTileX, float endTileY) {
		// float horizontalManhattanDistance = 0f;
		// float verticalManhattanDistance = 0f;

		// if(endTileX > startTileX) {
		// 	horizontalManhattanDistance = (endTileX - startTileX);
		// }
		// else {
		// 	horizontalManhattanDistance = (startTileX - endTileX);
		// }

		// if(endTileY > startTileY) {
		// 	verticalManhattanDistance = (endTileY - startTileY);
		// }
		// else {
		// 	verticalManhattanDistance = (startTileY - endTileY);
		// }
		// return (horizontalManhattanDistance + verticalManhattanDistance);
        return 0f;
	}

    //Closed nodes are actually the reference to the bathroom tile game object
    public List<GameObject> GetListCopyOfAStarClosedNodes() {
    	// List<GameObject> copyOfPermanentNodes = new List<GameObject>();
    	// foreach(GameObject gameObj in closedNodes) {
    	// 	copyOfPermanentNodes.Add(gameObj);
    	// }

    	// return copyOfPermanentNodes;
        return null;
    }

    //Closed nodes are actually the reference to the bathroom tile game object
    public List<GameObject> GetListCopyOfAllClosedNodes() {
        // // Debug.Log("Added tile blocker tiles");
        // List<GameObject> copyOfPermanentNodes = new List<GameObject>();
        // foreach(GameObject gameObj in closedNodes) {
        //   copyOfPermanentNodes.Add(gameObj);
        // }

        // copyOfPermanentNodes.AddRange(BathroomTileBlockerManager.Instance.GetListOfBathroomTileGameObjectsContainingBathroomTileBlockers());

        // return copyOfPermanentNodes;
        return null;
    }
}
