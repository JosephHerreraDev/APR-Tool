using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        CalculateAPR calculateApr = new CalculateAPR(
            amountFinanced: 5000M, 
            estimatedAPR: 1M,
            paymentAmount: 230M, 
            numberOfPayments: 24, 
            periodsPerYear: GetPeriodsPerYear("monthly"), 
            monthlyRate: 0M, 
            finalTotalPayment: 0M,
            lastPayment:280);

        Console.WriteLine("APR: " + calculateApr.APR + "%");
    }

    public class CalculateAPR
    {
        decimal amountFinanced; // Total a pagar
        decimal estimatedAPR; // Estimacion inicial APR
        decimal paymentAmount; // Cantidad a pagar
        int numberOfPayments; // Number of payments
        int periodsPerYear; // Numero de periodos año

        decimal monthlyRate; // Tasa de interés mensual
        decimal finalTotalPayment; // Total para revision final
        decimal lastPayment;

        decimal apr;

        public decimal APR
        {
            get { return apr; }
        }

        public CalculateAPR(
            decimal amountFinanced, 
            decimal estimatedAPR, 
            decimal paymentAmount, 
            int numberOfPayments, 
            int periodsPerYear, 
            decimal monthlyRate, 
            decimal finalTotalPayment, 
            decimal lastPayment)
        {
            this.amountFinanced = amountFinanced;
            this.estimatedAPR = estimatedAPR;
            this.paymentAmount = paymentAmount;
            this.numberOfPayments = numberOfPayments;
            this.periodsPerYear = periodsPerYear;
            this.monthlyRate = monthlyRate;
            this.finalTotalPayment = finalTotalPayment;
            this.lastPayment = lastPayment;

            ReturnResult();
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
                if (lastPayment > 0 && i == numberOfPayments)
                {
                    result += lastPayment / (decimal)Math.Pow((double)(1M + monthlyRate), i);
                }
                else 
                {
                    result += paymentAmount / (decimal)Math.Pow((double)(1M + monthlyRate), i);
                }
                
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

        public void ReturnResult()
        {
            // Actualiza iterativamente el APR hasta que el pago total final coincida con el monto financiado
            while (Math.Round(finalTotalPayment, 2) != amountFinanced)
            {
                UpdateAPR();
                finalTotalPayment = CalculateResult();
            }

            apr = Math.Round(estimatedAPR, 4);

            // Calcular el importe final financiado
            decimal finalAmountFinanced = 0M;

            CalculateMonthlyRate();

            for (int i = 1; i <= numberOfPayments; i++)
            {
                finalAmountFinanced += paymentAmount / (decimal)Math.Pow((double)(1M + estimatedAPR / 100 / periodsPerYear), i);
                finalAmountFinanced = Math.Round(finalAmountFinanced, 1);
            }
        }   
    }
}

