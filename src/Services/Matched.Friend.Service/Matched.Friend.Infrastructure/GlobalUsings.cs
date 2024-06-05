global using Commons.Pagination;
global using Contracts.Commons.Constants;
global using Contracts.Commons.Interfaces;
global using Contracts.Domains.Interfaces;
global using Dapper.Extensions;
global using Infrastructure.Commons;
global using Infrastructure.Extensions;
global using Infrastructure.Helper;
global using Infrastructure.ORMs.Dapper;
global using Matched.Friend.Application.Commons.Interfaces;
global using Matched.Friend.Application.Commons.Models;
global using Matched.Friend.Domain.Entities;
global using Matched.Friend.Domain.Infrastructure.Enums;
global using Matched.Friend.Infrastructure.Persistence;
global using Matched.Friend.Infrastructure.Persistence.EntitiesConfigure;
global using Matched.Friend.Infrastructure.Persistence.Query;
global using Matched.Friend.Infrastructure.Repository;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using MySql.EntityFrameworkCore.Extensions;
global using Newtonsoft.Json;
global using Serilog;
global using System;