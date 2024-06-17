global using API.Intergration.Config.Service.Protos;
global using Authenticate.Verify.Service;
global using AutoMapper;
global using Calzolari.Grpc.AspNetCore.Validation.Internal;
global using Commons.Pagination;
global using Consul;
global using Contracts.Commons.Constants;
global using Contracts.Commons.Interfaces;
global using ExternalAPI;
global using ExternalAPI.Models;
global using Grpc.Net.ClientFactory;
global using Infrastructure.Commons;
global using Infrastructure.Extensions;
global using Infrastructure.Factorys;
global using Infrastructure.Helper;
global using Infrastructure.Middleware;
global using Management.Friends.Action.Application.Commons.Interfaces;
global using Management.Friends.Action.Application.Commons.Models;
global using Management.Friends.Action.Application.Feature.v1.ApiConfigService;
global using Management.Friends.Action.Application.Feature.v1.Query.GetFriendsActionByUserQuery;
global using Management.Friends.Action.Application.Infrastructure.Helpers;
global using Management.Friends.Action.Application.Messages;
global using Management.Friends.Action.Domain.Entities;
global using Matched.Friend.Domain.Infrastructure.Enums;
global using MediatR;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Shared.DTOs;
global using Shared.Resources;
global using Shared.SeedWorks;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Reflection;
global using static API.Intergration.Config.Service.Protos.ApiConfigGrpc;
global using static Authenticate.Verify.Service.AuthenticateVerify;
global using ILogger = Serilog.ILogger;