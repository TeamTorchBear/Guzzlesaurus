using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JarBtnClick : MonoBehaviour
{

    public CanBeUpgrade i1, i2, i3, i4, i5, i6, kitchen;
    int quantity;
    bool is1, is2, is3, is4, is5, is6, ks;
    // Use this for initialization
    void Start()
    {
        i1 = i1.GetComponent<CanBeUpgrade>();
        i2 = i2.GetComponent<CanBeUpgrade>();
        i3 = i3.GetComponent<CanBeUpgrade>();
        i4 = i4.GetComponent<CanBeUpgrade>();
        i5 = i5.GetComponent<CanBeUpgrade>();
        i6 = i6.GetComponent<CanBeUpgrade>();
        kitchen = kitchen.GetComponent<CanBeUpgrade>();
        quantity = 0;
        is1 = false;
        is2 = false;
        is3 = false;
        is4 = false;
        is5 = false;
        is6 = false;
        ks = false;

        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        IfCanUpgrade();

    }

    void IfCanUpgrade() {
        if (!is1)
        {
            if (i1.moneyEnough)
                quantity++;
            is1 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is2)
        {
            if (i2.moneyEnough)
                quantity++;
            is2 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is3)
        {
            if (i3.moneyEnough)
                quantity++;
            is3 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is4)
        {
            if (i4.moneyEnough)
                quantity++;
            is4 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is5)
        {
            if (i5.moneyEnough)
                quantity++;
            is5 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!is6)
        {
            if (i6.moneyEnough)
                quantity++;
            is6 = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
        if (!ks)
        {
            if (kitchen.moneyEnough)
                quantity++;
            ks = true;
            Resources.Load<Inventory>("Inventory").canUpgradeQuantity = quantity;
        }
    }

    void OnClick()
    {

    }
}
