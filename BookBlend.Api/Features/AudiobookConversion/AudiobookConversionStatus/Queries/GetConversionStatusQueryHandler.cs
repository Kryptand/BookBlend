using BookBlend.Api.Database;
using BookBlend.Api.Entities;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;

namespace BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus.Queries;

public class GetConversionStatusQueryHandler : IRequestHandler<GetConversionStatusQuery, Result<ConversionJob>>
{
    private readonly AudiobookDbContext _dbContext;
    private readonly IValidator<GetConversionStatusQuery> _validator;
    
    public GetConversionStatusQueryHandler(AudiobookDbContext dbContext, IValidator<GetConversionStatusQuery> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }
    
    public Task<Result<ConversionJob>> Handle(GetConversionStatusQuery request, CancellationToken cancellationToken)
    {
        var validationResult=_validator.Validate(request);
        
        if (!validationResult.IsValid)
        {
            return Task.FromResult(Result.Failure<ConversionJob>(new Error("GetConversionStatusQuery.Validation",
                validationResult.ToString())));
        }
        
        var conversionJob = _dbContext.ConversionJobs.FirstOrDefault(x => x.Id == request.ConversionJobId);
        
        if (conversionJob == null)
        {
            return Task.FromResult(Result.Failure<ConversionJob>(new Error("GetConversionStatusQuery.Validation", "Conversion job not found")));
        }
        
        return Task.FromResult(Result.Success(conversionJob));
    }
}