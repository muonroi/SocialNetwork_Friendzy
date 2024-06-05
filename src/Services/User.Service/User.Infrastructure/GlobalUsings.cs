global using AutoMapper;
global using Contracts.Commons.Constants;
global using Contracts.Commons.Interfaces;
global using Contracts.Domains.Interfaces;
global using Contracts.Services;
global using Dapper.Extensions;
global using Infrastructure.Commons;
global using Infrastructure.Extensions;
global using Infrastructure.Helper;
global using Infrastructure.ORMs.Dapper;
global using Infrastructure.Services;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Newtonsoft.Json;
global using System;
global using System.Collections.Generic;
global using System.Data;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using User.Application.Commons;
global using User.Application.Commons.Interfaces;
global using User.Application.Commons.Models;
global using User.Domain.Entities;
global using User.Infrastructure.Persistence;
global using User.Infrastructure.Persistence.EntitiesConfigure;
global using User.Infrastructure.Repository;
global using ILogger = Serilog.ILogger;