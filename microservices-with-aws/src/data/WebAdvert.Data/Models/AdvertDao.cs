using Amazon.DynamoDBv2.DataModel;
using System;
using WebAdvert.Models;

namespace WebAdvert.Data.Models
{
    [DynamoDBTable("sz-webadvert-adverts")]
    public class AdvertDao
    {
        [DynamoDBHashKey] 
        public string Id { get; set; }

        [DynamoDBProperty] 
        public string Title { get; set; }

        [DynamoDBProperty] 
        public string Description { get; set; }

        [DynamoDBProperty] 
        public double Price { get; set; }

        [DynamoDBProperty] 
        public DateTime CreationDateTime { get; set; }

        [DynamoDBProperty] 
        public AdvertStatus Status { get; set; }

        [DynamoDBProperty] 
        public string FilePath { get; set; }

        [DynamoDBProperty] 
        public string UserName { get; set; }
    }

    public static class AdvertDaoExtensions
    {
        public static AdvertDao SetDefaults(this AdvertDao dao)
        {
            dao.Id = new Guid().ToString();
            dao.CreationDateTime = DateTime.UtcNow;
            dao.Status = AdvertStatus.Pending;

            return dao;
        }
    }
}
