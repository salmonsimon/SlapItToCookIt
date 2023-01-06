using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/UpgradeTable")]
public class UpgradeTable : ScriptableObject
{
    [SerializeField] private List<UpgradeInfo> upgradeInfo = new List<UpgradeInfo>();
    public List<UpgradeInfo> UpgradeInfo { get { return upgradeInfo; } }
}