using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerNewText : MonoBehaviour
{
    public TextBoxManager m_textBox;
    public string p_scriptedTextFunctionName;
    private bool triggerable = true;

    public void TriggerTextBox()
    {
        m_textBox.Invoke(p_scriptedTextFunctionName, 0);
    }   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_textBox == null || p_scriptedTextFunctionName == null)
            return;
        if (triggerable && other.gameObject.CompareTag("Player"))
        {
            TriggerTextBox();
            triggerable = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (m_textBox == null || p_scriptedTextFunctionName == null)
            return;
        if (triggerable && other.gameObject.CompareTag("Player"))
        {
            TriggerTextBox();
            triggerable = false;
        }
    }
}
