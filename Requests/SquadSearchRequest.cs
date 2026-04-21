using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct SquadSearchRequest
{
    [TdfMember("CLNT")] 
    public UltimateTeamClientType mClientType;

    [TdfMember("MAXN")] 
    public byte mMAXN;
    
    [TdfMember("NAME")] 
    public string mNAME;
    
    [TdfMember("TOR")] 
    public byte mTOR;
    
    [TdfMember("UID")] 
    public long mUID;
}