using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// REQUIREMENTS FOR THE TILE MAP:
// ALL ROWS MUST BE EQUAL IN WIDTH
// IF USING THE ROW CONTAINER CONVENIENCE LIST TILES MUST BE IN ORDER
public class TileMap : BaseBehavior {
// public class TileMap : MonoBehaviour {
    public float singleTileWidth = -1;
    public float singleTileHeight = -1;
    public int tilesWide = 0;
    public int tilesHigh = 0;
    public int tileCount = 0;
    
    public GameObject[][] tiles;
    public List<GameObject> rowContainers;
    
    protected override void Awake() {
        base.Awake();
    }
    // public virtual void Awake() {
    // }
    
    // Use this for initialization
    protected virtual void Start() {
        // ConfigureRows();
        // ConfigureTileMap();
    }
    
    // Update is called once per frame
    public virtual void Update() {
    }
    
    public virtual void PrintDebug() {
        Debug.Log("singleTileWidth: " + singleTileWidth);
        Debug.Log("singleTileHeight: " + singleTileHeight);
        Debug.Log("tilesWide: " + tilesWide);
        Debug.Log("tilesHigh: " + tilesHigh);
        Debug.Log("tileCount: " + tileCount);
    }
    
    // TO DO: probably move this over to bathroom tile map
    public void ConfigureRows() {
        foreach(Transform child in this.gameObject.transform) {
            rowContainers.Add(child.gameObject);
        }
    }
    
