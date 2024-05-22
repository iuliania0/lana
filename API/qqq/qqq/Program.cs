using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using qqq;
using qqq.umiheeva;

var builder = WebApplication.CreateBuilder();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", options => { });
builder.Services.AddCors();
builder.Services.AddSwaggerGen(options =>
{ 
    options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Description = "Введите логин и пароль",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Basic"
    });    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lana");
});

app.MapGet("/", () => "Общедоступный ресурс")
    .WithTags("-")
    .AllowAnonymous();

app.MapPost("/auth/login",
    async (context) =>
    {
        var formData = await context.Request.ReadFormAsync();

        var login = formData["name"].ToString();
        var password = formData["password"].ToString();

        var contextCustomer = new umiheeva_onlineshopContext();

        Customer customer = contextCustomer.Customers.FirstOrDefault(x => x.Name == login);

        if (customer != null && password == customer.Password)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(customer);
        }
        else
            context.Response.StatusCode = 401;
    })
    .WithTags("-")
    .AllowAnonymous();

app.MapPost("/auth/register", (Customer postCustomer) =>
    {
        using (var context = new umiheeva_onlineshopContext())
        {
            context.Add(postCustomer);
            context.SaveChanges();
            return context.Customers.Find(postCustomer.Id);
        }
    })
    .AllowAnonymous()
    .WithTags("-")
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status200OK);

app.MapGet("/customer/{id:int}", (int id) =>
    {
        using (var context = new umiheeva_onlineshopContext())
        {
            var customer = context.Customers
                .Include(u => u.Orders)
                .ThenInclude(o => o.OrderProducts)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(u => u.Id == id);
        }
    }).RequireAuthorization()
    .WithTags("-")
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status200OK);

app.MapPut("/customer/{id:int}", 
        (int id, Customer postCustomer) =>
        {
            using (var context = new umiheeva_onlineshopContext())
            {
                var customer = context.Customers.Find(id);
                if (customer == null)
                    return Results.NotFound();

                customer.Name = postCustomer.Name;

                context.Update(customer);
                context.SaveChanges();
            }

            return Results.Ok();
        })
    .RequireAuthorization()
    .WithTags("-")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status404NotFound);

app.MapDelete("/customer/{id:int}",
        (int id) =>
        {
            using (var context = new umiheeva_onlineshopContext())
            {
                var customer = context.Customers.Find(id);
                if (customer == null)
                    return Results.NotFound();

                context.Customers.Remove(customer);
                context.SaveChanges();
            }

            return Results.Ok();
        }).RequireAuthorization()
    .WithTags("-")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status404NotFound);

app.UseAuthentication();
app.UseAuthorization();
app.Run();