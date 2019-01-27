using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StatsManager : MonoBehaviour
{
    public PlayerDataSO playerData;

    [Header("Battery")]

    public Image batteryImage;
    public Sprite[] batterySprites;
    public TextMeshProUGUI batteryPercentageText;

    private int batteryState = 3;
    private int batteryPercentageValue = 100;

    /*[Header("Fuel")]

    public Image fuelImage;
    public TextMeshProUGUI fuelPercentageText;

    private float fuelPercentageValue = 100;*/

    [Header("Life")]

    public Image[] heartImages;
    public TextMeshProUGUI lifePercentageText;

    private int lifeState = 6;
    private int lifePercentageValue = 100;

    void Update()
    {
        UpdateBattery();
        //UpdateFuel();
        UpdateLife();
    }


    void UpdateBattery()
    {
        batteryPercentageValue = Mathf.Clamp(playerData.m_battery, 0, playerData.m_maxBattery);
        batteryPercentageText.text = batteryPercentageValue + "\n%";
        batteryState = (batteryPercentageValue * 6 / playerData.m_maxBattery) + 1;
        if (batteryPercentageValue <= 0) { batteryState = 0; }

        switch (batteryState)
        {
            case 0:                                            // 0 à 25
                batteryImage.sprite = batterySprites[0];
                break;
            case 1:
                batteryImage.sprite = batterySprites[1];
                break;
            case 2:                                            // 25 à 50
                batteryImage.sprite = batterySprites[2];
                break;
            case 3:                                            // 50 à 75 
                batteryImage.sprite = batterySprites[3];
                break;
            case 4:                                            // 50 à 75 
                batteryImage.sprite = batterySprites[4];
                break;
            case 5:                                            // 50 à 75 
                batteryImage.sprite = batterySprites[5];
                break;
            case 6:                                            // 75 à 100
            case 7:                                            // 100
                batteryImage.sprite = batterySprites[6];
                break;
            default:
                Debug.Log("[Battery] current state not found. Cannot change UI Image");
                break;
        }
    }

    //void UpdateFuel()
    //{
    //    fuelPercentageText.text = (int)fuelPercentageValue + "\n%";
    //    fuelImage.fillAmount = fuelPercentageValue / 100;
    //}

    void UpdateLife()
    {
        lifePercentageValue = Mathf.Clamp(playerData.m_life, 0, playerData.m_maxLife);
        lifePercentageText.text = lifePercentageValue + "\n%";
        lifeState = (lifePercentageValue * 6 / playerData.m_maxLife) + 1;
        if (lifePercentageValue <= 0) { lifeState= 0; }

        switch (lifeState)
        {
            case 0:
                foreach (Image heart in heartImages)
                    heart.fillAmount = 0;
                break;
            case 1:
                heartImages[0].fillAmount = 0.5f;
                break;
            case 2:
                heartImages[0].fillAmount = 1;
                heartImages[1].fillAmount = 0f;
                break;
            case 3:
                heartImages[1].fillAmount = 1;
                heartImages[1].fillAmount = 0.5f;
                break;
            case 4:
                heartImages[1].fillAmount = 1;
                heartImages[2].fillAmount = 0;
                break;
            case 5:
                heartImages[1].fillAmount = 1;
                heartImages[2].fillAmount = 0.5f;
                break;
            case 6:
            case 7:
                foreach (Image heart in heartImages)
                    heart.fillAmount = 1;
                break;
        }
    }
}
