using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct SquadListResponse
{
    [TdfMember("ACTV")] 
    public uint mActiveSquad;
    
    [TdfMember("SQDS")] 
    public List<SquadSmall> mSquads;

}