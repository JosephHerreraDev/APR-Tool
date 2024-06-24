decimal amountFinanced = 1056M; // Total que va a pagar
decimal estimatedAPR = 1M; // Estimacion inicial de APR
decimal payment = 33.61M; // Cantidad de pago al mes
int numberOfPayments = 36; // Numero de pagos
int periods = 0; //Numero de periodos (meses)

decimal monthRate = 0M; // Estimacion de la tasa al mes
decimal finalTotalPayment = 0; // Total para la revision final

string[] typeOfDuration = new string[] { "monthly", "semiMonthly", "weekly", "biweekly" };

string durationSelection = typeOfDuration[0];

switch (durationSelection)
{
    case "monthly":
        periods = 12;
        break;
    case "semiMonthly":
        periods = 24;        
        break;
    case "weekly":
        periods = 52;
        break;
    case "biweekly":
        periods = 26;
        break;
    default:
        Console.WriteLine("Unknown duration type");
        break;
}

// Sumar 0.1 a la estimacion mensual
decimal Addrate()
{
    return estimatedAPR = Decimal.Add(estimatedAPR, 0.1M);
}

// Calcular la tasa mensual 
void calculateMonthlyRate()
{
    monthRate = Math.Round(estimatedAPR / (periods * 100), 9);
}

// Calcular el resultado
decimal calculateResult()
{
    decimal result = 0M;
    calculateMonthlyRate();

    for (int x = 1; x < numberOfPayments + 1; x++)
    {
        result = Decimal.Add(result, (decimal)((double)payment / Math.Pow((double)(1M + monthRate), x)));
    }
    return result;
}

// Calculo
void calculation()
{
    decimal firstValue = calculateResult();
    decimal firstRate = estimatedAPR;
    Addrate();
    decimal secondValue = calculateResult();
    estimatedAPR = firstRate + (Decimal)0.1 * ((amountFinanced - firstValue) / (secondValue - firstValue));
    finalTotalPayment = calculateResult();
}

// Mientras el resultado no sea el resultado final hacer el calculo
while (Math.Round(finalTotalPayment, 2) != amountFinanced)
{
    calculation();
}

Console.WriteLine("APR: %" + Math.Round(estimatedAPR, 2));

estimatedAPR = Math.Round(estimatedAPR / 100 / 12, 4) ;

decimal result = 0M;

// Imprime lo que deberia de ser el Amount financed total
for (int x = 1; x < numberOfPayments + 1; x++)
{
    result = Decimal.Add(result, (decimal)((double)payment / Math.Pow((double)(1M + estimatedAPR), x)));
    result = Math.Round(result, 1);
}

Console.WriteLine("Amount financed: " + result);