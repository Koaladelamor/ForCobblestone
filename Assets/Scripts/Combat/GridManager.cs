using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int MAX_FILAS;
    [SerializeField] int MAX_COLUMNAS;

    [SerializeField] Transform m_initialPosition;
    static GridManager m_instance = null;
    public GameObject[,] m_tiles;
    public GameObject m_tile;
    public Vector3[] m_directions;

    bool blackTilePainted;
    float m_tileSize;


    private void Awake()
    {
        //Singleton
        if (m_instance == null) { m_instance = this; }
        else { Destroy(this.gameObject); }

    }

    private void Start()
    {
        m_tiles = new GameObject[MAX_FILAS, MAX_COLUMNAS];

        //Inicializar tabla de posiciones
        for (int i = 0; i < MAX_FILAS; i++)
        {
            for (int j = 0; j < MAX_COLUMNAS; j++)
            {
                m_tiles[i, j] = Instantiate(m_tile);
                m_tiles[i, j].transform.position = new Vector3(m_initialPosition.position.x + j * 40, m_initialPosition.position.y - i * 40, m_initialPosition.position.z);
                m_tiles[i, j].transform.parent = this.transform;
                m_tiles[i, j].name = i + "-" + j;

                m_tiles[i, j].transform.parent = null;
                m_tiles[i, j].GetComponent<TileManager>().TakePawn();

                //Tile color
                if (!blackTilePainted)
                {
                    //Black
                    m_tiles[i, j].GetComponent<SpriteRenderer>().color = new Vector4((float)0.5, (float)0.5, (float)0.5, (float)0.5f);
                }
                blackTilePainted = !blackTilePainted;
            }
            //blackTilePainted = !blackTilePainted;
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                m_tiles[i, j].GetComponent<TileManager>().playerDraggableOnTile = true;
            }
        }

        m_tileSize = (m_tiles[0, 0].transform.position - m_tiles[0, 1].transform.position).magnitude;
    }

    static public GridManager Instance
    {
        get { return m_instance; }
        private set { }
    }

    public bool AssignPawnToTile(GameObject p_pawn, Vector2 p_tilePosition)
    {
        TileManager currentTile = m_tiles[(int)p_tilePosition.y, (int)p_tilePosition.x].GetComponent<TileManager>();

        //Debug.Log(p_tilePosition.x + " | " + p_tilePosition.y);

        return currentTile.AddPawn(p_pawn);

    }

    public void TakePawnFromTile(Vector2 p_tilePosition)
    {
        TileManager currentTile = m_tiles[(int)p_tilePosition.y, (int)p_tilePosition.x].GetComponent<TileManager>();

        currentTile.TakePawn();

    }

    public TileManager GetTile(Vector2 p_tilePosition)
    {
        TileManager currentTile = m_tiles[(int)p_tilePosition.y, (int)p_tilePosition.x].GetComponent<TileManager>();

        return currentTile;

    }

    public Vector2 ScreenToTilePosition(Vector3 p_mousePosition)
    {
        float tileX;
        float tileY;

        Vector2 leftTopBoardPosition = new Vector2(m_initialPosition.position.x - m_tileSize / 2, m_initialPosition.position.y + m_tileSize / 2);

        p_mousePosition = Camera.main.ScreenToWorldPoint(p_mousePosition);

        Vector2 rightBottomBoardPosition = leftTopBoardPosition + new Vector2(MAX_COLUMNAS * m_tileSize, -MAX_FILAS * m_tileSize);

        //Out of grid
        if (p_mousePosition.x < leftTopBoardPosition.x || p_mousePosition.x > rightBottomBoardPosition.x || p_mousePosition.y > leftTopBoardPosition.y || p_mousePosition.y < rightBottomBoardPosition.y)
        {
            return Vector2.positiveInfinity;
        }
        else
        {
            tileX = (int)((p_mousePosition.x - leftTopBoardPosition.x) / m_tileSize);
            tileY = -(int)((p_mousePosition.y - leftTopBoardPosition.y) / m_tileSize);

            return new Vector2(tileX, tileY);
        }
    }


    public bool IsTileEmpty(Vector2 p_tilePosition)
    {
        if (p_tilePosition.x >= 0 && p_tilePosition.x < MAX_COLUMNAS && p_tilePosition.y >= 0 && p_tilePosition.y < MAX_FILAS)
        {
            return m_tiles[(int)p_tilePosition.y, (int)p_tilePosition.x].GetComponent<TileManager>().IsTileEmpty;
        }
        return false;
    }

    public void StartingTiles_LightsOn()
    {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 2; j++)
            {
                m_tiles[i, j].GetComponent<TileManager>().EnableHighlight();
            }
        }
    }

    public void StartingTiles_LightsOff()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                m_tiles[i, j].GetComponentInChildren<TileManager>().DisableHighlight();
            }
        }
    }

    public float GetTileSize() { return m_tileSize; }
}