using Blaze3SDK;
using Blaze3SDK.Blaze.Example;
using BlazeCommon;
using ZamboniUltimateTeam.Requests;
using ZamboniUltimateTeam.Responses;
using ZamboniUltimateTeam.Structs;
using GetConfigResponse = ZamboniUltimateTeam.Responses.GetConfigResponse;

namespace ZamboniUltimateTeam;

public class CardHouseComponent : CardHouseComponentBase.Server
{
    public override async Task<LoginResponse> LoginRequestAsync(LoginRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        var gamerInfo = await HutManager.GetGamerInfo(userId);
        if (gamerInfo == null) return new LoginResponse();
        return new LoginResponse
        {
            mTeamAbbreviation = gamerInfo.Value.mTeamAbbreviation,
            mBonusAwarded = 0,
            mCVER = new List<int>
            {
                1, 2, 3
            },
            mDRRC = 1,
            mDRRL = 1,
            mDRRO = 1,
            mDRRW = 1,
            mTeamName = gamerInfo.Value.mTeamName,
            mRewardType = 0,
            mRewardValue = 0,
            mTRBS = 0,
            mUserId = 0,
        };
    }


    public override async Task<NumericResponse> LogoutRequestAsync(LogoutRequest request, BlazeRpcContext context)
    {
        return new NumericResponse();
    }

