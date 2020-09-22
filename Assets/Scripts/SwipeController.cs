using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Vector2 firstTouchPos;
    private Vector2 secondTouchPos;
    public float swipeAngle;

    //Matristeki hücreleri seçmek için
    public int ycell;
    public int xCell;
    public int targetX;
    public int targetY;
    //Çevredeki hexagonların pozisyonlarını saklamak için olan değişkenler
    private Vector3 firstPos;
    private Vector3 secondPos;
    private Vector3 thirdPos;
    //Çevredeki hexagonların renk kodunu saklamak ve gerektiğinde karşılaştırmak için olan değişkenler
    private SpriteRenderer firstRender;
    private SpriteRenderer secondRender;
    private SpriteRenderer thirdRender;

    //Diğer scriptlerdeki metodlara vs. erişmek için kullanılan kodlar
    private HexagonGenerator hexagonGenerator;
    private ScoreController scoreController;

    //Oyun durup durmadığının Kontrolünün yapılması
    public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        //Diğer scirpteki dizilere referans vermek için kullanıyorum.
        hexagonGenerator = FindObjectOfType<HexagonGenerator>();
        //ScoreController Scriptindeki SetScoreText ögesine erişmek için yaptığım tanımlama
        scoreController = FindObjectOfType<ScoreController>();


    }

    // Update is called once per frame
    void Update()
    {
        targetX = xCell;
        targetY = ycell;
    }
    private void OnMouseDown()
    {
        //Dokunulan ya da Tıklanan yerin ilk pozisyonunu saklamak ve gerektiğinde karşılaştırma yapmak
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetX = (int)firstTouchPos.x;
        targetY = (int)firstTouchPos.y;
        xCell = targetX;
        ycell = targetY;

    }
    private void OnMouseUp()
    {
        secondTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GetSwipeAngle();
    }

    public void GetSwipeAngle()
    {
        //İlk ve son pozisyonları göre hesaplanan radya değerini 180 ile çarpıp PI ile bölerek derece değerini buldum.
        swipeAngle = Mathf.Atan2(secondTouchPos.y - firstTouchPos.y, secondTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
        if (isPaused == false)
        {
            MoveHexagons();
        }  
    }

    public void MoveHexagons()
    {
        firstRender = hexagonGenerator.allHexagons[xCell, ycell].gameObject.GetComponent<SpriteRenderer>();
        secondRender = hexagonGenerator.allHexagons[xCell, ycell + 1].gameObject.GetComponent<SpriteRenderer>();
        thirdRender = hexagonGenerator.allHexagons[xCell, ycell + 2].gameObject.GetComponent<SpriteRenderer>();
        if ((swipeAngle>-45 && swipeAngle <= 45)||(swipeAngle>45 && swipeAngle<=135))
        {
            RotateCounterClockwise();      
            HexaDestroy();
        }
        else if ((swipeAngle > 135 && swipeAngle <= -135) || (swipeAngle < -45 && swipeAngle >= -135))
        {
            RotateClockwise();
            HexaDestroy();
           
        }

    }

    public void RotateCounterClockwise()
    {
        //çevresindeki hexagonların poziyonlarını alıp onları saat yönü tersine çeviren kod bölümü
        firstPos = hexagonGenerator.allHexagons[xCell, ycell].transform.position;
        secondPos = hexagonGenerator.allHexagons[xCell, ycell + 1].transform.position;
        thirdPos = hexagonGenerator.allHexagons[xCell, ycell + 2].transform.position;
        hexagonGenerator.allHexagons[xCell, ycell].transform.position = new Vector3(secondPos.x, secondPos.y, secondPos.z);
        hexagonGenerator.allHexagons[xCell, ycell + 1].transform.position = new Vector3(thirdPos.x, thirdPos.y, thirdPos.z);
        hexagonGenerator.allHexagons[xCell, ycell + 2].transform.position = new Vector3(firstPos.x, firstPos.y, firstPos.z);
    }

    public void RotateClockwise()
    {

        firstPos = hexagonGenerator.allHexagons[xCell, ycell].transform.position;
        secondPos = hexagonGenerator.allHexagons[xCell, ycell + 1].transform.position;
        thirdPos = hexagonGenerator.allHexagons[xCell, ycell + 2].transform.position;
        hexagonGenerator.allHexagons[xCell, ycell + 2].transform.position = new Vector3(firstPos.x, firstPos.y, firstPos.z);
        hexagonGenerator.allHexagons[xCell, ycell + 1].transform.position = new Vector3(thirdPos.x, thirdPos.y, thirdPos.z);
        hexagonGenerator.allHexagons[xCell, ycell].transform.position = new Vector3(secondPos.x, secondPos.y, secondPos.z);  
    }

    public void HexaDestroy()
    {
        //Hexagonu ve komşu hexagonları yok etmek için kullanılan kodlar(Optimize edilmesi lazım bazen yok etmiyor)
        if (firstRender.color == secondRender.color || secondRender.color == thirdRender.color)
        {
            Destroy(hexagonGenerator.allHexagons[xCell, ycell].gameObject);
            Destroy(hexagonGenerator.allHexagons[xCell, ycell + 1].gameObject);
            Destroy(hexagonGenerator.allHexagons[xCell, ycell + 2].gameObject);
            //Dizideki hexagonların yerini boşaltmak ve yerine yeniden doldurmak için yazmaya çalıştığım kod (Düzeltilmesi gerek)
            /*hexagonGenerator.allHexagons[xCell, ycell] = null;
            hexagonGenerator.allHexagons[xCell, ycell+1] = null;
            hexagonGenerator.allHexagons[xCell, ycell+2] = null;*/
            Debug.Log("Hexagons Destroyed");
            //Yok eddilen hexagonların yerini aşağıya doğru kaydırarak doldurmak(Ancak çalışmıyor)
            hexagonGenerator.CollapseHexas();
            //Yok edilen hexagonların Score'unu yazdıran metoda erişim
            scoreController.SetScoreText();
        }
    }
}
