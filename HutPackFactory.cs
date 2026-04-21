using ZamboniUltimateTeam.Structs;

namespace ZamboniUltimateTeam;

public static class HutPackFactory
{
    public static async Task<List<CardData>> CreatePack(long userId, PackType packType)
    {
        var cardDataList = new List<CardData>();
        await HutManager.IncrementVersionInfo(userId, HutManager.VersionType.Unassigned);

        switch (packType)
        {
            case PackType.CARDHOUSE_CARD_PACK_TYPE_STARTER:
            {
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId, true, false));
                cardDataList.Add(await HutCardFactory.CreateRandomJerseyCard(userId, false, false));

                cardDataList.Add(await HutCardFactory.CreateRandomLogoCard(userId));

                cardDataList.Add(await HutCardFactory.CreateNonPlayerCard(userId, 6200000, CardSubType.CARDHOUSE_CARD_TYPE_CUSTOM_STADIUM));

                // cardDataList.Add(await HutCardFactory.CreateRandomTrainingCard(userId));

                // cardDataList.Add(await HutCardFactory.CreateRandomContractCard(userId));

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
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(0, 85), true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_C));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(0, 85), true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_LD));
                cardDataList.Add(await HutCardFactory.RollPlayerCard(userId, cardDataList, new Range(0, 85), true, CardSubType.CARDHOUSE_CARD_TYPE_PLAYER_GK));
                return cardDataList;
            }
            default: throw new NotImplementedException();
        }
    }
}