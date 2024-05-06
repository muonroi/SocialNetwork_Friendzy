global using MediatR;
global using Shared.SeedWorks;
global using ExternalAPI.DTOs;
global using Grpc.Net.ClientFactory;
global using static User.Config.Service.Protos.ApiConfigGrpc;
global using User.Config.Service.Protos;
global using Microsoft.AspNetCore.Mvc;
global using Distance.Service.Protos;
global using SearchPartners.Service;
global using FluentValidation;
global using SearchPartners.Aggregate.Service.Services.v1.Query.SearchPartners;
global using SearchPartners.Aggregate.Service.Infrastructure.Behaviours;
global using System.Diagnostics;
global using System.Reflection;
global using Newtonsoft.Json;
global using Infrastructure.Extensions;
global using Newtonsoft.Json.Converters;
global using Serilog;
global using SearchPartners.Aggregate.Service.Extensions;
global using static Distance.Service.Protos.DistanceService;
global using ExternalAPI;
global using Contracts.Commons.Constants;
global using Contracts.Commons.Interfaces;
global using static SearchPartners.Service.SearchPartnerService;
global using Infrastructure.Configurations;
global using SearchPartners.Aggregate.Service;
global using Infrastructure.Commons;
global using SearchPartners.Aggregate.Service.Services.v1.ApiConfigService;
global using Commons.Logging;
global using Calzolari.Grpc.AspNetCore.Validation.Internal;
global using SearchPartners.Aggregate.Service.Infrastructure.ErrorMessages;
global using SearchPartners.Aggregate.Service.Infrastructure.Endpoints;
global using System.Net.Http.Headers;
