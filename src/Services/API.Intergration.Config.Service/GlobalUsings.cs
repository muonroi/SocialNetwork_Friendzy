global using API.Intergration.Config.Service.Extensions;
global using API.Intergration.Config.Service.Protos;
global using API.Intergration.Config.Service.v1.DTOs;
global using API.Intergration.Config.Service.v1.Query;
global using API.Intergration.Config.Service.v1.Services;
global using Commons.Logging;
global using Contracts.Commons.Interfaces;
global using Dapper.Extensions;
global using Dapper.Extensions.PostgreSql;
global using Grpc.Core;
global using Infrastructure.Configurations;
global using Infrastructure.Extensions;
global using Infrastructure.ORMs.Dapper;
global using Serilog;
global using System.Text.Json;
global using static API.Intergration.Config.Service.Protos.ApiConfigGrpc;
