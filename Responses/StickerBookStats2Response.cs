using Tdf;
using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam.Responses;

[TdfStruct]
public struct StickerBookStats2Response
{
    [TdfMember("STAT")] 
    public List<StickerBookStatResult> mStats;

}