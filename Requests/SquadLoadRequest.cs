using Tdf;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct SquadLoadRequest
{
    
    [TdfMember("SQID")] 
    public long mSquadId;
    
    [TdfMember("UID")] 
    public long mUserId;
    
}