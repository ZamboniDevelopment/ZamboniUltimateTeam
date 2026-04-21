using Tdf;

namespace ZamboniUltimateTeam.Structs;

[TdfStruct]
public struct SquadInfo
{
    [TdfMember("CHEM")] 
    public uint mChemistry;
    
    [TdfMember("CHNG")] 
    public uint mCHNG;
    
    [TdfMember("FORM")] 
    public uint mFormationId;
    
    [TdfMember("JERA")] 
    public int mJERA;
    
    [TdfMember("JERH")] 
    public int mJERH;
    
    [TdfMember("LINE")] 
    public List<int> mLines;
    
    [TdfMember("LOGO")] 
    public int mLOGO;
    
    [TdfMember("MNGR")] 
    public CardData mManager;
    
    [TdfMember("NAME")] 
    public string mSquadName;

    [TdfMember("PLRS")] 
    public List<CardData> mPlayers;
    
    [TdfMember("RTNG")] 
    public uint mStarRating;
    
    [TdfMember("SQID")] 
    public uint mSquadId;
    
    [TdfMember("STAD")] 
    public int mSTAD;
    
    [TdfMember("TMAB")] 
    public string mTeamAbbreviation;
    
}