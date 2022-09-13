using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControl : Singleton<MapControl>
{
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;
    [SerializeField]
    private List<Transform> redBlockTs;
    [SerializeField]
    private List<Transform> yellowBlockTs;
    [SerializeField]
    private List<Transform> greenBlockTs;
    [SerializeField]
    private List<Transform> blueBlockTs;
    public enum AreaColor
    {
        None,
        Red,
        Yellow,
        Green,
        Blue
    }
    private Dictionary<Transform, AreaColor> blocksDic = new Dictionary<Transform, AreaColor>();
    protected override void Awake()
    {
        base.Awake();

        foreach (var item in redBlockTs)
        {
            blocksDic.Add(item, AreaColor.Red);
        }
        foreach (var item in yellowBlockTs)
        {
            blocksDic.Add(item, AreaColor.Yellow);
        }
        foreach (var item in greenBlockTs)
        {
            blocksDic.Add(item, AreaColor.Green);
        }
        foreach (var item in blueBlockTs)
        {
            blocksDic.Add(item, AreaColor.Blue);
        }
    }
    public List<Vector3> GetAllBlockPoses()
    {
        List<Vector3> poses = new List<Vector3>();
        foreach (var item in blocksDic)
        {
            poses.Add(item.Key.position);
        }
        return poses;
    }
    public List<Vector3> GetNextBlockPoses(Vector3 pos)
    {
        List<Vector3> poses = new List<Vector3>();
        Transform currentBlock = null;
        foreach (var item in blocksDic)
        {
            if (item.Key.position == pos)
            {
                currentBlock = item.Key;
                break;
            }
        }
        if (currentBlock != null)
        {
            float maxDistance = gridLayoutGroup.cellSize.x * (4f / 3f);
            foreach (var item in blocksDic)
            {
                if (item.Key != currentBlock && Vector3.Distance(item.Key.position, pos) < maxDistance)
                {
                    poses.Add(item.Key.position);
                }
            }
        }
        return poses;
    }
    public AreaColor GetBlockAreaColor(Vector3 pos)
    {
        foreach (var item in blocksDic)
        {
            if (item.Key.position == pos)
            {
                return item.Value;
            }
        }
        return AreaColor.None;
    }
}
