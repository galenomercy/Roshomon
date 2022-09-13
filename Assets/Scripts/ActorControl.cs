using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ActorControl : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    protected Image headImage;
    protected List<Vector3> posRecordsList = new List<Vector3>();
    protected virtual void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        headImage = this.transform.Find("HeadImage").GetComponent<Image>();
    }
    public void Init()
    {
        Restore(true);
        SetActive(false);
        List<Vector3> allBlockPoses = MapControl.Instance.GetAllBlockPoses();
        this.transform.position = allBlockPoses[Random.Range(0, allBlockPoses.Count)];
        posRecordsList.Add(this.transform.position);
    }
    public void MoveNext()
    {
        List<Vector3> nextBlockPoses = MapControl.Instance.GetNextBlockPoses(this.transform.position);
        this.transform.position = nextBlockPoses[Random.Range(0, nextBlockPoses.Count)];
        posRecordsList.Add(this.transform.position);
    }
    public Vector3 GetPosRecord(int time)
    {
        if (time + 1 > posRecordsList.Count - 1)
        {
            return posRecordsList[posRecordsList.Count - 1];
        }
        else
        {
            return posRecordsList[time + 1];
        }
    }
    public virtual void Restore(bool withData)
    {
        if (withData)
        {
            posRecordsList.Clear();
        }
        headImage.color = Color.white;
    }
    public void SetActive(bool isActive)
    {
        canvasGroup.alpha = isActive ? 1f : 0f;
    }
}
