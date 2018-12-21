// Copyright Small & Fast 2018

namespace DestinyArmourSelector
{
    public interface IArmourPieceFactory
    {
        ArmourPiece CreateArmourPiece(int rowNumber, string[] tokens);
    }
}
