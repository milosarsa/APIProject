using System.Net;


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
            CloudTableClient _tableClient = storageAccount.CreateCloudTableClient();
            table = _tableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync();
        }

        public async Task<int> CreateAsync(TaskEntity entity)
        {
            int currentId = await GetHighestId();
            currentId++;
            entity.PartitionKey = currentId.ToString();

            memoryCache.Set("CurrentTaskId", currentId);
            TableOperation tableOperation = TableOperation.Insert(entity);
            TableResult tableResult = await table.ExecuteAsync(tableOperation);
            if (tableResult.HttpStatusCode == 204)
                return currentId;
            else
                throw new HttpRequestException("Entity creation failed.", null, (HttpStatusCode)tableResult.HttpStatusCode);

        }

        public async Task<List<TaskEntity>> ReadAllAsync()
        {
            List<TaskEntity> entities = new List<TaskEntity>();

            //Creating table query to query all Task entities
            TableQuerySegment<TaskEntity>? querySegment = null;
            TableQuery<TaskEntity> tableQuery = new TableQuery<TaskEntity>();

            while (querySegment == null || querySegment.ContinuationToken != null)
            {
                querySegment = await table.ExecuteQuerySegmentedAsync<TaskEntity>(tableQuery, querySegment != null ? querySegment.ContinuationToken : null);
                entities.AddRange(querySegment.Results);
            }
            return entities;


        }

        public async Task<List<TaskEntity>> ReadByKeyAsync(int key)
        {
            List<TaskEntity> entities = new List<TaskEntity>();

            //Creating table query to query all Task entities
            TableQuerySegment<TaskEntity>? querySegment = null;
            TableQuery<TaskEntity> tableQuery = new TableQuery<TaskEntity>();
            tableQuery.Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, key.ToString()));

            while (querySegment == null || querySegment.ContinuationToken != null)
            {
                querySegment = await table.ExecuteQuerySegmentedAsync<TaskEntity>(tableQuery, querySegment != null ? querySegment.ContinuationToken : null);
                entities.AddRange(querySegment.Results);
            }
            return entities;

        }

        public async Task<TaskEntity> ReadAsync(int id)
        {
            TableQuerySegment<TaskEntity>? querySegment = null;
            TableQuery<TaskEntity> tableQuery = new TableQuery<TaskEntity>();
            tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id.ToString()));

            querySegment = await table.ExecuteQuerySegmentedAsync<TaskEntity>(tableQuery, null);
            if (querySegment.Results.Any())
            {
                TaskEntity entity = querySegment.Results[0] != null ? querySegment.Results[0] : new TaskEntity();
                return entity; ;
            }
            else
                throw new HttpRequestException("Entity not found.", null, HttpStatusCode.NotFound);
        }

        public async Task UpdateAsync(TaskEntity entity)
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
                    throw new HttpRequestException("Delete failed.", null, (HttpStatusCode)tableResult.HttpStatusCode);
            }
            else
                throw new HttpRequestException("Entity not found.", null, HttpStatusCode.NotFound);
        }

        //Checking to see if the entity exists in the table using a provided entity partitionKey and rowKey
        public async Task<bool> Exists(Entity entity)
        {
            //Try to retrieve entity

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
                currentId = currentId  < startId ? startId : currentId;
                memoryCache.Set("CurrentProjectId", currentId);
            }
            return currentId;

        }


    }
}

