using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commodity : MonoBehaviour
{
    public Button buyBtn;

    public Button soldOutBtn;

    public Button adsBtn;

    public float priceValue;

    public float getCoinValue;

    public Text priceText;

    public Text getCoinText;

    private void Start()
    {
        priceText.text = priceValue.ToString();

        getCoinText.text = getCoinValue.ToString();

        buyBtn.onClick.AddListener(() => { ClickBuyBtn(); });
        adsBtn.onClick.AddListener(() => { ClickAds(); });
        soldOutBtn.onClick.AddListener(() => { ClickSold(); });
        if (Random.Range(0, 100) > 50)
        {

            if (Random.Range(0, 3) > 1)
            {
                ShowADS();

            }
            else
            {
                ShowSold();
            }
        }


    }

    public void ShowSold()
    {
        soldOutBtn.SetActive(true);
    }

    public void ShowADS()
    {
        adsBtn.SetActive(true);
    }

    public void ClickAds()
    {
        print("ADS");
    }

    public void ClickBuyBtn()
    {
        print("Buy");
    }

    public void ClickSold()
    {
        print("Sold");
    }

}
