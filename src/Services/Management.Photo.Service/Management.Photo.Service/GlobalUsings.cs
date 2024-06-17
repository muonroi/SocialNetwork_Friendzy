global using Commons.Logging;
global using Dapper.Extensions;
global using Dapper.Extensions.MSSQL;
global using Infrastructure.Configurations;
global using Infrastructure.Extensions;
global using Infrastructure.Middleware;
global using Infrastructure.ORMs.Dapper;
global using Management.Photo.Application;
global using Management.Photo.Application.Commons.Models;
global using Management.Photo.Application.Extensions;
global using Management.Photo.Application.Feature.v1.Commands.ImportMultipleResource;
global using Management.Photo.Application.Feature.v1.Commands.ImportResoure;
global using Management.Photo.Application.Feature.v1.Queries.GetResource;
global using Management.Photo.Application.Feature.v1.Queries.GetResourceById;
global using Management.Photo.Infrastructure;
global using Management.Photo.Infrastructure.Persistence;
global using Management.Photo.Service.Extension;
global using Management.Photo.Service.Infrastructures;
global using Management.Photo.Service.Infrastructures.Endpoints;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Serilog;
global using Shared.SeedWorks;
global using System.Net;