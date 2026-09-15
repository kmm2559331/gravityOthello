using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    bool?[,] stone = new bool?[8, 8];//false:黒 true:白 null:ブロックなし

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
                TurnOver(x, i);
            }
        }
    }
    public void TurnOver(int x, int y)//効率化と隣のブロックチェック、for文の中にひっくり返す処理を書く
    {
        for (int i = x;i >= 0; i--) { }//左
        for (int i = x;i <= 7; i++) { }//右
        for (int i = y;i >= 0; i--) { }//上
        for (int i = y;i <= 7; i++) { }//下
        for (Vector2Int i = new(x,y); i.x>=0||i.y>=0; i.x--, i.y--) { }//左上
        for (Vector2Int i = new(x,y); i.x<=7||i.y>=0; i.x++, i.y--) { }//右上
        for (Vector2Int i = new(x,y); i.x>=0||i.y<=7; i.x--, i.y++) { }//左下
        for (Vector2Int i = new(x,y); i.x<=7||i.y<=7; i.x++, i.y++) { }//右下
    }
}
