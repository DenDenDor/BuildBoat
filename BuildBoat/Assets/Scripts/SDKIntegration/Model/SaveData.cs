using System;

[Serializable]
public class SaveData
{
    [AutoGenerateSaveMethod] public float MusicValue = 0.5f;
    [AutoGenerateSaveMethod] public float SoundValue = 0.5f;
    [AutoGenerateSaveMethod] public int Coins = 0;
    [AutoGenerateSaveMethod] public int GoldAmount = 0;
    [AutoGenerateSaveMethod] public SavableItem[] Items = Array.Empty<SavableItem>();
}

[Serializable]
public class SavableItem
{
    public BlockType BlockType;
    public int Amount;
}