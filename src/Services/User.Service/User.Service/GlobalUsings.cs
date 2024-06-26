global using Commons.Logging;
global using Dapper.Extensions.MSSQL;
global using Infrastructure.Extensions;
global using MediatR;
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
global using User.Infrastructure;
global using User.Infrastructure.Persistence;