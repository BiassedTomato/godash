using System;

public static class QMath
{
    public static float Gauss(float mean, float stdDev)
    {
        Random rand = new Random();

        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        double randNormal =
                     mean + stdDev * randStdNormal;

        return (float)randNormal;
    }
}