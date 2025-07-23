
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.EndpointFilters;
using MinimalAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MinimalAPI.RouteGroups
{
    public static class ProductsMapGroup
    {
        private static List<Product> products = new List<Product>() {
   new Product() { Id = 1, ProductName = "Smart Phone" },
   new Product() { Id = 2, ProductName = "Smart TV" }
  };

        public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (HttpContext context) =>
            {
                return Results.Ok(products);
            });

            group.MapGet("/{id:int}", async (HttpContext context, int id) =>
            {
                Product? product = products.FirstOrDefault(temp => temp.Id == id);
                if (product == null)
                {
                    return Results.BadRequest(new { error = "Incorrect Product ID" });
                }

                return Results.Ok(product);
            });

            group.MapPost("/", (HttpContext context, Product product) =>
            {
                products.Add(product);

                return Results.Ok(new { message = "Product Added" });
            })
             .AddEndpointFilter<CustomEndpointFilter>()

             .AddEndpointFilter(async (EndpointFilterInvocationContext context, EndpointFilterDelegate next) =>
             {
                 var product = context.Arguments.OfType<Product>().FirstOrDefault();

                 if (product == null)
                 {
                     return Results.BadRequest("Product details are not found in the request");
                 }

                 var validationContext = new ValidationContext(product);
                 List<ValidationResult> errors = new List<ValidationResult>();
                 bool isValid = Validator.TryValidateObject(product, validationContext, errors, true);

                 if (!isValid)
                 {
                     return Results.BadRequest(errors.FirstOrDefault()?.ErrorMessage);
                 }

                 var result = await next(context);

                 return result;
             })
             ;

            group.MapPut("/{id}", async (HttpContext context, int id, [FromBody] Product product) =>
            {
                Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
                if (productFromCollection == null)
                {
                    return Results.BadRequest(new { error = "Incorrect Product ID" });
                }

                productFromCollection.ProductName = product.ProductName;

                return Results.Ok(new { message = "Product Updated" });
            });

            group.MapDelete("/{id}", async (HttpContext context, int id) =>
            {
                Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
                if (productFromCollection == null)
                {
                    return Results.ValidationProblem(
                     new Dictionary<string, string[]> {
       {  "id", new  string[] { "Incorrect Product ID" } }
                     });
                }

                products.Remove(productFromCollection);

                return Results.Ok(new { message = "Product Deleted" });
            });

            return group;
        }
    }
}
