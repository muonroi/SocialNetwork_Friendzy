global using API.Intergration.Config.Service.Protos;
global using Authenticate.Verify.Service;
global using AutoMapper;
global using Calzolari.Grpc.AspNetCore.Validation.Internal;
global using Commons.Pagination;
global using Consul;
global using Contracts.Commons.Constants;
global using Contracts.Commons.Interfaces;
global using Contracts.DTOs.ResourceDTOs;
global using Contracts.Services;
global using Grpc.Net.ClientFactory;
global using Infrastructure.Commons;
global using Infrastructure.Extensions;
global using Infrastructure.Factorys;
global using Infrastructure.Helper;
global using Infrastructure.Middleware;
global using Management.Photo.Application.Commons.Interfaces;
global using Management.Photo.Application.Commons.Models;
global using Management.Photo.Application.Commons.Requests;
global using Management.Photo.Application.Extensions;
global using Management.Photo.Application.Feature.v1.ApiConfigService;
global using Management.Photo.Domain.Entities;
global using MediatR;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Shared.Enums;
global using Shared.SeedWorks;
global using Shared.Services.Resources;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Reflection;
global using static API.Intergration.Config.Service.Protos.ApiConfigGrpc;
global using static Authenticate.Verify.Service.AuthenticateVerify;
global using ILogger = Serilog.ILogger;global using Shared.Models;
