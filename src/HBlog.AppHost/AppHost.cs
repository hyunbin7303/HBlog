var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("postgresdb");

var hblogApi = builder
        .AddProject<Projects.HBlog_Api>("chatapi")
        .WithReference(postgresdb).WaitFor(postgresdb)
        .WithOtlpExporter()
        .WithExternalHttpEndpoints();

    hblogApi
        .WithUrlForEndpoint("scalar", (callback) =>
        {
            callback.DisplayText = "Scalar";
            callback.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
            callback.Url += "/scalar/v1";
        });

builder.Build().Run();
