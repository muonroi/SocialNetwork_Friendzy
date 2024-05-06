global using AutoMapper;
global using Calzolari.Grpc.AspNetCore.Validation;
global using Contracts.Commons.Configurations;
global using Contracts.Commons.Interfaces;
global using Contracts.Domains;
global using Contracts.Services;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Google.Protobuf.WellKnownTypes;
global using Grpc.Core;
global using Grpc.Core.Interceptors;
global using Grpc.Net.Client.Configuration;
global using Grpc.Net.ClientFactory;
global using Infrastructure.Commons;
global using Infrastructure.Configurations;
global using Infrastructure.Extensions;
global using Infrastructure.Middleware;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Localization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Primitives;
global using Microsoft.Net.Http.Headers;
global using MimeKit;
global using MimeKit.Text;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Converters;
global using Newtonsoft.Json.Serialization;
global using RestEase;
global using Serilog;
global using Shared.Resources;
global using Shared.SeedWorks;
global using Shared.Services.Emails;
global using System.Collections;
global using System.Diagnostics;
global using System.Globalization;
global using System.Linq.Expressions;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using BCryptCore = BCrypt.Net;
global using GrpcCoreInterceptor = Grpc.Core.Interceptors.Interceptor;
global using SmtpClient = MailKit.Net.Smtp.SmtpClient;
global using ILogger = Serilog.ILogger;global using Consul;
global using Infrastructure.Exceptions;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Hosting.Server.Features;
global using System.Text;
global using Shared.DTOs;
global using System.Transactions;
global using Dapper;
global using System.Data.Common;
global using Microsoft.Data.SqlClient;
global using System.Data;
global using Microsoft.OpenApi.Models;
global using System.Security.Cryptography;
global using Microsoft.AspNetCore.Mvc.Routing;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.IdentityModel.Tokens;
global using Infrastructure.Caching.Distributed.Redis;
global using Microsoft.Extensions.Caching.Distributed;
global using Dapper.Extensions;
global using Infrastructure.Security;
global using Grpc.Net.Client;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Infrastructure.ORMs.Dappers.Interfaces;
global using Commons.Pagination;
global using StackExchange.Redis;
global using Dapper.Extensions.Caching.Redis;
global using Microsoft.AspNetCore.Mvc.Abstractions;
global using Microsoft.AspNetCore.Mvc.Infrastructure;
global using Microsoft.AspNetCore.Routing;
global using FreeRedis;