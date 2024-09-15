using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    public GameObject PlayerIcon;
    public GameObject EnemyIcon;

    public List<GameObject> EnemyIconList;

    public void CreateIconOnMap()
    {
        GameObject Icon = Instantiate(EnemyIcon);
        Icon.transform.SetParent(transform);
        EnemyIconList.Add(Icon);
    }

    public void RemoveIconFromMap(int ID)
    {
        GameObject icon = EnemyIconList[ID];
        EnemyIconList[ID] = null;
        Destroy(icon);
    }

    public void ClearIconList()
    {
        EnemyIconList.Clear();
    }
}
