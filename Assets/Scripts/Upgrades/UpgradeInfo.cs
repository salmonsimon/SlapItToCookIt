using UnityEngine;

[System.Serializable]
public struct UpgradeInfo
{
    [SerializeField] private int coinPrice;
    public int CoinPrice { get { return coinPrice; } set { coinPrice = value; } }

    [SerializeField] private int rubyPrice;
    public int RubyPrice { get { return rubyPrice; } set { rubyPrice = value; } }

    [SerializeField] private float waitingTimeInSeconds;
    public float WaitingTimeInSeconds { get { return waitingTimeInSeconds; } set { waitingTimeInSeconds = value; } }

    [SerializeField] private int fastForwardRubyPrice;
    public int FastForwardRubyPrice { get { return fastForwardRubyPrice; } set { fastForwardRubyPrice = value; } }
}
