using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext dbContext = new ProductShopContext();

            string outputJson = GetUsersWithProducts(dbContext);
            File.WriteAllText("../../../Datasets/categories-by-products.json", outputJson);

            Console.WriteLine(outputJson);
        }

        //Exercise 1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            var userDtos = JsonConvert
                .DeserializeObject<List<ImportUserDto>>(inputJson);

            List<User> users = new List<User>();
            foreach (var userDto in userDtos)
            {
                var user = mapper.Map<User>(userDto);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        //Exercise 2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            var productDtos = JsonConvert
                .DeserializeObject<List<ImportProductDto>>(inputJson);

            var products = mapper.Map<List<Product>>(productDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        //Exercise 3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            var categoryDtos = JsonConvert.DeserializeObject<List<ImportCategoryDto>>(inputJson);
            var categories = new List<Category>();

            foreach (var categoryDto in categoryDtos)
            {
                if (!string.IsNullOrWhiteSpace(categoryDto.Name))
                {
                    var category = mapper.Map<Category>(categoryDto);
                    categories.Add(category);
                }
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //Exercise 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            var categoryProductDtos = JsonConvert
                .DeserializeObject<List<ImportCategoryProductDto>>(inputJson);

            var categoriesProducts = mapper
                .Map<List<CategoryProduct>>(categoryProductDtos);

            context.CategoriesProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

        //Exercise 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();

            return JsonConvert
                .SerializeObject(products, Formatting.Indented);
        }

        //Exercise 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold.Select(ps => new
                    {
                        name = ps.Name,
                        price = ps.Price,
                        buyerFirstName = ps.Buyer.FirstName,
                        buyerLastName = ps.Buyer.LastName
                    }).ToList()
                }).ToList();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        //Exercise 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count())
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count(),
                    averagePrice = $"{c.CategoriesProducts.Average(cp => cp.Product.Price):f2}",
                    totalRevenue = $"{c.CategoriesProducts.Sum(cp => cp.Product.Price):f2}"
                }).ToList();

            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }

        //Exercise 8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold
                            .Count(p => p.Buyer != null),
                        products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            }).ToList()
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToList();

            var toConvert = new
            {
                usersCount = users.Count,
                users = users
            };

            return JsonConvert.SerializeObject(toConvert, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
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