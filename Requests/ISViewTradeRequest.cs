using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct ISViewTradeRequest
{
    [TdfMember("REM")] 
    public uint mRemove;

    [TdfMember("TID")] 
    public long mTradeId;
    
    [TdfMember("UID")] 
    public ulong mUserId;
    
}