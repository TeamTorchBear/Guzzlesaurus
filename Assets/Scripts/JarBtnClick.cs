using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JarBtnClick : MonoBehaviour
{
    public bool isFireflies;
    public Image i1, kitchen, Screen;
    public Transform canvas;
    public int quantity;
    public Image prefab;
    public Text text;

    static Data data;
    CanBeUpgrade c1, kc;
    bool isClick, isClick1, isClickk;
    bool ireach1 = false, ireachk = false;
    Image fireflies1, firefliesk;
    bool isc1 = false, isck = false;
    float startTime1 = 0, startTimek = 0;
    float journeyLength1 = 0, journeyLengthk = 0;
    bool is1, ks;

    // Use this for initialization
    void Awake()
    {
        data = SaveNLoadTxt.Load();
        c1 = i1.GetComponent<CanBeUpgrade>();
        //c2 = i2.GetComponent<CanBeUpgrade>();
        //c3 = i3.GetComponent<CanBeUpgrade>();
        //c4 = i4.GetComponent<CanBeUpgrade>();
        //c5 = i5.GetComponent<CanBeUpgrade>();
        //c6 = i6.GetComponent<CanBeUpgrade>();
        kc = kitchen.GetComponent<CanBeUpgrade>();
        quantity = 0;
        is1 = false;
        //is2 = false;
        //is3 = false;
        //is4 = false;
        //is5 = false;
        //is6 = false;
        ks = false;
        isClick = false;
        isFireflies = false;

        isClick1 = false; /*isClick2 = false; isClick3 = false; isClick4 = false; isClick5 = false; isClick6 = false;*/
        isClickk = false;

        text = text.GetComponent<Text>();

        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);

        Button btn1 = c1.GetComponent<Button>();
        btn1.onClick.AddListener(OnClick1);
        //Button btn2 = c2.GetComponent<Button>();
        //btn2.onClick.AddListener(OnClick2);
        //Button btn3 = c3.GetComponent<Button>();
        //btn3.onClick.AddListener(OnClick3);
        //Button btn4 = c4.GetComponent<Button>();
        //btn4.onClick.AddListener(OnClick4);
        //Button btn5 = c5.GetComponent<Button>();
        //btn5.onClick.AddListener(OnClick5);
        //Button btn6 = c6.GetComponent<Button>();
        //btn6.onClick.AddListener(OnClick6);
        Button btnk = kc.GetComponent<Button>();
        btnk.onClick.AddListener(OnClickk);

        IfCanUpgrade();
    }

    // Update is called once per frame
    void Update()
    {
        data = SaveNLoadTxt.Load();
        text.text = "Cash:" + data.moneyWeHave;

        if (isClick)
        {
            if (!isFireflies)
            {
                isFireflies = true;
                quantity = 0;
                is1 = false;
                //is2 = false;
                //is3 = false;
                //is4 = false;
                //is5 = false;
                //is6 = false;
                ks = false;
                isc1 = false;
                //isc2 = false;
                //isc3 = false;
                //isc4 = false;
                //isc5 = false;
                //isc6 = false;
                isck = false;
                AllItemUpdate();
                Screen.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }
            else if (isFireflies)
            {
                isFireflies = false;
                quantity = 0;
                is1 = false;
                //is2 = false;
                //is3 = false;
                //is4 = false;
                //is5 = false;
                //is6 = false;
                ks = false;
                isc1 = false;
                //isc2 = false;
                //isc3 = false;
                //isc4 = false;
                //isc5 = false;
                //isc6 = false;
                isck = false;
                AllItemUpdate();
                Screen.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            isClick = false;
            IfCanUpgrade();
        }
        CreateFireflies();
        DestroyFireflies();
        Reach();
        if (isClick1)
        {
            if (c1.moneyEnough && isFireflies&& data.tableLevel != 2)
            {
                Debug.Log("Upgrade " + c1.name);
                c1.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
                //is2 = false;
                //is3 = false;
                //is4 = false;
                //is5 = false;
                //is6 = false;
                ks = false;
            }
            isClick1 = false;

            IfCanUpgrade();
        }

        if (isClickk)
        {
            if (kc.moneyEnough && isFireflies && data.kitchenLevel != 2)
            {
                Debug.Log("Upgrade " + kc.name);
                kc.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
                //is2 = false;
                //is3 = false;
                //is4 = false;
                //is5 = false;
                //is6 = false;
                ks = false;
            }
            isClickk = false;
            IfCanUpgrade();
        }

        if (ireach1)
        {
            i1.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        }
        else
        {
            i1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        if (ireachk)
        {
            kitchen.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        }
        else
        {
            kitchen.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    public void IfCanUpgrade()
    {
        if (!is1)
        {
            if (c1.moneyEnough)
            {
                quantity++;
                //    if (isFireflies)
                //    {
                //        if (fireflies1.transform.position == i1.transform.position)
                //        {
                //            i1.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
                //        }
                //    }
                //    else
                //    {
                //        i1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                //    }
                //}
                //else
                //{
                //    i1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }

            is1 = true;
            //PlayerPrefs.SetInt("moneyWeHave", quantity);
        }

        if (!ks)
        {
            if (kc.moneyEnough)
            {
                quantity++;
                //    if (isFireflies)
                //    {
                //        kitchen.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
                //    }
                //    else
                //    {
                //        kitchen.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                //    }
                //}
                //else
                //{
                //    kitchen.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            ks = true;
        }
    }

    void OnClick()
    {
        isClick = true;
    }

    void OnClick1()
    {
        isClick1 = true;
    }

    void OnClickk()
    {
        isClickk = true;
    }

    void AllItemUpdate()
    {
        c1.updates();
        //c2.updates();
        //c3.updates();
        //c4.updates();
        //c5.updates();
        //c6.updates();
        kc.updates();
    }

    void CreateFireflies()
    {
        if (isFireflies)
        {
            if (c1.moneyEnough && data.tableLevel != 2)
            {
                if (!fireflies1)
                {
                    fireflies1 = Instantiate(prefab);
                    fireflies1.transform.SetParent(canvas);
                    fireflies1.transform.position = this.transform.position;
                }
                if (!isc1)
                {
                    startTime1 = Time.time;
                    journeyLength1 = Vector2.Distance(fireflies1.transform.position, i1.GetComponent<Transform>().position);
                    isc1 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime1) * 10.0f;
                    float fracJourney = distCovered / journeyLength1;
                    fireflies1.GetComponent<Transform>().position = Vector2.Lerp(fireflies1.transform.position, i1.GetComponent<Transform>().position, fracJourney);
                }
            }

            if (kc.moneyEnough && data.kitchenLevel != 2)
            {
                if (!firefliesk)
                {
                    firefliesk = Instantiate(prefab);
                    firefliesk.transform.SetParent(canvas);
                    firefliesk.transform.position = this.transform.position;
                }
                if (!isck)
                {
                    startTimek = Time.time;
                    journeyLengthk = Vector2.Distance(firefliesk.transform.position, kitchen.GetComponent<Transform>().position);
                    isck = true;
                }
                else
                {
                    float distCovered = (Time.time - startTimek) * 10.0f;
                    float fracJourney = distCovered / journeyLengthk;
                    firefliesk.GetComponent<Transform>().position = Vector2.Lerp(firefliesk.transform.position, kitchen.GetComponent<Transform>().position, fracJourney);

                }
            }
        }
    }

    void DestroyFireflies()
    {
        if (!isFireflies)
        {
            if (fireflies1)
            {
                if (!isc1)
                {
                    fireflies1.transform.position = i1.transform.position;
                    startTime1 = Time.time;
                    journeyLength1 = Vector2.Distance(fireflies1.transform.position, this.GetComponent<Transform>().position);
                    isc1 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime1) * 10.0f;
                    float fracJourney = distCovered / journeyLength1;
                    fireflies1.GetComponent<Transform>().position = Vector2.Lerp(fireflies1.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies1.GetComponent<Transform>().position.y >= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies1.gameObject);
            }

            if (firefliesk)
            {
                if (!isck)
                {
                    firefliesk.transform.position = kitchen.transform.position;
                    startTimek = Time.time;
                    journeyLengthk = Vector2.Distance(firefliesk.transform.position, this.GetComponent<Transform>().position);
                    isck = true;
                }
                else
                {
                    float distCovered = (Time.time - startTimek) * 10.0f;
                    float fracJourney = distCovered / journeyLengthk;
                    firefliesk.GetComponent<Transform>().position = Vector2.Lerp(firefliesk.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (firefliesk.GetComponent<Transform>().position.y >= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(firefliesk.gameObject);
            }
            ireach1 = false;
            //ireach2 = false;
            //ireach3 = false;
            //ireach4 = false;
            //ireach5 = false;
            //ireach6 = false;
            ireachk = false;
        }


        if (fireflies1)
        {
            if (!c1.moneyEnough|| data.tableLevel == 2)
            {
                ireach1 = false;
                if (!isc1)
                {
                    fireflies1.transform.position = i1.transform.position;
                    startTime1 = Time.time;
                    journeyLength1 = Vector2.Distance(fireflies1.transform.position, this.GetComponent<Transform>().position);
                    isc1 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime1) * 10.0f;
                    float fracJourney = distCovered / journeyLength1;
                    fireflies1.GetComponent<Transform>().position = Vector2.Lerp(fireflies1.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies1.GetComponent<Transform>().position.y >= this.GetComponent<Transform>().position.y * 1.1f)
                    Destroy(fireflies1.gameObject);
            }
        }


        if (firefliesk)
        {
            if (!kc.moneyEnough|| data.kitchenLevel == 2)
            {
                ireachk = false;
                if (!isck)
                {
                    firefliesk.transform.position = kitchen.transform.position;
                    startTimek = Time.time;
                    journeyLengthk = Vector2.Distance(firefliesk.transform.position, this.GetComponent<Transform>().position);
                    isck = true;
                }
                else
                {
                    float distCovered = (Time.time - startTimek) * 10.0f;
                    float fracJourney = distCovered / journeyLengthk;
                    firefliesk.GetComponent<Transform>().position = Vector2.Lerp(firefliesk.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (firefliesk.GetComponent<Transform>().position.y >= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(firefliesk.gameObject);
            }
        }
    }

    //void MoveBack(Image fireflies)
    //{


    //}

    void Reach()
    {
        if (fireflies1)
        {
            if (fireflies1.transform.position.y <= i1.GetComponent<Transform>().position.y * 0.95f)
            {
                ireach1 = true;
            }
            else
            {
                ireach1 = false;
            }
        }

        if (firefliesk)
        {
            if (firefliesk.transform.position.y >= kitchen.GetComponent<Transform>().position.y * 0.95f)
            {
                ireachk = true;
            }
            else
            {
                ireachk = false;
            }
        }
    }
}
