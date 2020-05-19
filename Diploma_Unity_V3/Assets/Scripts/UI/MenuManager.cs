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

    public InputField input_count;

    GenericAlgorythm ga;
    RandomValues rv;

    public GameObject menu;
    public GameObject menuCreation;

    private void Start()
    {
        input_Vitality = GameObject.Find("Vitality_Input").GetComponent<InputField>();
        input_Endurance = GameObject.Find("Endurance_Input").GetComponent<InputField>();
        input_Strength = GameObject.Find("Strength_Input").GetComponent<InputField>();
        input_Gluttony = GameObject.Find("Gluttony_Input").GetComponent<InputField>();

        input_Ph = GameObject.Find("Ph_Input").GetComponent<InputField>();
        input_Pp = GameObject.Find("Pp_Input").GetComponent<InputField>();
        input_Name = GameObject.Find("Name_Input").GetComponent<InputField>();

        input_count = GameObject.Find("CountAgent").GetComponent<InputField>();

        menu = GameObject.Find("Menu");
        menuCreation = GameObject.Find("Create_Menu");
        menuCreation.SetActive(false);

        UI.SetActive(false);

        ga = GetComponent<GenericAlgorythm>();
        rv = FindObjectOfType<RandomValues>();
    }

    bool isHide;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isHide)
            {
                UI.SetActive(false);
                isHide = true;
            }
            else
            {
                UI.SetActive(true);
                isHide = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (UI.active)
        {


            if (menu.active)
            {
                input_count = GameObject.Find("CountAgent").GetComponent<InputField>();
            }

            if (menuCreation.active)
            {
                input_Vitality = GameObject.Find("Vitality_Input").GetComponent<InputField>();
                input_Endurance = GameObject.Find("Endurance_Input").GetComponent<InputField>();
                input_Strength = GameObject.Find("Strength_Input").GetComponent<InputField>();
                input_Gluttony = GameObject.Find("Gluttony_Input").GetComponent<InputField>();

                input_Ph = GameObject.Find("Ph_Input").GetComponent<InputField>();
                input_Pp = GameObject.Find("Pp_Input").GetComponent<InputField>();
                input_Name = GameObject.Find("Name_Input").GetComponent<InputField>();
            }
        }
    }

    public void Switch()
    {
        if(menu.active)
        {
            menu.SetActive(false);
            menuCreation.SetActive(true);
        } else if(menuCreation.active)
        {
            menuCreation.SetActive(false);
            menu.SetActive(true);
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

        ph = (float)int.Parse(input_Ph.text) / 100f;
        pp = (float)int.Parse(input_Pp.text) / 100f;

        name = input_Name.text;

        ga = new GenericAlgorythm(vitality, endurance, strength, gluttony, ph, pp, name);

        UI.SetActive(false);
        ToNull();
    }

    public void CountChild()
    {
        CreateChild(int.Parse(input_count.text));
    }

    private void CreateChild(int count)
    {
        for (int i = 0; i < count; i++)
        {
            vitality = Random.Range(1, 99);
            endurance = Random.Range(1, 99);
            strength = Random.Range(1, 99);
            gluttony = Random.Range(1, 99);

            ph = Random.Range(1f, 99f);
            pp = Random.Range(1f, 99f);

            name = rv.GetRandomName();

            ga = new GenericAlgorythm(vitality, endurance, strength, gluttony, ph, pp, name);
            UI.SetActive(false);
        }

        input_count.text = "";
    }

    private void ToNull()
    {
        input_Vitality.text = "";
        input_Endurance.text = "";
        input_Strength.text = "";
        input_Gluttony.text = "";

        input_Ph.text = "";
        input_Pp.text = "";

        input_Name.text = "";
    }
}
