using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CalculusScript : MonoBehaviour {

    public MeshFilter Complex2DMesh;
    public MeshFilter Complex3DMesh;

    // Use this for initialization
    void Start () {
        List<Complex> points = LaplaceTransformPoints(
                  0f, 999f,
                  0.1f,
                  new Complex() { X = 0.00001f, Y = 0.00001f, T = 0f }, new Func<double, double>(TestFunction)
              );
        //List<Complex> points = VOCFTPoints(new Func<double, double>(TestFunction), 100, 100, 1f);
        double testMag = SumAreaByMagnitude(points);
        double testArea = SumAreaByQuadrilateral(points);
        Debug.Log(testMag);
        Debug.Log(testArea);
        BuildMeshes(points);

        //points = VOCFTPoints(new Func<double, double>(TestFunction), )
	}

    public double TestFunction(double x)
    {
        return 10f * Math.Sin(x / 10f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public class Complex
    {
        public double X;
        public double Y;
        public double T;

        public static Complex operator- (Complex A, Complex B)
        {
            return new Complex() { X = A.X - B.X, Y = A.Y - B.Y, T = A.T - B.T };
        }

        public static Complex operator+ (Complex A, Complex B)
        {
            return new Complex() { X = A.X + B.X, Y = A.Y + B.Y, T = A.T + B.T };
        }

        public static double DotProduct(Complex A, Complex B)
        {
            return A.X * B.X + A.Y * B.Y + A.T * B.T;
        }

        public static Complex CrossProduct(Complex A, Complex B)
        {
            return new Complex()
            {
                X = A.Y * B.T - A.T * B.Y,
                Y = A.T * B.X - A.X * B.T,
                T = A.X * B.Y - A.Y * B.X
            };
        }

        public override string ToString()
        {
            return string.Format("(X:{0}, Y:{1}, T:{2})", X, Y, T);
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + T * T);
            }
        }

        public double Length2D
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }
    }

    public void BuildMeshes(List<Complex> complices)
    {
        Mesh mesh2D = GenerateComplexMesh2D(complices);
        Mesh mesh3D = GenerateComplexMesh3D(complices);
        Complex2DMesh.mesh = mesh2D;
        Complex3DMesh.mesh = mesh3D;
    }

    public Complex[] GetComplexQuadrilateral(Complex A, Complex B)
    {
        Complex point1 = A;
        Complex point2 = new Complex() { X = 0f, Y = 0f, T = A.T };
        Complex point3 = B;
        Complex point4 = new Complex() { X = 0f, Y = 0f, T = B.T };
        return new Complex[] { point1, point2, point3, point4 };
    }

    public double ComplexArea(Complex A, Complex B)
    {
        // What am I doing here? Well, I'm treating this as a quadrilateral shape and getting the area of magnitude
        // as it rotates, rather than just getting the sums of magnitudes as areas between points. This gets you a
        // better approximation when I tested with the VODFT for music samples, because it accounts for the change
        // in the curves. T is treated as a Z axis in this case, so that the quadrilateral can be treated as two
        // triangles whose areas can be computed and then summed.
        Complex[] quad = GetComplexQuadrilateral(A, B);

        Complex triangle1 = Complex.CrossProduct(quad[1] - quad[0], quad[2] - quad[0]);
        Complex triangle2 = Complex.CrossProduct(quad[1] - quad[3], quad[2] - quad[3]);

        return triangle1.Length / 2.0f + triangle2.Length / 2.0f;
    }

    public double Derivative(Func<double, double> function, double x, double h)
    {
        return (function(x+h) - function(x)) / h;
    }

    public class Quad
    {
        public Vector3[] Vertices { get; set; }
        public Vector3[] Normals { get; set; }
        public int[] TriangleIndices { get; set; }

        public static Quad GetQuad(Vector3[] verts, int triangleOffset, bool flipNormals)
        {
            Quad q = new Quad();
            // 0 = top left, 1 = top right, 2 = bottom left, 3 = bottom right
            q.Vertices = verts;
            q.TriangleIndices = new int[] { triangleOffset, triangleOffset + 1, triangleOffset + 2, triangleOffset + 2, triangleOffset + 1, triangleOffset + 3 };
            q.Normals = new Vector3[] {
                Vector3.Cross(verts[1] - verts[0], verts[2] - verts[0]).normalized,
                Vector3.Cross(verts[0] - verts[1], verts[2] - verts[1]).normalized,
                Vector3.Cross(verts[0] - verts[2], verts[1] - verts[2]).normalized,
                Vector3.Cross(verts[2] - verts[3], verts[1] - verts[3]).normalized,
            };
            if (flipNormals)
            {
                for(int z=0;z<q.Normals.Length;z++)
                {
                    q.Normals[z] = -1f * q.Normals[z];
                }
            }
            return q;
        }
    }

    public Mesh GenerateComplexMesh2D(List<Complex> complexNumbers)
    {
        complexNumbers = complexNumbers.OrderBy(x => x.T).ToList();
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        int triangle = 0;
        for (int t = 1; t < complexNumbers.Count; t++)
        {
            Complex complice1 = complexNumbers[t-1];
            Complex complice2 = complexNumbers[t];
            double magnitude1 = complice1.Length2D;
            double magnitude2 = complice1.Length2D;
            Quad q = Quad.GetQuad(new Vector3[] {
                new Vector3(0f,(float)magnitude1,(float)complice1.T),
                new Vector3(0f,0f,(float)complice1.T),
                new Vector3(0f,(float)magnitude2,(float)complice2.T),
                new Vector3(0f,0f,(float)complice2.T),
            }, triangle, false);
            vertices.AddRange(q.Vertices);
            triangles.AddRange(q.TriangleIndices);
            normals.AddRange(q.Normals);
            triangle += 4;
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        return mesh;
    }

    public Mesh GenerateComplexMesh3D(List<Complex> complexNumbers)
    {
        complexNumbers = complexNumbers.OrderBy(x => x.T).ToList();
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        int triangle = 0;
        for (int t = 1; t < complexNumbers.Count; t++)
        {
            Complex[] complices = GetComplexQuadrilateral(complexNumbers[t - 1], complexNumbers[t]);
            Quad q = Quad.GetQuad(new Vector3[] {
                new Vector3((float)complices[0].X,(float)complices[0].Y,(float)complices[0].T),
                new Vector3((float)complices[1].X,(float)complices[1].Y,(float)complices[1].T),
                new Vector3((float)complices[2].X,(float)complices[2].Y,(float)complices[2].T),
                new Vector3((float)complices[3].X,(float)complices[3].Y,(float)complices[3].T)
            }, triangle, false);
            vertices.AddRange(q.Vertices);
            triangles.AddRange(q.TriangleIndices);
            normals.AddRange(q.Normals);
            triangle += 4;
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        return mesh;
    }

    public double SumAreaByQuadrilateral(List<Complex> complexNumbers)
    {
        // This method sums up the areas for a list of complex numbers by 3D quadrilateral method, using t as a Z-Axis
        double result = 0;
        complexNumbers = complexNumbers.OrderBy(x => x.T).ToList();
        for (int t=1;t<complexNumbers.Count;t++)
        {
            result += ComplexArea(complexNumbers[t - 1], complexNumbers[t]);
        }
        return result;
    }

    public double SumAreaByMagnitude(List<Complex> complexNumbers)
    {
        // This method sums up the areas for a list of complex numbers by simply summing the magnitudes, 
        // this is the traditional way of getting values on DFTs, I prefer the quadrilateral method above though.
        // This is more like a Reimann Sums method, it's faster but less accurate, and it's used often to
        // compute areas in mathematics.
        double result = 0;
        complexNumbers = complexNumbers.OrderBy(x => x.T).ToList();
        for (int t = 1; t < complexNumbers.Count; t++)
        {
            result += complexNumbers[t].Length2D * (complexNumbers[t].T - complexNumbers[t-1].T);
        }
        return result;
    }

    public List<Complex> LaplaceTransformPoints(double lower, double upper, double stepSize, Complex s, Func<double, double> function)
    {
        // Laplace Transform: The integral from 0 to infinity of f(t)*e^(-st) with respect to t.
        //                    Normally you want a lower limit of 0, but in case you're doing a
        //                    bilateral Laplace transform with a lower limit of -infinity, I 
        //                    parameterize it for you.
        List<Complex> results = new List<Complex>();
        for (double t=lower; t<upper;)
        {
            double f = function(t);
            double x = s.X * -1f * t;
            double y = s.Y * -1f * t;
            x = f * Math.Pow(Math.E, x);
            y = f * Math.Pow(Math.E, y);
            results.Add(new Complex() { X = x, Y = y, T = t });
            t += stepSize;
        }
        return results;
    }

    public List<Complex> ZTransformPoints(double A, double theta, double[] input)
    {
        // Z-Transform: The sum from -infinity to infinity of Input_n * (A * (cosine(theta) + i*sine(theta))^-n)
        //              Except for one small fact. The formula is trying to get a sum of -infinity to infinity for n 
        //              with a given discrete time samples, so instead I run n from 0 to the input size, since on the
        //              computer arrays start with an index of 0 instead of -infinity.
        List<Complex> results = new List<Complex>();
        for (int n = 0; n < input.Length;)
        {
            double x = Math.Pow(input[n] * (A * (Math.Cos(theta))), -1f * (double)n);
            double y = Math.Pow(input[n] * (A * (Math.Sin(theta))), -1f * (double)n);
            results.Add(new Complex() { X = x, Y = y, T = n });
            n++;
        }
        return results;
    }

    public List<Complex> VODFTPoints(double[] input, int N, int K)
    {
        // Variable Output Discrete Fourier Transform: The sum from 0 to N of Input_n * (cosine(M) - i*sine(M)) where M = (2*pi*n*(k/K*N))/N
        //                                             If you make N and K the same size, the formula will be equivalent to the DFT.
        List<Complex> results = new List<Complex>();
        for (int k=0;k<K;k++)
        {
            double x = 0;
            double y = 0;
            for (int n=0;n<N;n++)
            {
                double M = (2.0f * Math.PI * (double)n * ((double)k/(double)K*(double)N)) / (double)N;
                x += input[n] * Math.Cos(M);
                y += input[n] * Math.Sin(M) * -1f;
            }
            results.Add(new Complex() { X = x, Y = y, T = (double)k/(double)K });
        }
        return results;
    }

    public List<Complex> VOCFTPoints(Func<double, double> function, int N, int K, double stepSize = 1f)
    {
        // Variable Output Continuous Fourier Transform: This is a continuous function version of the VODFT that takes a function rather
        //                                               than a discrete set of samples, and provides the same transform. Notice also
        //                                               that I've included a fractional step size for the function, with 1f being essentially
        //                                               integer-like in behavior. This gives you a bit of flexibility in the function.
        List<Complex> results = new List<Complex>();
        for (int k = 0; k < K; k++)
        {
            double x = 0;
            double y = 0;
            for (int n = 0; n < N; n++)
            {
                double M = (2.0f * Math.PI * (double)n * ((double)k / (double)K * (double)N)) / (double)N;
                double nStep = stepSize * (double)n;
                x += function(nStep) * Math.Cos(M);
                y += function(nStep) * Math.Sin(M) * -1f;
            }
            results.Add(new Complex() { X = x, Y = y, T = (double)k / (double)K });
        }
        return results;
    }
}