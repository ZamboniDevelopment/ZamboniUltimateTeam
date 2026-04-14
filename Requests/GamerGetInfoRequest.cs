using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct GamerGetInfoRequest
{
    [TdfMember("TUID")] 
    public ulong mTargetUserId;

    [TdfMember("UID")] 
    public ulong mUserId;
    
}