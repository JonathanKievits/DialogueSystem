using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;

public class NpcScript : MonoBehaviour
{
    private Dialogue dia;
    private GameObject dia_window;
    private GameObject node_txt;
    private GameObject npcName_txt;
    private GameObject option_1;
    private GameObject option_2;
    private GameObject option_3;
    private GameObject option_4;
    private GameObject exit;

    private int selected_option = -2;

    public string DialogueDataFilePath;
    public GameObject DialogueWindowPrefab;

    private void Start()
    {
        dia = Load_Dialogue("Assets/" + DialogueDataFilePath);
        var canvas = GameObject.Find("Canvas");

        dia_window = Instantiate<GameObject>(DialogueWindowPrefab);
        dia_window.transform.SetParent(canvas.transform, false);

        RectTransform dia_window_tansform = (RectTransform)dia_window.transform;
        dia_window_tansform.localPosition = new Vector3(0,0,0);

        node_txt = GameObject.Find("DiaNodeText");
        npcName_txt = GameObject.Find("NpcName");
        option_1 = GameObject.Find("Option1");
        option_2 = GameObject.Find("Option2");
        option_3 = GameObject.Find("Option3");
        option_4 = GameObject.Find("Option4");
        exit = GameObject.Find("End");

        exit.GetComponent<Button>().onClick.AddListener(delegate { SetSelectedOption(-1);  });

        dia_window.SetActive(false);

        RunDialogue();
    }

    public void RunDialogue()
    {
        StartCoroutine(run());
    }

    public void SetSelectedOption(int x)
    {
        selected_option = x;
    }

    public IEnumerator run()
    {
        dia_window.SetActive(true);

        int node_id = 0;

        while (node_id != -1)
        {
            Display_Node(dia.Nodes[node_id]);
            selected_option = -2;
            while (selected_option == -2)
            {
                yield return new WaitForSeconds(0.25f);
            }
            node_id = selected_option;
        }
        dia_window.SetActive(false);
    }

    private void Display_Node(DialogueNode node)
    {
        node_txt.GetComponent<Text>().text = node.Text;
        //npcName_txt.GetComponent<Text>().text = **;
        option_1.SetActive(false);
        option_2.SetActive(false);
        option_3.SetActive(false);
        option_4.SetActive(false);

        for (int i = 0; i < node.Options.Count && i <4; i++)
        {
            switch (i)
            {
                case 0:
                    set_option_button(option_1, node.Options[i]);
                    break;
                case 1:
                    set_option_button(option_2, node.Options[i]);
                    break;
                case 2:
                    set_option_button(option_3, node.Options[i]);
                    break;
                case 3:
                    set_option_button(option_4, node.Options[i]);
                    break;
            }
        }
    }

    private void set_option_button(GameObject button, DialogueOption opt)
    {
        button.SetActive(true);
        button.GetComponentInChildren<Text>().text = opt.Text;
        button.GetComponent<Button>().onClick.AddListener(delegate { SetSelectedOption(opt.DestinationNodeID); });
    }

    public static Dialogue Load_Dialogue(string path)
    {
        XmlSerializer serz = new XmlSerializer(typeof(Dialogue));
        StreamReader reader = new StreamReader(path);

        Dialogue dia = (Dialogue) serz.Deserialize(reader);
        return dia;
    }
}