    // TO DO: probably move this over to bathroom tile map
    public void ConfigureTileMap() {
        if(tiles != null && tiles.Length > 0) {
            // Error checking to make sure that row width is congruent/symmetrical in the tiles array
            int rowWidth = -1;
            foreach(GameObject[] row in tiles) {
                if(rowWidth == -1) {
                    rowWidth = row.Length;
                }
                else {
                    if(rowWidth != row.Length) {
                        Debug.LogError("The tile map width is incongruent, please verify that all rows are equivilant in length!");
                    }
                }
            }
            
            tilesWide = rowWidth;
            tilesHigh = tiles.Length;
        }
        
        // Perform rowContainers configuration ONLY IF THERE ARE ONLY ROWS IN THE ROW LIST
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
            
            tilesWide = rowWidth;
            tilesHigh = tiles.Length;
        }
    }
    
    public void SetTiles(GameObject[][] newTiles) {
        tiles = newTiles;
    }
    public GameObject[][] GetTiles() {
        return tiles;
    }
    
    public List<GameObject> GetTilesAsList() {
        List<GameObject> tileList = new List<GameObject>();
        foreach(GameObject[] row in tiles) {
            foreach(GameObject tile in row) {
                tileList.Add(tile);
            }
        }
        return tileList;
    }
    
    public GameObject[] GetTilesAsSingleDimensionalArray() {
        GameObject[] tilesArray = new GameObject[tilesWide * tilesHigh];
        int index = 0;
        foreach(GameObject[] row in tiles) {
            foreach(GameObject tile in row) {
                tilesArray[index] = tile;
                index++;
            }
        }
        return tilesArray;
    }
    
    // Helper functions to get some key points in the tile map
    public GameObject GetTopLeftTileGameObject() {
        return GetTileGameObjectByIndex(0, tilesHigh - 1);
    }
    public GameObject GetTopCenterTileGameObject() {
        return GetTileGameObjectByIndex(tilesWide / 2, tilesHigh - 1);
    }
    public GameObject GetTopRightTileGameObject() {
        return GetTileGameObjectByIndex(tilesWide - 1, tilesHigh - 1);
    }
    public GameObject GetMiddleLeftTileGameObject() {
        return GetTileGameObjectByIndex(0, tilesHigh / 2);
    }
    public GameObject GetMiddleTileGameObject() {
        return GetTileGameObjectByIndex(tilesWide / 2, tilesHigh / 2);
    }
    public GameObject GetMiddleRightTileGameObject() {
        return GetTileGameObjectByIndex(tilesWide - 1, tilesHigh / 2);
    }
    public GameObject GetBottomLeftTileGameObject() {
        return GetTileGameObjectByIndex(0, 0);
    }
    public GameObject GetBottomCenterTileGameObject() {
        return GetTileGameObjectByIndex(tilesWide / 2, 0);
    }
    public GameObject GetBottomRightTileGameObject() {
        return GetTileGameObjectByIndex(tilesWide - 1, 0);
    }
    public GameObject GetTileGameObjectByIndex(int tileX, int tileY, bool catchBoundaryValues = false) {
        if(tiles != null) {
            if(catchBoundaryValues) {
                if(tileX < 0) {
                    tileX = 0;
                }
                else if(tileX > tilesWide) {
                    tileX = tilesWide - 1;
                }
                if(tileY < 0) {
                    tileY = 0;
                }
                else if(tileY > tilesHigh) {
                    tileY = tilesHigh - 1;
                }
            }
            return tiles[tileY][tileX];
        }
        return null;
    }
    
    public GameObject GetTileGameObjectByWorldPosition(Vector3 position, bool returnClosestTile) {
        return GetTileGameObjectByWorldPosition(position.x, position.y, returnClosestTile);
    }
    
    public GameObject GetTileGameObjectByWorldPosition(Vector2 position, bool returnClosestTile) {
        return GetTileGameObjectByWorldPosition(position.x, position.y, returnClosestTile);
    }
    
    public GameObject GetTileGameObjectByWorldPosition(float xPosition, float yPosition, bool returnClosestTile) {
        GameObject closestTile = null;
        float closestTileXDistance = 0f;
        float closestTileYDistance = 0f;
        
        foreach(GameObject[] row in tiles) {
            foreach(GameObject tile in row) {
                float leftBound = tile.transform.position.x - singleTileWidth / 2;
                float rightBound = tile.transform.position.x + singleTileWidth / 2;
                
                float bottomBound = tile.transform.position.y - singleTileHeight / 2;
                float topBound = tile.transform.position.y + singleTileHeight / 2;
                
                if(leftBound <= xPosition
                    && rightBound > xPosition
                    && bottomBound <= yPosition
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
    
    // +90 - counter clockwise
    // TODO - This is not the best it could be, if you get a chance some day refactor this section
    public void RotateTileMatrixLeft() {
        tiles = TransposeMatrix(tiles, tilesWide, tilesHigh);
        int temp = tilesWide;
        tilesWide = tilesHigh;
        tilesHigh = temp;
        
        ReverseAllRows(tiles);
        SetPositionsInWorldBasedOnTileMapProperties(tiles);
        UpdateAllTilesIsometricDisplay(tiles);
        
    }
    // -90 - clockwise
    public void RotateTileMatrixRight() {
        tiles = TransposeMatrix(tiles, tilesWide, tilesHigh);
        int temp = tilesWide;
        tilesWide = tilesHigh;
        tilesHigh = temp;
        
        ReverseAllColumns(tiles, tilesWide);
        SetPositionsInWorldBasedOnTileMapProperties(tiles);
        UpdateAllTilesIsometricDisplay(tiles);
        
    }
    
    // This should really be a generic, but whatever
    public GameObject[][] TransposeMatrix(GameObject[][] tileMapToRotate, int tilesWide, int tilesHigh) {
        GameObject[][] tempTileMap = new GameObject[tilesWide][];
        for(int j = 0; j < tilesWide; j++) {
            tempTileMap[j] = new GameObject[tilesHigh];
        }
        
        
        for(int j = 0; j < tilesWide; j++) {
            for(int i = 0; i < tilesHigh; i++) {
                tempTileMap[j][i] = tiles[i][j];
            }
        }
        return tempTileMap;
    }
    
    public GameObject[][] ReverseAllColumns(GameObject[][] tileMapToRotate, int tilesWide) {
        for(int i = 0; i < tilesWide; i++) {
            // Debug.Log(i);
            ReverseColumn(tileMapToRotate, i);
        }
        return tileMapToRotate;
    }
    // This should really be a generic, but whatever
    public GameObject[][] ReverseColumn(GameObject[][] tileMapToRotate, int columnToRotate) {
        for(int j = 0; j < tilesHigh / 2; j++) {
            GameObject tempTileGameObject = tileMapToRotate[j][columnToRotate];
            tileMapToRotate[j][columnToRotate] = tileMapToRotate[tilesHigh - j - 1][columnToRotate];
            tileMapToRotate[tileMapToRotate.Length - j - 1][columnToRotate] = tempTileGameObject;
        }
        return tileMapToRotate;
    }
    
    public GameObject[][] ReverseAllRows(GameObject[][] tileMapToRotate) {
        for(int j = 0; j < tileMapToRotate.Length; j++) {
            ReverseRow(tileMapToRotate, j);
        }
        return tileMapToRotate;
    }
    
    // This should really be a generic, but whatever
    public GameObject[][] ReverseRow(GameObject[][] tileMapToRotate, int rowToReverse) {
        System.Array.Reverse(tileMapToRotate[rowToReverse]);
        return tileMapToRotate;
    }
    
    public GameObject[][] SetPositionsInWorldBasedOnTileMapProperties(GameObject[][] tileMapToSetPositions) {
        for(int j = 0; j < tileMapToSetPositions.Length; j++) {
            for(int i = 0; i < tileMapToSetPositions[j].Length; i++) {
                tileMapToSetPositions[j][i].transform.position = new Vector3(i * singleTileWidth, j * singleTileHeight, tileMapToSetPositions[j][i].transform.position.z);
            }
        }
        return tileMapToSetPositions;
    }
    
    public GameObject[][] UpdateAllTilesIsometricDisplay(GameObject[][] tilesToUpdate) {
        foreach(GameObject[] row in tilesToUpdate) {
            foreach(GameObject cell in row) {
                if(cell.GetComponent<IsometricDisplay>() != null) {
                    cell.GetComponent<IsometricDisplay>().UpdateDisplayPosition();
                }
            }
        }
        
        return tilesToUpdate;
    }
}
