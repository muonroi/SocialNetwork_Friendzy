global using Commons.Logging;
global using Contracts.Commons.Interfaces;
global using Contracts.Domains;
global using Contracts.Domains.Interfaces;
global using Dapper.Extensions;
global using Dapper.Extensions.MSSQL;
global using Distance.Service.Domains;
global using Distance.Service.Extensions;
global using Distance.Service.Infrastructure;
global using Distance.Service.Infrastructure.Interface;
global using Distance.Service.Infrastructure.Query;
global using Distance.Service.Models;
global using Distance.Service.Persistences;
global using Distance.Service.Persistences.Configures;
global using Distance.Service.Protos;
global using Distance.Service.Services;
global using Grpc.Core;
global using Infrastructure.Commons;
global using Infrastructure.Extensions;
global using Infrastructure.Helper;
global using Infrastructure.ORMs.Dapper;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Newtonsoft.Json;
global using Serilog;
global using System.Data;
global using static Distance.Service.Protos.DistanceService;
global using ILogger = Serilog.ILogger;