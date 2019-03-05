using UnityEngine;

public class CheckerPiece : MonoBehaviour
{
    public bool isRedPiece;
    public bool isKing;

    public bool CheckSameColor(CheckerPiece[,] board, int x, int y)
    {
        CheckerPiece cp = board[x, y];
        if (cp)
        {
            if (isRedPiece)
            {
                if (cp.isRedPiece)
                {
                    return true;
                }
            }
            else
            {
                if (!cp.isRedPiece)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckValidMove(CheckerPiece[,] board, int x1, int y1, int x2, int y2)
    {
        if (board[x2, y2] != null)
        {
            return false;
        }

        int deltaMoveX = Mathf.Abs(x2 - x1);
        int deltaMoveY = y2 - y1;

        // Red piece check
        if (isRedPiece || isKing)
        {
            if (deltaMoveX == 1)
            {
                if (deltaMoveY == 1)
                {
                    return true;
                }
            }
            else if (deltaMoveX == 2)
            {
                if (deltaMoveY == 2)
                {
                    CheckerPiece cp = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (cp != null && cp.isRedPiece != isRedPiece)
                    {
                        return true;
                    }
                }
            }
        }
        // Blue piece check
        if (!isRedPiece || isKing)
        {
            if (deltaMoveX == 1)
            {
                if (deltaMoveY == -1)
                {
                    return true;
                }
            }
            else if (deltaMoveX == 2)
            {
                if (deltaMoveY == -2)
                {
                    CheckerPiece cp = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (cp != null && cp.isRedPiece != isRedPiece)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
