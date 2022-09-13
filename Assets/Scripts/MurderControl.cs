using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MurderControl : PageControl<MurderControl>
{
    [SerializeField]
    private Button basicMurderButton;
    [SerializeField]
    private GameObject murderFailureHintTextG;
    [SerializeField]
    private Button closeReviewButton;

    [SerializeField]
    private List<PeopleControl> peopleControlsList;
    public List<PeopleControl> Peoples
    {
        get
        {
            return peopleControlsList;
        }
    }
    [SerializeField]
    private VictimControl victimControl;
    public VictimControl Victim
    {
        get
        {
            return victimControl;
        }
    }
    public List<ActorControl> Actors
    {
        get
        {
            List<ActorControl> actors = new List<ActorControl>();
            actors.AddRange(peopleControlsList);
            actors.Add(victimControl);
            return actors;
        }
    }

    private Coroutine reviewMurderCoroutine;
    private Sequence reviewSequence;
    // Start is called before the first frame update
    void Start()
    {
        basicMurderButton.onClick.AddListener(() =>
        {
            bool isMurderSuccess = Murder(out RangeInt timeRange);
            murderFailureHintTextG.SetActive(!isMurderSuccess);
            if (isMurderSuccess)
            {
                this.Hide();
                ReasoningControl.Instance.Show(timeRange);
            }
        });

        closeReviewButton.onClick.AddListener(() =>
        {
            foreach (var item in Actors)
            {
                item.SetActive(false);
            }
            reviewSequence.Kill();
            StopCoroutine(reviewMurderCoroutine);
            this.Hide();
        });

        foreach (var item in Actors)
        {
            item.SetActive(false);
        }
    }
    public void Show(bool isReview)
    {
        base.Show();
        basicMurderButton.gameObject.SetActive(!isReview);
        closeReviewButton.gameObject.SetActive(isReview);
        if (isReview)
        {
            ReviewMurder();
        }
    }
    private bool Murder(out RangeInt timeRange)
    {
        timeRange = new RangeInt(-1, 0);
        foreach (var item in peopleControlsList)
        {
            item.Init();
        }
        PeopleControl killer = peopleControlsList[Random.Range(0, peopleControlsList.Count)];
        killer.IsKiller = true;
        victimControl.Init();
        bool isMurderSuccess = false;
        bool isKillRound = false;
        for (int i = 0; i < 24; i++)
        {
            if (!isMurderSuccess)
            {
                if (MapControl.Instance.GetBlockAreaColor(killer.transform.position) == MapControl.Instance.GetBlockAreaColor(victimControl.transform.position) && Random.Range(0f, 1f) > 0.5f)
                {
                    isKillRound = true;
                    killer.Kill(true);
                    victimControl.Die(true, i);
                    if (i < 6)
                    {
                        int startTime = Random.Range(0, i + 1);
                        timeRange = new RangeInt(startTime, 5);
                    }
                    else if (i > 17)
                    {
                        int endTime = Random.Range(i, 24);
                        timeRange = new RangeInt(i - (5 - (endTime - i)), 5);
                    }
                    else
                    {
                        int startTime = Random.Range(i - 5, i + 1);
                        timeRange = new RangeInt(startTime, 5);
                    }
                    isMurderSuccess = true;
                }
            }
            foreach (var item in peopleControlsList)
            {
                if (isKillRound && item == killer)
                {
                    isKillRound = false;
                    continue;
                }
                item.MoveNext();
            }
            if (!victimControl.IsDeath)
            {
                victimControl.MoveNext();
            }
        }
        return isMurderSuccess;
    }
    private IEnumerator ReviewMurderCoroutine()
    {
        foreach (var item in peopleControlsList)
        {
            item.Restore(false);
            item.SetActive(true);
            item.transform.position = item.GetPosRecord(-1);
        }
        victimControl.Restore(false);
        victimControl.SetActive(true);
        victimControl.transform.position = victimControl.GetPosRecord(-1);
        for (int i = 0; i < 24; i++)
        {
            reviewSequence = DOTween.Sequence();
            foreach (var item in peopleControlsList)
            {
                if (i == victimControl.DeathTime && item.IsKiller)
                {
                    item.Kill(false);
                    victimControl.Die(false);
                }
                reviewSequence.Join(item.transform.DOMove(item.GetPosRecord(i), 1f));
            }
            reviewSequence.Join(victimControl.transform.DOMove(victimControl.GetPosRecord(i), 1f));
            yield return new WaitForSeconds(2f);
        }
    }
    private void ReviewMurder()
    {
        reviewMurderCoroutine = StartCoroutine(ReviewMurderCoroutine());
    }
}
