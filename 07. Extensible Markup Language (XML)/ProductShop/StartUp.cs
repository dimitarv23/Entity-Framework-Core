using AutoMapper;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Xml.Linq;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext dbContext = new ProductShopContext();

            Console.WriteLine(
                GetUsersWithProducts(dbContext));
        }

        //Exercise 1
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            var userDtos = xmlHelper
                .Deserialize<List<UserDto>>(inputXml, "Users");

            List<User> users = new List<User>();

            foreach (var userDto in userDtos
                .Where(u => !string.IsNullOrWhiteSpace(u.FirstName) &&
                        !string.IsNullOrWhiteSpace(u.LastName)))
            {
                var user = mapper.Map<User>(userDto);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        //Exercise 2
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();
            List<ProductDto> productDtos = xmlHelper
                .Deserialize<List<ProductDto>>(inputXml, "Products");

            var products = new List<Product>();

            foreach (var productDto in productDtos
                .Where(p => !string.IsNullOrWhiteSpace(p.Name)))
            {
                var product = mapper.Map<Product>(productDto);
                products.Add(product);
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //Exercise 3
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();
            List<CategoryDto> categoryDtos = xmlHelper
                .Deserialize<List<CategoryDto>>(inputXml, "Categories");

            var categories = new List<Category>();

            foreach (var categoryDto in categoryDtos
                .Where(c => !string.IsNullOrWhiteSpace(c.Name)))
            {
                var category = mapper.Map<Category>(categoryDto);
                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //Exercise 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();
            var categoryProductDtos = xmlHelper
                .Deserialize<List<CategoryProductDto>>(inputXml, "CategoryProducts");

            var categoryProducts = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductDtos
                .Where(cp => cp.CategoryId != 0 &&
                        cp.ProductId != 0))
            {
                var categoryProduct = mapper.Map<CategoryProduct>(categoryProductDto);
                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //Exercise 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var products = context.Products
                .Where(p => p.Price >= 500 &&
                        p.Price <= 1500)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ExportProductDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerName = p.Buyer.FirstName + " " + p.Buyer.LastName
                }).ToList();

            return xmlHelper.Serialize(products, "Products");
        }

        //Exercise 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new ExportUserDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProductsSold = u.ProductsSold
                        .Select(p => new ExportProductDto()
                        {
                            Name = p.Name,
                            Price = p.Price
                        }).ToList()
                }).ToList();

            return xmlHelper.Serialize(users, "Users");
        }

        //Exercise 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var categories = context.Categories
                .Select(c => new ExportCategoryDto()
                {
                    Name = c.Name,
                    CountProducts = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(c => c.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(c => c.Product.Price)
                })
                .OrderByDescending(c => c.CountProducts)
                .ThenBy(c => c.TotalRevenue)
                .ToList();

            return xmlHelper.Serialize(categories, "Categories");
        }

        //Exercise 8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var xmlHelper = new XmlHelper();

            //var users = context.Users
            //    .Where(u => u.ProductsSold.Any())
            //    .Select(u => new ExportUserDto()
            //    {
            //        FirstName = u.FirstName,
            //        LastName = u.LastName,
            //        Age = u.Age,
            //        ProductsSold = u.ProductsSold
            //            .Select(p => new ExportProductDto()
            //            {
            //                Name = p.Name,
            //                Price = p.Price
            //            }).ToList()
            //    })
            //    .OrderByDescending(u => u.ProductsSold.Count)
            //    .ToList();

            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count)
                .Take(10)
                .Select(u => new ExportUserSoldProductsDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportSoldProductsDto()
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                            .Select(p => new ExportProductDto()
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToList()
                    }
                })
                .ToList();

            var output = new ExportCountUsersDto()
            {
                Count = context.Users
                    .Where(u => u.ProductsSold.Any())
                    .Count(),
                Users = users
            };

            return xmlHelper.Serialize(output, "Users");
        }

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }
    }
}