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
    public GameObject StockCard;

    private List<Stock> SpawnList = new List<Stock>();
    private int TotalPrice = 0;

    private void Start()
    {
        GH = GameObject.Find("ScriptHolder").GetComponent<GameHandler>();

        foreach (Stock Stock in GH.Stock)
        {
            GameObject NewCard = GenerateLocoCard(Stock);
            GameObject SpawnedCard = Instantiate(NewCard, ScrollView.transform);
            SpawnedCard.transform.Find("Base").GetComponent<Button>().onClick.AddListener(() => AddSelection(Stock));
        }
    }

    private GameObject GenerateLocoCard(Stock Stock)
    {
        GameObject NewCard = StockCard;
        Transform CardBase = NewCard.transform.Find("Base");
        CardBase.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = Stock.name;
        CardBase.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = "$" + Stock.price;
        CardBase.transform.Find("Icon").GetComponent<Image>().sprite = Stock.icon;
        return NewCard;
    }

    public void CloseMenu()
    {
        InteractionUI.SetActive(false);
    }

    public void AddSelection(Stock Stock)
    {
        TotalPrice += Stock.price;
        SpawnList.Add(Stock);
        print("Added stock");
    }

    public void SpawnStock()
    {
        StartCoroutine(FixedStockSpawn());
    }

    private IEnumerator FixedStockSpawn()
    {
        if (GH.Savegame.playerBalance >= TotalPrice)
        {
            print("Spawned");
            GH.Savegame.playerBalance -= TotalPrice;
            foreach (Stock Stock in SpawnList)
            {
                GameObject SpawnedStock = Instantiate(Stock.model, transform.position, transform.rotation, GH.TrainsParent.transform);
                while (SpawnedStock.GetComponent<TrainController>().NextSpline == null)
                {
                    SpawnedStock.transform.position = Vector3.MoveTowards(SpawnedStock.transform.position, SpawnedStock.transform.position + Vector3.forward, Time.deltaTime);
                    yield return null;
                }
            }
        }
        else
        {
            SpawnList.Clear();
        }
    }
}
