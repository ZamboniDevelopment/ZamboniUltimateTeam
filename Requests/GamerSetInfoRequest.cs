using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct GamerSetInfoRequest
{
    [TdfMember("INFO")] 
    public GamerInfo mGamerInfo;

    [TdfMember("UID")] 
    public long mUserId;
    
}