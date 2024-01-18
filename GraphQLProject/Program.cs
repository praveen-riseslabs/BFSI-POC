using GraphQL.Types;
using GraphQL;
using GraphQLProject.Interfaces;
using GraphQLProject.Services;
using GraphQLProject.Type;
using GraphQLProject.Query;
using GraphQLProject.Schema;
using GraphQLProject.Mutations;
using GraphiQl;
using Microsoft.AspNetCore.Server.Kestrel.Core;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMenuRepository, MenuRepository>();
builder.Services.AddTransient<MenuType>();
builder.Services.AddTransient<MenuQuery>();
builder.Services.AddTransient<MenuMutation>();
builder.Services.AddTransient<MenuInputType>();
builder.Services.AddTransient<ISchema, MenuSchema>();
builder.Services.AddGraphQL(b => b.AddAutoSchema<ISchema>().AddSystemTextJson());
//builder.services.AddSingleton<ISchema, MenuSchema>();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.UseGraphiQl("/graphql");
app.UseGraphQL<ISchema>();
app.UseAuthorization();

app.MapControllers();

app.Run();
