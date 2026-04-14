using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Requests;

[TdfStruct]
public struct CreatePackRequest
{
    [TdfMember("DCID")] 
    public uint mCardDbId;

    [TdfMember("PTYP")] 
    public PackType mPackType;
    
    [TdfMember("UID")] 
    public uint mUserId;
    
}