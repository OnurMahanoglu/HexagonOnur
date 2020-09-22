using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class HexagonGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject HexaTile;
    public GameObject BombTile;
    public float zOffSet;
    public float xOffSet;
    public GameObject[,] allHexagons;
    public Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        allHexagons = new GameObject[width, height];
        GenerateHexagons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateHexagons()
    {
        //X ve Y değerlerini kullanarak matris oluşturma ve matris koordinatlarına hexagonları atamak.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = x * xOffSet;
                //hexagonlar 2. sırada ise biraz aşağı inmesini sağlayan offset kodları.
                if (y % 2 == 1)
                {
                    xPos += xOffSet / 2;
                }
                //Editörde seçilen hexagon renklerine göre diziden rastgele bir renk seçen kod
                int randomColor = Random.Range(0, colors.Length);
                GameObject hexagon = Instantiate(HexaTile, new Vector3(xPos, y * zOffSet, 0), Quaternion.identity) as GameObject;
                //Eğer skor 1000 in üzerinde ise bomba spawnlamayı sağlayan kod(Bomba spawnlanıyor ancak optimize edilmesi ve geri sayımının kodlanması lazım)
                if (ScoreController.generalScore == 1000)
                {
                    if (y % 3 == 0)
                    {
                        GameObject bombHexa = Instantiate(BombTile, new Vector3(xPos, y * zOffSet, 0), Quaternion.identity) as GameObject;
                        allHexagons[x, y] = bombHexa;
                    }
                }
                //Hiyerarşi düzenini sağlamak için Scriptin bulunduğu Gameobjecte child ataması yapma.
                hexagon.transform.parent = transform;
                //Daha kolay Gameobject erişimi için ad verme
                hexagon.name = "(" + x + "," + y + ")";
               //Boş hexagonları renklendirme.
                SpriteRenderer spriteRenderer = hexagon.GetComponent<SpriteRenderer>();
                spriteRenderer.color = colors[randomColor];
                allHexagons[x, y] = hexagon;
            }
        }
    }


    //Yok edilen matristeki hexagonların yerini doldurmak.
    public void CollapseHexas()
    {
        int nullCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allHexagons[x, y] == null)
                {
                    nullCount++;
                }else if (nullCount > 0)
                {
                    allHexagons[x, y].GetComponent<SwipeController>().xCell -= 1;
                }
            }
        }
        StartCoroutine(FillHexaCo());
    }

    public void RefillHexagons()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allHexagons[x, y] == null)
                {
                    Vector2 tempPos = new Vector2(x, y);
                    int randomColor = Random.Range(0, colors.Length);  
                    GameObject piece = Instantiate(HexaTile, tempPos, Quaternion.identity);
                    SpriteRenderer spriteRenderer = piece.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = colors[randomColor];
                    allHexagons[x, y] = piece;
;                }
            }
        }
    }
    private IEnumerator FillHexaCo()
    {
        RefillHexagons();
        yield return new WaitForSeconds(1f);
    }
}
