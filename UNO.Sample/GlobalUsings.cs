global using System.Collections.Immutable;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using UNO.Sample.Models;
global using UNO.Sample.DataContracts;
global using UNO.Sample.DataContracts.Serialization;
global using UNO.Sample.Services.Caching;
global using UNO.Sample.Services.Endpoints;
#if MAUI_EMBEDDING
global using UNO.Sample.MauiControls;
#endif
global using ApplicationExecutionState = Windows.ApplicationModel.Activation.ApplicationExecutionState;
[assembly: Uno.Extensions.Reactive.Config.BindableGenerationTool(3)]
