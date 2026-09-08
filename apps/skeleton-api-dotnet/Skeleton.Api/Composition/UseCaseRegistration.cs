using Skeleton.Core.UseCases.Widgets.Attachments.Get;
using Skeleton.Core.UseCases.Widgets.Attachments.Upload;
using Skeleton.Core.UseCases.Widgets.Create;
using Skeleton.Core.UseCases.Widgets.Delete;
using Skeleton.Core.UseCases.Widgets.GetById;
using Skeleton.Core.UseCases.Widgets.List;
using Skeleton.Core.UseCases.Widgets.Update;

namespace Skeleton.Api.Composition;

public static class UseCaseRegistration
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateWidgetHandler, CreateWidgetHandler>();
        services.AddScoped<IGetWidgetByIdHandler, GetWidgetByIdHandler>();
        services.AddScoped<IListWidgetsHandler, ListWidgetsHandler>();
        services.AddScoped<IUpdateWidgetHandler, UpdateWidgetHandler>();
        services.AddScoped<IDeleteWidgetHandler, DeleteWidgetHandler>();
        services.AddScoped<IUploadWidgetAttachmentHandler, UploadWidgetAttachmentHandler>();
        services.AddScoped<IGetWidgetAttachmentHandler, GetWidgetAttachmentHandler>();

        return services;
    }
}
