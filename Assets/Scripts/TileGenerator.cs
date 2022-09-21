using UnityEditor;
using UnityEngine;

public class TileGenerator : EditorWindow
{
    GameObject tile;
    Vector2Int size = new Vector2Int(5, 5);

    [MenuItem("Window / Tile Generator")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(TileGenerator));
        window.position = new Rect(0, 0, 275, 150);
    }

    private void OnGUI()
    {
        tile = (GameObject)EditorGUI.ObjectField(new Rect(5f, 80f, position.width - 6f, 20f), "Tile", tile, typeof(GameObject), true);
        if (tile != null && !tile.GetComponent<Tile>())
            GUILayout.Label("No tile component attached to Tile object");

        size.x = EditorGUILayout.IntField("Width", size.x);
        size.y = EditorGUILayout.IntField("Length", size.y);

        if (GUILayout.Button("Generate"))
        {
            GenerateGrid();
        }
    }
    
    Vector2 TileSize()
    {
        Bounds b = tile.GetComponent<MeshFilter>().sharedMesh.bounds;

        float x = (b.extents.x * 2);
        float y = (b.extents.z * 2);

        return new Vector2(x, y);
    }

    void GenerateGrid()
    {
        Vector3 position = Vector3.zero;
        GameObject parent = new GameObject("Tiles");
        Vector2 tileSize = TileSize();
        
        for (int x = 0; x < size.x; x++)
        {
            position.x += tileSize.x;
            position.z = 0f;

            for (int z = 0; z < size.y; z++)
            {
                position.z += tileSize.y;

                CreateTile(position, new Vector2Int(x, z), parent.transform);
            }
        }
    }

    void CreateTile(Vector3 pos, Vector2Int id, Transform parent)
    {
        GameObject newTile = Instantiate(tile, pos, Quaternion.identity, parent.transform);
        newTile.GetComponent<Tile>().GridCoordinates = id;
    }
}
