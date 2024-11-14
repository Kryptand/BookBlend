using BookBlend.Api.Entities;
using BookBlend.Api.Shared;
using MediatR;

namespace BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus.Queries;

public class GetConversionStatusQuery:IRequest<Result<ConversionJob>>
{
    public Guid ConversionJobId { get; set; }
}