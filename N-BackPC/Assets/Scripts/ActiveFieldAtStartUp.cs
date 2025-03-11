
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveFieldAtStartUp : MonoBehaviour
{
    private TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void activateField()
    {
        inputField.ActivateInputField();
    }
}
