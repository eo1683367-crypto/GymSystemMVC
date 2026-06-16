using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemMVC.BLL.Common
{

    // If There Isn't Any Value To Return
    public record Result(bool Success , string? Error = null ,ResultKind Kind = ResultKind.Ok )
    {
        public static Result Ok() => new Result(true);
        public static Result Fail(string errorMessage, ResultKind kind = ResultKind.Conflict) => new Result(false, errorMessage, kind);
        public static Result NotFound(string errorMessage = "Not Found") => new Result(false, errorMessage, ResultKind.NotFound);
        public static Result Validation(string errorMessage) => new Result(false, errorMessage, ResultKind.ValidationFailed);

    }

    // To Return Value
    public record Result<T>(bool Success, T? Value, string? Error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new (true, value);
        public static Result<T> Fail(string errorMessage, ResultKind kind = ResultKind.Conflict) => new (false,default ,errorMessage, kind);
        public static Result<T> NotFound(string errorMessage = "Not Found") => new (false,default ,errorMessage, ResultKind.NotFound);

    }


    
    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        ValidationFailed,
        Forbidden,
    }
}
