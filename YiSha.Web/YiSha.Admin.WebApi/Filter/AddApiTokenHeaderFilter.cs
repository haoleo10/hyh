using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace YiSha.Admin.WebApi.Filter
{
    /// <summary>
    /// Swagger 全局参数：给每个接口加上 ApiToken 请求头输入框，
    /// 便于在 Swagger 页面（/api-doc）上直接测试需要登录token的接口
    /// </summary>
    public class AddApiTokenHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "ApiToken",
                In = ParameterLocation.Header,
                Required = false,
                Description = "登录token（POST /User/Login 接口返回的 ApiToken）",
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}
