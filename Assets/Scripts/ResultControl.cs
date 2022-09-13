using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultControl : PageControl<ResultControl>
{
    [SerializeField]
    private Dropdown timeDropdown;
    [SerializeField]
    private Dropdown actorDropdown;
    [SerializeField]
    private Dropdown areaColorDropdown;
    private List<GameObject> rightGsList = new List<GameObject>();
    private List<GameObject> errorGsList = new List<GameObject>();
    [SerializeField]
    private Button submitButton;
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Text resultText;
    [SerializeField]
    private Button viewAnswerButton;
    [SerializeField]
    private Button viewScoreButton;
    [SerializeField]
    private Button reviewMurderButton;
    [SerializeField]
    private Button restartButton;
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> dropdownTsList = new List<Transform>();
        dropdownTsList.Add(timeDropdown.transform);
        dropdownTsList.Add(actorDropdown.transform);
        dropdownTsList.Add(areaColorDropdown.transform);
        foreach (var item in dropdownTsList)
        {
            rightGsList.Add(item.Find("RightImage").gameObject);
            errorGsList.Add(item.Find("ErrorImage").gameObject);
        }

        submitButton.onClick.AddListener(() =>
        {
            submitButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            Submit();
            ReasoningControl.Instance.Hide();
        });

        backButton.onClick.AddListener(() =>
        {
            this.Hide();
        });

        viewAnswerButton.onClick.AddListener(() =>
        {
            viewAnswerButton.gameObject.SetActive(false);
            viewScoreButton.gameObject.SetActive(true);
            Answer();
        });

        viewScoreButton.onClick.AddListener(() =>
        {
            viewAnswerButton.gameObject.SetActive(true);
            viewScoreButton.gameObject.SetActive(false);
            Submit();
        });

        reviewMurderButton.onClick.AddListener(() =>
        {
            MurderControl.Instance.Show(true);
        });

        restartButton.onClick.AddListener(() =>
        {
            this.Hide();
            MurderControl.Instance.Show(false);
        });

        this.Hide();
    }
    public override void Show()
    {
        base.Show();
        Init();
    }
    private void Init()
    {
        foreach (var item in rightGsList)
        {
            item.SetActive(false);
        }
        foreach (var item in errorGsList)
        {
            item.SetActive(false);
        }

        timeDropdown.ClearOptions();
        List<string> timeOptions = new List<string>();
        for (int i = ReasoningControl.Instance.TimeRange.start; i <= ReasoningControl.Instance.TimeRange.end; i++)
        {
            timeOptions.Add($"{i.ToString("00")}:00");
        }
        timeDropdown.AddOptions(timeOptions);

        actorDropdown.ClearOptions();
        List<string> actorOptions = new List<string>();
        for (int i = 0; i < 4; i++)
        {
            actorOptions.Add(((ReasoningControl.AskRecord.Actor)i).ToString());
        }
        actorDropdown.AddOptions(actorOptions);

        areaColorDropdown.ClearOptions();
        List<string> areaColorOptions = new List<string>();
        for (int i = 1; i < 5; i++)
        {
            areaColorOptions.Add($"{((MapControl.AreaColor)i).ToString()} Area");
        }
        areaColorDropdown.AddOptions(areaColorOptions);

        submitButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);

        resultText.gameObject.SetActive(false);
        viewAnswerButton.gameObject.SetActive(true);
        viewScoreButton.gameObject.SetActive(false);
    }
    private void Submit()
    {
        List<bool> isRightsList = new List<bool>();
        int time = Convert.ToInt32(timeDropdown.captionText.text.Split(':')[0]);
        isRightsList.Add(time == MurderControl.Instance.Victim.DeathTime);
        foreach (var item in MurderControl.Instance.Peoples)
        {
            if (item.IsKiller)
            {
                isRightsList.Add(actorDropdown.captionText.text == item.name);
                break;
            }
        }
        MapControl.AreaColor areaColor = MapControl.AreaColor.None;
        if (areaColorDropdown.captionText.text == "Red Area")
        {
            areaColor = MapControl.AreaColor.Red;
        }
        else if (areaColorDropdown.captionText.text == "Yellow Area")
        {
            areaColor = MapControl.AreaColor.Yellow;
        }
        else if (areaColorDropdown.captionText.text == "Green Area")
        {
            areaColor = MapControl.AreaColor.Green;
        }
        else if (areaColorDropdown.captionText.text == "Blue Area")
        {
            areaColor = MapControl.AreaColor.Blue;
        }
        isRightsList.Add(areaColor == MapControl.Instance.GetBlockAreaColor(MurderControl.Instance.Victim.GetPosRecord(MurderControl.Instance.Victim.DeathTime)));
        int rightCount = 0;
        for (int i = 0; i < isRightsList.Count; i++)
        {
            if (isRightsList[i])
            {
                rightGsList[i].SetActive(true);
                rightCount++;
            }
            else
            {
                errorGsList[i].SetActive(true);
            }
        }
        resultText.text = $"Corrent reasoning: + {rightCount} * 10 = {rightCount * 10} points\n"
        + $"Rounds consumed: - {ReasoningControl.Instance.CurrentRound} * 2 = {ReasoningControl.Instance.CurrentRound * 2} points\n"
        + $"Final Score: {rightCount * 10 - ReasoningControl.Instance.CurrentRound * 2} points";
        resultText.gameObject.SetActive(true);
    }
    private void Answer()
    {
        resultText.text = $"Facts of the case:\n\n"
        + $"Time: {MurderControl.Instance.Victim.DeathTime.ToString("00")}:00\n";
        foreach (var item in MurderControl.Instance.Peoples)
        {
            if (item.IsKiller)
            {
                resultText.text += $"Characters: {item.name}\n";
                break;
            }
        }
        resultText.text += $"Location: {MapControl.Instance.GetBlockAreaColor(MurderControl.Instance.Victim.GetPosRecord(MurderControl.Instance.Victim.DeathTime)).ToString()} Area";
    }
}
