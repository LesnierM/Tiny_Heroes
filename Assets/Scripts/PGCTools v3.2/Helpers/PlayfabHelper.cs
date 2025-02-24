using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using PGCTools.MethodExtensions;
#if Playfab
#if ENABLE_PLAYFABSERVER_API
using PlayFab.ServerModels;
#endif
using PGCTools.Helpers.Playfab.DataStructs;
using PlayFab.EconomyModels;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFab.MultiplayerModels;
using PlayFab.ProfilesModels;
using Unity.VisualScripting;
using System.Linq;

namespace PGCTools.Helpers.Playfab
{
    public static class PlayfabHelper
    {
        #region Cloud Scripts Function Names
        static class CloudScriptsFuntionNames
        {
            public static string UpdateUserStatisticsCloudFuntion = "updateUserStatistics";
            public static string UpdateUserInternalDataCloudFuntion = "updateUserInternalData";
        }
        #endregion

        public static class CloudScriptsDataTemplates
        {
            public static CloudScriptParameterData[] UpdateUserStatisticsCloudTemplate = new CloudScriptParameterData[] {
            new CloudScriptParameterData { Key = "statisticName"},
            new CloudScriptParameterData { Key = "value"}
        };
            public static CloudScriptParameterData[] UpdateUserInternalDataCloudTemplate = new CloudScriptParameterData[] {
            new CloudScriptParameterData { Key = "deletedKeys"},
        };
            public static CloudScriptParameterData[] CommonKeyValueCloudTemplate = new CloudScriptParameterData[] {
            new CloudScriptParameterData { Key = "dataName"},
            new CloudScriptParameterData { Key = "dataValue"},
        };
        }

        /// <summary>
        /// Set to true when a request has finished.
        /// </summary>
        static bool _gotResponse;

        #region Request results
        public static LoginResult LogInResult;
        public static UpdateUserTitleDisplayNameResult DisplayNameUpdateResult;
        public static PlayFab.ClientModels.GetUserInventoryResult UserInventoryResult;
        public static PlayFab.ClientModels.ExecuteCloudScriptResult CloudScriptResult;
        public static GetMatchmakingTicketResult GetMatchmakingTicketResult;
        public static GetMatchResult GetMatchResult;
        public static RequestMultiplayerServerResponse RequestMultiplayerServerResult;
        public static SearchItemsResponse SearchItemsResult;
        public static PlayFab.ClientModels.GetCatalogItemsResult CatalogueItemsResult;
        public static PlayFab.ClientModels.UpdateUserDataResult UpdateUserDataResult;
        public static PlayFab.ProfilesModels.GetEntityProfilesResponse GetEntityProfilesResponse;

        static PlayFab.ClientModels.GetPlayerProfileResult _playerProfileResult;
        static GetLeaderboardAroundPlayerResult _leaderboardAroundPlayerResult;
        static GetLeaderboardResult _leaderboardResult;
        static PlayFab.ClientModels.UpdatePlayerStatisticsResult _updatePlayerStaticsResult;
        static PlayFabError _requestError;
        static CreateMatchmakingTicketResult _createMatchmakingTicketResult;
        static CancelAllMatchmakingTicketsForPlayerResult _cancellAllTicketsForPlayerResult;
        static CancelMatchmakingTicketResult _cancelMatchmakingTicktResult;
        static PurchaseInventoryItemsResponse _purchaseInvetoryItemsResult;
        static GetInventoryItemsResponse _getInventoryItemsResponse;
        static AddInventoryItemsResponse _addInventoryItemsResponse;
        static PlayFab.ClientModels.GetUserDataResult _getUserDataResult;

#if ENABLE_PLAYFABSERVER_API
        static PlayFab.ServerModels.GetPlayerProfileResult PlayerProfileResultServer;
        static PlayFab.ServerModels.GetUserDataResult _getUserDataResultFromServer;
        static GrantItemsToUserResult _grantItemsToUserResultServer;
        static PlayFab.ServerModels.ModifyUserVirtualCurrencyResult _modifyUserVirtualCurrencyResultServer;
        public static PlayFab.ServerModels.GetTitleDataResult GetTitleDataResult;
        public static PlayFab.ServerModels.GetTimeResult CurrentServerTimeResult;
        public static PlayFab.ServerModels.GetUserDataResult GetUserDataResultServer;
        public static PlayFab.ServerModels.GetUserInventoryResult GetuserInventoryResultServer;
#endif
        #endregion

