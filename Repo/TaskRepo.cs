using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Repo
{
    //Responsible for data access
    public class TaskRepo : ITaskRepo
    {
        private const string tableName = "Task";
        private CloudTable table;
        private ILogger<TaskRepo> logger;
        private IMemoryCache memoryCache;
        private int startId = 1000;

        public TaskRepo(CloudStorageAccount storageAccount, ILogger<TaskRepo> _logger, IMemoryCache _memoryCache)
        {
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
            memoryCache = _memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            try
            {
                CloudTableClient _tableClient = storageAccount.CreateCloudTableClient();
                table = _tableClient.GetTableReference(tableName);
                table.CreateIfNotExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Storage Account init failed.\n" +
                    ex.Message);
                throw new HttpRequestException("Storage account init failed.", ex, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<int> CreateAsync(TaskEntity entity)
        {
            try
            {
                int currentId = await GetHighestId();
                currentId++;
                entity.PartitionKey = currentId.ToString();

                memoryCache.Remove("CurrentTaskId");
                TableOperation tableOperation = TableOperation.Insert(entity);
                TableResult tableResult = await table.ExecuteAsync(tableOperation);
                if (tableResult.HttpStatusCode == 204)
                    return currentId;
                else
                    throw new HttpRequestException("Entity creation failed.", null, (HttpStatusCode)tableResult.HttpStatusCode);
            }
            catch (Exception ex)
            {
                logger.LogError("Creating new entity failed.\n" +
                    ex.Message);
                throw new HttpRequestException("Creating new entity failed with error.\n{ex.Message}", ex, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<List<TaskEntity>> ReadAllAsync()
        {
            List<TaskEntity> entities = new List<TaskEntity>();
            try
            {
                //Creating table query to query all Task entities
                TableQuerySegment<TaskEntity> querySegment = null;
                TableQuery<TaskEntity> tableQuery = new TableQuery<TaskEntity>();

                while (querySegment == null || querySegment.ContinuationToken != null)
                {
                    querySegment = await table.ExecuteQuerySegmentedAsync<TaskEntity>(tableQuery, querySegment != null ? querySegment.ContinuationToken : null);
                    entities.AddRange(querySegment.Results);
                }
                return entities;

            }
            catch (Exception ex)
            {
                logger.LogError("Reading all entities failed.\n" +
                    ex.Message);
                throw new HttpRequestException($"Reading all entities failed with error.\n{ex.Message}", ex, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<List<TaskEntity>> ReadByKeyAsync(int key)
        {
            List<TaskEntity> entities = new List<TaskEntity>();
            try
            {
                //Creating table query to query all Task entities
                TableQuerySegment<TaskEntity> querySegment = null;
                TableQuery<TaskEntity> tableQuery = new TableQuery<TaskEntity>();
                tableQuery.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, key.ToString()));

                while (querySegment == null || querySegment.ContinuationToken != null)
                {
                    querySegment = await table.ExecuteQuerySegmentedAsync<TaskEntity>(tableQuery, querySegment != null ? querySegment.ContinuationToken : null);
                    entities.AddRange(querySegment.Results);
                }
                return entities;

            }
            catch (Exception ex)
            {
                logger.LogError("Reading all entities failed.\n" +
                    ex.Message);
                throw new HttpRequestException("Reading all entities failed with error.\n{ex.Message}", ex, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<TaskEntity> ReadAsync(int id)
        {
            try
            {
                TableQuerySegment<TaskEntity> querySegment = null;
                TableQuery<TaskEntity> tableQuery = new TableQuery<TaskEntity>();
                tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id.ToString()));

                querySegment = await table.ExecuteQuerySegmentedAsync<TaskEntity>(tableQuery, null);
                if (querySegment.Results.Any())
                {
                    TaskEntity entity = querySegment.Results[0] != null ? querySegment.Results[0] : new TaskEntity();
                    return entity; ;
                }
                else
                {
                    throw new HttpRequestException("Entity not found.", null, HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Reading entity failed.\n" +
                    ex.Message);
                throw new HttpRequestException("Reading entity failed with error.\n{ex.Message}", ex, HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(TaskEntity entity)
        {
            entity.ETag = "*";
            try
            {
                if (await Exists(entity))
                {
                    TableOperation tableOperation = TableOperation.Merge(entity);
                    TableResult tableResult = await table.ExecuteAsync(tableOperation);
                    if (tableResult.HttpStatusCode == 200)
                    {
                        return;
                    }
                    else
                    {
                        logger.LogError($"Update failed!\nTable operation returned {((HttpStatusCode)tableResult.HttpStatusCode).ToString()} status");
                        throw new HttpRequestException("Update failed.", null, (HttpStatusCode)tableResult.HttpStatusCode);
                    }
                }
                else
                {
                    throw new HttpRequestException("Entity not found.", null, HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Updating entity failed.\n" +
                    ex.Message);
                throw new HttpRequestException($"Updating entity failed with error.\n{ex.Message}", ex, HttpStatusCode.InternalServerError);
            }
        }
        public async Task DeleteAsync(Entity entity)
        {
            try
            {
                if (await Exists(entity))
                {
                    TableOperation tableOperation = TableOperation.Delete(entity);
                    TableResult tableResult = await table.ExecuteAsync(tableOperation);
                    if (tableResult.HttpStatusCode == 204)
                    {
                        return;
                    }
                    else
                    {
                        throw new HttpRequestException("Delete failed.", null, (HttpStatusCode)tableResult.HttpStatusCode);
                    }
                }
                else
                {
                    throw new HttpRequestException("Entity not found.", null, HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Deleting entity failed.\n" +
                    ex.Message);
                throw new HttpRequestException("Deleting entity failed with error.\n{ex.Message}", ex, HttpStatusCode.InternalServerError);
            }
        }

        //Checking to see if the entity exists in the table
        public async Task<bool> Exists(Entity entity)
        {
            //Try to retrieve entity, returns false if status code is 404 
            try
            {
                TableOperation tableOperation = TableOperation.Retrieve(entity.PartitionKey, entity.RowKey);
                TableResult tableResult = await table.ExecuteAsync(tableOperation);
                if (tableResult.HttpStatusCode == 404)
                {
                    return false;
                }
                else if (tableResult.HttpStatusCode == 200)
                {
                    return true;
                }
                else
                {
                    logger.LogError($"Retrieving entity from {tableName} table failed.\n" +
                        $"Table operation returned {((HttpStatusCode)tableResult.HttpStatusCode).ToString()} status ");
                    throw new HttpRequestException("Retrieving entity failed.", null, (HttpStatusCode)tableResult.HttpStatusCode);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Retrieving entity failed with error: \n" +
                    ex.Message +
                    "\nReturning not found!");
                throw new HttpRequestException($"Retrieving entity failed with error.\n{ex.Message}", ex, HttpStatusCode.InternalServerError);
            }
        }
        private async Task<int> GetHighestId()
        {
            try
            {
                int currentId = startId;
                if (!memoryCache.TryGetValue("CurrentTaskId", out currentId))
                {
                    List<Entity> entities = new List<Entity>();
                    //Creating table query to query all project entities
                    TableQuerySegment<Entity> querySegment = null;
                    TableQuery<Entity> tableQuery = new TableQuery<Entity>();

                    while (querySegment == null || querySegment.ContinuationToken != null)
                    {
                        querySegment = await table.ExecuteQuerySegmentedAsync<Entity>(tableQuery, querySegment != null ? querySegment.ContinuationToken : null);
                        entities.AddRange(querySegment.Results);
                    }
                    int tempId = 0;
                    foreach (Entity entity in entities)
                    {
                        int entityId = int.Parse(entity.PartitionKey);
                        tempId = tempId < entityId ? entityId : tempId;
                    }
                    currentId = tempId < startId ? startId : tempId;
                    memoryCache.Set("CurrentTaskId", currentId);
                }
                return currentId;

            }
            catch (Exception ex)
            {
                logger.LogError("Reading all entities failed.\n" +
                    ex.Message);
                throw new HttpRequestException($"Reading all entities failed with error.\n{ex.Message}", ex, HttpStatusCode.InternalServerError);
            }
        }
    }
}

