namespace Signia.Core.CQRS.Saga;

public interface ISaga
{
    ISagaDescriptor[] Setup();
}