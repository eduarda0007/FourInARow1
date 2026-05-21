using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // TAMANHO DO TABULEIRO
    public int rows = 6;
    public int columns = 7;

    // PREFABS DAS PEÇAS
    public GameObject redPiece;
    public GameObject yellowPiece;

    // TEXTO DE VITÓRIA (UI)
    public TextMeshProUGUI victoryText;

    // TABULEIRO LÓGICO
    private int[,] board;

    // JOGADOR ATUAL (1 = vermelho, 2 = amarelo)
    private int currentPlayer = 1;

    void Start()
    {
        board = new int[rows, columns];

        // Esconde o texto de vitória ao iniciar
        if (victoryText != null)
            victoryText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int column = Mathf.RoundToInt(mousePos.x);
            DropPiece(column);
        }
    }

    // FAZ A PEÇA CAIR NA COLUNA
    void DropPiece(int column)
    {
        if (column < 0 || column >= columns) return;

        for (int row = 0; row < rows; row++)
        {
            if (board[row, column] == 0)
            {
                board[row, column] = currentPlayer;
                SpawnPiece(row, column);

                if (CheckWin(row, column))
                {
                    ShowVictory();
                }
                else
                {
                    SwitchPlayer();
                }
                break;
            }
        }
    }

    // CRIA A PEÇA NA TELA
    void SpawnPiece(int row, int column)
    {
        GameObject piecePrefab = currentPlayer == 1 ? redPiece : yellowPiece;
        Vector2 position = new Vector2(column, row);
        Instantiate(piecePrefab, position, Quaternion.identity);
    }

    // TROCA O JOGADOR
    void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
    }

    // VERIFICA SE ALGUÉM GANHOU
    bool CheckWin(int row, int column)
    {
        return CheckDirection(row, column, 1, 0) ||   // Horizontal
               CheckDirection(row, column, 0, 1) ||   // Vertical
               CheckDirection(row, column, 1, 1) ||   // Diagonal \
               CheckDirection(row, column, 1, -1);    // Diagonal /
    }

    bool CheckDirection(int row, int column, int dirX, int dirY)
    {
        int count = 1;

        count += CountPieces(row, column, dirX, dirY);
        count += CountPieces(row, column, -dirX, -dirY);

        return count >= 4;
    }

    int CountPieces(int row, int column, int dirX, int dirY)
    {
        int count = 0;
        int r = row + dirY;
        int c = column + dirX;

        while (r >= 0 && r < rows &&
               c >= 0 && c < columns &&
               board[r, c] == currentPlayer)
        {
            count++;
            r += dirY;
            c += dirX;
        }

        return count;
    }

    // MOSTRA A VITÓRIA NA TELA
    void ShowVictory()
    {
        if (victoryText == null) return;

        victoryText.gameObject.SetActive(true);

        if (currentPlayer == 1)
            victoryText.text = "VITÓRIA\nEquipe Vermelha";
        else
            victoryText.text = "VITÓRIA\nEquipe Amarela";

        enabled = false; // Para o jogo
    }
}