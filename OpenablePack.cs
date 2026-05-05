using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam;

public class OpenablePack
{
    public static readonly List<OpenablePack> OpenablePacks = new();

    public PackType packType;
    public StorePackTypeData storePackTypeData;
    
    static OpenablePack()
    {
        OpenablePacks.Add(new OpenablePack
        {
            storePackTypeData = new StorePackTypeData(),
            packType = PackType.CARDHOUSE_CARD_PACK_TYPE_STARTER
        });
        
        OpenablePacks.Add(new OpenablePack
        {
            storePackTypeData = new StorePackTypeData
            {
                mAttributes = StorePackAttribute.CARDHOUSE_STOREPACKATTRIBUTES_MAX,
                mAvailability = StorePackAvailability.CARDHOUSE_STOREPACKAVAILABILITY_COINS,
                mCoinCost = 350,
                mEndDate = 0,
                mId = StorePackId.CARDHOUSE_CARD_PACK_TYPE_PEEWEE,
                mQuantity = 0,
                mSaleType = StoreSaleType.CARDHOUSE_STORESALETYPE_PRICE,
                mStartDate = 0,
                mState = StorePackState.CARDHOUSE_STOREPACKSTATE_ACTIVE
            },
            packType = PackType.CARDHOUSE_CARD_PACK_TYPE_PEEWEE
        });

        OpenablePacks.Add(new OpenablePack
        {
            storePackTypeData = new StorePackTypeData
            {
                mAttributes = StorePackAttribute.CARDHOUSE_STOREPACKATTRIBUTES_SAVINGS_COINS,
                mAvailability = StorePackAvailability.CARDHOUSE_STOREPACKAVAILABILITY_COINS,
                mCoinCost = 5000,
                mEndDate = 0,
                mId = StorePackId.CARDHOUSE_CARD_PACK_TYPE_PLAYERS_PREMIUM_MEGA_DEAL,
                mQuantity = 0,
                mSaleType = StoreSaleType.CARDHOUSE_STORESALETYPE_PRICE,
                mStartDate = 0,
                mState = StorePackState.CARDHOUSE_STOREPACKSTATE_ACTIVE
            },
            packType = PackType.CARDHOUSE_CARD_PACK_TYPE_PLAYERS_PREMIUM_MEGA_DEAL
        });
        
        OpenablePacks.Add(new OpenablePack
        {
            storePackTypeData = new StorePackTypeData
            {
                mAttributes = StorePackAttribute.CARDHOUSE_STOREPACKATTRIBUTES_SAVINGS_COINS,
                mAvailability = StorePackAvailability.CARDHOUSE_STOREPACKAVAILABILITY_COINS,
                mCoinCost = 1500,
                mEndDate = 0,
                mId = StorePackId.CARDHOUSE_CARD_PACK_TYPE_PLAYERS_PREMIUM,
                mQuantity = 0,
                mSaleType = StoreSaleType.CARDHOUSE_STORESALETYPE_PRICE,
                mStartDate = 0,
                mState = StorePackState.CARDHOUSE_STOREPACKSTATE_ACTIVE
            },
            packType = PackType.CARDHOUSE_CARD_PACK_TYPE_PLAYERS_PREMIUM
        });
        
        OpenablePacks.Add(new OpenablePack
        {
            storePackTypeData = new StorePackTypeData
            {
                mAttributes = StorePackAttribute.CARDHOUSE_STOREPACKATTRIBUTES_SAVINGS_COINS,
                mAvailability = StorePackAvailability.CARDHOUSE_STOREPACKAVAILABILITY_COINS,
                mCoinCost = 150,
                mEndDate = 0,
                mId = StorePackId.CARDHOUSE_CARD_PACK_TYPE_TEAM_ITEMS_PREMIUM,
                mQuantity = 0,
                mSaleType = StoreSaleType.CARDHOUSE_STORESALETYPE_PRICE,
                mStartDate = 0,
                mState = StorePackState.CARDHOUSE_STOREPACKSTATE_ACTIVE
            },
            packType = PackType.CARDHOUSE_CARD_PACK_TYPE_TEAM_ITEMS_PREMIUM
        });
        
        OpenablePacks.Add(new OpenablePack
        {
            storePackTypeData = new StorePackTypeData
            {
                mAttributes = StorePackAttribute.CARDHOUSE_STOREPACKATTRIBUTES_SAVINGS_COINS,
                mAvailability = StorePackAvailability.CARDHOUSE_STOREPACKAVAILABILITY_COINS,
                mCoinCost = 500,
                mEndDate = 0,
                mId = StorePackId.CARDHOUSE_CARD_PACK_TYPE_TEAM_ITEMS_PREMIUM_MEGA_DEAL,
                mQuantity = 0,
                mSaleType = StoreSaleType.CARDHOUSE_STORESALETYPE_PRICE,
                mStartDate = 0,
                mState = StorePackState.CARDHOUSE_STOREPACKSTATE_ACTIVE
            },
            packType = PackType.CARDHOUSE_CARD_PACK_TYPE_TEAM_ITEMS_PREMIUM_MEGA_DEAL
        });
        
        OpenablePacks.Add(new OpenablePack
        {
            storePackTypeData = new StorePackTypeData
            {
                mAttributes = StorePackAttribute.CARDHOUSE_STOREPACKATTRIBUTES_SAVINGS_COINS,
                mAvailability = StorePackAvailability.CARDHOUSE_STOREPACKAVAILABILITY_COINS,
                mCoinCost = 250,
                mEndDate = 0,
                mId = StorePackId.CARDHOUSE_CARD_PACK_TYPE_CONSUMABLE_STANDARD,
                mQuantity = 0,
                mSaleType = StoreSaleType.CARDHOUSE_STORESALETYPE_PRICE,
                mStartDate = 0,
                mState = StorePackState.CARDHOUSE_STOREPACKSTATE_ACTIVE
            },
            packType = PackType.CARDHOUSE_CARD_PACK_TYPE_CONSUMABLE_STANDARD
        });
        
        OpenablePacks.Add(new OpenablePack
        {
            storePackTypeData = new StorePackTypeData
            {
                mAttributes = StorePackAttribute.CARDHOUSE_STOREPACKATTRIBUTES_SAVINGS_COINS,
                mAvailability = StorePackAvailability.CARDHOUSE_STOREPACKAVAILABILITY_COINS,
                mCoinCost = 1500,
                mEndDate = 0,
                mId = StorePackId.CARDHOUSE_CARD_PACK_TYPE_CONSUMABLE_STANDARD_MEGA_DEAL,
                mQuantity = 0,
                mSaleType = StoreSaleType.CARDHOUSE_STORESALETYPE_PRICE,
                mStartDate = 0,
                mState = StorePackState.CARDHOUSE_STOREPACKSTATE_ACTIVE
            },
            packType = PackType.CARDHOUSE_CARD_PACK_TYPE_CONSUMABLE_STANDARD_MEGA_DEAL
        });
    }

