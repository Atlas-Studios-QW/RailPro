using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DepotBehaviour : MonoBehaviour
{
    [Header("Game Handler")]
    public GameHandler GH;

    [Header("Connect UI")]
    public GameObject InteractionUI;
    public GameObject ScrollView;

    [Header("Card Prefab")]
    public GameObject LocoCard;

    private void Start()
    {
        GH = GameObject.Find("ScriptHolder").GetComponent<GameHandler>();

        foreach (Locomotive Locomotive in GH.Locomotives)
        {
            GameObject NewCard = GenerateLocoCard(Locomotive);
            Instantiate(NewCard, ScrollView.transform);
        }
    }

    private GameObject GenerateLocoCard(Locomotive Locomotive)
    {
        GameObject NewCard = Instantiate(LocoCard);
        Transform CardBase = NewCard.transform.Find("Base");
        CardBase.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = Locomotive.name;
        CardBase.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "$" + Locomotive.price;
        CardBase.transform.Find("Icon").GetComponent<Image>().sprite = Locomotive.icon;
        return NewCard;
    }

    public void CloseMenu()
    {
        InteractionUI.SetActive(false);
    }
}
