namespace PokedexBackend.Domain.Enums;

public enum EOperationResultCode
{
    Success = 200,
    NotFound = 404,
    ServerError = 500,
    BadRequest = 400, 
}