    public async Task<List<CardData>> GiveCards(long userId)
    {
        await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Unassigned);
        var cardDataList = new List<CardData>();

        switch (packType)
        {
            case PackType.CARDHOUSE_CARD_PACK_TYPE_STARTER:
            {
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId, true, false));
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId, false, false));

                cardDataList.Add(await HutCardFactory.CreateRandomLogoCard(userId));

                cardDataList.Add(await HutCardFactory.CreateNonPlayerCard(userId, 6200000, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_STADIUM));

                cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));

                cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));

                var starterOverallRange = new Range(0, 85);

                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_GK));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_GK));

                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LD));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LD));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LD));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RD));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RD));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RD));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RD));

                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LW));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LW));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LW));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LW));

                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 82), true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_C));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(80, 84), true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_C));

                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RW));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RW));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RW));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_RW));

                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_C));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_C));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, starterOverallRange, true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_C));

                return cardDataList;
            }
            case PackType.CARDHOUSE_CARD_PACK_TYPE_PEEWEE:
            {
                cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(0, 80), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(0, 80), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(0, 80), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(0, 80), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(0, 88), false, CardHouseComponent.PlayerTypes));
                if (HutCardFactory.Random.Next(100) < 5) //5% Chance
                {
                    cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(85, 99), true, CardHouseComponent.PlayerTypes));
                }

                return cardDataList;
            }
            case PackType.CARDHOUSE_CARD_PACK_TYPE_PLAYERS_PREMIUM:
            {
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 88), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 88), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 99), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 99), true, CardHouseComponent.PlayerTypes));
                return cardDataList;
            }
            case PackType.CARDHOUSE_CARD_PACK_TYPE_PLAYERS_PREMIUM_MEGA_DEAL:
            {
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 85), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 86), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 86), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(78, 86), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(80, 99), false, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(85, 99), true, CardHouseComponent.PlayerTypes));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(80, 99), true, CardHouseComponent.PlayerTypes));
                return cardDataList;
            }
            case PackType.CARDHOUSE_CARD_PACK_TYPE_TEAM_ITEMS_PREMIUM:
            {
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomLogoCard(userId));
                if (HutCardFactory.Random.Next(100) < 25) //25% Chance
                {
                    cardDataList.Add(await HutCardFactory.CreateRandomStadiumCard(userId));
                }

                return cardDataList;
            }

            case PackType.CARDHOUSE_CARD_PACK_TYPE_TEAM_ITEMS_PREMIUM_MEGA_DEAL:
            {
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId, null, true));
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId, null, true));

                cardDataList.Add(await HutCardFactory.CreateRandomLogoCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomLogoCard(userId));
                cardDataList.Add(await HutCardFactory.CreateNonPlayerCard(userId, (uint)HutCardFactory.Random.Next(6200000, 6200006), CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_STADIUM));

                if (HutCardFactory.Random.Next(100) < 25) //25% Chance
                {
                    cardDataList.Add(await HutCardFactory.CreateRandomHeadCoachCard(userId));
                }

                if (HutCardFactory.Random.Next(100) < 5) //5% Chance
                {
                    cardDataList.Add(await HutCardFactory.CreateNonPlayerCard(userId, 6200006, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_STADIUM));
                }

                return cardDataList;
            }
            case PackType.CARDHOUSE_CARD_PACK_TYPE_CONSUMABLE_STANDARD:
            {
                cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomHealingCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));
                if (HutCardFactory.Random.Next(100) < 25) //25% Chance
                {
                    cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomHealingCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));
                }

                return cardDataList;
            }

            case PackType.CARDHOUSE_CARD_PACK_TYPE_CONSUMABLE_STANDARD_MEGA_DEAL:
            {
                cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomHealingCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomHealingCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomHealingCard(userId));
                cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));
                if (HutCardFactory.Random.Next(100) < 50) //50% Chance
                {
                    cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomHealingCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomHealingCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomHealingCard(userId));
                    cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));
                }

                return cardDataList;
            }
            default: throw new NotImplementedException();
        }
    }
}