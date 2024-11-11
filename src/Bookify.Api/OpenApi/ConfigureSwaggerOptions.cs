using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bookify.Api.OpenApi;

public sealed class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
	private readonly IApiVersionDescriptionProvider provider;

	public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
	{
		this.provider = provider;
	}

	public void Configure(string? name, SwaggerGenOptions options)
	{
		foreach (var description in provider.ApiVersionDescriptions)
		{
			options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
		}
	}

	public void Configure(SwaggerGenOptions options)
	{
		Configure(options);
	}

	private static OpenApiInfo CreateVersionInfo(ApiVersionDescription apiVersionDescription)
	{
		var openApiInfo = new OpenApiInfo
		{
			Title = $"Bookify.Api v{apiVersionDescription.ApiVersion}",
			Version = apiVersionDescription.ApiVersion.ToString()
		};

		if (apiVersionDescription.IsDeprecated)
		{
			openApiInfo.Description += " This API version has been deprecated.";
		}

		return openApiInfo;
	}
}
