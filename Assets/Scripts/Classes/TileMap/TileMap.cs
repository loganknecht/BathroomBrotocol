using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// REQUIREMENTS FOR THE TILE MAP:
// ALL ROWS MUST BE EQUAL IN WIDTH
// IF USING THE ROW CONTAINER CONVENIENCE LIST TILES MUST BE IN ORDER
public class TileMap : BaseBehavior {

	public float singleTileWidth = -1;
	public float singleTileHeight = -1;
    public int tilesWide = 0;
    public int tilesHigh = 0;
    public int tileCount = 0;

	public GameObject[][] tiles;
    public List<GameObject> rowContainers;

    protected override void Awake() {
    }

	// Use this for initialization
	protected virtual void Start () {
        ConfigureTileMap();
	}

    // Update is called once per frame
    public virtual void Update () {
    }

    public void ConfigureTileMap() {
        if((rowContainers == null || rowContainers.Count == 0) && (tiles == null || tiles.Length == 0)) {
            Debug.LogError("The the rowContainers list and tiles array has not been initialized or is empty, please fix this otherwise nothing will work!");
        }
        else if(rowContainers != null && rowContainers.Count > 0 && tiles != null && tiles.Length > 0) {
            Debug.LogError("You cannot configure the tile map using the rowContainers AND the tiles list. You must configure the tile map using only one of these");
        }
        else {
            // Perform tiles configuration
            if(tiles != null && tiles.Length > 0) {
                // Error checking to make sure that row width is congruent/symmetrical in the tiles array
                int rowWidth = -1;
                bool rowWidthsHaveSameLength = true;
                foreach(GameObject[] row in tiles) {
                    if(rowWidth == -1) {
                        rowWidth = row.Length;
                    }
                    else {
                        if(rowWidth != row.Length) {
                            rowWidthsHaveSameLength = false;
                            Debug.LogError("The tile map width is incongruent, please verify that all rows are equivilant in length!");
                        }
                    }
                }
            }

            // Perform rowContainers configuration
            if(rowContainers != null && rowContainers.Count > 0) {
                // Error checking to make sure that the row containers are congruent/symmetrical
                bool rowWidthsHaveSameLength = true;
                int rowWidth = -1;
                foreach(GameObject rowContainer in rowContainers) {
                    if(rowWidth == -1) {
                        rowWidth = rowContainer.transform.childCount;
                    }
                    else  {
                        if(rowWidth != rowContainer.transform.childCount) {
                            rowWidthsHaveSameLength = false;
                            Debug.LogError("The tile map width is incongruent, please verify that all row container rows are equivilant in length!");
                        }
                    }
                }

                if(rowWidthsHaveSameLength) {
                    // So freaking stupid using jagged arrays instead of a freaking multi dimensional array, I hate you Unity!!!
                    int tileArrayHeight = rowContainers.Count;
                    tiles = new GameObject[tileArrayHeight][];
                    for(int i = 0; i < tileArrayHeight; i++) {
                        tiles[i] = new GameObject[rowWidth];
                    }

                    foreach(GameObject rowContainer in rowContainers) {
                        // Debug.Log(rowContainer.name);
                        foreach(Transform childTransform in rowContainer.transform) {
                            // Debug.Log(childTransform.name);

                            int x = childTransform.gameObject.GetComponent<Tile>().tileX;
                            int y = childTransform.gameObject.GetComponent<Tile>().tileY;
                            // Debug.Log("x: " + x + " y: " + y);
                            tiles[y][x] = childTransform.gameObject;
                        }
                    }
                }
            }
        }
    }

	public GameObject GetTileByXandY(int tileX, int tileY) {
        if(tiles != null) {
           return tiles[tileY][tileX];
        }
		return null;
	}

	public GameObject GetTileGameObjectByWorldPosition(float xPosition, float yPosition, bool returnClosestTile) {
        GameObject closestTile = null;
        float closestTileXDistance = 0f;
        float closestTileYDistance = 0f;

		foreach(GameObject[] row in tiles) {
            foreach(GameObject tile in row) {
    			float leftBound = tile.transform.position.x - singleTileWidth/2;
    			float rightBound = tile.transform.position.x + singleTileWidth/2;

    			float bottomBound = tile.transform.position.y - singleTileHeight/2;
    			float topBound = tile.transform.position.y + singleTileHeight/2;

    			if(leftBound < xPosition
    			   && rightBound > xPosition
    			   && bottomBound < yPosition
    			   && topBound > yPosition) {
    				return tile;
                }

                if(returnClosestTile) {
                    float currentClosestTileCheckXDistance = Mathf.Abs(xPosition - tile.transform.position.x);
                    float currentClosestTileCheckYDistance = Mathf.Abs(yPosition - tile.transform.position.y);
                    if(closestTile == null) {
                        closestTile = tile;
                        closestTileXDistance = currentClosestTileCheckXDistance;
                        closestTileYDistance = currentClosestTileCheckYDistance;
                    }
                    else {
                        if(currentClosestTileCheckXDistance <= closestTileXDistance
                           && currentClosestTileCheckYDistance <= closestTileYDistance) {
                            closestTile = tile;
                            closestTileXDistance = currentClosestTileCheckXDistance;
                            closestTileYDistance = currentClosestTileCheckYDistance;
                        }
                    }
                }
            }
		}

        if(returnClosestTile) {
            return closestTile;
        }
        else {
            return null;
        }
	}
}
