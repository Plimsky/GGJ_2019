using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    public TextBoxSO m_textBoxData;
    public TextMeshProUGUI m_text;
    public Image panel;
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
        DialogExample();
        panelOriginalColor = panel.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        m_textBoxData.currentPage = m_text.pageToDisplay;
        if (Input.GetKeyDown(KeyCode.Space) && m_text.pageToDisplay + 1 <= m_textBoxData.pageMax)
            m_text.pageToDisplay++;
        else if (Input.GetKeyDown(KeyCode.Space) && m_text.pageToDisplay + 1 > m_textBoxData.pageMax)
            FadeOut();

        if (panel.color == Color.clear && Input.GetKey(KeyCode.R))
            curentState = state.FADEIN;
        if (curentState == state.FADEOUT)
        {
            panel.color = Color.Lerp(panel.color, Color.clear, timeToFade * Time.deltaTime);
            m_text.color = Color.Lerp(m_text.color, Color.clear, timeToFade * Time.deltaTime);
            if (panel.color == Color.clear)
            {
                curentState = state.STABLE;
                m_text.pageToDisplay = 1;
            }
        }
        if (curentState == state.FADEIN)
        {
            panel.color = Color.Lerp(panel.color, panelOriginalColor, timeToFade * Time.deltaTime);
            m_text.color = Color.Lerp(m_text.color, Color.white, timeToFade * Time.deltaTime);
            if (panel.color == panelOriginalColor)
                curentState = state.STABLE;
        }
    }

    void DialogExample()
    {
        m_textBoxData.text = "Huh... Where... Am I... I... Have a hard time remembering...\nHey... I'm on my spaceship ! I need to get back home ! ...But without my hyperdrive fuel I'll never make it... I need to find crystals !";
        m_text.text = m_textBoxData.text;
        m_textBoxData.pageMax = 2;
        m_text.pageToDisplay = 1;
    }

    void FadeOut()
    {
        curentState = state.FADEOUT;
    }

    void FadeIn()
    {
        curentState = state.FADEIN;
        m_text.pageToDisplay = 0;
        m_textBoxData.currentPage = 0;
    }

}
