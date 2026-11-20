public static class ColaIngreso
{
    public static ResultadoCola Calcular(double llegadasPorMinuto, double segundosAtencion)
    {
        double mu = 60.0 / segundosAtencion;
        if (llegadasPorMinuto >= mu)
            return new ResultadoCola(false, llegadasPorMinuto / mu, double.PositiveInfinity, double.PositiveInfinity);

        double rho = llegadasPorMinuto / mu;
        double wq = rho / (mu - llegadasPorMinuto);
        double lq = llegadasPorMinuto * wq;
        return new ResultadoCola(true, rho, wq, lq);
    }
}
