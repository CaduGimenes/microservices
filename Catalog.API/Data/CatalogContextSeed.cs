using Catalog.API.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();

                if (!existProduct) productCollection.InsertManyAsync(GetMyProducts());
        }

        private static IEnumerable<Product> GetMyProducts()
        {
            return JsonSerializer.Deserialize<List<Product>>(File.ReadAllText("./Data/products.json"));
        }
    }
}
