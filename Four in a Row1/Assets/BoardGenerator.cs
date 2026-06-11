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
        void CreateBoard()
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Vector2 position = new Vector2(
                        x - (columns / 2f) + 0.5f,
                        y - (rows / 2f) + 0.5f
                    );

                    Instantiate(cellPrefab, position, Quaternion.identity, transform);
                }
            }
        }
     }
}
