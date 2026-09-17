using System.Drawing;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    bool?[,] stone = new bool?[8, 8];//false:黒 true:白 null:ブロックなし
    bool?[,] change = new bool?[8, 8];

    [SerializeField] GameObject StoneParent;
    private void Start()
    {
        stone[0,7] = false;
        stone[7,7] = true;

        
    }

    public void FallStone(int x, bool color)//石を落としたら（マウスクリック）
    {
        for (int i = 7; i >= 0; i--)
        {
            if (stone[x, i] == null)
            {
                stone[x, i] = color;

                //ひっくり返す関数を呼ぶ
                TurnOver(x, i, color);
            }
        }
    }
    public void TurnOver(int x, int y , bool color)//効率化と隣のブロックチェック、for文の中にひっくり返す処理を書く
    {
        for (int i = x--;i >= 0; i--) { if (StoneCheck(i, y, color)) { break; } }//左
        for (int i = x++;i <= 7; i++) { if (StoneCheck(i, y, color)) { break; } }//右
        for (int i = y--;i >= 0; i--) { if (StoneCheck(x, i, color)) { break; } }//上
        for (int i = y++;i <= 7; i++) { if (StoneCheck(x, i, color)) { break; } }//下
        for (Vector2Int i = new(x--,y--); i.x>=0||i.y>=0; i.x--, i.y--) { if (StoneCheck(i.x, i.y, color)) { break; } }//左上
        for (Vector2Int i = new(x++,y--); i.x<=7||i.y>=0; i.x++, i.y--) { if (StoneCheck(i.x, i.y, color)) { break; } }//右上
        for (Vector2Int i = new(x--,y++); i.x>=0||i.y<=7; i.x--, i.y++) { if (StoneCheck(i.x, i.y, color)) { break; } }//左下
        for (Vector2Int i = new(x++,y++); i.x<=7||i.y<=7; i.x++, i.y++) { if (StoneCheck(i.x, i.y, color)) { break; } }//右下
    }

    public bool StoneCheck(int x, int y, bool color)
    {
        if (stone[x, y] != null) 
        { 
            if (stone[x, y] != color)
            {
                change[x,y] = stone[x, y];
                return false;
            }
            //間をひっくり返す

        }
        return true;
    }
}
