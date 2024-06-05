global using Contracts.Commons.Interfaces;
global using Contracts.Domains.Interfaces;
global using Contracts.Services;
global using Dapper.Extensions;
global using Dapper.Extensions.MSSQL;
global using Infrastructure.Commons;
global using Infrastructure.Extensions;
global using Infrastructure.ORMs.Dapper;
global using Infrastructure.Services;
global using Management.Photo.Application.Commons.Interfaces;
global using Management.Photo.Application.Commons.Models;
global using Management.Photo.Application.Commons.Requests;
global using Management.Photo.Domain.Entities;
global using Management.Photo.Infrastructure.Persistence;
global using Management.Photo.Infrastructure.Persistence.EntitiesConfigure;
global using Management.Photo.Infrastructure.Persistence.Query;
global using Management.Photo.Infrastructure.Query;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Newtonsoft.Json;
global using Serilog;
global using Shared.Enums;
global using System;
global using System.Data;global using Infrastructure.Helper;
global using Contracts.Commons.Constants;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Dapper;