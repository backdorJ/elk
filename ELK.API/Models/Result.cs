namespace ELK.API.Models;

/// <summary>
/// Паттерн Result
/// </summary>
public class Result<TResult>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="value">Результат</param>
    /// <param name="statusCode">Статус</param>
    private Result(TResult value, int statusCode)
    {
        Value = value;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="errorMessage">Сообщение об ошибки</param>
    /// <param name="statusCode">Статус ошибки</param>
    private Result(string errorMessage, int statusCode = StatusCodes.Status500InternalServerError)
    {
        Value = default;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
    
    /// <summary>
    /// Результат
    /// </summary>
    public TResult? Value { get; }
    
    /// <summary>
    /// Успешно
    /// </summary>
    public bool IsSuccess => StatusCode is >= 200 and < 300;

    /// <summary>
    /// Статус код
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Сообщение об ошибки
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Успешно
    /// </summary>
    /// <param name="value">Результат</param>
    /// <param name="statusCode">Статус</param>
    /// <returns>Результат</returns>
    public static Result<TResult> Success(TResult value, int statusCode = StatusCodes.Status200OK)
        => new(value, statusCode);
    
    /// <summary>
    /// Ошибка
    /// </summary>
    /// <param name="statusCode">Статус</param>
    /// <param name="errorMessage">Сообщение об ошибки</param>
    /// <returns>Ответ</returns>
    public static Result<TResult> Failure(
        string errorMessage,
        int statusCode = StatusCodes.Status500InternalServerError)
        => new(errorMessage, statusCode);
}

/// <summary>
/// Результат операции без возвращаемого значения.
/// </summary>
public class Result
{
    /// <summary>
    /// Конструктор для успешного результата.
    /// </summary>
    /// <param name="statusCode">Код успешного статуса.</param>
    private Result(int statusCode)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// Конструктор для ошибки.
    /// </summary>
    /// <param name="errorMessage">Сообщение об ошибке.</param>
    /// <param name="statusCode">Код ошибки.</param>
    private Result(string errorMessage, int statusCode)
    {
        ErrorMessage = errorMessage;
        StatusCode = statusCode;
    }
    
    /// <summary>
    /// Успешно ли выполнена операция.
    /// </summary>
    public bool IsSuccess => StatusCode is >= 200 and < 300;

    /// <summary>
    /// Статус-код результата.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Сообщение об ошибке, если операция завершилась с ошибкой.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Успешный результат.
    /// </summary>
    /// <param name="statusCode">Статус-код (по умолчанию 200).</param>
    public static Result Success(int statusCode = StatusCodes.Status200OK)
        => new(statusCode);

    /// <summary>
    /// Ошибочный результат.
    /// </summary>
    /// <param name="errorMessage">Сообщение об ошибке.</param>
    /// <param name="statusCode">Статус-код (по умолчанию 500).</param>
    public static Result Failure(string errorMessage, int statusCode = StatusCodes.Status500InternalServerError)
        => new(errorMessage, statusCode);
}