using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Axa din spatiul N-dimensional de preparate.
/// Contine un id de preparat si scorul corespunzator lui.
/// </summary>
public class Axa
{
	public Axa(int id)
	{
        id_preparat_ = id;
        scor_ = 0;
	}

    // Setteri si getteri.
    public int IdPreparat
    {
        get { return id_preparat_; }
    }

    public double Scor
    {
        get { return scor_; }
        set { scor_ = value; }
    }

    private double scor_;
    private int id_preparat_;
}