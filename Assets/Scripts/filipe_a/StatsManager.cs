using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StatsManager : MonoBehaviour
{
    [Header("Battery")]
    public Image batteryImage;
    public Sprite[] batterySprites;
    public TextMeshProUGUI batteryPercentageText;

    private int batteryState = 3;
    //Remove serializeField, it's just for the easy debug
    [SerializeField] [Range(0, 100)]
    private int batteryPercentageValue = 100;

    [Header("Fuel")]
    public Image fuelImage;
    public TextMeshProUGUI fuelPercentageText;

    [SerializeField] [Range(0, 100)]
    private float fuelPercentageValue = 100;

    [Header("Shield")]
    public Image[] shields;

    [SerializeField] [Range(0, 6)]
    private int shieldState = 6;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBattery();
        UpdateFuel();
        UpdateShields();
    }

    void UpdateFuel()
    {
        fuelPercentageText.text = (int)fuelPercentageValue + "\n%";
        fuelImage.fillAmount = fuelPercentageValue / 100;
    }

    void UpdateBattery()
    {
        batteryState = (batteryPercentageValue / 25);
        batteryPercentageText.text = batteryPercentageValue + "\n%";

        switch (batteryState)
        {
            case 0:
            case 1:
                batteryImage.sprite = batterySprites[0];
                break;
            case 2:
                batteryImage.sprite = batterySprites[1];
                break;
            case 3:
                batteryImage.sprite = batterySprites[2];
                break;
            case 4:
                batteryImage.sprite = batterySprites[3];
                break;
            default:
                Debug.Log("[Battery] current state not found. Cannot change UI Image");
                break;
        }
    }

    void UpdateShields()
    {
        switch (shieldState)
        {
            case 0:
                shields[0].fillAmount = 0;
                break;
            case 1:
                shields[0].fillAmount = 0.5f;
                break;
            case 2:
                shields[0].fillAmount = 1;
                shields[1].fillAmount = 0f;
                break;
            case 3:
                shields[1].fillAmount = 0.5f;
                break;
            case 4:
                shields[1].fillAmount = 1;
                shields[2].fillAmount = 0;
                break;
            case 5:
                shields[2].fillAmount = 0.5f;
                break;
            case 6:
                shields[2].fillAmount = 1;
                break;
        }
    }
}