        #region Matchmaking Api Calls
        public static IEnumerator RequestMultiplayerServer(List<string> regions, List<MatchmakingPlayerWithTeamAssignment> members, string sessionId, string buildId)
        {
            RequestMultiplayerServerResult = null;
            resetData();
            RequestMultiplayerServerRequest _request = new RequestMultiplayerServerRequest
            {
                BuildId = buildId,
                SessionId = sessionId,
                PreferredRegions = regions,
                InitialPlayers = ConvertMembersListIntoStringList(members)
            };
            PlayFabMultiplayerAPI.RequestMultiplayerServer(_request, _data => { RequestMultiplayerServerResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets a mtachmaking ticket.This methods can not be called in less than 6 seconds after the last call.
        /// </summary>
        /// <param name="ticketId">The ticket id.</param>
        /// <param name="queueName">The name of the queue.</param>
        public static IEnumerator GetMatchmakingTicket(string ticketId, string queueName)
        {
            GetMatchmakingTicketResult = null;
            resetData();
            GetMatchmakingTicketRequest _request = new GetMatchmakingTicketRequest { TicketId = ticketId, QueueName = queueName };
            PlayFabMultiplayerAPI.GetMatchmakingTicket(_request, _data => { GetMatchmakingTicketResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Cancels any lost or inactive ticket the player may have.
        /// </summary>
        /// <returns></returns>
        public static IEnumerator CancellAllPlayerTickets(PlayFab.MultiplayerModels.EntityKey entityKey, string queueName)
        {
            _cancellAllTicketsForPlayerResult = null;
            resetData();
            CancelAllMatchmakingTicketsForPlayerRequest _request = new CancelAllMatchmakingTicketsForPlayerRequest
            {
                QueueName = queueName,
                Entity = entityKey
            };
            PlayFabMultiplayerAPI.CancelAllMatchmakingTicketsForPlayer(_request, _data => { _cancellAllTicketsForPlayerResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Creates a multiplayer ticket for matchmaking.
        /// </summary>
        /// <param name="playerEntityId">Player entity id.</param>
        /// <param name="playerEntityType">Player entity type.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="queueName">The name of the matchmaking queue.</param>
        public static IEnumerator CreateMatchMakingTicket(string playerEntityId, string playerEntityType, MatchmakingPlayerAttributes attributes, string queueName)
        {
            _createMatchmakingTicketResult = null;
            resetData();
            CreateMatchmakingTicketRequest _request = new CreateMatchmakingTicketRequest
            {
                Creator = new MatchmakingPlayer
                {
                    Entity = new PlayFab.MultiplayerModels.EntityKey
                    {
                        Id = playerEntityId,
                        Type = playerEntityType
                    },
                    Attributes = attributes
                },
                QueueName = queueName,
                GiveUpAfterSeconds = 3599
            };
            PlayFabMultiplayerAPI.CreateMatchmakingTicket(_request, _data => { _createMatchmakingTicketResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Cancels a ticket if is in matchmaking.
        /// </summary>
        /// <param name="ticketId">The id of the ticket.</param>
        /// <param name="queueName">The name of the queue.</param>
        public static IEnumerator CancelMatchMaking(string ticketId, string queueName)
        {
            _createMatchmakingTicketResult = null;
            resetData();
            CancelMatchmakingTicketRequest _request = new CancelMatchmakingTicketRequest
            {
                QueueName = queueName,
                TicketId = ticketId
            };
            PlayFabMultiplayerAPI.CancelMatchmakingTicket(_request, _data => { _cancelMatchmakingTicktResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets the matchinformation.
        /// </summary>
        /// <param name="matchId">The match id.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns></returns>
        public static IEnumerator GetMatch(string matchId, string queueName)
        {
            GetMatchResult = null;
            resetData();
            GetMatchRequest _request = new GetMatchRequest
            {
                QueueName = queueName,
                MatchId = matchId
            };
            PlayFabMultiplayerAPI.GetMatch(_request, _data => { GetMatchResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        #endregion

        #region Economyv1 Api Calls
        public static IEnumerator GetPlayerInventory()
        {
            UserInventoryResult = null;
            resetData();
            PlayFab.ClientModels.GetUserInventoryRequest _request = new PlayFab.ClientModels.GetUserInventoryRequest { };
            PlayFabClientAPI.GetUserInventory(_request, _data => { UserInventoryResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        public static IEnumerator GetCatalogueItems(string catalogue = null)
        {
            CatalogueItemsResult = null;
            resetData();
            PlayFab.ClientModels.GetCatalogItemsRequest _request = new PlayFab.ClientModels.GetCatalogItemsRequest()
            {
                CatalogVersion = catalogue
            };
            PlayFabClientAPI.GetCatalogItems(_request, _data => { CatalogueItemsResult = _data; RequestFinished(); },
                _error => { _requestError = _error; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        #endregion

        #region Economyv2 Api Calls
        /// <summary>
        /// Pendant
        /// </summary>
        /// <returns></returns>
        public static IEnumerator AddInventoryItem()
        {
            _addInventoryItemsResponse = null;
            resetData();
            AddInventoryItemsRequest _request = new AddInventoryItemsRequest()
            {

            };
            PlayFabEconomyAPI.AddInventoryItems(_request, data => { _addInventoryItemsResponse = data; RequestFinished(); },
                error => { _requestError = error; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Get player inventory items.
        /// </summary>
        /// <param name="itemAmount">The amount of items to return.TODO implement continuos ids</param>
        /// <returns></returns>
        public static IEnumerator GetInventoryItems(int itemAmount)
        {
            _getInventoryItemsResponse = null;
            resetData();
            GetInventoryItemsRequest _request = new GetInventoryItemsRequest()
            {
                Count = itemAmount,
            };
            PlayFabEconomyAPI.GetInventoryItems(_request, data => { _getInventoryItemsResponse = data; RequestFinished(); },
                error => { _requestError = error; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Purchases an item.
        /// </summary>
        /// <param name="itemId">Id of the item to purchase.</param>
        /// <param name="amount">how many items to purchase.</param>
        /// <param name="data">The pricea mount list needed by this item.</param>
        /// <param name="currencyId">The currency used for purchasing the item.</param>
        /// <param name="price">The price of the item.</param>
        /// <param name="collectionIdToLookForPrices">The collection id where requiered items are located.Default is default.</param>
        /// <param name="customTags">Custom tags to add to operation.Default null.</param>
        /// <returns></returns>
        public static IEnumerator PurchaseItem(string itemId, int amount, DataStructs.CloudScriptParameterData[] data, string collectionIdToLookForPrices = "default", string detinationStackid = "default", Dictionary<string, string> customTags = null)
        {
            _purchaseInvetoryItemsResult = null;
            resetData();
            PurchaseInventoryItemsRequest _request = new PurchaseInventoryItemsRequest()
            {
                Amount = amount,
                DeleteEmptyStacks = true,
                Item = new InventoryItemReference()
                {
                    Id = itemId,
                    StackId = detinationStackid,
                },
                PriceAmounts = ConvertCustomDataToPurchasePriceAmountList(data),
                CollectionId = collectionIdToLookForPrices,
                CustomTags = customTags
            };
            PlayFabEconomyAPI.PurchaseInventoryItems(_request, data => { _purchaseInvetoryItemsResult = data; RequestFinished(); },
                error => { _requestError = error; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Searches for items.
        /// </summary>
        /// <param name="resultAmout">The amount of items to return in the call.Default is 10.</param>
        /// <param name="searchText">An optional text to filter search.Default is empty.</param>
        /// <param name="filterQuery">A query to filter the results.</param>
        /// <returns></returns>
        public static IEnumerator SearchItems(int resultAmout = 10, string searchText = default, string filterQuery = "")
        {
            SearchItemsResult = null;
            resetData();
            SearchItemsRequest _request = new SearchItemsRequest()
            {
                Count = resultAmout,
                Search = searchText,
                Filter = filterQuery
            };
            PlayFabEconomyAPI.SearchItems(_request, data => { SearchItemsResult = data; RequestFinished(); },
                error => { _requestError = error; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        #endregion

#if ENABLE_PLAYFABSERVER_API
        #region Server Api Calls
        /// <summary>
        /// Gets a player data.
        /// </summary>
        /// <param name="plyfabId">The player to get the data from.</param>
        /// <param name="keys">The keys to get.</param>
        public static IEnumerator GetPlayerDataFromServer(string plyfabId, List<string> keys = null)
        {
            _getUserDataResult = null;
            resetData();

            PlayFab.ServerModels.GetUserDataRequest _request = new PlayFab.ServerModels.GetUserDataRequest
            {
                Keys = keys,
                PlayFabId = plyfabId
            };

            PlayFabServerAPI.GetUserData(_request, _data => { _getUserDataResultFromServer = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });

            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Retrieve the player read only data.
        /// </summary>
        /// <returns></returns>
        /// <param name="playfabId">The id of the player.</param>
        /// <param name="keys">The keys to retrieve.Leave null to get them all.</param>
        public static IEnumerator GetPlayerReadOnlyDataFromServer(string playfabId, List<string> keys = null)
        {
            GetUserDataResultServer = null;

            resetData();

            PlayFab.ServerModels.GetUserDataRequest _request = new PlayFab.ServerModels.GetUserDataRequest
            {
                PlayFabId = playfabId,
                Keys = keys
            };

            PlayFabServerAPI.GetUserReadOnlyData(_request, _data => { GetUserDataResultServer = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });

            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Grants currency to a player.
        /// </summary>
        /// <param name="playfabId">The player id.</param>
        /// <param name="currency">The currency code.</param>
        /// <param name="amount">The amount to grant.</param>
        /// <returns></returns>
        public static IEnumerator GrantCurrencyToPlayerServer(string playfabId, string currency, int amount)
        {
            _modifyUserVirtualCurrencyResultServer = null;

            resetData();

            PlayFab.ServerModels.AddUserVirtualCurrencyRequest _request = new PlayFab.ServerModels.AddUserVirtualCurrencyRequest
            {
                PlayFabId = playfabId,
                VirtualCurrency = currency,
                Amount = amount
            };

            PlayFabServerAPI.AddUserVirtualCurrency(_request, _data => { _modifyUserVirtualCurrencyResultServer = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });

            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Grants an item to player from server
        /// </summary>
        /// <param name="itemIds">The list of item to grant.</param>
        /// <param name="playfabId">The id of the player.</param>
        /// <param name="catalogue">The catalogue from where the items are granted.</param>
        /// <returns></returns>
        public static IEnumerator GrantItemToPlayerServer(List<string> itemIds, string playfabId, string catalogue)
        {
            _grantItemsToUserResultServer = null;

            resetData();

            GrantItemsToUserRequest _request = new GrantItemsToUserRequest
            {
                ItemIds = itemIds,
                PlayFabId = playfabId,
                CatalogVersion = catalogue

            };

            PlayFabServerAPI.GrantItemsToUser(_request, _data => { _grantItemsToUserResultServer = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });

            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Updates player statistic.
        /// </summary>
        /// <param name="leaderboard">The statistic to update.</param>
        /// <param name="currentScore">The value of the statistic.</param>
        /// <param name="playfabid">The player playfabid to update statistic.</param>
        public static IEnumerator UpdatePlayerStaticsFromServer(string leaderboard, int currentScore, string playfabid)
        {
            resetData();
            PlayFab.ServerModels.UpdatePlayerStatisticsRequest _request = new PlayFab.ServerModels.UpdatePlayerStatisticsRequest
            {
                PlayFabId = playfabid,
                Statistics = new List<PlayFab.ServerModels.StatisticUpdate>()
                {
                    new PlayFab.ServerModels.StatisticUpdate(){StatisticName=leaderboard,Value=currentScore}
                }
            };
            PlayFabServerAPI.UpdatePlayerStatistics(_request, _data => { RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets the server current time.
        /// </summary>
        /// <returns></returns>
        public static IEnumerator GetServerTime()
        {
            CurrentServerTimeResult = null;
            resetData();
            PlayFab.ServerModels.GetTimeRequest _request = new PlayFab.ServerModels.GetTimeRequest();
            PlayFabServerAPI.GetTime(_request, _data => { CurrentServerTimeResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets title data.
        /// </summary>
        /// <param name="keys">The data to get.leave null to get them all</param>
        public static IEnumerator GetTitleDataFromServer(List<string> keys)
        {
            GetTitleDataResult = null;
            resetData();
            PlayFab.ServerModels.GetTitleDataRequest _request = new PlayFab.ServerModels.GetTitleDataRequest
            {
                Keys = keys
            };
            PlayFabServerAPI.GetTitleData(_request, _data => { GetTitleDataResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets title internal data.
        /// </summary>
        /// <param name="keys">The data to get.leave null to get them all</param>
        public static IEnumerator GetTitleInternaDataFromServer(List<string> keys)
        {
            GetTitleDataResult = null;
            resetData();
            PlayFab.ServerModels.GetTitleDataRequest _request = new PlayFab.ServerModels.GetTitleDataRequest
            {
                Keys = keys
            };
            PlayFabServerAPI.GetTitleInternalData(_request, _data => { GetTitleDataResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Retrieves all user inventory items.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public static IEnumerator GetUserInventoryServer(string userId)
        {
            GetuserInventoryResultServer = null;

            resetData();

            PlayFab.ServerModels.GetUserInventoryRequest _request = new PlayFab.ServerModels.GetUserInventoryRequest
            {
                PlayFabId = userId
            };

            PlayFabServerAPI.GetUserInventory(_request, _data => { GetuserInventoryResultServer = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });

            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets teh data of the provided playfab id.
        /// </summary>
        /// <param name="playerPlayfabId"></param>
        /// <returns></returns>
        public static IEnumerator GetPlayerProfileOfServer(string playerPlayfabId)
        {
            PlayerProfileResult = null;
            resetData();
            PlayFab.ServerModels.GetPlayerProfileRequest _request = new PlayFab.ServerModels.GetPlayerProfileRequest { PlayFabId = playerPlayfabId };
            PlayFabServerAPI.GetPlayerProfile(_request, _data => { PlayerProfileResultServer = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        #endregion
#endif

        #region Api Calls
        public static IEnumerator GetEntityProfiles(List<PlayFab.ProfilesModels.EntityKey> entityes)
        {
            GetEntityProfilesResponse = null;
            resetData();

            PlayFab.ProfilesModels.GetEntityProfilesRequest _request = new PlayFab.ProfilesModels.GetEntityProfilesRequest
            {
                Entities = entityes
            };

            PlayFabProfilesAPI.GetProfiles(_request, _data => { GetEntityProfilesResponse = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });

            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets the a player data.
        /// </summary>
        /// <param name="plyfabId">The player to get the data from,leave null to get current logged player.</param>
        /// <param name="keys">The keys to get.</param>
        public static IEnumerator GetPlayerData(List<string> keys = null)
        {
            _getUserDataResult = null;
            resetData();

            PlayFab.ClientModels.GetUserDataRequest _request = new PlayFab.ClientModels.GetUserDataRequest
            {
                Keys = keys
            };

            PlayFabClientAPI.GetUserData(_request, _data => { _getUserDataResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });

            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Updates/create the user data.
        /// </summary>
        /// <param name="data">The data to update</param>
        /// <returns></returns>
        public static IEnumerator UpdatePlayerData(Dictionary<string, string> data)
        {
            UpdateUserDataResult = null;
            resetData();

            PlayFab.ClientModels.UpdateUserDataRequest _request = new PlayFab.ClientModels.UpdateUserDataRequest
            {
                Data = data
            };

            PlayFabClientAPI.UpdateUserData(_request, _data => { UpdateUserDataResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Retrieves the player internal data.
        /// </summary>
        /// <param name="keys">The data to retrieve.Leave null to get all of them.</param>
        /// <param name="plyfabId">The id to get the data from.Levae null to get the current logged one.</param>
        public static IEnumerator GetPlayerReadonlyData(string plyfabId = null, List<string> keys = null)
        {
            _getUserDataResult = null;
            resetData();

            PlayFab.ClientModels.GetUserDataRequest _request = new PlayFab.ClientModels.GetUserDataRequest
            {
                Keys = keys,
                PlayFabId = plyfabId
            };

            PlayFabClientAPI.GetUserReadOnlyData(_request, _data => { _getUserDataResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Updates player internal data.
        /// </summary>
        /// <param name="data">The data must have the next format.
        /// The first object must be a string containing all the data fields to delete if needed otherwise
        /// leave it null.
        /// The rest of objects are a key value pair whre key is the name opf the data to update create and value its value.
        /// </param>
        /// <returns></returns>
        public static IEnumerator UpdatePlayerInternalData(CloudScriptParameterValues[] data)
        {
            yield return ExecuteCloudScript(CloudScriptsFuntionNames.UpdateUserInternalDataCloudFuntion, ConvertParameters(data, CloudScriptsDataTemplates.UpdateUserInternalDataCloudTemplate, CloudScriptsDataTemplates.CommonKeyValueCloudTemplate));
        }
        /// <summary>
        /// Updates a player statistic via cloud script.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public static IEnumerator UpdatePlayerStatisticByScript(CloudScriptParameterValues[] parameters)
        {
            yield return ExecuteCloudScript(CloudScriptsFuntionNames.UpdateUserStatisticsCloudFuntion, ConvertParameters(parameters, CloudScriptsDataTemplates.UpdateUserStatisticsCloudTemplate));
        }
        /// <summary>
        /// Calls a cloud funtion.
        /// </summary>
        /// <param name="functionName">The anme of the cloud funtion.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="revisionSelection">The revision selection.Default=Live</param>
        /// <param name="specificRevision">Specific revision.Default=null.</param>
        internal static IEnumerator ExecuteCloudScript(string functionName, JsonObject[] parameters, PlayFab.ClientModels.CloudScriptRevisionOption revisionSelection = PlayFab.ClientModels.CloudScriptRevisionOption.Live, int? specificRevision = null)
        {
            CloudScriptResult = null;
            resetData();
            ExecuteCloudScriptRequest _request = new ExecuteCloudScriptRequest
            {
                FunctionName = functionName,
                FunctionParameter = parameters,
                RevisionSelection = revisionSelection,
                SpecificRevision = specificRevision,
            };
            PlayFabClientAPI.ExecuteCloudScript(_request, _data => { CloudScriptResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Login with custom id and wait until it finish.
        /// </summary>
        /// <param name="id">The custom id</param>
        /// <param name="loginResult">If request is succes this will be the result.</param>
        /// <param name="error">If request fails this is the resulkt.</param>
        /// <param name="createUser">if create a new user in case doesnt exists an accout with the id.</param>
        public static IEnumerator LogInWithCustom(string id, bool createUser, bool getPlayerProfile = false, bool getPlayerStatistics = false)
        {
            LogInResult = null;
            resetData();
            PlayFab.ClientModels.LoginWithCustomIDRequest _request = new LoginWithCustomIDRequest
            {
                InfoRequestParameters = new PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = getPlayerProfile,
                    GetPlayerStatistics = getPlayerStatistics
                },
                CreateAccount = createUser,
                CustomId = id
            };
            PlayFabClientAPI.LoginWithCustomID(_request, _data => { LogInResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        public static IEnumerator LogInWithAndroidDeviceId(bool createUser, bool getPlayerProfile = false, bool getPlayerStatistics = false)
        {
            LogInResult = null;
            resetData();
            LoginWithAndroidDeviceIDRequest _request = new LoginWithAndroidDeviceIDRequest
            {
                InfoRequestParameters = new PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = getPlayerProfile,
                    GetPlayerStatistics = getPlayerStatistics
                },
                CreateAccount = createUser,
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier
            };
            PlayFabClientAPI.LoginWithAndroidDeviceID(_request, _data => { LogInResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Updates player display name.
        /// </summary>
        /// <param name="newName">The new name.</param>
        public static IEnumerator UpdateUserDisplayName(string newName)
        {
            DisplayNameUpdateResult = null;
            resetData();
            UpdateUserTitleDisplayNameRequest _request = new UpdateUserTitleDisplayNameRequest { DisplayName = newName };
            PlayFabClientAPI.UpdateUserTitleDisplayName(_request, _data => { DisplayNameUpdateResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets a player profile.
        /// </summary>
        /// <param name="playerPlayfabId">The palyfab id of the player you want to get.</param>
        public static IEnumerator GetPlayerProfileOf(string playerPlayfabId)
        {
            _playerProfileResult = null;
            resetData();
            PlayFab.ClientModels.GetPlayerProfileRequest _request = new PlayFab.ClientModels.GetPlayerProfileRequest { PlayFabId = playerPlayfabId };
            PlayFabClientAPI.GetPlayerProfile(_request, _data => { _playerProfileResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets teh leaderboard around provided player id.
        /// </summary>
        /// <param name="playerId">If null or empty it will use the current logged player.</param>
        /// <param name="leaderBoard">The name of the leaderboard to retrieve.</param>
        /// <param name="maxResults">The max result to retrieve having the target player in the center of it.</param>
        /// <returns></returns>
        public static IEnumerator GetLeaderboardAroundPlayer(string playerId, string leaderBoard, int maxResults)
        {
            _leaderboardAroundPlayerResult = null;
            resetData();
            GetLeaderboardAroundPlayerRequest _request = new GetLeaderboardAroundPlayerRequest
            { PlayFabId = playerId, StatisticName = leaderBoard, MaxResultsCount = maxResults };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(_request, _data => { _leaderboardAroundPlayerResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        /// <summary>
        /// Gets the leaderboard list.
        /// </summary>
        /// <param name="leaderboard">The name of the leaderboard.</param>
        /// <param name="startPosition">The start position</param>
        /// <param name="maxPositions">The max positions to return</param>
        /// <returns></returns>
        public static IEnumerator GetLeaderboard(string leaderboard, int startPosition, int maxPositions)
        {
            _leaderboardResult = null;
            resetData();
            GetLeaderboardRequest _request = new GetLeaderboardRequest
            {
                StatisticName = leaderboard,
                StartPosition = startPosition,
                MaxResultsCount = maxPositions
            };
            PlayFabClientAPI.GetLeaderboard(_request, _data => { _leaderboardResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });

            yield return new WaitUntil(() => _gotResponse);
        }
        public static IEnumerator UpdatePlayerStaticsFromClient(string leaderboard, int currentScore)
        {
            _updatePlayerStaticsResult = null;
            resetData();
            PlayFab.ClientModels.UpdatePlayerStatisticsRequest _request = new PlayFab.ClientModels.UpdatePlayerStatisticsRequest
            {
                Statistics = new List<PlayFab.ClientModels.StatisticUpdate>()
                {
                    new PlayFab.ClientModels.StatisticUpdate{StatisticName=leaderboard,Value=currentScore}
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(_request, _data => { _updatePlayerStaticsResult = _data; RequestFinished(); },
                _data => { _requestError = _data; RequestFinished(); });
            yield return new WaitUntil(() => _gotResponse);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Converts a json string of an object array (Ex:[{},{}]) string[] each string represents an object.
        /// </summary>
        public static string[] ParseJsonStringObjectArray(string jsonStringArray)
        {
            List<string> _result = new List<string>();
            string _objectString = string.Empty;
            foreach (var character in jsonStringArray)
            {
                //find teh open bracket
                if (character.Equals('{'))
                    _objectString = character.ToString();
                else if (character.Equals('}'))//find the close bracket and add object so list
                {
                    _objectString += character;
                    _result.Add(_objectString);
                    _objectString = string.Empty;
                }
                else if (!_objectString.IsNullOrEmptyOrWhiteSpaced())//if an open brackets has been found then keep adding character until close bracket
                    _objectString += character;
            }
            return _result.ToArray();
        }
        /// <summary>
        /// Converts custom data array to purchasepriceamount list.
        /// </summary>
        /// <param name="data">The data to convert.</param>
        static List<PurchasePriceAmount> ConvertCustomDataToPurchasePriceAmountList(DataStructs.CloudScriptParameterData[] data)
        {
            if (data == null)
                return null;
            List<PurchasePriceAmount> _result = new List<PurchasePriceAmount>();
            foreach (var item in data)
            {
                _result.Add(new PurchasePriceAmount()
                {
                    ItemId = item.Key,
                    Amount = int.Parse(item.Value)
                });
            }
            return _result;
        }
        static List<string> ConvertMembersListIntoStringList(List<MatchmakingPlayerWithTeamAssignment> members)
        {
            List<string> _resul = new List<string>();
            foreach (var item in members)
                _resul.Add(item.Entity.Type + "!" + item.Entity.Id);
            return _resul;
        }
        /// <summary>
        /// Converts a region array to a list of strings.
        /// </summary>
        //static List<string> ConvertRegionArrayToList(PlayFab.ClientModels.Region[] regions)
        //{
        //    List<string> _result = new List<string>();
        //    foreach (var item in regions)
        //        _result.Add(item.ToString());
        //    return _result;
        //}
        /// <summary>
        /// Called from cloud script tester.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonObject[] ConvertParameters(CloudScriptParameterData[][] data)
        {
            JsonObject[] _result = new JsonObject[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                _result[i] = new JsonObject();
                _result[i].Add(data[i][0].Key, data[i][0].Value);
                if (data[i].Length == 2)//if needs another parametter like statistics which needs 2 a key and a value.
                    _result[i].Add(data[i][1].Key, data[i][1].Value);
            }
            return _result;
        }
        /// <summary>
        /// Converts the parameters array to json object.
        /// </summary>
        /// <param name="data">The paramters to add.</param>
        /// <returns>The json object will all the aprameters.</returns>
        public static JsonObject[] ConvertParameters(CloudScriptParameterValues[] data, params CloudScriptParameterData[][] _templates)
        {
            if (_templates.Length == 0)
            {
                Debug.LogError("Templates parameter cant be null");
                return null;
            }
            JsonObject[] _result = new JsonObject[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                _result[i] = new JsonObject();
                int _templateIndex = _templates.Length == 1 ? 0 : Mathf.Clamp(i, 0, _templates.Length - 1);//limit template index to the las index if there is more tahn 1 and less than data length.
                _result[i].Add(_templates[_templateIndex][0].Key, data[i].KeyValue);
                if (_templates[_templateIndex].Length == 2)//if needs another parametter like statistics which needs 2 a key and a value.
                    _result[i].Add(_templates[_templateIndex][1].Key, data[i].ValueValue);
            }
            return _result;
        }
        /// <summary>
        /// Is this id mine?
        /// </summary>
        /// <param name="playerId"></param>
        public static bool IsMyPlayer(string playerId)
        {
            return LogInResult.PlayFabId == playerId;
        }
        /// <summary>
        /// Resets the common data in methods.
        /// </summary>
        private static void resetData()
        {
            _gotResponse = false;
            _requestError = null;
        }
        /// <summary>
        /// Activates the _gotRequest to return corroutine.
        /// </summary>
        static void RequestFinished()
        {
            _gotResponse = true;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the last result of a player data get request.
        /// </summary>
        public static Dictionary<string, PlayFab.ClientModels.UserDataRecord> UserData => _getUserDataResult.Data;
#if Playfab_Server
        /// <summary>
        /// Gets the last result of a player data get request from server.
        /// </summary>
        public static Dictionary<string, PlayFab.ServerModels.UserDataRecord> UserDataFromServer => _getUserDataResultFromServer.Data;
        public static PlayFab.ServerModels.GetPlayerProfileResult LastPlayerProfileGotServer => PlayerProfileResultServer;

#endif
        /// <summary>
        /// Was last request successfull?
        /// </summary>
        public static bool WasLastRequestSuccessful => _requestError == null;
        public static bool WasLastCloudScriptReqeustSuccessful => CloudScriptResult != null && CloudScriptResult.Error == null;
        public static PlayFabError LastRequestError => _requestError;
        public static PlayFab.ClientModels.ScriptExecutionError LastCloudRequestError => CloudScriptResult.Error;
        public static PlayFab.ClientModels.GetPlayerProfileResult LastPlayerProfileGot => _playerProfileResult;
        public static List<PlayFab.ClientModels.PlayerLeaderboardEntry> LeaderboardAroundPlayerList => _leaderboardAroundPlayerResult.Leaderboard;
        /// <summary>
        /// Gets the logged player last ledaerboard around player called leaderboard entry.
        /// </summary>
        public static PlayFab.ClientModels.PlayerLeaderboardEntry GetPlayerLeaderboardEntry
        {
            get
            {
                if (LogInResult == null)
                    return null;

                return _leaderboardAroundPlayerResult.Leaderboard.FirstOrDefault(data => data.PlayFabId == LogInResult.PlayFabId);
            }
        }
        public static List<PlayFab.ClientModels.PlayerLeaderboardEntry> LeaderboardList => _leaderboardResult.Leaderboard;
        public static DateTime? leaderboardNextResetTime => _leaderboardAroundPlayerResult.NextReset;
        /// <summary>
        /// Is the helper waiting ofr response?
        /// </summary>
        public static bool IsBusy => !_gotResponse;
        /// <summary>
        /// Returns the id of last ticket created.
        /// </summary>
        public static string LastCreatedTicketId => _createMatchmakingTicketResult != null ? _createMatchmakingTicketResult.TicketId : "";
        /// <summary>
        /// Has matchmaking been calnceled?:
        /// </summary>
        public static bool WasMatchmakingCanceled => _cancelMatchmakingTicktResult != null;
        /// <summary>
        /// Gets the server current utc time.
        /// </summary>
        #endregion
    }
    public enum MatchmakingTicketStatuStates
    {
        None,
        WaitingForMatch,
        Matched,
        WaitingForServer,
        Canceled
    }
}
#endif