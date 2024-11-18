using System.Security.Claims;
using System.Text;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhoneVault.Data;
using PhoneVault.Enums;
using PhoneVault.Models;
using PhoneVault.Repositories;
using PhoneVault.Services;

const string firebaseProjectId = "phone-vault-2438d";

var builder = WebApplication.CreateBuilder(args);

string? firebaseJson = null;
var firebaseEnvVar = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
var firebaseConfig = builder.Configuration["Firebase:Credentials"];
if (firebaseEnvVar is not null)
{
    firebaseJson = File.ReadAllText(firebaseEnvVar);
}
else if(firebaseConfig is not null)
{
    firebaseJson = firebaseConfig;
}
if (firebaseJson is null)
{
    throw new ArgumentException("Firebase credentials are missing");
}

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromJson(firebaseJson),
    ProjectId = firebaseProjectId
});

// Add services to the container.
builder.Services.AddControllers();

// Configure SQL Server and EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PhoneVaultContext>(options =>
    options.UseSqlServer(connectionString));

/*builder.Services.AddDbContext<PhoneVaultContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);*/

// Register repositories and services for dependency injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ReviewService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddScoped<IAdminLogRepository, AdminLogRepository>();
builder.Services.AddScoped<AdminLogService>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<PaymentService>();

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<InventoryService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
builder.Services.AddScoped<ShippingService>();

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ShoppingCartService>();

builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<OrderItemService>();

builder.Services.AddScoped<IShoppingCartItemRepository, ShoppingCartItemRepository>();
builder.Services.AddScoped<ShoppingCartItemService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<ImageBlobService>();

// Register Swagger services for API documentation
builder.Services.AddSwaggerGen();

var secret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrEmpty(secret))
{
    throw new ArgumentException("JWT secret is missing");
}

var privateKey = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(privateKey),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    })
    // Google
    .AddJwtBearer("Firebase", options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = firebaseProjectId,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", p => p.RequireRole(UserTypes.Admin));
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
        JwtBearerDefaults.AuthenticationScheme,
        "Firebase");
    defaultAuthorizationPolicyBuilder =
        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});


// Add additional repositories and services as needed
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();

var app = builder.Build();

// Get the database context and apply migrations
// using var context = app.Services.GetRequiredService<PhoneVaultContext>();
// context.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable middleware to serve generated Swagger as a JSON endpoint
    app.UseSwagger();

    // Enable middleware to serve Swagger UI
    app.UseSwaggerUI();
}

app.UseCors(policyBuilder => policyBuilder
    .AllowCredentials()
    .WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

// Middleware to add a Google user to database
app.Use(async (context, next) =>
{
    string? userId = null;
    try
    {
        var user = context.User;
        userId = user.FindFirstValue("user_id");
    }
    catch (Exception e)
    {
        // Do nothing
    }

    if (userId is null)
    {
        await next.Invoke();
        return;
    }

    var db = context.RequestServices.GetRequiredService<PhoneVaultContext>();
    var existingUser = await db.Users.FindAsync(userId);
    if (existingUser is not null)
    {
        await next.Invoke();
        return;
    }

    // Find user on firebase
    var firebaseUser = await FirebaseAuth.DefaultInstance.GetUserAsync(userId);

    if (firebaseUser is not null)
    {
        var newUser = new User
        {
            Id = userId,
            Email = firebaseUser.Email,
            Name = firebaseUser.DisplayName,
            UserType = UserTypes.Customer,
            Password = "",
            Orders = new List<Order>(),
            ShoppingCart = new ShoppingCart(),
            Reviews = new List<Review>(),
            AccountType = UserAccountType.Firebase
        };
        await db.Users.AddAsync(newUser);
        await db.SaveChangesAsync();
    }

    await next.Invoke();
});

app.MapControllers();

app.Run();