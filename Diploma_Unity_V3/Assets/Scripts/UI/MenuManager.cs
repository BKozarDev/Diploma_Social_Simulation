using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject UI;

    // Create Menu values
    [Header("InputFields")]
    public InputField input_Vitality;
    public InputField input_Endurance;
    public InputField input_Strength;
    public InputField input_Gluttony;

    public InputField input_Ph;
    public InputField input_Pp;
    public InputField input_Name;

    GenericAlgorythm ga;

    private void Start() {
        input_Vitality = GameObject.Find("Vitality_Input").GetComponent<InputField>();
        input_Endurance = GameObject.Find("Endurance_Input").GetComponent<InputField>();
        input_Strength = GameObject.Find("Strength_Input").GetComponent<InputField>(); 
        input_Gluttony = GameObject.Find("Gluttony_Input").GetComponent<InputField>();

        input_Ph = GameObject.Find("Ph_Input").GetComponent<InputField>();
        input_Pp = GameObject.Find("Pp_Input").GetComponent<InputField>();
        input_Name = GameObject.Find("Name_Input").GetComponent<InputField>();

        ga = GetComponent<GenericAlgorythm>();
    }

    bool isHide;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isHide)
            {
                UI.SetActive(false);
                isHide = true;
            } else
            {
                UI.SetActive(true);
                isHide = false;
            }
        }
    }

    private int vitality;
    private int endurance;
    private int strength;
    private int gluttony;

    private float ph;
    private float pp;

    private string name;

    public void CreateChild()
    {
        vitality = int.Parse(input_Vitality.text);
        endurance = int.Parse(input_Endurance.text);
        strength = int.Parse(input_Strength.text);
        gluttony = int.Parse(input_Gluttony.text);

        ph = (float) int.Parse(input_Ph.text) / 100f;
        pp = (float) int.Parse(input_Pp.text) / 100f;

        name = input_Name.text;

        ga = new GenericAlgorythm(vitality, endurance, strength, gluttony, ph, pp, name);
    }
}
