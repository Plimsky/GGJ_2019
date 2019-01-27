using UnityEngine;

[CreateAssetMenu(fileName = "TextBox", menuName = "Scritpable Objects/Text Box", order = 1)]
public class TextBoxSO : ScriptableObject
{
    [TextArea]
    public string text;
    public int pageMax = 0;
    public int currentPage = 0;
}