    public override async Task<MoveCardResponse> MoveCardAsync(MoveCardRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        if (request.mSwapCardId != 0) throw new NotImplementedException();

        var card = await HutManager.GetCard(request.mCardId, userId);

        CardData cardData = card.Card;
        DeckType from = card.DeckType;
        switch (request.mDeckType)
        {
            case DeckType.CARDHOUSE_DECK_ESCROW:
                await HutCardFactory.CreateOrUpdateCard(cardData, userId, DeckType.CARDHOUSE_DECK_ESCROW);
                await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Escrow);
                break;
            case DeckType.CARDHOUSE_DECK_UNASSIGNED:
                await HutCardFactory.CreateOrUpdateCard(cardData, userId, DeckType.CARDHOUSE_DECK_UNASSIGNED);
                await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Unassigned);
                break;
            case DeckType.CARDHOUSE_DECK_STICKERBOOK: break;
            default: throw new NotImplementedException();
        }

        switch (from)
        {
            case DeckType.CARDHOUSE_DECK_ESCROW:
                await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Escrow);
                break;
            case DeckType.CARDHOUSE_DECK_UNASSIGNED:
                await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Unassigned);
                break;
            case DeckType.CARDHOUSE_DECK_STICKERBOOK: break;
            case DeckType.CARDHOUSE_DECK_INBOX: break;
            default: throw new NotImplementedException();
        }

        var versionInfo = await HutManager.GetVersionInfo(userId);

        return new MoveCardResponse
        {
            mDisplacedCardId = request.mCardId,
            mDisplacedDeckType = request.mDeckType,
            mDisplacedCardPosition = 0,
            mVersionInfo = versionInfo.Value
        };
    }

    public override async Task<GamerGetInfoResponse> GetGamerInfoRequestAsync(GamerGetInfoRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var gamerInfo = await HutManager.GetGamerInfo(userId);
        if (gamerInfo == null) throw new BlazeRpcException(Blaze3RpcError.CARDHOUSE_ERR_NO_PLAYER_INFO);
        return new GamerGetInfoResponse
        {
            mGamerInfo = gamerInfo.Value,
            mUserId = 0 //TODO USE 0 FOR NOW FOR EVERYONE BECAUSE CLIENT SEEMS TO NOT "KNOW" ITS UID
        };
    }

    public override async Task<NumericResponse> SetGamerInfoRequestAsync(GamerSetInfoRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var gamerInfo = await HutManager.GetGamerInfo(userId);
        if (gamerInfo == null)
        {
            if (!await HutManager.IsTeamNameAvailable(request.mGamerInfo.mTeamName.Trim()))
            {
                throw new BlazeRpcException(Blaze3RpcError.CARDHOUSE_ERR_NAME_EXISTS);
            }

            await HutManager.InsertNameReservation(userId, UltimateTeam.Server.GetUserNameByUserId(userId), request.mGamerInfo.mTeamName, request.mGamerInfo.mTeamAbbreviation);
        }

        await HutManager.SetGamerInfo(request.mGamerInfo, userId);
        return new NumericResponse();
    }

    public override async Task<DeckInfoResponse> GetDeckInfoAsync(DeckInfoRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        var generalInfo = await HutManager.GetGeneralInfo(userId);
        if (generalInfo == null)
            generalInfo = await HutManager.SetGeneralInfo(new GeneralInfo
            {
                mCredits = 1000,
                mStats = new List<int>()
                {
                    //Todo figure if other placeholders have meaning
                    1, 2, 3, 4, 5, 6, 7, 8,
                    0, //WINS
                    0, //LOSES
                    0, //OTL
                    12, 13, 14, 15, 16, 17, 18, 19, 20,
                    21, 22, 23, 24, 25, 26, 27, 28, 29, 30
                }
            }, userId);

        var squadInfoList = await HutManager.GetSquadList(userId);
        uint teamRating = 0;
        if (squadInfoList.Count >= 1) teamRating = squadInfoList[0].mStarRating;

        var versionInfo = await HutManager.GetVersionInfo(userId);
        if (versionInfo == null)
        {
            versionInfo = await HutManager.CreateVersionInfo(new VersionInfo
            {
                mVersionEscrow = 1,
                mVersionGeneral = 1,
                mVersionUnassigned = 1
            }, userId);
        }


        var escrowList = await HutManager.GetCardList(userId, DeckType.CARDHOUSE_DECK_ESCROW, CardState.CARDHOUSE_CARDSTATE_FREE);
        var unassignedList = await HutManager.GetCardList(userId, DeckType.CARDHOUSE_DECK_UNASSIGNED, CardState.CARDHOUSE_CARDSTATE_FREE);

        return new DeckInfoResponse
        {
            mDuplicateEscrowCardIdPairList = await HutManager.FindDuplicates(userId, escrowList),
            mDuplicateUnassignedCardIdPairList = await HutManager.FindDuplicates(userId, unassignedList),
            mEscrowCardDataList = escrowList,
            mEscrowCount = (byte)escrowList.Count,
            mGeneralInfo = generalInfo.Value,
            mTeamRating = teamRating,
            mUnassignedCardDataList = unassignedList,
            mUserId = 0,
            mVersionInfo = versionInfo.Value
        };
    }

    public override async Task<GetConfigResponse> GetConfigRequestAsync(GetConfigRequest request, BlazeRpcContext context)
    {
        return new GetConfigResponse
        {
            mConfigList = new List<int>
            {
                //Todo figure if other placeholders have meaning
                11, 21, 22, 33, 44, 55, 66, 77, 88, 99, 10,
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                21, 22, 23, 24, 25, 26, 27,
                500, //Game End: Completion award
                29,
                50, //Game End Positive: Pucks per Goal
                31,
                2, //Game End Positive: Pucks per Shots on goal
                33,
                2, //Game End Positive: Pucks per Hits
                35,
                2, //Game End Positive: Pucks per Faceoffs Won
                37,
                5, //Game End Positive: Pucks per Time on attack MINUTES
                39,
                1, //Game End Positive: Pucks per Powerplay % something?
                41,
                1, //Game End Positive: Pucks per successful pass
                43,
                -50, //Game End Negative: Puck deduction per opponent goals
                45,
                -2, //Game End Negative: Puck deduction per penalty minute
                47,
                -2, //Game End Negative: Puck deduction per icing
                49,
                -2, //Game End Negative: Puck deduction per offside
                313,
                50, //Create Auction Bid increment
                129,
                200, //Default starting price for auction
                200, //Min starting price for auction
                100000, //Max starting price for auction
                250, //Min buyout price for auction
                100000, //Max buyout price for auction
                250, //Starting bid vs buyout differential first jump something idk
                0, //Default buyout price for auction
                603,
                604,
                605,
                606,
                607,
                608,
                609,
                200, //Search min bet minimum
                100000, //Search min bet maximum
                200, //Search max bet starting
                100000, //Search max bet maximum
                200, //Search min buyout starting
                100000, //Search min buyout maximum
                200, //Search max buyout starting
                100000, //Search max buyout maximum
                100000, //Bid bid Max amount
                50, //Bid bid increment
                616,
                100000, //Trade offer bid Max amount
                50, //Trade offer bid increment
                311, 362, 173,
                10, //Difficulty Rookie Multiplier
                11, //Difficulty Pro Multiplier
                12, //Difficulty All-Star Multiplier
                13, //Difficulty Super-Star Multiplier
                88, 89, 90,
                91, 92, 93, 94, 95, 96, 97, 98, 99, 100,
                101, 102, 103,
                100, //Max consumables in inventory
                105, 106, 107, 108, 109, 110,
                111, 112, 113, 114, 115, 116, 117, 118, 119, 120,
                121, 122, 123, 124, 125, 126, 127, 128, 129, 130,
                131, 132, 133, 134, 135, 136, 137, 138, 139, 140,
                141, 142, 143, 144, 145, 146, 147, 148, 149, 150,
                151, 152, 153, 154, 155, 156, 157, 158, 159, 160,
                161, 162, 163, 164, 165, 166, 167, 168, 169, 170,
                171, 172, 173, 174, 175, 176, 177, 178, 179, 180,
                181, 182, 183, 184, 185, 186, 187, 188, 189, 190,
                191, 192, 193, 194, 195, 196, 197, 198, 199, 200,
                201, 202, 203, 204, 205, 206, 207, 208, 209, 210,
                211, 212, 213, 214, 215, 216, 217, 218, 219, 220,
                221, 222, 223, 224, 225, 226, 227, 228, 229, 230,
                231, 232, 233, 234, 235, 236, 237, 238, 239, 240,
                241, 242, 243, 244, 245, 246, 247, 248, 249, 250,
                251, 252, 253, 254, 255, 256, 257, 258, 259, 260,
                261, 262, 263, 264, 265, 266, 267, 268, 269, 270,
                271, 272, 273, 274, 275, 276, 277, 278, 279, 280,
                281, 282, 283, 284, 285, 286, 287, 288, 289, 290,
                291, 292, 293, 294, 295, 296, 297, 298, 299, 300,
                301, 302, 303, 304, 305, 306, 307, 308, 309, 310,
                311, 312, 313, 314, 315, 316, 317, 318, 319, 320,
                321, 322, 323, 324, 325, 326, 327, 328, 329, 330,
                331, 332, 333, 334, 335, 336, 337, 338, 339, 340,
                341, 342, 343, 344, 345, 346, 347, 348, 349, 350,
                351, 352, 353, 354, 355, 356, 357, 358, 359, 360,
                361, 362, 363, 364, 365, 366, 367, 368, 369, 370,
            }
        };
    }

    public override async Task<StoreGetPackTypesResponse> StoreGetPackTypesAsync(StoreGetPackTypesRequest request, BlazeRpcContext context)
    {
        return new StoreGetPackTypesResponse
        {
            mFreePack = 0,
            mPremiumPacksHidden = 0,
            mPackTypeList = UltimateTeam.PackConfig.Packs.Select(pack => pack.StorePackTypeData).ToList(),
            mServerTime = (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
        };
    }

    public override async Task<StorePackQuantitiesResponse> StorePackQuantitiesAsync(StorePackQuantitiesRequest request, BlazeRpcContext context)
    {
        return new StorePackQuantitiesResponse
        {
            mPackQuantitiesList = new List<int>
            {
                10, 20
            }
        };
    }

    public override async Task<DiscardCardResponse> DiscardCardAsync(DiscardCardRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var cardData = await HutManager.GetCard(request.mCardId, userId);

        switch (cardData.DeckType)
        {
            case DeckType.CARDHOUSE_DECK_ESCROW: await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Escrow); break;
            case DeckType.CARDHOUSE_DECK_UNASSIGNED: await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Unassigned); break;
            case DeckType.CARDHOUSE_DECK_STICKERBOOK: break;
            default: throw new NotImplementedException();
        }

        await HutManager.HardDelete(userId, cardData.Card.mCardId);
        await HutHelper.Deposit(userId, request.mCredits);

        var versionInfo = await HutManager.GetVersionInfo(userId);

        return new DiscardCardResponse
        {
            mCredits = request.mCredits,
            mVersionInfo = versionInfo.Value
        };
    }

    public override async Task<StaffBonusResponse> GetStaffBonusAsync(NumericRequest request, BlazeRpcContext context)
    {
        return new StaffBonusResponse
        {
            mStaffBonusInfo = new StaffBonusInfo
            {
                mPhysioArmBonus = 0,
                mPhysioBackBonus = 0,
                mContractBonus = 0,
                mFitnessBonus = 0,
                mPhysioFootBonus = 0,
                mGKDivingBonus = 0,
                mGKHandlingBonus = 0,
                mGKKickingBonus = 0,
                mGKOneOnOneBonus = 0,
                mGKPositioningBonus = 0,
                mGKReflexesBonus = 0,
                mPhysioHeadBonus = 0,
                mPhysioHipBonus = 0,
                mPhysioLegBonus = 0,
                mDefendingBonus = 0,
                mDribblingBonus = 0,
                mHeadingBonus = 0,
                mPaceBonus = 0,
                mPassingBonus = 0,
                mShootingBonus = 0,
                mPhysioShoulderBonus = 0,
                mManagerTalkBonus = 0
            }
        };
    }

    public override async Task<AssignCardsResponse> AssignCardsAsync(AssignCardsRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        await Task.WhenAll(request.mList.Select(async assignCardCard =>
        {
            var cardData = (await HutManager.GetCard(assignCardCard.mCardId)).Card;
            cardData.mCardStateId = assignCardCard.mCardStateId;
            await HutCardFactory.CreateOrUpdateCard(cardData, userId, assignCardCard.mDeckType);
        }));

        await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Unassigned);
        var versionInfo = await HutManager.GetVersionInfo(userId);
        return new AssignCardsResponse
        {
            mVersionInfo = versionInfo.Value
        };
    }


    public override async Task<UserReliabilityInfoResponse> GetUserReliabilityInfoAsync(NumericRequest request, BlazeRpcContext context)
    {
        return new UserReliabilityInfoResponse
        {
            mPreviousMatchUnfinished = 0,
            mMatchesFinished = 10,
            mMatchesStarted = 10,
            mReliability = 100,
            mUserId = 0
        };
    }

    public override async Task<NumericResponse> ResetUserRequestAsync(NumericRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        if (!await HutManager.HardDelete(userId)) throw new BlazeRpcException(Blaze3RpcError.CARDHOUSE_ERR_DELETE_LAST_SQUAD);
        return new NumericResponse
        {
            mNumber = 0,
        };
    }

    public override async Task<SquadListResponse> GetSquadListAsync(NumericRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var squadInfo = await HutManager.GetSquadList(userId);
        if (squadInfo.Count <= 0) return new SquadListResponse();
        var squadSmalls = squadInfo.Select(squadInfoS => new SquadSmall
            {
                mChemistry = squadInfoS.mChemistry,
                mFormation = squadInfoS.mFormationId,
                mRating = squadInfoS.mStarRating,
                mSquadId = squadInfoS.mSquadId,
                mSquadName = squadInfoS.mSquadName
            })
            .ToList();
        return new SquadListResponse
        {
            mActiveSquad = squadInfo[0].mSquadId,
            mSquads = squadSmalls
        };
    }

    public override async Task<ViewCardsResponse> ViewCardsAsync(ViewCardsRequest request, BlazeRpcContext context)
    {
        var cardDataList = (await Task.WhenAll(
            request.mCardIdList.Select(cardId => HutManager.GetCard(cardId))
        )).Select(result => result.Card).ToList();

        return new ViewCardsResponse
        {
            mCardDataList = cardDataList
        };
    }

    public override async Task<SquadSaveResponse> SquadSaveAsync(SquadSaveRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        return new SquadSaveResponse
        {
            mSquadId = await HutManager.SaveSquadInfo(request, userId)
        };
    }

    public override async Task<SquadSaveResponse> SquadRenameAsync(SquadRenameRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var squad = await HutManager.GetSquad(userId, request.mSquadId, false);
        if (squad == null) throw new BlazeRpcException(Blaze3RpcError.CARDHOUSE_ERR_UNKNOWN);

        return new SquadSaveResponse
        {
            mSquadId = await HutManager.RenameSquad(request.mNewName, userId, request.mSquadId)
        };
    }
    
    public override async Task<NullStruct> SquadDeleteAsync(SquadDeleteRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        await HutManager.HardDeleteSquad(userId, request.mSquadId);
        return new NullStruct();
    }


    public static readonly CardSubType[] PlayerTypes =
    {
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_C,
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LW,
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RW,
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LD,
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RD,
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_GK
    };

    public static readonly CardSubType[] FieldPlayerTypes =
    {
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_C,
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LW,
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RW,
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LD,
        CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RD,
    };

    public static readonly CardSubType[] TrophyTypes =
    {
        CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_OFFLINE,
        CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_ONLINE,
        CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_LIVE,
        CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_PLAYOFF,
    };

    public static readonly CardSubType[] TrainingPlayerTypes =
    {
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_ATTRIBUTE_SKATING,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_ATTRIBUTE_SHOOTING,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_ATTRIBUTE_HANDS,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_ATTRIBUTE_CHECKING,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_ATTRIBUTE_DEFENSE,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_ALL,
    };

    public static readonly CardSubType[] TrainingGoalieTypes =
    {
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_GK_ATTRIBUTE_HIGH,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_GK_ATTRIBUTE_LOW,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_GK_ATTRIBUTE_QUICKNESS,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_GK_ATTRIBUTE_POSITIONING,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_GK_ATTRIBUTE_REBOUNDCONTROL,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_GK_ALL,
    };

    public static readonly CardSubType[] TrainingPositionTypes =
    {
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_POS_RW_C,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_POS_C_RW,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_POS_LW_C,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_POS_C_LW,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_POS_RW_LW,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_POS_LW_RW,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_POS_RD_LD,
        CardSubType.CARDHOUSE_CARD_TYPE_TRAINING_PLAYER_POS_LD_RD,
    };

    public static readonly CardSubType[] HealingTypes =
    {
        CardSubType.CARDHOUSE_CARD_TYPE_HEALING_HEALTH_TORSO,
        CardSubType.CARDHOUSE_CARD_TYPE_HEALING_HEALTH_ARMS,
        CardSubType.CARDHOUSE_CARD_TYPE_HEALING_HEALTH_LEGS,
        CardSubType.CARDHOUSE_CARD_TYPE_HEALING_HEALTH_ALL,
    };

    public static readonly CardSubType[] TeamInformationTypes =
    {
        CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_KIT,
        CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_BADGE,
    };

    public static readonly CardSubType[] TrainingTypes = TrainingPlayerTypes
        .Concat(TrainingGoalieTypes)
        .Concat(TrainingPositionTypes)
        .ToArray();

    public static readonly CardSubType[] ConsumablesTypes = TrainingTypes
        .Concat(HealingTypes)
        .Append(CardSubType.CARDHOUSE_CARD_TYPE_CONTRACT_PLAYER)
        .ToArray();


    private static int debugCounter = 0;

    public override async Task<StickerBookStats2Response> StickerBookStats2Async(StickerBookStats2Request request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        // if (request.mBASE >= 1) throw new NotImplementedException();
        List<StickerBookStatResult> stats = new();

        if (request.mContextId == RequestContext.CARDHOUSE_STICKERBOOK_STATS_REQUEST_CONTEXT_TOP)
        {
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_PLAYERS,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, PlayerTypes)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_STAFF_HEADCOACH,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_STAFF_HEADCOACH)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_STADIA,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_STADIUM)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_KITS,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_KIT)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_BADGES,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_BADGE)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_TROPHIES,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, TrophyTypes)
            });
        }

        if (request.mContextId == RequestContext.CARDHOUSE_STICKERBOOK_STATS_REQUEST_CONTEXT_YEAR)
        {
            foreach (var leagueId in await HutHelper.GetAllLeagueIds())
            {
                var correction = 0;
                if (leagueId is 0 or 1 or 2) correction = 2; //why? that's what I am wondering too
                if (leagueId == 3) correction = 1;
                var playerCounts = await HutManager.GetTeamCardCountsAsync(userId, leagueId, DeckType.CARDHOUSE_DECK_STICKERBOOK, PlayerTypes);

                stats.Add(new StickerBookStatResult
                {
                    mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                    mContextValue = leagueId,
                    mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_PLAYERS_BRONZE,
                    mValue = playerCounts.Values.Sum() + correction
                });

                var jerseyCounts = await HutManager.GetTeamCardCountsAsync(userId, leagueId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_KIT);

                stats.Add(new StickerBookStatResult
                {
                    mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                    mContextValue = leagueId,
                    mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_KITS,
                    mValue = jerseyCounts.Values.Sum()
                });

                var badgeCounts = await HutManager.GetTeamCardCountsAsync(userId, leagueId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_BADGE);

                stats.Add(new StickerBookStatResult
                {
                    mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                    mContextValue = leagueId,
                    mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_BADGES,
                    mValue = badgeCounts.Values.Sum()
                });
            }

            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 12,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_STADIA,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_STADIUM)
            });

            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 13,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_BALLS,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_STAFF_HEADCOACH)
            });

            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 14,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_TROPHIES_OFFLINE,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_OFFLINE)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 14,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_TROPHIES_ONLINE,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_ONLINE)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 14,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_TROPHIES_LIVE,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_LIVE)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 14,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_TROPHIES_PLAYOFF,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_PLAYOFF)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 14,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_CONSUMABLES,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, ConsumablesTypes)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 14,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_LEGENDS,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, true, PlayerTypes)
            });
        }

        if (request.mContextId == RequestContext.CARDHOUSE_STICKERBOOK_STATS_REQUEST_CONTEXT_LEAGUE)
        {
            int leagueId = request.mValue;
            var teamPlayerCounts = await HutManager.GetTeamCardCountsAsync(userId, leagueId, DeckType.CARDHOUSE_DECK_STICKERBOOK, PlayerTypes);
            foreach (var teamId in teamPlayerCounts.Keys)
            {
                stats.Add(new StickerBookStatResult
                {
                    mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_TEAM,
                    mContextValue = teamId,
                    mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_PLAYERS,
                    mValue = teamPlayerCounts[teamId]
                });
            }

            var teamJerseyCounts = await HutManager.GetTeamCardCountsAsync(userId, leagueId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_KIT);
            foreach (var teamId in teamJerseyCounts.Keys)
            {
                stats.Add(new StickerBookStatResult
                {
                    mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_TEAM,
                    mContextValue = teamId,
                    mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_KITS,
                    mValue = teamJerseyCounts[teamId]
                });
            }
        }

        if (request.mContextId == RequestContext.CARDHOUSE_STICKERBOOK_STATS_REQUEST_CONTEXT_NEW_CARDS_SCREEN)
        {
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_PLAYERS,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, PlayerTypes)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_KITS,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_KIT)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_KITS_HOME,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, 0, false, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_KIT)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_KITS_AWAY,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, 1, false, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_KIT)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_BADGES,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_BADGE)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_STADIA,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_STADIUM)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_STAFF_HEADCOACH,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_STAFF_HEADCOACH)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_CONSUMABLES,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, ConsumablesTypes)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_CONSUMABLES_CONTRACT,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, CardSubType.CARDHOUSE_CARD_TYPE_CONTRACT_PLAYER)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_CONSUMABLES_TRAINING,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, TrainingTypes)
            });
            stats.Add(new StickerBookStatResult
            {
                mContextId = ResultContext.CARDHOUSE_STICKERBOOK_STAT_RESULT_CONTEXT_YEAR,
                mContextValue = 2,
                mTypeId = ResultType.CARDHOUSE_STICKERBOOK_STAT_RESULT_TYPE_CONSUMABLES_HEALING,
                mValue = await HutManager.GetCardCountAsync(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, null, false, HealingTypes)
            });
        }

        return new StickerBookStats2Response { mStats = stats, mTotals = new List<StickerBookStatTotals>() };
    }


    public override async Task<StickerBookSearchResponse> StickerBookSearchAsync(StickerBookSearchRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        // if (request.mBASE >= 1) throw new NotImplementedException();
        List<CardData> cardDatas = await HutManager.GetCardList(userId, request);

        return new StickerBookSearchResponse
        {
            mSearchResults = cardDatas
        };
    }

    public override async Task<StickerBookCardResponse> StickerBookCardAsync(StickerBookCardRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var card = (await HutManager.GetCard(request.mCardId, userId));
        await HutCardFactory.CreateOrUpdateCard(card.Card, userId, DeckType.CARDHOUSE_DECK_STICKERBOOK);
        var generalInfo = await HutManager.GetGeneralInfo(userId);
        switch (card.DeckType)
        {
            case DeckType.CARDHOUSE_DECK_ESCROW:
            {
                await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Escrow);
                if (request.mSwapCardId != 0)
                {
                    var swapCard = (await HutManager.GetCard(request.mSwapCardId, userId));
                    await HutCardFactory.CreateOrUpdateCard(swapCard.Card, userId, DeckType.CARDHOUSE_DECK_ESCROW);
                }

                break;
            }
            case DeckType.CARDHOUSE_DECK_UNASSIGNED:
            {
                await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Unassigned);
                if (request.mSwapCardId != 0)
                {
                    var swapCard = (await HutManager.GetCard(request.mSwapCardId, userId));
                    await HutCardFactory.CreateOrUpdateCard(swapCard.Card, userId, DeckType.CARDHOUSE_DECK_UNASSIGNED);
                }

                break;
            }
            default: throw new NotImplementedException();
        }

        var versionInfo = await HutManager.GetVersionInfo(userId);

        return new StickerBookCardResponse
        {
            mTotalCredits = generalInfo.Value.mCredits,
            mVersionInfo = versionInfo.Value
        };
    }


    public override async Task<ISSearchResponse> ISSearchAsync(ISSearchRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        return await HutTradeManager.SearchTradesAsync(request, userId);
    }

    public override async Task<ISStartResponse> ISStartAsync(ISStartRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var tradeId = await HutTradeManager.InsertTrade(request, userId, UltimateTeam.Server.GetUserNameByUserId(userId));

        return new ISStartResponse
        {
            mTradeId = tradeId
        };
    }

    public override async Task<ISOfferTradeResponse> ISOfferTradeAsync(ISOfferTradeRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var result = await HutTradeManager.InsertOffer(request, userId);

        if (result.Exception != null)
        {
            throw new BlazeRpcException(result.Exception.ErrorCode);
        }

        return new ISOfferTradeResponse
        {
            mOfferId = result.OfferId
        };
    }

    public override async Task<ISWatchListResponse> ISWatchListAsync(ISWatchListRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var tradeInfos = await HutTradeManager.GetWatchedTrades(userId);
        return new ISWatchListResponse
        {
            mTradeResults = tradeInfos,
            mTotalCount = tradeInfos.Count
        };
    }

    public override async Task<ISWatchTradeResponse> ISWatchTradeAsync(ISWatchTradeRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        await HutTradeManager.InsertWatching(request.mTradeId, userId);
        return new ISWatchTradeResponse();
    }

    public override async Task<ISRemoveWatchResponse> ISRemoveWatchAsync(ISRemoveWatchRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        await HutTradeManager.RemoveWatching(request, userId);
        return new ISRemoveWatchResponse();
    }

    public override async Task<ISViewTradeResponse> ISViewTradeAsync(ISViewTradeRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var result = await HutTradeManager.ViewTradeAsync(request, userId);

        if (result.Exception != null)
        {
            throw new BlazeRpcException(result.Exception.ErrorCode);
        }

        return result.Response;
    }

    public override async Task<ISAdminOfferResponse> ISAdminOfferAsync(ISAdminOfferRequest request, BlazeRpcContext context)
    {
        var tradeId = await HutTradeManager.GetTradeId(request.mOfferId);
        BlazeRpcException? exception;
        switch (request.mOfferState)
        {
            case OfferState.CARDHOUSE_OFFERSTATE_ACCEPTED:
                exception = await HutTradeManager.AdminAcceptOffer(tradeId, request.mOfferId);
                break;
            case OfferState.CARDHOUSE_OFFERSTATE_REJECTED:
                exception = await HutTradeManager.AdminRejectOffer(request.mOfferId);
                break;
            default: throw new NotImplementedException();
        }

        if (exception != null) throw exception;
        return new ISAdminOfferResponse();
    }

    public override async Task<ISGetOffersResponse> ISGetOffersAsync(ISGetOffersRequest request, BlazeRpcContext context)
    {
        var offers = await HutTradeManager.SearchOffersAsync(request);
        return new ISGetOffersResponse
        {
            mOfferList = offers,
            mTotalCount = offers.Count,
        };
    }

    public override async Task<ActivateCardResponse> ActivateCardAsync(ActivateCardRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        var cardList = await HutManager.GetCardList(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, request.mActiveState);
        var previousActive = cardList[0];
        previousActive.mCardStateId = CardState.CARDHOUSE_CARDSTATE_FREE;

        await HutCardFactory.CreateOrUpdateCard(previousActive, userId);

        var target = await HutManager.GetCard(request.mCardId, userId);
        target.Card.mCardStateId = request.mActiveState;
        await HutCardFactory.CreateOrUpdateCard(target.Card, userId);

        return new ActivateCardResponse
        {
            mCardId = request.mCardId
        };
    }

    private static int debugCounter2 = 0;

    public override async Task<TournamentListResponse> TournamentListRequestAsync(NullStruct request, BlazeRpcContext context)
    {
        List<TournamentInfo> tournamentInfos = new List<TournamentInfo>();
        //For now, we just do singleplayer tournaments
        //Multiplayer tournaments require some more effort to implement...
        for (int i = 1; i <= 12; i++)
        {
            tournamentInfos.Add(new TournamentInfo
            {
                mAiGroup = 0,
                mBlazeTournamentId = 0,
                mDifficulty = (int)Math.Round((i - 1) * (5.0 / 11.0)),
                mElg1Type = ElgType.ELG_NONE,
                mElg1Data = 0,
                mElg2Type = ElgType.ELG_NONE,
                mElg12Data = 0,
                mEndTime = 0,
                mMatchLenght = 0,
                mPrize = 1000 * i,
                mReward1 = 1000,
                mReward2 = 1000,
                mReward3 = 1000,
                mReward4 = 1000,
                mSalaryCap = 2000,
                mStartTime = 0,
                mTrophyCardDbId = 8200000 + i - 1,
                mTournamentId = i,
                mType = 0,
                mTrophiesRequiredToEnter = i - 1
            });
        }

        return new TournamentListResponse
        {
            mServerTime = (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
            mTournaments = tournamentInfos
        };
    }


    public override async Task<TournamentSaveDataResponse> TournamentSaveDataRequestAsync(TournamentSaveDataRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        await HutTournamentManager.SaveTournament(request, userId);
        return new TournamentSaveDataResponse();
    }

    public override async Task<TournamentLoadDataResponse> TournamentLoadDataRequestAsync(TournamentLoadDataRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        byte[] tournamentData = await HutTournamentManager.LoadTournamentData(request, userId);
        if (tournamentData.Length <= 0) throw new BlazeRpcException(Blaze3RpcError.CARDHOUSE_ERR_NO_TOURNAMENT_DATA);

        return new TournamentLoadDataResponse
        {
            mData = tournamentData
        };
    }

    public override async Task<ApplyCardResponse> ApplyCardAsync(ApplyCardRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        if (request.mTargetCards.Count > 1) throw new NotImplementedException();

        var target = await HutManager.GetCard(request.mTargetCards[0]);
        var consumable = await HutManager.GetCard(request.mCardId, userId);

        var updated = target.Card;

        switch (consumable.Card.mCardSubTypeId)
        {
            case var subtype when TrainingPlayerTypes.Contains(subtype) || TrainingGoalieTypes.Contains(subtype):
            {
                var trainingCard = await UltimateDatabase.GetTrainingCardByDbIdAsync(consumable.Card.mCardDbId);
                updated.mListTrainingCards.Add(trainingCard.IndexedConsumableId);
                break;
            }
            case CardSubType.CARDHOUSE_CARD_TYPE_CONTRACT_PLAYER:
                var contractCard = await UltimateDatabase.GetContractCardByDbIdAsync(consumable.Card.mCardDbId);
                updated.mUsesRemaining += (byte)contractCard.Value;
                break;
            case var subtype when HealingTypes.Contains(subtype):
                var healingCard = await UltimateDatabase.GetHealingCardByDbIdAsync(consumable.Card.mCardDbId);
                updated.mInjuryGames = healingCard.Amount >= updated.mInjuryGames ? (byte)0 : (byte)(updated.mInjuryGames - healingCard.Amount);
                break;
            case var subtype when TrainingPositionTypes.Contains(subtype):
                var trainingCardMovePosition = await UltimateDatabase.GetTrainingCardByDbIdAsync(consumable.Card.mCardDbId);
                var destinationPositionId = trainingCardMovePosition.IndexedConsumableId;
                updated.mPreferredPositionId = (byte)destinationPositionId;
                updated.mFormationId = (byte)destinationPositionId;
                break;
            default: throw new NotImplementedException();
        }

        await HutManager.HardDelete(userId, consumable.Card.mCardId);
        await HutCardFactory.CreateOrUpdateCard(updated, userId);

        return new ApplyCardResponse
        {
            mCardId = request.mCardId,
            mCardDataList = new List<CardData>
            {
                updated
            },
            mUserId = 0
        };
    }

    public override async Task<ApplySalaryCapResponse> ApplySalaryCapAsync(ApplySalaryCapRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var target = await HutManager.GetCard(request.mPlayerCardId, userId);

        var updated = target.Card;
        updated.mSalaryCap = request.mSalaryCap;
        await HutCardFactory.CreateOrUpdateCard(updated, userId);

        return new ApplySalaryCapResponse
        {
            mPlayerCardId = request.mPlayerCardId,
            mSalaryCap = request.mSalaryCap,
            mUserId = 0
        };
    }


    public override async Task<MatchRegisterStartResponse> MatchRegisterStartAsync(MatchRegisterStartRequest request, BlazeRpcContext context)
    {
        return new MatchRegisterStartResponse();
    }

    public override async Task<NumericResponse> MatchRegisterFinishAsync(MatchRegisterFinishRequest request, BlazeRpcContext context)
    {
        return new NumericResponse();
    }

    public override async Task<ChangePlayersResponse> ChangePlayersAsync(ChangePlayersRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        var updateTasks = request.mCardDataList.Select(async loopVar => 
        {
            var cardResult = await HutManager.GetCard(loopVar.mCardId);
            CardData cardData = cardResult.Card;

            cardData.mUsesRemaining--;
            cardData.mInjuryGames = loopVar.mInjuryGames;
            cardData.mInjuryType = loopVar.mInjuryType;
            cardData.mListStats = loopVar.mListStats;

            await HutCardFactory.CreateOrUpdateCard(cardData, userId);
        });

        await Task.WhenAll(updateTasks);

        return new ChangePlayersResponse();
    }
    
    public static CardSubType ToCardSubType(TournamentType type)
    {
        return type switch
        {
            TournamentType.CARDHOUSE_TOURNAMENTTYPE_OFFLINE => CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_OFFLINE,
            TournamentType.CARDHOUSE_TOURNAMENTTYPE_ONLINE => CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_ONLINE,
            TournamentType.CARDHOUSE_TOURNAMENTTYPE_LIVE_OFFLINE => CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_LIVE,
            TournamentType.CARDHOUSE_TOURNAMENTTYPE_LIVE_ONLINE => CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_LIVE,
            TournamentType.CARDHOUSE_TOURNAMENTTYPE_PLAYOFF => CardSubType.CARDHOUSE_CARD_TYPE_UNLOCKS_TROPHY_PLAYOFF,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public override async Task<PlayGameResponse> PlayGameAsync(PlayGameRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        switch (request.mMatchResult)
        {
            case MatchResult.CARDHOUSE_MATCHRESULT_INVALID: break;
            case MatchResult.CARDHOUSE_MATCHRESULT_WON: await HutHelper.IncrementGeneralInfo(userId, HutHelper.Outcome.WIN); break;
            case MatchResult.CARDHOUSE_MATCHRESULT_LOST: await HutHelper.IncrementGeneralInfo(userId, HutHelper.Outcome.LOSS); break;
            case MatchResult.CARDHOUSE_MATCHRESULT_DRAW: await HutHelper.IncrementGeneralInfo(userId, HutHelper.Outcome.OTL); break;
            case MatchResult.CARDHOUSE_MATCHRESULT_WON_CUP: await HutHelper.IncrementGeneralInfo(userId, HutHelper.Outcome.WIN); break;
            case MatchResult.CARDHOUSE_MATCHRESULT_LOST_CUP: await HutHelper.IncrementGeneralInfo(userId, HutHelper.Outcome.LOSS); break;
            default: throw new NotImplementedException();
        }

        var created = false;
        if (request.mState == PlayGameState.CARDHOUSE_PGSTATE_ENDING)
        {
            await HutHelper.Deposit(userId, request.mCredits);
            if (request.mTournamentId >= 1 && request.mMatchResult == MatchResult.CARDHOUSE_MATCHRESULT_WON_CUP)
            {
                var card = await HutManager.GetCard(8200000 + request.mTournamentId, userId);
                if (card.Card.mCardId == 0)
                {
                    created = true;
                    await HutCardFactory.CreateNonPlayerCard(userId, (uint)(8200000 + request.mTournamentId - 1), ToCardSubType(request.mTournamentType));
                }
                else
                {
                    var updated = card.Card;
                    updated.mUsesRemaining = (byte)(card.Card.mUsesRemaining + 1);
                    await HutCardFactory.CreateOrUpdateCard(updated, userId, card.DeckType);
                }
            }
        }

        var generalInfo = await HutManager.GetGeneralInfo(userId);
        var versionInfo = await HutManager.GetVersionInfo(userId);

        return new PlayGameResponse
        {
            mBonusAwarded = 1,
            mCredits = generalInfo.Value.mCredits,
            mGoldenTickets = request.mGoldenTickets,
            mPrestige = request.mPrestige,
            mTrophyCardCreated = created ? (byte)1 : (byte)0,
            mVersionInfo = versionInfo.Value
        };
    }


    public override async Task<SquadLoadActiveResponse> SquadLoadActiveAsync(SquadLoadActiveRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var squadInfoList = await HutManager.GetSquadList(userId);
        if (squadInfoList.Count <= 0) throw new BlazeRpcException(Blaze3RpcError.CARDHOUSE_ERR_UNKNOWN);
        var activeSquad = squadInfoList[0];
        List<CardData> activeCards = new();
        activeCards.AddRange(await HutManager.GetCardList(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardState.CARDHOUSE_CARDSTATE_ACTIVE_BADGE));
        activeCards.AddRange(await HutManager.GetCardList(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardState.CARDHOUSE_CARDSTATE_ACTIVE_AWAY_KIT));
        activeCards.AddRange(await HutManager.GetCardList(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardState.CARDHOUSE_CARDSTATE_ACTIVE_HOME_KIT));
        activeCards.AddRange(await HutManager.GetCardList(userId, DeckType.CARDHOUSE_DECK_STICKERBOOK, CardState.CARDHOUSE_CARDSTATE_ACTIVE_STADIUM));

        return new SquadLoadActiveResponse
        {
            mActiveCards = activeCards,
            mSquadInfo = activeSquad,
            mTargetUserId = request.mTargetUserId,
        };
    }

    public override async Task<SquadSearchResponse> SquadSearchAsync(SquadSearchRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        return new SquadSearchResponse
        {
            mResultList = await HutManager.GetAllSquadsAsOfflineOpponents(userId)
        };
    }

    public override async Task<SquadLoadResponse> SquadLoadAsync(SquadLoadRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);
        var makeActive = true;
        if (request.mUserId != null && request.mUserId >= 1)
        {
            userId = request.mUserId;
            makeActive = false;
        }

        var squadPromise = await HutManager.GetSquad(userId, request.mSquadId, makeActive);
        if (squadPromise == null) throw new BlazeRpcException(Blaze3RpcError.CARDHOUSE_ERR_UNKNOWN);

        return new SquadLoadResponse
        {
            mChemistry = (int)squadPromise.Value.mChemistry,
            mCHNG = (int)squadPromise.Value.mCHNG,
            mFormation = (int)squadPromise.Value.mFormationId,
            mJerseyAwayDbId = squadPromise.Value.mJerseyAwayDbId,
            mJerseyHomeDbId = squadPromise.Value.mJerseyHomeDbId,
            mLines = squadPromise.Value.mLines,
            mLogoCardDbId = squadPromise.Value.mLogoCardDbId,
            mManager = squadPromise.Value.mManager,
            mTeamName = squadPromise.Value.mSquadName,
            mPlayers = squadPromise.Value.mPlayers,
            mStarRating = (int)squadPromise.Value.mStarRating,
            mSquadId = squadPromise.Value.mSquadId,
            mStadiumDbId = squadPromise.Value.mStadiumDbId,
            mTeamAbbreviation = squadPromise.Value.mTeamAbbreviation,
            mUserId = request.mUserId,
        };
    }

    public override async Task<CreatePackResponse> CreatePackAsync(CreatePackRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        var pack = UltimateTeam.PackConfig.Packs.FirstOrDefault(pack => pack.PackId.Equals((int)request.mPackType));
        if (pack == null) throw new BlazeRpcException(Blaze3RpcError.CARDHOUSE_ERR_NOT_IMPLEMENTED);

        if (!await HutHelper.Withdraw(userId, pack.StorePackTypeData.mCoinCost))
        {
            throw new BlazeRpcException(Blaze3RpcError.CARDHOUSE_ERR_NOT_ENOUGH_CREDITS);
        }

        var cards = await HutPackFactory.RollPackAsync(pack, userId);
        var duplicates = await HutManager.FindDuplicates(userId, cards);

        var versionInfo = await HutManager.GetVersionInfo(userId);

        return new CreatePackResponse
        {
            mCardDataList = cards,
            mDuplicateCardIdPairList = duplicates,
            mNumCards = cards.Count,
            mNumPackPurchased = 0,
            mRandPackType = 0,
            mVersionInfo = versionInfo.Value
        };
    }

    public override async Task<GetFriendHistoryResponse> GetFriendHistoryAsync(NumericRequest request, BlazeRpcContext context)
    {
        return new GetFriendHistoryResponse
        {
            mHistoryList = await HutManager.QueryTeamStats()
        };
    }

    public override async Task<GetFriendGameListResponse> GetFriendGameListAsync(GetFriendGameListRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        return new GetFriendGameListResponse
        {
            mGameList = await HutManager.QueryFriendGameList(userId, request)
        };
    }

    public override async Task<GetPersistenceInfoResponse> GetPersistenceInfoAsync(NumericRequest request, BlazeRpcContext context)
    {
        var userId = UltimateTeam.Server.GetUserIdByConnectionId(context.Connection.ID);

        return new GetPersistenceInfoResponse
        {
            mEST = 0,
            mVLUE = 10,
        };
    }
    
    public override async Task<NullStruct> StorePlayAFriendGameAsync(StorePlayAFriendGameRequest request, BlazeRpcContext context)
    {
        return new NullStruct();
    }
}