using System.Drawing;
using UnityEngine;

//enum StoneState
//{
//    no,
//    yes
//}
//
//enum StoneColor
//{
//    black,
//    white
//}
//
//public class Stone
//{
//    bool live = stone;
//
//    bool color;
//}

public class FieldGenerator : MonoBehaviour
{
    const int stoneMax = 8;//配列の上限

    bool?[,] stone = new bool?[stoneMax, stoneMax];//false:黒 true:白 null:ブロックなし

    //bool?[,] change = new bool?[stoneMax, stoneMax];
    //Stone[,] stones = new bool?[stoneMax, stoneMax];
    //bool[,] stone = new bool[stoneMax, stoneMax];//false:黒 true:白

    int waitStone;//上部に待機中の石

    Vector2 Linepos;//横線の座標（石の上）
    Vector2 Stonepos;

    [SerializeField] GameObject LineParent;//横線の親オブジェクト
    [SerializeField] GameObject LinePrefab;//横線のプレハブ
    [SerializeField] GameObject StoneBlackPrefab;//石のプレハブ
    [SerializeField] GameObject StoneWihtePrefab;//石のプレハブ
    private void Start()
    {
        //stone[0,stoneMax-1] = false;
        //stone[stoneMax-1,stoneMax-1] = true;


        FallStone(0, false);//左下に黒石を置く
        FallStone(stoneMax-1, true);//右下に白石を置く
    }


    public void Update()
    {
        
    }
    public void FallStone(int x, bool color)//石を落としたら（マウスクリック）
    {
        for (int i = stoneMax-1; i >= 0; i--)
        {
            if (stone[x, i] == null)
            {
                stone[x, i] = color;
                Stonepos = new Vector2((float)(x + 0.5), -(float)(i+0.5));//0.5を足す
                if (color)
                {
                    Instantiate(StoneWihtePrefab, Stonepos, Quaternion.identity);//石を置く
                } else 
                {
                    Instantiate(StoneBlackPrefab, Stonepos, Quaternion.identity);//石を置く
                }

                if (i != 0)
                {
                    Linepos = new Vector2((float)(x + 0.5), -i);
                    Instantiate(LinePrefab, Linepos, Quaternion.identity, LineParent.transform);//石の上に線を引く
                }

                //ひっくり返す関数を呼ぶ
                TurnOver(x, i, color);

                break;
            }
        }
    }

    public void GenerateStone(bool color)
    {
        waitStone = 0;//黒なら左(0)

        FallStone(waitStone, color);//クリックしたら
    }

    public void TurnOver(int x, int y , bool color)//効率化と隣のブロックチェック、for文の中にひっくり返す処理を書く
    {
        for (int i = x-1;i >= 0; i--) { if (StoneCheck(i, y, color)) { break; } } //左
        for (int i = x+1;i <= stoneMax-1; i++) { if (StoneCheck(i, y, color)) { break; } } //右
        for (int i = y-1;i >= 0; i--) { if (StoneCheck(x, i, color)) { break; } } //上
        for (int i = y+1;i <= stoneMax-1; i++) { if (StoneCheck(x, i, color)) { break; } } //下
        for (Vector2Int i = new(x-1,y-1); i.x>=0 && i.y>=0; i.x--, i.y--) { if (StoneCheck(i.x, i.y, color)) { break; } } //左上
        for (Vector2Int i = new(x+1,y-1); i.x<=stoneMax-1 && i.y>=0; i.x++, i.y--) { if (StoneCheck(i.x, i.y, color)) { break; } } //右上
        for (Vector2Int i = new(x-1,y+1); i.x>=0 && i.y<=stoneMax-1; i.x--, i.y++) { if (StoneCheck(i.x, i.y, color)) { break; } } //左下
        for (Vector2Int i = new(x+1,y+1); i.x<=stoneMax-1 && i.y<=stoneMax-1; i.x++, i.y++) { if (StoneCheck(i.x, i.y, color)) { break; } } //右下
    }

    public bool StoneCheck(int x, int y, bool color)
    {
        if (stone[x, y] != null) 
        { 
            if (stone[x, y] != color)
            {
                //change[x,y] = stone[x, y];
                return false;
            }
            //間をひっくり返す

        }
        return true;
    }
}
