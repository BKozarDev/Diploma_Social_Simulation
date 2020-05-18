using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewStats : MonoBehaviour
{
    GameObject prefab;
    protected Image stats;

    private void Start() {
        prefab = Resources.Load<GameObject>("Prefabs/UI/Stats");

        stats = Instantiate(prefab, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
    }

    private void Update() {
        stats.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.5f, 0));    
    }
}
