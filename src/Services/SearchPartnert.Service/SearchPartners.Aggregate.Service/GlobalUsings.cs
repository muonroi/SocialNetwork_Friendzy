global using Calzolari.Grpc.AspNetCore.Validation.Internal;
global using Commons.Logging;
global using Contracts.Commons.Constants;
global using Contracts.Commons.Interfaces;
global using Distance.Service.Protos;
global using ExternalAPI;
global using Grpc.Net.ClientFactory;
global using Infrastructure.Commons;
global using Infrastructure.Configurations;
global using Infrastructure.Extensions;
global using Infrastructure.Factorys;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Converters;
global using SearchPartners.Aggregate.Service.Extensions;
global using SearchPartners.Aggregate.Service.Infrastructure.Endpoints;
global using SearchPartners.Aggregate.Service.Infrastructure.ErrorMessages;
global using SearchPartners.Aggregate.Service.Services.v1.ApiConfigService;
global using SearchPartners.Aggregate.Service.Services.v1.Query.SearchPartners;
global using SearchPartners.Service;
global using Serilog;
global using Shared.DTOs;
global using Shared.SeedWorks;
global using System.Net.Http.Headers;
global using System.Reflection;
global using static Distance.Service.Protos.DistanceService;
global using static SearchPartners.Service.SearchPartnerService;
