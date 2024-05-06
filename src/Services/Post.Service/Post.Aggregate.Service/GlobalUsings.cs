global using Contracts.Commons.Constants;
global using Grpc.Net.ClientFactory;
global using static User.Config.Service.Protos.ApiConfigGrpc;
global using User.Config.Service.Protos;
global using Shared.SeedWorks;
global using Post.Aggregate.Service.Infrastructure.Helpers;
global using System.Net;
global using Shared.DTOs;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using FluentValidation;
global using Post.API.Protos;
global using Post.Aggregate.Service.Services.v1.Query.GetPosts;
global using Commons.Pagination;
global using System.Diagnostics;
global using Post.Aggregate.Service.Infrastructure.Behaviours;
global using System.Reflection;
global using ExternalAPI.DTOs;
global using Post.Aggregate.Service;
global using Newtonsoft.Json.Converters;
global using Serilog;
global using Infrastructure.Extensions;
global using Post.Aggregate.Service.Extensions;
global using static Post.API.Protos.PostApiService;
global using System.Net.Http.Headers;
global using Consul;
global using Infrastructure.Factorys;
global using ExternalAPI;
global using Contracts.Commons.Interfaces;
global using Post.Aggregate.Service.Infrastructure.Endpoints;
global using Commons.Logging;
global using Newtonsoft.Json;
global using Post.Aggregate.Service.Services.v1.ApiConfigService;
global using Infrastructure.Commons;
global using Infrastructure.Configurations;
global using Post.Aggregate.Service.Infrastructure.ErrorMessages;
global using Calzolari.Grpc.AspNetCore.Validation.Internal;
