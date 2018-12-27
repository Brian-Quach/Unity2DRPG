using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapTester : MonoBehaviour {
    
    [Header("Map Dimensions")]
    public int mapWidth = 20;
    public int mapHeight = 20;

    [Space]
    [Header("Visualize Map")]
    public GameObject mapContainer;
    public GameObject tilePrefab;
    public Vector2 tileSize = new Vector2(16,16);

    [Space]
    [Header("Map Sprites")]
    public Texture2D islandTexture;
    public Texture2D fogTexture;

    [Space]
    [Header("Player")]
    public GameObject playerPrefab;
    public GameObject player;

    [Space]
    [Header("Decorate Map")]
    [Range(0, .9f)]
    public float erodePercent = .5f;
    public int erodeIterations = 2;
    [Range(0, .9f)]
    public float treePercent = .3f;
    [Range(0, .9f)]
    public float hillPercent = .2f;
    [Range(0, .9f)]
    public float mountainPercent = .1f;
    [Range(0, .9f)]
    public float townPercent = .05f;
    [Range(0, .9f)]
    public float monsterPercent = .1f;
    [Range(0, .9f)]
    public float lakePercent = .05f;

    public Map map;

    // Used for calculations when needed
    private int tempX;
    private int tempY;
    private Sprite[] islandTileSprites;
    private Sprite[] fogTileSprites;

    // Use this for initialization
    void Start () {
        islandTileSprites = Resources.LoadAll<Sprite>(islandTexture.name);
        fogTileSprites = Resources.LoadAll<Sprite>(fogTexture.name);

        map = new Map();
        MakeMap();
        StartCoroutine(AddPlayer());
	}

    IEnumerator AddPlayer() {
        yield return new WaitForEndOfFrame();
        CreatePlayer();
    }
	
    public void MakeMap() {
        map.NewMap(mapWidth, mapHeight);
        map.CreateIsland(
            erodePercent,
            erodeIterations,
            treePercent,
            hillPercent,
            mountainPercent,
            townPercent,
            monsterPercent,
            lakePercent
            );
        CreateGrid();
        CenterMap(map.castleTile.id);
    }

    void CreateGrid() {
        ClearMapContainer();

        var total = map.tiles.Length;
        var maxColumns = map.columns;
        var column = 0;
        var row = 0;

        for (var i = 0; i< total; i++) {
            column = i % maxColumns;

            var newX = column * tileSize.x;
            var newY = -row * tileSize.y;

            var go = Instantiate(tilePrefab);
            go.name = "Tile " + i;
            go.transform.SetParent(mapContainer.transform);
            go.transform.position = new Vector3(newX, newY, 0);

            DecorateTile(i);

            if (column == (maxColumns - 1)) {
                row++;
            }
        }
    }

    private void DecorateTile(int tileID) {
        var tile = map.tiles[tileID];
        var spriteID = tile.autoTileID;
        var go = mapContainer.transform.GetChild(tileID).gameObject;

        if (spriteID >= 0) {
            var sr = go.GetComponent<SpriteRenderer>();

            if (tile.visited) {
                sr.sprite = islandTileSprites[spriteID];
            } else {
                tile.CalculateFoWAutotileID();
                sr.sprite = fogTileSprites[Mathf.Min(tile.fowAutoTileId, fogTileSprites.Length - 1)];

            }
        }
    }

    public void CreatePlayer() {
        player = Instantiate(playerPrefab);
        player.name = "Player";
        player.transform.SetParent(mapContainer.transform);

        var controller = player.GetComponent<MapMovementController>();
        controller.map = map;
        controller.tileSize = tileSize;
        controller.MoveTo(map.castleTile.id);
        controller.tileActionCallback += TileActionCallback;

        controller.MoveTo(map.castleTile.id);

        var moveScript = Camera.main.GetComponent<MoveCamera>();
        moveScript.target = player;
    }

    void TileActionCallback(int type) {
        Debug.Log("OnType " + type);
    }

    void ClearMapContainer() {
        var children = mapContainer.transform.GetComponentsInChildren<Transform>();
        for (var i = children.Length - 1; i > 0; i--) {
            Destroy(children[i].gameObject);
        }
    }


    void CenterMap(int index) {
        var camPos = Camera.main.transform.position;
        var width = map.columns;

        PosUtil.CalculatePos(index, width, out tempX, out tempY);

        camPos.x = tempX * tileSize.x;
        camPos.y = -(tempY * tileSize.y);
        Camera.main.transform.position = camPos;
    }
}
