using Application.Commands;
using Application.Commands.Interfaces;
using Application.Commands.Mediator;
using Application.DTO.Response;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient(typeof(IValidator<ProductionPlanCommand>), typeof(ProductionPlanCommandValidator));
builder.Services.AddTransient(typeof(ICommandHandler<ProductionPlanCommand, List<PowerPlantResult>>), typeof(ProductionPlanCommandHandler));
builder.Services.AddTransient(typeof(IMediator), typeof(Mediator));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
