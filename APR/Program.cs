decimal amountFinanced = 975M; // Total que va a pagar
decimal payment = 340M; // Cantidad de pago al mes
int numberOfPayments = 3; // Numero de pagos
//bool monthly = true;
decimal estimatedAPR = 27.4849M; // Estimacion inicial de APR
int periods = 12; //Numero de periodos (meses)

decimal monthRate = 0M; // Estimacion de la tasa al mes
decimal finalTotalPayment = 0; // Total para la revision final

// Suponiendo que pDate y lDate son variables DateTime que representan la fecha de pago y la fecha de inicio del préstamo respectivamente
//DateTime lDate = new DateTime(2024, 1, 1); // Fecha de inicio del préstamo (año, mes, día)
//DateTime pDate = new DateTime(2024, 12, 31); // Fecha de pago (año, mes, día)


//TimeSpan duration = pDate - lDate;
//int totalDays = duration.Days;
//int daysInUP = /* Aquí debes definir el número de días en unidades de período (daysInUP) */;
//int units = (int)Math.Floor((double)totalDays / daysInUP);
//return totalDays - (daysInUP * units);


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

Console.WriteLine("APR: %" + Math.Round(estimatedAPR, 4));

estimatedAPR = Math.Round(estimatedAPR / 100 / 12, 4) ;

decimal result = 0M;

// Imprime lo que deberia de ser el Amount financed total
for (int x = 1; x < numberOfPayments + 1; x++)
{
    result = Decimal.Add(result, (decimal)((double)payment / Math.Pow((double)(1M + estimatedAPR), x)));
    result = Math.Round(result, 1);
}

Console.WriteLine("Amount financed: " + result);