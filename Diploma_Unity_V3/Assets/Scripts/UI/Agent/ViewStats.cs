using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewStats : MonoBehaviour
{
    GameObject prefab;
    protected Image stats;

    protected Camera camera;

    private void Start() {
        camera = FindObjectOfType<Camera>();
        prefab = Resources.Load<GameObject>("Prefabs/UI/Stats");

        // stats = Instantiate(prefab, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
    }

    private void Update() {
        transform.LookAt(2 * transform.position - camera.transform.position);
        // stats.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.5f, 0));    
    }
}
