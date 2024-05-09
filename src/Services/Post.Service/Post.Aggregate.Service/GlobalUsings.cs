global using Calzolari.Grpc.AspNetCore.Validation.Internal;
global using Commons.Behaviours;
global using Commons.Logging;
global using Commons.Pagination;
global using Consul;
global using Contracts.Commons.Constants;
global using Contracts.Commons.Interfaces;
global using ExternalAPI;
global using ExternalAPI.DTOs;
global using FluentValidation;
global using Grpc.Net.ClientFactory;
global using Infrastructure.Commons;
global using Infrastructure.Configurations;
global using Infrastructure.Extensions;
global using Infrastructure.Factorys;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Converters;
global using Post.Aggregate.Service;
global using Post.Aggregate.Service.Extensions;
global using Post.Aggregate.Service.Infrastructure.Endpoints;
global using Post.Aggregate.Service.Infrastructure.ErrorMessages;
global using Post.Aggregate.Service.Infrastructure.Helpers;
global using Post.Aggregate.Service.Services.v1.ApiConfigService;
global using Post.Aggregate.Service.Services.v1.Query.GetPosts;
global using Post.API.Protos;
global using Serilog;
global using Shared.DTOs;
global using Shared.SeedWorks;
global using System.Diagnostics;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Reflection;
global using User.Config.Service.Protos;
global using static Post.API.Protos.PostApiService;
global using static User.Config.Service.Protos.ApiConfigGrpc;