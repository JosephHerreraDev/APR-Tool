decimal amountFinanced = 1000M; // Total a pagar
decimal estimatedAPR = 1M; // Estimacion inicial APR
decimal monthlyPayment = 33.61M; // Cantidad a pagar
int numberOfPayments = 36; // Number of payments
int periodsPerYear = GetPeriodsPerYear("monthly"); // Numero de periodos año

decimal monthlyRate = 0M; // Tasa de interés mensual
decimal finalTotalPayment = 0M; // Total para revision final

// Función para determinar períodos por año según el tipo de duración.
int GetPeriodsPerYear(string durationType)
{
    return durationType switch
    {
        "monthly" => 12,
        "semiMonthly" => 24,
        "weekly" => 52,
        "biweekly" => 26,
        _ => throw new ArgumentException("Unknown duration type")
    };
}

// Calcular la tarifa mensual
void CalculateMonthlyRate()
{
    monthlyRate = Math.Round(estimatedAPR / (periodsPerYear * 100), 9);
}

// Calcular el resultado
decimal CalculateResult()
{
    decimal result = 0M;
    CalculateMonthlyRate();

    for (int i = 1; i <= numberOfPayments; i++)
    {
        result += monthlyPayment / (decimal)Math.Pow((double)(1M + monthlyRate), i);
    }
    return result;
}

// Actualizar estimado de APR
void UpdateAPR()
{
    decimal initialResult = CalculateResult();
    estimatedAPR += 0.1M;
    decimal updatedResult = CalculateResult();

    estimatedAPR -= 0.1M; // Revertir el incremento de la APR para volver a calcularlo
    estimatedAPR += 0.1M * ((amountFinanced - initialResult) / (updatedResult - initialResult));
}

// Actualiza iterativamente el APR hasta que el pago total final coincida con el monto financiado
while (Math.Round(finalTotalPayment, 2) != amountFinanced)
{
    UpdateAPR();
    finalTotalPayment = CalculateResult();
}

Console.WriteLine($"APR: {Math.Round(estimatedAPR, 2)}%");

// Imprimir el importe final financiado
decimal finalAmountFinanced = 0M;
CalculateMonthlyRate();

for (int i = 1; i <= numberOfPayments; i++)
{
    finalAmountFinanced += monthlyPayment / (decimal)Math.Pow((double)(1M + estimatedAPR / 100 / periodsPerYear), i);
    finalAmountFinanced = Math.Round(finalAmountFinanced, 1);
}

Console.WriteLine($"Amount financed: {finalAmountFinanced}");
