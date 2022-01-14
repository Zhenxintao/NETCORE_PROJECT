using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using THMS.Core.API.Configuration;
using THMS.Core.API.ExceptionFilter;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.Common;

namespace THMS.Core.API
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 注册Hangfire服务
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(ConfigAppsetting.SqlServerConfig, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();
            services.AddMvc();
            services.AddMvc().AddJsonOptions(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //配置序列化时时间格式为yyyy-MM-dd HH:mm:ss
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //配置大小写问题，默认是首字母小写
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();

            });
            services.AddSwaggerGen(c =>
            {
                //配置hangFire定时任务路径

                string hangFireUrl = ConfigAppsetting.HangFireUrl;
                c.SwaggerDoc("XlinkSystem", new Info
                {
                    Version = "XlinkSystem",
                    Title = "Xlink系统 API",
                    Description = "XlinkSystem ASP.NET Core Web API ",
                    TermsOfService = "None",
                    Contact = new Contact { 
                    Name = "HangFire定时任务",
                    Url = $"{hangFireUrl}/hangfire/"
                    }
                });
                c.SwaggerDoc("IndoorSystem", new Info
                {
                    Version = "IndoorSystem",
                    Title = "室内测温系统 API",
                    Description = "IndoorSystem ASP.NET Core Web API ",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "HangFire定时任务",
                        Url = $"{hangFireUrl}/hangfire/"
                    }
                });
                c.SwaggerDoc("NetBalance", new Info
                {
                    Version = "NetBalance",
                    Title = "二网平衡系统 API",
                    Description = "NetBalance ASP.NET Core Web API ",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "HangFire定时任务",
                        Url = $"{hangFireUrl}/hangfire/"
                    }
                });
                c.SwaggerDoc("PVSS", new Info
                {
                    Version = "PVSS",
                    Title = "PVSS数据同步 API",
                    Description = "PVSS ASP.NET Core Web API ",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "HangFire定时任务",
                        Url = $"{hangFireUrl}/hangfire/"
                    }
                });
                c.SwaggerDoc("MultiStation", new Info
                {
                    Version = "MultiStation",
                    Title = "多站参数 API",
                    Description = "MultiStation ASP.NET Core Web API ",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "HangFire定时任务",
                        Url = $"{hangFireUrl}/hangfire/"
                    }
                });
                c.SwaggerDoc("ForecastSystem", new Info
                {
                    Version = "ForecastSystem",
                    Title = "热网负荷预测 API",
                    Description = "ForecastSystem ASP.NET Core Web API ",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "HangFire定时任务",
                        Url = $"{hangFireUrl}/hangfire/"
                    }
                });
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    var versions = methodInfo.DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<ApiExplorerSettingsAttribute>()
                    .Select(attr => attr.GroupName);
                    if (docName.ToLower() == "v1" && versions.FirstOrDefault() == null)
                    {
                        return true;//无绑定ApiExplorerSettingsAttribute的将在v1中显示
                    }
                    return versions.Any(v => v.ToString() == docName);

                });
                //为Swagger的JSON和UI设置XML注释
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "THMS.Core.API.xml");
                c.IncludeXmlComments(xmlPath, true);
                //var xmlModelPath = Path.Combine(basePath, "ApiModel.xml");
                //c.IncludeXmlComments(xmlModelPath);
            });

            //1、注册服务Swagger
            services.AddSwaggerGen(options =>
            {
                #region 启用swagger验证功能
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称一致即可，CoreAPI。
                var security = new Dictionary<string, IEnumerable<string>> { { "CoreAPI", new string[] { } }, };
                options.AddSecurityRequirement(security);
                options.AddSecurityDefinition("CoreAPI", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion

            });
            //添加cors 服务 配置跨域处理            
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });

            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();//注册ISchedulerFactory的实例。
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            })
 .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMvc(opt =>
            {
                opt.Filters.Add(new ProducesAttribute("application/json"));
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<JwtSetting>(Configuration.GetSection("JwtSetting"));

            var jwtSetting = new JwtSetting();
            Configuration.Bind("JwtSetting", jwtSetting);

            services.AddScoped<ITokenService, TokenService>();

            //添加策略鉴权模式
            services.AddAuthorization()
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            //Token验证
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(600),

                    ValidateAudience = true,//是否验证Audience
                                            //ValidAudience = Const.GetValidudience(),//Audience
                                            //这里采用动态验证的方式，在重新登陆时，刷新token，旧token就强制失效了
                    AudienceValidator = (m, n, z) =>
                    {
                        return m != null && m.FirstOrDefault().Equals(jwtSetting.Audience);
                    },
                    ValidateIssuer = true,//是否验证Issuer
                    ValidIssuer = jwtSetting.Issuer,//Issuer，这两项和前面签发jwt的设置一致

                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecurityKey))//拿到SecurityKey
                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();//jwt验证服务，Token

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger(
                //c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; }
                );
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/XlinkSystem/swagger.json", "Xlink系统 API");
                c.SwaggerEndpoint("/swagger/IndoorSystem/swagger.json", "室内测温系统 API");
                c.SwaggerEndpoint("/swagger/NetBalance/swagger.json", "二网平衡系统 API");
                c.SwaggerEndpoint("/swagger/PVSS/swagger.json", "PVSS数据同步 API");
                c.SwaggerEndpoint("/swagger/MultiStation/swagger.json", "多站参数纵览 API");
                c.SwaggerEndpoint("/swagger/ForecastSystem/swagger.json", "热网负荷预测 API");
                c.RoutePrefix = string.Empty;
            });
            app.UseRequestResponseLogging();
            //配置Hangfire定时任务
            app.UseHangfireDashboard();
            //配置Cors
            app.UseCors("any");
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
