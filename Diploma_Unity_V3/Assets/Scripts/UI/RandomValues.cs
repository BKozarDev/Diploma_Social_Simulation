using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomValues : MonoBehaviour
{
    InputField iField;

    private void Start() {
        iField = GetComponent<InputField>();
    }

    public void SetRandom()
    {
        iField.text = Random.Range(1, 99).ToString();
    }

    public string[] names;
    public void SetName()
    {
        var random = Random.Range(0, names.Length-1);

        iField.text = names[random];
    }
}
