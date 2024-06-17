global using Commons.Logging;
global using Dapper.Extensions;
global using Dapper.Extensions.MSSQL;
global using Infrastructure.Configurations;
global using Infrastructure.Extensions;
global using Infrastructure.Middleware;
global using Infrastructure.ORMs.Dapper;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Serilog;
global using Shared.SeedWorks;
global using System;
global using System.Net;
global using System.Threading.Tasks;
global using User.Application;
global using User.Application.Commons.Models;
global using User.Application.Extensions;
global using User.Application.Feature.v1.Users.Commands.UserLoginCommand;
global using User.Application.Feature.v1.Users.Commands.UserRegisterCommand;
global using User.Application.Feature.v1.Users.Commands.UserUpdateInfoCommand;
global using User.Application.Feature.v1.Users.Queries.GetMultipleUsersQuery;
global using User.Application.Feature.v1.Users.Queries.GetUserQuery;
global using User.Application.Infrastructure;
global using User.Infrastructure;
global using User.Infrastructure.Persistence;
global using User.Service.Extensions;
global using User.Service.Infrastructures;
global using User.Service.Infrastructures.Endpoints;