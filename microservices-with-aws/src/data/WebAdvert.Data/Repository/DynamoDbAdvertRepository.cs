using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.Data.Models;
using WebAdvert.Models;

namespace WebAdvert.Data.Repository
{
    public class DynamoDbAdvertRepository : IAdvertRepository
    {
        private readonly IMapper _mapper;

        public DynamoDbAdvertRepository()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Advert, AdvertDao>();
            });

            _mapper = config.CreateMapper();
        }

        public async Task<IEnumerable<Advert>> GetAllAsync()
        {
            using var client = new AmazonDynamoDBClient();
            using var context = new DynamoDBContext(client);

            var scanResult = await context.ScanAsync<AdvertDao>(new List<ScanCondition>()).GetNextSetAsync();
            return scanResult.Select(item => _mapper.Map<Advert>(item)).ToList();
        }

        public async Task<Advert> GetByIdAsync(string id)
        {
            using (var client = new AmazonDynamoDBClient())
            {
                using var context = new DynamoDBContext(client);

                var dao = await context.LoadAsync<AdvertDao>(id);
                if (dao != null) return _mapper.Map<Advert>(dao);
            }

            throw new KeyNotFoundException($"A record with ID={id} was not found.");
        }

        public async Task<string> AddAsync(Advert model)
        {
            var dao = _mapper.Map<AdvertDao>(model).SetDefaults();

            using var client = new AmazonDynamoDBClient();
            using var dbContext = new DynamoDBContext(client);

            // save data
            await dbContext.SaveAsync(dao);

            return dao.Id;
        }

        public async Task ConfirmAsync(ConfirmAdvert model)
        {
            using var client = new AmazonDynamoDBClient();
            using var context = new DynamoDBContext(client);

            // fetch record
            var record = await context.LoadAsync<AdvertDao>(model.Id);
            if (record == null)
            {
                throw new KeyNotFoundException($"A record with ID={model.Id} was not found.");
            }

            if (model.Status == AdvertStatus.Active)
            {
                record.Status = AdvertStatus.Active;
                await context.SaveAsync(record);
            }
            else
            {
                await context.DeleteAsync(record);
            }
        }

        public async Task<bool> CheckHealthAsync()
        {
            Console.WriteLine("Health check in progress.");

            using var client = new AmazonDynamoDBClient();
            var tableData = await client.DescribeTableAsync("sz-webadvert-adverts");

            return string.Compare(tableData.Table.TableStatus, "active", true) == 0;
        }
    }
}
