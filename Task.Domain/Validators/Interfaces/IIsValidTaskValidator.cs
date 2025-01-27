using Task.CrossCutting.Validator;

namespace Task.Domain.Validators.Interfaces;

public interface IIsValidTaskValidator: IValidator<Entities.Task>
{
    
}