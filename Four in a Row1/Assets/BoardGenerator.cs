using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public int rows = 6;
    public int columns = 7;
    public float cellSize = 1f;

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2 position = new Vector2(col * cellSize, row * cellSize);
                Instantiate(cellPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
