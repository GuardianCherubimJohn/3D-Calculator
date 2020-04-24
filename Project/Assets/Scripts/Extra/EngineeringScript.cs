using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EngineeringScript : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public double Derivative(Func<double, double> function, double x, double h)
    {
        return (function(x + h) - function(x)) / h;
    }

    public double SecondDerivative(Func<double, double> function, double x, double h)
    {
        return (function(x + h) - 2 * function(x) + function(x - h)) / (h * h);
    }

    public double ThirdDerivative(Func<double, double> function, double x, double h)
    {
        return (function(x + 3 * h) - 3 * function(x + 2 * h) + 3 * function(x + h) - function(x)) / (h * h * h);
    }

    public double PartialDerivativeX(Func<double, double, double> function, double x, double y, double h)
    {
        return (function(x + h, y) - function(x, y)) / h;
    }

    public double PartialDerivativeY(Func<double, double, double> function, double x, double y, double h)
    {
        return (function(x, y + h) - function(x, y)) / h;
    }

    public double Current(Func<double, double> electricalChargeFunction, Func<double, double> timeFunction, double t, double h)
    {
        return Derivative(electricalChargeFunction, t, h) / Derivative(timeFunction, t, h);
    }

    public double Voltage(Func<double, double> energyFunction, Func<double, double> electricalChargeFunction, double t, double h)
    {
        return Derivative(energyFunction, t, h) / Derivative(electricalChargeFunction, t, h);
    }

    public double Power(Func<double, double> electricalChargeFunction, Func<double,double> energyFunction, Func<double,double> timeFunction, double t, double h)
    {
        return Current(electricalChargeFunction, timeFunction, t, h) * Voltage(energyFunction, electricalChargeFunction, t, h);
    }
}
