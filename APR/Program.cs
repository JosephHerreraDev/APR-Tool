class Program
{
    static void Main(string[] args)
    {
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

        // Para calcular el unit period del ultimo pago es necesario pasarle la cantidad de pagos que van a realizarse


        CalculateAPR calculateAPR = new CalculateAPR(
            amountFinanced: 1000M, 
            estimatedAPR: 1M, 
            monthlyPayment: 33.61M, 
            numberOfPayments: 36, 
            periodsPerYear: GetPeriodsPerYear("monthly"), 
            monthlyRate: 0M, 
            finalTotalPayment: 0M);

        calculateAPR.PrintResult();
    }

    public class CalculateAPR
    {
        decimal amountFinanced; // Total a pagar
        decimal estimatedAPR; // Estimacion inicial APR
        decimal monthlyPayment; // Cantidad a pagar
        int numberOfPayments; // Number of payments
        int periodsPerYear; // Numero de periodos año

        decimal monthlyRate; // Tasa de interés mensual
        decimal finalTotalPayment; // Total para revision final

        public CalculateAPR(decimal amountFinanced, decimal estimatedAPR, decimal monthlyPayment, int numberOfPayments, int periodsPerYear, decimal monthlyRate, decimal finalTotalPayment)
        {
            this.amountFinanced = amountFinanced;
            this.estimatedAPR = estimatedAPR;
            this.monthlyPayment = monthlyPayment;
            this.numberOfPayments = numberOfPayments;
            this.periodsPerYear = periodsPerYear;
            this.monthlyRate = monthlyRate;
            this.finalTotalPayment = finalTotalPayment;
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

        public void PrintResult()
        {

            // Actualiza iterativamente el APR hasta que el pago total final coincida con el monto financiado
            while (Math.Round(finalTotalPayment, 2) != amountFinanced)
            {
                UpdateAPR();
                finalTotalPayment = CalculateResult();
            }

            Console.WriteLine($"APR: {Math.Round(estimatedAPR, 4)}%");

            // Imprimir el importe final financiado
            decimal finalAmountFinanced = 0M;
            CalculateMonthlyRate();

            for (int i = 1; i <= numberOfPayments; i++)
            {
                finalAmountFinanced += monthlyPayment / (decimal)Math.Pow((double)(1M + estimatedAPR / 100 / periodsPerYear), i);
                finalAmountFinanced = Math.Round(finalAmountFinanced, 1);
            }

            Console.WriteLine($"Amount financed: {finalAmountFinanced}");
        }   
    }
}

