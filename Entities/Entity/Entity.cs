using Microsoft.Azure.Cosmos.Table;

namespace Entities.Entity
{
    public class Entity : TableEntity
    {
        public Entity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
        public Entity()
        {

        }
    }

    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
            : base(String.Format("Provided entity does not exist in the table"))
        {

        }
    }
}
