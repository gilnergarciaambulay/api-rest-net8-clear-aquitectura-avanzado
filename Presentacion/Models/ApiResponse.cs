namespace Presentacion.Models
{
    public record ApiResponse<T>(
        string Message,
        T Results,
        bool Confirmation
    );
}
