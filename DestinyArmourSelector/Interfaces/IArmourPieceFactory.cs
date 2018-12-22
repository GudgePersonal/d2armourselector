// Copyright Small & Fast 2018

namespace DestinyArmourSelector.Interfaces
{
    public interface IArmourPieceFactory
    {
        ArmourPiece CreateArmourPiece(int rowNumber, string[] tokens);
    }
}
