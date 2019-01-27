using Managers;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    public TextBoxSO m_textBoxData;
    public TextMeshProUGUI m_spacebar;
    public TextMeshProUGUI m_text;
    public Image panel;
    public GameObject controls;
    private Color panelOriginalColor;
    private float timeToFade = 1.0f;

    enum state
    {
        STABLE,
        FADEOUT,
        FADEIN
    }

    private state curentState;

    private void Start()
    {
        m_text.pageToDisplay = 0;
        curentState = state.STABLE;
        panelOriginalColor = panel.GetComponent<Image>().color;
        panel.color = Color.clear;
        m_text.color = Color.clear;
        GameStart();
        controls.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        m_textBoxData.currentPage = m_text.pageToDisplay;
        if (Input.GetKeyDown(KeyCode.Space) && m_text.pageToDisplay + 1 <= m_textBoxData.pageMax)
            m_text.pageToDisplay++;
        else if (Input.GetKeyDown(KeyCode.Space) && m_text.pageToDisplay + 1 > m_textBoxData.pageMax)
            FadeOut();

        /*if (GameManager.instance.m_playerData.m_fragments >= GameManager.instance.m_lvlList[GameManager.instance.m_lvlIndex].m_minimalFragments)
            FindingMinimalCrystalsLvl1();*/

        if (curentState == state.FADEOUT)
        {
            Time.timeScale = 1;
            panel.color = Color.Lerp(panel.color, Color.clear, timeToFade * Time.unscaledDeltaTime);
            m_text.color = Color.Lerp(m_text.color, Color.clear, timeToFade * Time.unscaledDeltaTime);
            m_spacebar.color = Color.Lerp(m_spacebar.color, Color.clear, timeToFade * Time.unscaledDeltaTime);
            if (panel.color == Color.clear)
            {
                curentState = state.STABLE;
                m_text.pageToDisplay = 1;
            }
            if(controls.gameObject.activeSelf)
                controls.SetActive(false);

        }
        if (curentState == state.FADEIN)
        {
            Time.timeScale = 0;
            panel.color = Color.Lerp(panel.color, panelOriginalColor, timeToFade * Time.unscaledDeltaTime);
            m_text.color = Color.Lerp(m_text.color, Color.white, timeToFade * Time.unscaledDeltaTime);
            m_spacebar.color = Color.Lerp(m_spacebar.color, Color.yellow, timeToFade * Time.unscaledDeltaTime);
            if (panel.color == panelOriginalColor)
                curentState = state.STABLE;
        }
    }

    void GameStart()
    {
        m_textBoxData.text = "Huh... Where... Am I... I... Have a hard time remembering...\nHey... I'm on my spaceship ! I need to get back home ! ...But without my hyperdrive fuel I'll never make it... I need to find crystals !";
        m_text.text = m_textBoxData.text;
        m_textBoxData.pageMax = 1;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FirstEncounter()
    {
        m_textBoxData.text = "Oh ! Another spaceship ? Maybe I can ask for some help !";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FirstVictory()
    {
        m_textBoxData.text = "Well a weapon on this machine would have been useful. Good thing I had the tractor beam to protect myself. What are they ? And what do they want?";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;   
        FadeIn();
    }

    
    void FirstCrystalLvl1()
    {
        m_textBoxData.text = "This crystal gave me some coordinates. Maybe if I find more I could use them like some sort of map?";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FindingMoreCrystalsLvl1()
    {
        m_textBoxData.text = "I can feel it! I am getting closer and closer to my objective. I just need some more crystals and I will know where to make my jump. Let's hope it gets me home... !";
        m_textBoxData.pageMax = 2;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FindingMinimalCrystalsLvl1()
    {
        m_textBoxData.text = "The coordinates seem to have been acquired. I should try these. Or maybe i could find more crystals here? My next location could get more precise.. but it would cost me a lot of time...";
        m_textBoxData.pageMax = 2;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FindingLastCrystalsLvl1()
    {
        m_textBoxData.text = "I knew there would be more ! But the coordinates do not seem  to be getting more precise as I had expected it...";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;   
        FadeIn();
    }

    void EnteringLvl2()
    {
        m_textBoxData.text = "Well a weapon on this machine would have been useful. Good thing I had the tractor beam to protect myself. What are they ? And what do they want?";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FindOtherCrystalsAllLvl()
    {
        m_textBoxData.text = "Why do I keep trying to find more than I need of these? I know they are useless...";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FindLastCrytalsAllLvl()
    {
        m_textBoxData.text = "I think that was the last one. I wont find any more of them in here. It's time to go.";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void EnteringLevel2()
    {
        m_textBoxData.text = "What..? What was that..? That felt really... strange... And... Where am I now ? I thought this would be it... Why am I still in some space wasteland ? I need to find a new warp location... let's go hunt for some crystals again.";
        m_textBoxData.pageMax = 2;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FirstCrystalLvl2()
    {
        m_textBoxData.text = "As easy as last time ! At this rate I will be home in no time... I hope.";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void SecondEncounter()
    {
        m_textBoxData.text = "THEM AGAIN !?!? What is it they want ? Leave me alone already !";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void SecondVictory()
    {
        m_textBoxData.text = "THERE WE GO ! HOW DOES IT FEEL TO LOSE AGAINST A HUMAN !?\n I hate them...";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FindMinimalCrystalsLvl2()
    {
        m_textBoxData.text = "Time to warp again... Let's hope it's the last time. I dont like this feeling...";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void ThirdVictory()
    {
        m_textBoxData.text = "that'll teach them";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FirstCrystalLvl3()
    {
        m_textBoxData.text = "Once again, guess I know what to do.";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void LastCrystalsLvl3()
    {
        m_textBoxData.text = "Right... let's try again, this time is the right one.";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void BeamBatteryLow()
    {
        m_textBoxData.text = "The beam battery is low... I could probably find something around here to charge it.";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void LifeLow()
    {
        m_textBoxData.text = "My ship's structure is falling apart... I should be more careful. I don't want to end up like one of these wrecks";
        m_textBoxData.pageMax = 1;
        m_text.text = m_textBoxData.text;
        m_text.pageToDisplay = 1;
        FadeIn();
    }

    void FadeOut()
    {
        curentState = state.FADEOUT;
    }

    void FadeIn()
    {
        curentState = state.FADEIN;
        m_text.pageToDisplay = 1;
        m_textBoxData.currentPage = 1   ;
    }

}
