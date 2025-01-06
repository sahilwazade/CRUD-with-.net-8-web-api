using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TestWebApi.Context;
using TestWebApi.IRepositories;
using TestWebApi.Models;
using TestWebApi.Models.RequestModels;
using TestWebApi.Models.ResponseModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TestWebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _conString;

        public ProductRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _conString = config.GetConnectionString("DefaultConnection");
        }

        //with dapper and sp
        public async Task<GetProductResponse> GetProduct()
        {
            try
            {
                using (var con = new SqlConnection(_conString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

                    var result = await con.QueryAsync<Product>("GetProducts", parameters, commandType: CommandType.StoredProcedure );

                    var productList = result.ToList();
                    var message = parameters.Get<string>("@Message");

                    if (productList != null && productList.Count > 0)
                    {
                        return new GetProductResponse
                        {
                            Message = message,
                            IsSuccess = message.Contains("successfully") ? true : false,
                            products = productList
                        };
                    }

                    return new GetProductResponse
                    {
                        Message = "No products found.",
                        IsSuccess = false,
                        products = []
                    };
                }
            }
            catch (Exception ex) 
            {
                return new GetProductResponse
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}",
                };
            }
        }

        //with Entity Framework and sp
        public async Task<GetProductByIdResponse> GetProductById(int id)
        {
            if (id <= 0)
            {
                return new GetProductByIdResponse()
                {
                    Message = "Please enter valid Id",
                    IsSuccess = false
                };
            }

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@Message", System.Data.SqlDbType.VarChar, 500)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    }
                };

                var result = await _context.products.FromSqlRaw("Exec GetProductById @Id, @Message OUTPUT", parameters).AsNoTracking().ToListAsync();
                var product = result.First();

                var msg = parameters.FirstOrDefault(p => p.ParameterName == "@Message")?.Value?.ToString();

                if (product != null)
                {
                    return new GetProductByIdResponse()
                    {
                        IsSuccess = true,
                        Message = msg,
                        product = product
                    };
                }

                return new GetProductByIdResponse()
                {
                    IsSuccess = false,
                    Message = "No products found.",
                    product = null
                };
            }
            catch (Exception ex)
            {
                return new GetProductByIdResponse()
                {
                    IsSuccess = false,
                    Message = $"Error : {ex.Message}",
                };
            }
        }

        //with ADO.NET and sp
        public async Task<GenericResponse> AddProduct(AddProductRequest request)
        {
            try
            {
                using (var con = new SqlConnection(_conString))
                {
                    using (var command = new SqlCommand("AddProduct", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        var parameters = new[]
                        {
                            new SqlParameter("@Name", request.Name),
                            new SqlParameter("@Price", request.Price),
                            new SqlParameter("@Message", SqlDbType.NVarChar, 500)
                            {
                                Direction = ParameterDirection.Output
                            }
                        };
                        command.Parameters.AddRange(parameters);

                        await con.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        var msg = parameters.FirstOrDefault(p => p.ParameterName == "@Message")?.Value?.ToString();

                        return new GenericResponse()
                        {
                            IsSuccess = msg.Contains("exist") ? false : true,
                            Message = msg
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}",
                };
            }
        }

        //with Entity Framework and sp
        public async Task<GenericResponse> UpdateProduct(UpdateProductRequest request)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@Name", request.Name),
                    new SqlParameter("@Price", request.Price),
                    new SqlParameter("@Message", System.Data.SqlDbType.VarChar, 500)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    }
                };

                await _context.Database.ExecuteSqlRawAsync("Exec UpdateProduct @Id, @Name, @Price, @Message OUTPUT", parameters);
                var msg = parameters.FirstOrDefault(p => p.ParameterName == "@Message")?.Value?.ToString();

                return new GenericResponse()
                {
                    IsSuccess = msg.Contains("updated") ? true : false,
                    Message = msg
                };
               
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    IsSuccess = false,
                    Message = $" Error : {ex.Message}"
                };
            }
        }

        //with dapper and sp
        public async Task<GenericResponse> DeleteProduct(int id)
        {
            try
            {
                using (var con = new SqlConnection(_conString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", id, DbType.Int32);
                    parameters.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
                   
                    await con.ExecuteAsync("DeleteProduct", parameters, commandType: CommandType.StoredProcedure);
                    var message = parameters.Get<string>("@Message");

                    return new GenericResponse()
                    {
                        IsSuccess = message.Contains("Deleted") ? true : false,
                        Message = message
                    };
                }
            }
            catch (Exception ex)
            {
                return new GenericResponse()
                {
                    IsSuccess = false,
                    Message = $" Error : { ex.Message }"
                };
            }
        }
    }
}
