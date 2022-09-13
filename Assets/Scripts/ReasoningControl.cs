using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ReasoningControl : PageControl<ReasoningControl>
{
    [SerializeField]
    private Text hintText;
    private RangeInt timeRange;
    public RangeInt TimeRange
    {
        get
        {
            return timeRange;
        }
    }

    [SerializeField]
    private List<Button> peopleButtonsList;
    private int peopleIndex;
    [SerializeField]
    private Dropdown askTimeDropdown;
    [SerializeField]
    private Dropdown askActorDropdown;
    [SerializeField]
    private Dropdown askAreaColorDropdown;
    [SerializeField]
    private Button askButton;
    [SerializeField]
    private Text askText;
    [SerializeField]
    private Text answerText;
    public class AskRecord
    {
        public int AskTime;
        public enum Actor
        {
            A,
            B,
            C,
            D,
            Victim
        }
        public Actor AskActor;
        public MapControl.AreaColor AskAreaColor;
    }
    private List<AskRecord> askRecordsList = new List<AskRecord>();

    [SerializeField]
    private Text currentRoundText;
    private int currentRound;
    public int CurrentRound
    {
        get
        {
            return currentRound;
        }
    }
    [SerializeField]
    private Button nextRoundButton;
    [SerializeField]
    private Button completeButton;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in peopleButtonsList)
        {
            item.onClick.AddListener(() =>
            {
                foreach (var button in peopleButtonsList)
                {
                    if (button.interactable)
                    {
                        button.transform.localScale = Vector3.one * (button == item ? 1.5f : 1f);
                    }
                }
                peopleIndex = peopleButtonsList.IndexOf(item);
                askTimeDropdown.interactable = true;
                askActorDropdown.interactable = true;
                askAreaColorDropdown.interactable = true;
                askButton.interactable = true;
                UpdateAskText();
            });
        }

        askTimeDropdown.onValueChanged.AddListener(value =>
        {
            UpdateAskText();
        });

        askActorDropdown.onValueChanged.AddListener(value =>
        {
            UpdateAskText();
        });

        askAreaColorDropdown.onValueChanged.AddListener(value =>
        {
            UpdateAskText();
        });

        askButton.onClick.AddListener(() =>
        {
            Ask();
            InitWithRecords();
        });

        nextRoundButton.onClick.AddListener(() =>
        {
            Init();
        });

        completeButton.onClick.AddListener(() =>
        {
            ResultControl.Instance.Show();
        });

        this.Hide();
    }
    public void Show(RangeInt timeRange)
    {
        base.Show();
        hintText.text = string.Empty;
        hintText.DOText($"Court doctor puts the time of death between {timeRange.start.ToString("00")}:00 and {timeRange.end.ToString("00")}:00."
        + "As a detective, you reconstruct the scene, find the killer, uncover the truth.", 3f).SetEase(Ease.Linear);
        this.timeRange = timeRange;
        currentRound = 0;
        Init();
    }
    private void Init()
    {
        foreach (var item in peopleButtonsList)
        {
            item.interactable = true;
            item.transform.localScale = Vector3.one;
        }
        peopleIndex = -1;
        askRecordsList.Clear();
        InitWithRecords();
        askText.text = "Ask";
        answerText.text = string.Empty;
        currentRoundText.text = $"Current round: {++currentRound}";
    }
    private void InitWithRecords()
    {
        foreach (var item in peopleButtonsList)
        {
            if (peopleButtonsList.IndexOf(item) == peopleIndex)
            {
                item.interactable = false;
                break;
            }
        }

        askTimeDropdown.interactable = false;
        askTimeDropdown.ClearOptions();
        List<string> askTimeOptions = new List<string>();
        for (int i = timeRange.start; i <= timeRange.end; i++)
        {
            bool hasRecord = false;
            foreach (var item in askRecordsList)
            {
                if (i == item.AskTime)
                {
                    hasRecord = true;
                    break;
                }
            }
            if (!hasRecord)
            {
                askTimeOptions.Add($"{i.ToString("00")}:00");
            }
        }
        askTimeDropdown.AddOptions(askTimeOptions);

        askActorDropdown.interactable = false;
        askActorDropdown.ClearOptions();
        List<string> askActorOptions = new List<string>();
        for (int i = 0; i < 5; i++)
        {
            bool hasRecord = false;
            foreach (var item in askRecordsList)
            {
                if (i == (int)item.AskActor)
                {
                    hasRecord = true;
                    break;
                }
            }
            if (!hasRecord)
            {
                askActorOptions.Add(((AskRecord.Actor)i).ToString());
            }
        }
        askActorDropdown.AddOptions(askActorOptions);

        askAreaColorDropdown.interactable = false;
        askAreaColorDropdown.ClearOptions();
        List<string> askAreaColorOptions = new List<string>();
        for (int i = 1; i < 5; i++)
        {
            bool hasRecord = false;
            foreach (var item in askRecordsList)
            {
                if (i == (int)item.AskAreaColor)
                {
                    hasRecord = true;
                    break;
                }
            }
            if (!hasRecord)
            {
                askAreaColorOptions.Add($"{((MapControl.AreaColor)i).ToString()} Area");
            }
        }
        askAreaColorDropdown.AddOptions(askAreaColorOptions);

        askButton.interactable = false;
    }
    private void UpdateAskText()
    {
        askText.text = $"Ask {((AskRecord.Actor)peopleIndex).ToString()}: "
        + $"Was {askActorDropdown.captionText.text} in the {askAreaColorDropdown.captionText.text} at {askTimeDropdown.captionText.text}?";
    }
    private void Ask()
    {
        int askTime = Convert.ToInt32(askTimeDropdown.captionText.text.Split(':')[0]);
        AskRecord.Actor askActor = AskRecord.Actor.A;
        if (askActorDropdown.captionText.text == "B")
        {
            askActor = AskRecord.Actor.B;
        }
        else if (askActorDropdown.captionText.text == "C")
        {
            askActor = AskRecord.Actor.C;
        }
        else if (askActorDropdown.captionText.text == "D")
        {
            askActor = AskRecord.Actor.D;
        }
        else if (askActorDropdown.captionText.text == "Victim")
        {
            askActor = AskRecord.Actor.Victim;
        }
        MapControl.AreaColor askAreaColor = MapControl.AreaColor.None;
        if (askAreaColorDropdown.captionText.text == "Red Area")
        {
            askAreaColor = MapControl.AreaColor.Red;
        }
        else if (askAreaColorDropdown.captionText.text == "Yellow Area")
        {
            askAreaColor = MapControl.AreaColor.Yellow;
        }
        else if (askAreaColorDropdown.captionText.text == "Green Area")
        {
            askAreaColor = MapControl.AreaColor.Green;
        }
        else if (askAreaColorDropdown.captionText.text == "Blue Area")
        {
            askAreaColor = MapControl.AreaColor.Blue;
        }
        askRecordsList.Add(new AskRecord() { AskTime = askTime, AskActor = askActor, AskAreaColor = askAreaColor });
        ActorControl people = MurderControl.Instance.Actors[peopleIndex];
        ActorControl actor = MurderControl.Instance.Actors[(int)askActor];
        MapControl.AreaColor actorRealAreaColor = MapControl.Instance.GetBlockAreaColor(actor.GetPosRecord(askTime));
        if (MapControl.Instance.GetBlockAreaColor(people.GetPosRecord(askTime)) == actorRealAreaColor)
        {
            if (askAreaColor == actorRealAreaColor)
            {
                answerText.text = "Answer: Yes";
            }
            else
            {
                answerText.text = "Answer: No";
            }
        }
        else
        {
            answerText.text = "Answer: Don't know";
        }
    }
}
