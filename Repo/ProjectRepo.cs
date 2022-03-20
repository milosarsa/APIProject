using System.Net;

namespace Repo
{
    //Responsible for data access
    public class ProjectRepo : IProjectRepo
    {
        private const string tableName = "Project";
        private CloudTable table;
        private ILogger<ProjectRepo> logger;
        private IMemoryCache memoryCache;
        private int startId = 1000;

        public ProjectRepo(CloudStorageAccount storageAccount, ILogger<ProjectRepo> _logger, IMemoryCache _memoryCache)
        {
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
            memoryCache = _memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            CloudTableClient _tableClient = storageAccount.CreateCloudTableClient();
            table = _tableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync();
        }

        public async Task CreateAsync(ProjectEntity entity)
        {
            int currentId = await GetHighestId();
            currentId++;
            entity.PartitionKey = currentId.ToString();
            if (await Exists(entity.RowKey, "RowKey"))
            {
                throw new HttpRequestException($"Project with that name already exists.", null, HttpStatusCode.Conflict);
            }

            memoryCache.Set("CurrentProjectId", currentId);
            TableOperation tableOperation = TableOperation.Insert(entity);
            TableResult tableResult = await table.ExecuteAsync(tableOperation);
            if (tableResult.HttpStatusCode == 204)
                return;
            else
                throw new HttpRequestException($"Entity creation with id {currentId} failed.", null, (HttpStatusCode)tableResult.HttpStatusCode);
        }

        public async Task<List<ProjectEntity>> ReadAllAsync()
        {
            List<ProjectEntity> entities = new List<ProjectEntity>();
            //Creating table query to query all project entities
            TableQuerySegment<ProjectEntity>? querySegment = null;
            TableQuery<ProjectEntity> tableQuery = new TableQuery<ProjectEntity>();

            while (querySegment == null || querySegment.ContinuationToken != null)
            {
                querySegment = await table.ExecuteQuerySegmentedAsync<ProjectEntity>(tableQuery, querySegment?.ContinuationToken ?? null);
                entities.AddRange(querySegment.Results);
            }
            return entities;
        }

        public async Task<ProjectEntity> ReadAsync(int id)
        {
            TableQuerySegment<ProjectEntity>? querySegment = null;
            TableQuery<ProjectEntity> tableQuery = new TableQuery<ProjectEntity>();
            tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id.ToString()));
            querySegment = await table.ExecuteQuerySegmentedAsync<ProjectEntity>(tableQuery, null);
            if (querySegment.Results.Any())
            {
                ProjectEntity entity = querySegment.Results[0] != null ? querySegment.Results[0] : new ProjectEntity();
                return entity; ;
            }
            else
                throw new HttpRequestException("Entity not found.", null, HttpStatusCode.NotFound);

        }

        public async Task UpdateAsync(ProjectEntity entity)
        {
            entity.ETag = "*";
            if (await Exists(entity))
            {
                TableOperation tableOperation = TableOperation.Merge(entity);
                TableResult tableResult = await table.ExecuteAsync(tableOperation);
                if (tableResult.HttpStatusCode != 204)
                {
                    logger.LogError($"Update failed!\nTable operation returned {(HttpStatusCode)tableResult.HttpStatusCode} status");
                    throw new HttpRequestException("Update failed.", null, (HttpStatusCode)tableResult.HttpStatusCode);
                }
                return;
            }
            else
                throw new HttpRequestException("Entity not found.", null, HttpStatusCode.NotFound);
            
        }
        public async Task DeleteAsync(Entity entity)
        {
            if (await Exists(entity))
            {
                TableOperation tableOperation = TableOperation.Delete(entity);
                TableResult tableResult = await table.ExecuteAsync(tableOperation);
                if (tableResult.HttpStatusCode == 204)
                    return;
                else
                    throw new HttpRequestException("Deleting entity failed.", null, (HttpStatusCode)tableResult.HttpStatusCode);
                
            }
            throw new HttpRequestException("Entity not found.", null, HttpStatusCode.NotFound);
        }

        //Checking to see if the entity exists in the table using a provided entity partitionKey and rowKey
        public async Task<bool> Exists(Entity entity)
        {
            //Try to retrieve entity, returns false if status code is 404 
            TableOperation tableOperation = TableOperation.Retrieve(entity.PartitionKey, entity.RowKey);
            TableResult tableResult = await table.ExecuteAsync(tableOperation);
            if (tableResult.HttpStatusCode == 200) 
                return true;
            return false;
        }

        // Check to see if the entity exists in the table using a provided value and key
        // Ex.
        // Exists("MyProjectId")
        // Exists("MyProjectName", "RowKey")
        public async Task<bool> Exists(string value, string key = "PartitionKey")
        {
            List<Entity> entities = new List<Entity>();
            TableQuerySegment<Entity>? querySegment = null;
            TableQuery<Entity> tableQuery = new TableQuery<Entity>();
            tableQuery.Where(TableQuery.GenerateFilterCondition(key, QueryComparisons.Equal, value));

            while (querySegment == null || querySegment.ContinuationToken != null)
            {
                querySegment = await table.ExecuteQuerySegmentedAsync<Entity>(tableQuery, querySegment != null ? querySegment.ContinuationToken : null);
                entities.AddRange(querySegment.Results);
            }
            if (entities.Any()) 
                return true;
            else
                return false;
        }

        //Query all the entities in the current table and return the highest id
        //We know that the table will be queried in ascending order so no need to iterate through entities
        private async Task<int> GetHighestId()
        {
            int currentId = startId;
            if (!memoryCache.TryGetValue("CurrentProjectId", out currentId))
            {
                List<Entity> entities = new List<Entity>();
                //Creating table query to query all project entities
                TableQuerySegment<Entity>? querySegment = null;
                TableQuery<Entity> tableQuery = new TableQuery<Entity>();

                while (querySegment == null || querySegment.ContinuationToken != null)
                {
                    querySegment = await table.ExecuteQuerySegmentedAsync<Entity>(tableQuery, querySegment != null ? querySegment.ContinuationToken : null);
                    entities.AddRange(querySegment.Results);
                }
                int.TryParse(entities.LastOrDefault()?.PartitionKey, out currentId);

                //Use startId if tempId is smaller than startId - 0 (no entries present)
                currentId = currentId < startId ? startId : currentId;
                memoryCache.Set("CurrentProjectId", currentId);
            }
            return currentId;
        }
        //Unused, leaving it here if needed
        //Get the current highest id in the table by querying all data and comparing
        private async Task<int> GetHighestIdOld()
        {
            try
            {
                int currentId = startId;
                if (!memoryCache.TryGetValue("CurrentProjectId", out currentId))
                {
                    List<Entity> entities = new List<Entity>();
                    //Creating table query to query all project entities
                    TableQuerySegment<Entity>? querySegment = null;
                    TableQuery<Entity> tableQuery = new TableQuery<Entity>();

                    while (querySegment == null || querySegment.ContinuationToken != null)
                    {
                        querySegment = await table.ExecuteQuerySegmentedAsync<Entity>(tableQuery, querySegment != null ? querySegment.ContinuationToken : null);
                        entities.AddRange(querySegment.Results);
                    }

                    //Iterating through all entities and comparing their id
                    int tempId = 0;
                    foreach (Entity entity in entities)
                    {
                        int entityId = int.Parse(entity.PartitionKey);
                        tempId = tempId < entityId ? entityId : tempId;
                    }

                    //Use startId if tempId is smaller than startId - 0 (no entries present)
                    currentId = tempId < startId ? startId : tempId;
                    memoryCache.Set("CurrentProjectId", currentId);
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
