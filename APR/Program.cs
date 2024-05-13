decimal amountFinanced = 1000M; // Total que va a pagar
decimal estimatedAPR = 1M; // Estimacion inicial de APR
decimal payment = 33.61M; // Cantidad de pago al mes
int numberOfPayments = 36; // Numero de pagos
decimal estimatedUnitPeriods = 0;
int periods = 0; //Numero de periodos (meses)

decimal monthRate = 0M; // Estimacion de la tasa al mes
decimal finalTotalPayment = 0; // Total para la revision final

// Fecha de pago
DateTime loanDate = new DateTime(2023, 1, 1);
// Fecha de inicio del préstamo
DateTime paymentDate = new DateTime(2024, 1, 1);

// Revisar si alguno es el ultimo del mes
bool isLoanDateLastDay = loanDate == new DateTime(loanDate.Year, loanDate.Month, DateTime.DaysInMonth(loanDate.Year, loanDate.Month));
bool isPaymentDateLastDay = paymentDate == new DateTime(paymentDate.Year, paymentDate.Month, DateTime.DaysInMonth(paymentDate.Year, paymentDate.Month));

int monthsDifference = ((paymentDate.Year - loanDate.Year) * 12) + paymentDate.Month - loanDate.Month;

// Revisa si ambos son el ultimo dia del mes y si los dias en el mes del pago son menos a los dias en el mes que empezo la deuda para agregar un mes
if (isLoanDateLastDay && isPaymentDateLastDay && DateTime.DaysInMonth(paymentDate.Year, paymentDate.Month) < DateTime.DaysInMonth(loanDate.Year, loanDate.Month))
{
    monthsDifference++;
}

string[] typeOfDuration = new string[] { "monthly", "multiplesOfMonth", "semiMonthly", "actualDays" };

string durationSelection = typeOfDuration[0];

switch (durationSelection)
{
    case "monthly":
        //periods = monthsDifference;
        periods = 12;
        break;
    case "multiplesOfMonth":
        periods = (int)Math.Floor(monthsDifference / estimatedUnitPeriods);
        break;
    case "semiMonthly":
        periods = 24;
        //decimal unitPeriods = monthsDifference * 2;
        //decimal oddDays = unitPeriods % 7;
        //while (oddDays >= 15)
        //{
        //    oddDays -= 15;
        //    unitPeriods++;
        //}
        //periods = (int)unitPeriods;
        break;
    case "actualDays":
        TimeSpan duration = paymentDate - loanDate;
        int totalDays = duration.Days;
        periods = (int)Math.Floor(totalDays / estimatedUnitPeriods);
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