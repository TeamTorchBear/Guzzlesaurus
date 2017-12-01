using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JarBtnClick : Clickable
{
    public bool isFireflies;
    public static bool isClick1, isClickk;
    public SpriteRenderer i1, kitchen, Screen, book, ovenpipe,unileft;
    public GameObject booknew,back;
    //public Transform canvas;
    public int quantity;
    public GameObject prefab;
    public Text text;

    public Sprite replacementTable;
    public Sprite replacementKitchen;
    public Sprite replacementOvenpipe;


    static Data data;
    CanBeUpgrade c1, kc;
    bool isClick;
    bool ireach1 = false, ireachk = false;
    GameObject fireflies1, firefliesk;
    bool isc1 = false, isck = false;
    float startTime1 = 0, startTimek = 0;
    float journeyLength1 = 0, journeyLengthk = 0;
    bool is1, ks;

    // Use this for initialization
    public override void OnStart()
    {
        data = SaveNLoadTxt.Load();
        c1 = i1.GetComponent<CanBeUpgrade>();
        kc = kitchen.GetComponent<CanBeUpgrade>();
        quantity = 0;
        is1 = false;
        ks = false;
        isClick = false;
        isFireflies = false;

        isClick1 = false; /*isClick2 = false; isClick3 = false; isClick4 = false; isClick5 = false; isClick6 = false;*/
        isClickk = false;
        
        IfCanUpgrade();
        Button btn = this.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnClick);
        }
    }

    // Update is called once per frame
    void Update()
    {
        data = SaveNLoadTxt.Load();

        if (data.tableLevel == 2)
        {
            i1.sprite = replacementTable;
        }
        if (data.kitchenLevel == 2)
        {
            ovenpipe.sprite = replacementOvenpipe;
            kitchen.sprite = replacementKitchen;
        }

        if (isClick)
        {
            if (!isFireflies)
            {
                back.GetComponent<BtnOnClick>().enabled = false;
                isFireflies = true;
                quantity = 0;
                is1 = false;
                ks = false;
                isc1 = false;
                isck = false;
                AllItemUpdate();
                Screen.color = new Color(0.0f, 0.0f, 0.0f,0.5f);
                book.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                book.gameObject.SetActive(true);
                booknew.SetActive(false);
            }
            else if (isFireflies)
            {

                back.GetComponent<BtnOnClick>().enabled = true;
                isFireflies = false;
                quantity = 0;
                is1 = false;
                ks = false;
                isc1 = false;
                isck = false;
                AllItemUpdate();
                Screen.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                book.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                i1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                kitchen.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                ovenpipe.color= new Color(1.0f, 1.0f, 1.0f, 1.0f);
                unileft.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                book.gameObject.SetActive(false);
                booknew.SetActive(true);
            }
            isClick = false;
            IfCanUpgrade();
        }
        CreateFireflies();
        DestroyFireflies();
        Reach();
        if (isClick1)
        {
            if (c1.moneyEnough && isFireflies && data.tableLevel != 2)
            {
                Debug.Log("Upgrade " + c1.name);
                c1.Upgrade();
                AllItemUpdate();
                quantity = 0;
                is1 = false;
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
                ks = false;
            }
            isClickk = false;
            IfCanUpgrade();
        }
        if (isFireflies)
        {
            if (ireach1)
            {
                i1.color = new Color(1, 1, 1, 1.0f);
                book.color= new Color(1, 1, 1, 1.0f);
                fireflies1.GetComponentsInChildren<ParticleSystem>(true)[0].gameObject.SetActive(false);
                fireflies1.GetComponentsInChildren<ParticleSystem>(true)[1].gameObject.SetActive(true);

            }
            else
            {
                i1.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                book.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                fireflies1.GetComponentsInChildren<ParticleSystem>(true)[0].gameObject.SetActive(true);
                fireflies1.GetComponentsInChildren<ParticleSystem>(true)[0].gameObject.SetActive(false);

            }

            if (ireachk)
            {
                ovenpipe.color = new Color(1, 1, 1, 1.0f);
                unileft.color = new Color(1, 1, 1, 1.0f);
                kitchen.color = new Color(1, 1, 1, 1.0f);
                firefliesk.GetComponentsInChildren<ParticleSystem>(true)[0].gameObject.SetActive(false);
                firefliesk.GetComponentsInChildren<ParticleSystem>(true)[1].gameObject.SetActive(true);

            }
            else
            {
                ovenpipe.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                unileft.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                kitchen.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

            }
        }
    }

    public void IfCanUpgrade()
    {
        if (!is1)
        {
            if (c1.moneyEnough)
            {
               // i1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                quantity++;
               
            }

            is1 = true;
        }

        if (!ks)
        {
            if (kc.moneyEnough)
            {
               // kitchen.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                quantity++;
               
            }
            ks = true;
        }
    }

    public override void OnClick()
    {
        Debug.Log("1");
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
                    float distCovered = (Time.time - startTime1) * 2.0f;
                    float fracJourney = distCovered / journeyLength1;
                    fireflies1.GetComponent<Transform>().position = Vector2.Lerp(fireflies1.transform.position, i1.GetComponent<Transform>().position, fracJourney);
                }
            }

            if (kc.moneyEnough && data.kitchenLevel != 2)
            {
                if (!firefliesk)
                {
                    firefliesk = Instantiate(prefab);
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
                    float distCovered = (Time.time - startTimek) * 2.0f;
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
                    fireflies1.GetComponentsInChildren<ParticleSystem>(true)[0].gameObject.SetActive(true);
                    fireflies1.GetComponentsInChildren<ParticleSystem>(true)[1].gameObject.SetActive(false);
                }
                else
                {
                    float distCovered = (Time.time - startTime1) * 2.0f;
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
                    firefliesk.GetComponentsInChildren<ParticleSystem>(true)[0].gameObject.SetActive(true);
                    firefliesk.GetComponentsInChildren<ParticleSystem>(true)[1].gameObject.SetActive(false);
                }
                else
                {
                    float distCovered = (Time.time - startTimek) * 2.0f;
                    float fracJourney = distCovered / journeyLengthk;
                    firefliesk.GetComponent<Transform>().position = Vector2.Lerp(firefliesk.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (firefliesk.GetComponent<Transform>().position.y >= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(firefliesk.gameObject);
            }
            ireach1 = false;
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
                    float distCovered = (Time.time - startTime1) * 2.0f;
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
                    float distCovered = (Time.time - startTimek) * 2.0f;
                    float fracJourney = distCovered / journeyLengthk;
                    firefliesk.GetComponent<Transform>().position = Vector2.Lerp(firefliesk.transform.position, this.GetComponent<Transform>().position, fracJourney);
                }
                if (firefliesk.GetComponent<Transform>().position.y >= this.GetComponent<Transform>().position.y * 1.05f)
                    Destroy(firefliesk.gameObject);
            }
        }
    }
    

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
            if (firefliesk.transform.position.y <= kitchen.GetComponent<Transform>().position.y * 0.95f)
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
