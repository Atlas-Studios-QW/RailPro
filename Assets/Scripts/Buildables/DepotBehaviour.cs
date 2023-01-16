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

    public GameObject LocomotivesView;
    public GameObject TraincarsView;

    [Header("Card Prefab")]
    public GameObject StockCard;

    private List<Stock> SpawnList = new List<Stock>();
    private int TotalPrice = 0;

    private void Start()
    {
        GH = GameObject.Find("ScriptHolder").GetComponent<GameHandler>();

        //Generate cars and add them to scrollview
        foreach (Stock Stock in GH.Locomotives)
        {
            GameObject NewCard = GenerateStockCard(Stock, true);
            GameObject SpawnedCard = Instantiate(NewCard, LocomotivesView.transform);
            SpawnedCard.transform.Find("Base").GetComponent<Button>().onClick.AddListener(() => AddSelection(Stock));
        }

        foreach (Stock Stock in GH.Traincars)
        {
            GameObject NewCard = GenerateStockCard(Stock, false);
            GameObject SpawnedCard = Instantiate(NewCard, TraincarsView.transform);
            SpawnedCard.transform.Find("Base").GetComponent<Button>().onClick.AddListener(() => AddSelection(Stock));
        }
    }

    //Generate a card that can be palced in the scroll view. when bool is true, makes for locomotive, if false, makes for traincar
    private GameObject GenerateStockCard(Stock Stock, bool IsLocomotive)
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

    //Start spawning stock
    public void SpawnStock()
    {
        StartCoroutine(FixedStockSpawn());
    }

    private IEnumerator FixedStockSpawn()
    {
        //Check balance and then spawn each bought item. Then sends it off
        if (GH.Savegame.playerBalance >= TotalPrice)
        {
            GameObject PreviousStock = null;

            print("Spawned");
            GH.Savegame.playerBalance -= TotalPrice;
            foreach (Stock Stock in SpawnList)
            {
                GameObject SpawnedStock = Instantiate(Stock.model, transform.position, transform.rotation, GH.TrainsParent.transform);
                SpawnedStock.GetComponent<TrainController>().StockInfo = Stock;
                if (PreviousStock != null) {
                    PreviousStock.GetComponent<TrainController>().StockInfo = Stock;
                }
                SpawnedStock.GetComponent<TrainController>().ConnectedStock.Add(PreviousStock);
                while (SpawnedStock.GetComponent<TrainController>().NextSpline == null)
                {
                    SpawnedStock.transform.position = Vector3.MoveTowards(SpawnedStock.transform.position, SpawnedStock.transform.position + SpawnedStock.transform.forward, Time.deltaTime);
                    yield return null;
                }
                PreviousStock = SpawnedStock;
            }
            SpawnList.Clear();
        }
        else
        {
            SpawnList.Clear();
            print("Not enough money to buy stock");
        }
    }

    //Shows selected scrollview, if true, it shows the traincars, if false, it shows the locomotives
    public void ShowScrollView(bool CarView)
    {
        LocomotivesView.SetActive(!CarView);
        TraincarsView.SetActive(CarView);
    }
}