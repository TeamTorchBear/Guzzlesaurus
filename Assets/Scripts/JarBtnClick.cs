using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JarBtnClick : MonoBehaviour
{
    public bool isFireflies;
    public Image i1, i2, i3, i4, i5, i6, kitchen, Screen;
    public Transform canvas;
    public int quantity;
    public Image prefab;

    CanBeUpgrade c1, c2, c3, c4, c5, c6, kc;
    bool isClick, isClick1, isClick2, isClick3, isClick4, isClick5, isClick6, isClickk;
    bool ireach1 = false, ireach2 = false, ireach3 = false, ireach4 = false, ireach5 = false, ireach6 = false, ireachk = false;
    Image fireflies1, fireflies2, fireflies3, fireflies4, fireflies5, fireflies6, firefliesk;
    bool isc1 = false, isc2 = false, isc3 = false, isc4 = false, isc5 = false, isc6 = false, isck = false;
    float startTime1 = 0, startTime2 = 0, startTime3 = 0, startTime4 = 0, startTime5 = 0, startTime6 = 0, startTimek = 0;
    float journeyLength1 = 0, journeyLength2 = 0, journeyLength3 = 0, journeyLength4 = 0, journeyLength5 = 0, journeyLength6 = 0, journeyLengthk = 0;
    bool is1, is2, is3, is4, is5, is6, ks;

    // Use this for initialization
    void Awake()
    {
        c1 = i1.GetComponent<CanBeUpgrade>();
        c2 = i2.GetComponent<CanBeUpgrade>();
        c3 = i3.GetComponent<CanBeUpgrade>();
        c4 = i4.GetComponent<CanBeUpgrade>();
        c5 = i5.GetComponent<CanBeUpgrade>();
        c6 = i6.GetComponent<CanBeUpgrade>();
        kc = kitchen.GetComponent<CanBeUpgrade>();
        quantity = 0;
        is1 = false;
        is2 = false;
        is3 = false;
        is4 = false;
        is5 = false;
        is6 = false;
        ks = false;
        isClick = false;
        isFireflies = false;

        isClick1 = false; isClick2 = false; isClick3 = false; isClick4 = false; isClick5 = false; isClick6 = false; isClickk = false;


        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);

        Button btn1 = c1.GetComponent<Button>();
        btn1.onClick.AddListener(OnClick1);
        Button btn2 = c2.GetComponent<Button>();
        btn2.onClick.AddListener(OnClick2);
        Button btn3 = c3.GetComponent<Button>();
        btn3.onClick.AddListener(OnClick3);
        Button btn4 = c4.GetComponent<Button>();
        btn4.onClick.AddListener(OnClick4);
        Button btn5 = c5.GetComponent<Button>();
        btn5.onClick.AddListener(OnClick5);
        Button btn6 = c6.GetComponent<Button>();
        btn6.onClick.AddListener(OnClick6);
        Button btnk = kc.GetComponent<Button>();
        btnk.onClick.AddListener(OnClickk);

        IfCanUpgrade();
    }

    // Update is called once per frame
    void Update()
    {

        if (isClick)
        {
            if (!isFireflies)
            {
                isFireflies = true;
                quantity = 0;
                is1 = false;
                is2 = false;
                is3 = false;
                is4 = false;
                is5 = false;
                is6 = false;
                ks = false;
                isc1 = false;
                isc2 = false;
                isc3 = false;
                isc4 = false;
                isc5 = false;
                isc6 = false;
                isck = false;
                AllItemUpdate();
                Screen.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }
            else if (isFireflies)
            {
                isFireflies = false;
                quantity = 0;
                is1 = false;
                is2 = false;
                is3 = false;
                is4 = false;
                is5 = false;
                is6 = false;
                ks = false;
                isc1 = false;
                isc2 = false;
                isc3 = false;
                isc4 = false;
                isc5 = false;
                isc6 = false;
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
            if (c1.moneyEnough && isFireflies)
            {
                Debug.Log("Upgrade " + c1.name);
                c1.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
                is2 = false;
                is3 = false;
                is4 = false;
                is5 = false;
                is6 = false;
                ks = false;
            }
            isClick1 = false;

            IfCanUpgrade();
        }
        if (isClick2)
        {
            if (c2.moneyEnough && isFireflies)
            {
                Debug.Log("Upgrade " + c2.name);
                c2.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
                is2 = false;
                is3 = false;
                is4 = false;
                is5 = false;
                is6 = false;
                ks = false;
            }
            isClick2 = false;
            IfCanUpgrade();
        }
        if (isClick3)
        {
            if (c3.moneyEnough && isFireflies)
            {
                Debug.Log("Upgrade " + c3.name);
                c3.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
                is2 = false;
                is3 = false;
                is4 = false;
                is5 = false;
                is6 = false;
                ks = false;
            }
            isClick3 = false;
            IfCanUpgrade();
        }
        if (isClick4)
        {
            if (c4.moneyEnough && isFireflies)
            {
                Debug.Log("Upgrade " + c4.name);
                c4.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
                is2 = false;
                is3 = false;
                is4 = false;
                is5 = false;
                is6 = false;
                ks = false;
            }
            isClick4 = false;
            IfCanUpgrade();
        }
        if (isClick5)
        {
            if (c5.moneyEnough && isFireflies)
            {
                Debug.Log("Upgrade " + c5.name);
                c5.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
                is2 = false;
                is3 = false;
                is4 = false;
                is5 = false;
                is6 = false;
                ks = false;
            }
            isClick5 = false;
            IfCanUpgrade();
        }
        if (isClick6)
        {
            if (c6.moneyEnough && isFireflies)
            {
                Debug.Log("Upgrade " + c6.name);
                c6.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
                is2 = false;
                is3 = false;
                is4 = false;
                is5 = false;
                is6 = false;
                ks = false;
            }
            isClick6 = false;
            IfCanUpgrade();
        }
        if (isClickk)
        {
            if (kc.moneyEnough && isFireflies)
            {
                Debug.Log("Upgrade " + kc.name);
                kc.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
                is2 = false;
                is3 = false;
                is4 = false;
                is5 = false;
                is6 = false;
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
        if (ireach2)
        {
            i2.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        }
        else
        {
            i2.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        if (ireach3)
        {
            i3.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        }
        else
        {
            i3.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        if (ireach4)
        {
            i4.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        }
        else
        {
            i4.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        if (ireach5)
        {
            i5.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        }
        else
        {
            i5.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        if (ireach6)
        {
            i6.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
        }
        else
        {
            i6.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is2)
        {
            if (c2.moneyEnough)
            {
                quantity++;
                //    if (isFireflies)
                //    {
                //        i2.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
                //    }
                //    else
                //    {
                //        i2.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                //    }
                //}
                //else
                //{
                //    i2.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            is2 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is3)
        {
            if (c3.moneyEnough)
            {
                quantity++;
                //    if (isFireflies)
                //    {
                //        i3.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
                //    }
                //    else
                //    {
                //        i3.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                //    }
                //}
                //else
                //{
                //    i3.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            is3 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is4)
        {
            if (c4.moneyEnough)
            {
                quantity++;
                //    if (isFireflies)
                //    {
                //        i4.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
                //    }
                //    else
                //    {
                //        i4.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                //    }
                //}
                //else
                //{
                //    i4.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            is4 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is5)
        {
            if (c5.moneyEnough)
            {
                quantity++;
                //    if (isFireflies)
                //    {
                //        i5.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
                //    }
                //    else
                //    {
                //        i5.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                //    }
                //}
                //else
                //{
                //    i5.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            is5 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is6)
        {
            if (c6.moneyEnough)
            {
                quantity++;
                //    if (isFireflies)
                //    {
                //        i6.color = new Color(1.0f, 1.0f, 0.5f, 1.0f);
                //    }
                //    else
                //    {
                //        i6.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                //    }
                //}
                //else
                //{
                //    i6.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            is6 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
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
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
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
    void OnClick2()
    {
        isClick2 = true;
    }
    void OnClick3()
    {
        isClick3 = true;
    }
    void OnClick4()
    {
        isClick4 = true;
    }
    void OnClick5()
    {
        isClick5 = true;
    }
    void OnClick6()
    {
        isClick6 = true;
    }
    void OnClickk()
    {
        isClickk = true;
    }

    void AllItemUpdate()
    {
        c1.updates();
        c2.updates();
        c3.updates();
        c4.updates();
        c5.updates();
        c6.updates();
        kc.updates();
    }

    void CreateFireflies()
    {
        if (isFireflies)
        {
            if (c1.moneyEnough)
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
            if (c2.moneyEnough)
            {
                if (!fireflies2)
                {
                    fireflies2 = Instantiate(prefab);
                    fireflies2.transform.SetParent(canvas);
                    fireflies2.transform.position = this.transform.position;
                }
                if (!isc2)
                {
                    startTime2 = Time.time;
                    journeyLength2 = Vector2.Distance(fireflies2.transform.position, i2.GetComponent<Transform>().position);
                    isc2 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime2) * 10.0f;
                    float fracJourney = distCovered / journeyLength2;
                    fireflies2.GetComponent<Transform>().position = Vector2.Lerp(fireflies2.transform.position, i2.GetComponent<Transform>().position, fracJourney);

                }
            }
            if (c3.moneyEnough)
            {
                if (!fireflies3)
                {
                    fireflies3 = Instantiate(prefab);
                    fireflies3.transform.SetParent(canvas);
                    fireflies3.transform.position = this.transform.position;
                }
                if (!isc3)
                {
                    startTime3 = Time.time;
                    journeyLength3 = Vector2.Distance(fireflies3.transform.position, i3.GetComponent<Transform>().position);
                    isc3 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime3) * 10.0f;
                    float fracJourney = distCovered / journeyLength3;
                    fireflies3.GetComponent<Transform>().position = Vector2.Lerp(fireflies3.transform.position, i3.GetComponent<Transform>().position, fracJourney);

                }
            }
            if (c4.moneyEnough)
            {
                if (!fireflies4)
                {
                    fireflies4 = Instantiate(prefab);
                    fireflies4.transform.SetParent(canvas);
                    fireflies4.transform.position = this.transform.position;
                }
                if (!isc4)
                {
                    startTime4 = Time.time;
                    journeyLength4 = Vector2.Distance(fireflies4.transform.position, i4.GetComponent<Transform>().position);
                    isc4 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime4) * 10.0f;
                    float fracJourney = distCovered / journeyLength4;
                    fireflies4.GetComponent<Transform>().position = Vector2.Lerp(fireflies4.transform.position, i4.GetComponent<Transform>().position, fracJourney);
                }
            }
            if (c5.moneyEnough)
            {
                if (!fireflies5)
                {
                    fireflies5 = Instantiate(prefab);
                    fireflies5.transform.SetParent(canvas);
                    fireflies5.transform.position = this.transform.position;
                }
                if (!isc5)
                {
                    startTime5 = Time.time;
                    journeyLength5 = Vector2.Distance(fireflies5.transform.position, i5.GetComponent<Transform>().position);
                    isc5 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime5) * 10.0f;
                    float fracJourney = distCovered / journeyLength5;
                    fireflies5.GetComponent<Transform>().position = Vector2.Lerp(fireflies5.transform.position, i5.GetComponent<Transform>().position, fracJourney);

                }
            }
            if (c6.moneyEnough)
            {
                if (!fireflies6)
                {
                    fireflies6 = Instantiate(prefab);
                    fireflies6.transform.SetParent(canvas);
                    fireflies6.transform.position = this.transform.position;
                }
                if (!isc6)
                {
                    startTime6 = Time.time;
                    journeyLength6 = Vector2.Distance(fireflies6.transform.position, i6.GetComponent<Transform>().position);
                    isc6 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime6) * 10.0f;
                    float fracJourney = distCovered / journeyLength6;
                    fireflies6.GetComponent<Transform>().position = Vector2.Lerp(fireflies6.transform.position, i6.GetComponent<Transform>().position, fracJourney);

                }
            }
            if (kc.moneyEnough)
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
                if (fireflies1.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies1);
            }
            if (fireflies2)
            {
                if (!isc2)
                {
                    fireflies2.transform.position = i2.transform.position;
                    startTime2 = Time.time;
                    journeyLength2 = Vector2.Distance(fireflies2.transform.position, this.GetComponent<Transform>().position);
                    isc2 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime2) * 10.0f;
                    float fracJourney = distCovered / journeyLength2;
                    fireflies2.GetComponent<Transform>().position = Vector2.Lerp(fireflies2.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies2.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies2);
            }
            if (fireflies3)
            {
                if (!isc3)
                {
                    fireflies3.transform.position = i3.transform.position;
                    startTime3 = Time.time;
                    journeyLength3 = Vector2.Distance(fireflies3.transform.position, this.GetComponent<Transform>().position);
                    isc3 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime3) * 10.0f;
                    float fracJourney = distCovered / journeyLength3;
                    fireflies3.GetComponent<Transform>().position = Vector2.Lerp(fireflies3.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies3.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies3);
            }
            if (fireflies4)
            {
                if (!isc4)
                {
                    fireflies4.transform.position = i4.transform.position;
                    startTime4 = Time.time;
                    journeyLength4 = Vector2.Distance(fireflies4.transform.position, this.GetComponent<Transform>().position);
                    isc4 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime4) * 10.0f;
                    float fracJourney = distCovered / journeyLength4;
                    fireflies4.GetComponent<Transform>().position = Vector2.Lerp(fireflies4.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies4.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies4);
            }
            if (fireflies5)
            {
                if (!isc5)
                {
                    fireflies5.transform.position = i5.transform.position;
                    startTime5 = Time.time;
                    journeyLength5 = Vector2.Distance(fireflies5.transform.position, this.GetComponent<Transform>().position);
                    isc5 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime5) * 10.0f;
                    float fracJourney = distCovered / journeyLength5;
                    fireflies5.GetComponent<Transform>().position = Vector2.Lerp(fireflies5.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies5.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies5);
            }
            if (fireflies6)
            {
                if (!isc6)
                {
                    fireflies6.transform.position = i6.transform.position;
                    startTime6 = Time.time;
                    journeyLength6 = Vector2.Distance(fireflies6.transform.position, this.GetComponent<Transform>().position);
                    isc6 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime6) * 10.0f;
                    float fracJourney = distCovered / journeyLength6;
                    fireflies6.GetComponent<Transform>().position = Vector2.Lerp(fireflies6.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies6.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies6);
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
                if (firefliesk.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(firefliesk);
            }
            ireach1 = false;
            ireach2 = false;
            ireach3 = false;
            ireach4 = false;
            ireach5 = false;
            ireach6 = false;
            ireachk = false;
        }

        if (fireflies1)
        {
            if (!c1.moneyEnough)
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
                if (fireflies1.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies1);
            }
        }
        if (fireflies2)
        {
            if (!c2.moneyEnough)
            {
                ireach2 = false;
                if (!isc2)
                {
                    fireflies2.transform.position = i2.transform.position;
                    startTime2 = Time.time;
                    journeyLength2 = Vector2.Distance(fireflies2.transform.position, this.GetComponent<Transform>().position);
                    isc2 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime2) * 10.0f;
                    float fracJourney = distCovered / journeyLength2;
                    fireflies2.GetComponent<Transform>().position = Vector2.Lerp(fireflies2.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies2.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies2);
            }
        }
        if (fireflies3)
        {
            if (!c3.moneyEnough)
            {
                ireach3 = false;
                if (!isc3)
                {
                    fireflies3.transform.position = i3.transform.position;
                    startTime3 = Time.time;
                    journeyLength3 = Vector2.Distance(fireflies3.transform.position, this.GetComponent<Transform>().position);
                    isc3 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime3) * 10.0f;
                    float fracJourney = distCovered / journeyLength3;
                    fireflies3.GetComponent<Transform>().position = Vector2.Lerp(fireflies3.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies3.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies3);
            }
        }
        if (fireflies4)
        {
            if (!c4.moneyEnough)
            {
                ireach4 = false;
                if (!isc4)
                {
                    fireflies4.transform.position = i4.transform.position;
                    startTime4 = Time.time;
                    journeyLength4 = Vector2.Distance(fireflies4.transform.position, this.GetComponent<Transform>().position);
                    isc4 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime4) * 10.0f;
                    float fracJourney = distCovered / journeyLength4;
                    fireflies4.GetComponent<Transform>().position = Vector2.Lerp(fireflies4.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies4.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies4);
            }
        }
        if (fireflies5)
        {
            if (!c5.moneyEnough)
            {
                ireach5 = false;
                if (!isc5)
                {
                    fireflies5.transform.position = i5.transform.position;
                    startTime5 = Time.time;
                    journeyLength5 = Vector2.Distance(fireflies5.transform.position, this.GetComponent<Transform>().position);
                    isc5 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime5) * 10.0f;
                    float fracJourney = distCovered / journeyLength5;
                    fireflies5.GetComponent<Transform>().position = Vector2.Lerp(fireflies5.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies5.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies5);
            }
        }
        if (fireflies6)
        {
            if (!c6.moneyEnough)
            {
                ireach6 = false;
                if (!isc6)
                {
                    fireflies6.transform.position = i6.transform.position;
                    startTime6 = Time.time;
                    journeyLength6 = Vector2.Distance(fireflies6.transform.position, this.GetComponent<Transform>().position);
                    isc6 = true;
                }
                else
                {
                    float distCovered = (Time.time - startTime6) * 10.0f;
                    float fracJourney = distCovered / journeyLength6;
                    fireflies6.GetComponent<Transform>().position = Vector2.Lerp(fireflies6.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (fireflies6.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(fireflies6);
            }
        }
        if (firefliesk)
        {
            if (!kc.moneyEnough)
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
                if (firefliesk.GetComponent<Transform>().position.y <= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(firefliesk);
            }
        }
    }

    void MoveBack(Image fireflies)
    {


    }

    void Reach()
    {
        if (fireflies1)
        {
            if (fireflies1.transform.position.y >= i1.GetComponent<Transform>().position.y * 0.95f)
            {
                ireach1 = true;
            }
            else
            {
                ireach1 = false;
            }
        }
        if (fireflies2)
        {
            if (fireflies2.transform.position.y >= i2.GetComponent<Transform>().position.y * 0.95f)
            {
                ireach2 = true;
            }
            else
            {
                ireach2 = false;
            }
        }
        if (fireflies3)
        {
            if (fireflies3.transform.position.y >= i3.GetComponent<Transform>().position.y * 0.95f)
            {
                ireach3 = true;
            }
            else
            {
                ireach3 = false;
            }
        }
        if (fireflies4)
        {
            if (fireflies4.transform.position.y >= i4.GetComponent<Transform>().position.y * 0.95f)
            {
                ireach4 = true;
            }
            else
            {
                ireach4 = false;
            }
        }
        if (fireflies5)
        {
            if (fireflies5.transform.position.y >= i5.GetComponent<Transform>().position.y * 0.95f)
            {
                ireach5 = true;
            }
            else
            {
                ireach5 = false;
            }
        }
        if (fireflies6)
        {
            if (fireflies6.transform.position.y >= i6.GetComponent<Transform>().position.y * 0.95f)
            {
                ireach6 = true;
            }
            else
            {
                ireach6 = false;
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
