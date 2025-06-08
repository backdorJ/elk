using Elastic.Clients.Elasticsearch;
using ELK.API;
using ELK.API.Models;
using ELK.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton(sp =>
{
    var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
        .DefaultIndex(nameof(Word).ToLowerInvariant())
        .ServerCertificateValidationCallback((o, certificate, chain, errors) => true);

    return new ElasticsearchClient(settings);
});

builder.Services.AddSingleton<IWordElasticSearchService, WordElasticSearchService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
