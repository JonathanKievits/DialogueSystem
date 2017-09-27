using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{

    [SerializeField]
    private Button startButton;

    private void Start()
    {
        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(Ready);
    }

    private void Ready()
    {
        SceneManager.LoadScene(1);
    }

}
