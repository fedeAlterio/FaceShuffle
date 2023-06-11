using FaceShuffle.Web.DependenciesInstallers;
using FaceShuffle.Web.Endpoints;
using FaceShuffle.Web.Endpoints.Generics.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAppAuthentication(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(AuthorizedWebRequestHandler<,>), typeof(AuthorizedWebRequestHandler<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapEndpoints();
await app.RunAsync();