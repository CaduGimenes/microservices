using Dapper;
using Discout.API.Entities;
using Npgsql;

namespace Discout.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private NpgsqlConnection GetConnectionPostgreSQL()
        {
            return new(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }


        public async Task<Coupon> GetDiscount(string productName)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon is null) return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            return coupon;
        }


        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var affected = await connection.ExecuteAsync(String.Concat("INSET INTO Coupon(ProductName, Description, Amount)", " VALUES (@ProductName, @Description, @Amount)"), new { coupon.ProductName, coupon.Description, coupon.Amount });

            if (affected == 0) return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var affected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id", new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

            if (affected == 0) return false;

            return true;

        }
        public async Task<bool> DeleteDiscount(string productName)
        {
            NpgsqlConnection connection = GetConnectionPostgreSQL();

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (affected == 0) return false;

            return true;

        }

    }
}
