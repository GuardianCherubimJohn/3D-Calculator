using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class MathExpression : MonoBehaviour {

    public UnityEngine.UI.Button ParseButton;
    public UnityEngine.UI.InputField EquationInput;
    public FunctionLineScript CurveLine;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ParseExpression()
    {
        float gridSize = 1000f;
        float halfGrid = gridSize / 2f;


        Expression.ExpressionNodes nodes = Expression.Parse(EquationInput.text);
        Expression.ExpressionResult result = Expression.Evaluate(nodes, 100);
        //Expression expression = Expression.Parse(EquationInput.text);
        //foreach(Expression.ExpressionSubset subset in expression.OperationGroups)
        //{
        //    Debug.Log(subset.ToString());
        //}
        if (result.ResultType == Expression.ExpressionResult.ExpressionResultType.Curve3D)
        {
            int stepsY = result.Curve3D.GetUpperBound(1) + 1;
            int stepsX = result.Curve3D.GetUpperBound(0) + 1;
            CurveLine.CurvePoints = new FunctionLineScript.LinePoints[stepsY];
            for (int y=0;y<stepsY;y++)
            {
                CurveLine.CurvePoints[y] = new FunctionLineScript.LinePoints();
                CurveLine.CurvePoints[y].Points = new Vector3[stepsX];
                for (int x=0;x<stepsX;x++)
                {
                    Vector3 invec = result.Curve3D[x, y];
                    CurveLine.CurvePoints[y].Points[x] = invec * (halfGrid);
                    Debug.Log(invec);
                }
            }
            FunctionLineScript.DeleteChildObjects(CurveLine.transform, "curve");
            SurfaceRenderer.BuildRenderMesh(CurveLine.transform, CurveLine.CurvePoints, CurveLine.MainMaterial, CurveLine.VertexColor);
            //FunctionLineScript.BuildMesh(CurveLine.transform, CurveLine.CurvePoints, CurveLine.MainMaterial, CurveLine.LineRadius, CurveLine.SphereIterations, CurveLine.CylinderCapSegments, CurveLine.VertexColor);
        }
        else if (result.ResultType == Expression.ExpressionResult.ExpressionResultType.Curve2D)
        {
            int stepsX = result.Curve2D.GetUpperBound(0) + 1;
            CurveLine.CurvePoints = new FunctionLineScript.LinePoints[1];
            CurveLine.CurvePoints[0] = new FunctionLineScript.LinePoints();
            CurveLine.CurvePoints[0].Points = new Vector3[stepsX];
            for (int x=0;x<stepsX;x++)
            {
                Vector3 invec = result.Curve2D[x];
                CurveLine.CurvePoints[0].Points[x] = invec * (halfGrid);
                Debug.Log(invec);
            }
            FunctionLineScript.DeleteChildObjects(CurveLine.transform, "curve");
            FunctionLineScript.BuildMesh(CurveLine.transform, CurveLine.CurvePoints, CurveLine.MainMaterial, CurveLine.LineRadius, CurveLine.SphereIterations, CurveLine.CylinderCapSegments, CurveLine.VertexColor);
        }
        else if (result.ResultType == Expression.ExpressionResult.ExpressionResultType.Scalar)
        {
            Debug.Log("Result: " + result.ScalarValue);
        }
    }

    public class Expression
    {
        public enum ExpressionResultType
        {
            Scalar, Curve2D, Area2D, Surface3D
        };

        public enum ExpressionNodeType
        {
            List,
            Matrix,
            Vector,
            Set,
            Function,
            Constant,
            UserVariable,
            Number,
            ComplexNumber,
            Exponent,
            Multiply,
            Divide,
            Add,
            Subtract,
            LeftShift,
            RightShift,
            Modulo,
            Equals,
            OpenParenthesis,
            CloseParenthesis,
            OpenSet,
            CloseSet,
            OpenIndex,
            CloseIndex,
            Comma,
            DoubleQuote,
            ExpressionContainer,
            FunctionParametersContainer,
            FunctionParameterContainer,
            SetContainer,
            IndexContainer
        }

        public enum ExpressionNodeTypeCategory
        {
            Collection,
            Function,
            Constant,
            UserVariable,
            Number,
            Operator,
            Equals,
            ContainerDelimiter,
            Comma,
            DoubleQuote,
            Container
        }

        public static Dictionary<ExpressionNodeType, ExpressionNodeTypeCategory> ExpressionNodeTypeCategories = new Dictionary<ExpressionNodeType, ExpressionNodeTypeCategory>()
        {
            {  ExpressionNodeType.Add, ExpressionNodeTypeCategory.Operator },
            {  ExpressionNodeType.CloseIndex, ExpressionNodeTypeCategory.ContainerDelimiter },
            {  ExpressionNodeType.CloseParenthesis, ExpressionNodeTypeCategory.ContainerDelimiter },
            {  ExpressionNodeType.CloseSet, ExpressionNodeTypeCategory.ContainerDelimiter },
            {  ExpressionNodeType.Comma, ExpressionNodeTypeCategory.Comma },
            {  ExpressionNodeType.ComplexNumber, ExpressionNodeTypeCategory.Number },
            {  ExpressionNodeType.Constant, ExpressionNodeTypeCategory.Constant },
            {  ExpressionNodeType.Divide, ExpressionNodeTypeCategory.Operator },
            {  ExpressionNodeType.DoubleQuote, ExpressionNodeTypeCategory.DoubleQuote },
            {  ExpressionNodeType.Equals, ExpressionNodeTypeCategory.Equals },
            {  ExpressionNodeType.Exponent, ExpressionNodeTypeCategory.Operator },
            {  ExpressionNodeType.ExpressionContainer, ExpressionNodeTypeCategory.Container },
            {  ExpressionNodeType.Function, ExpressionNodeTypeCategory.Function },
            {  ExpressionNodeType.FunctionParameterContainer, ExpressionNodeTypeCategory.Container },
            {  ExpressionNodeType.FunctionParametersContainer, ExpressionNodeTypeCategory.Container },
            {  ExpressionNodeType.IndexContainer, ExpressionNodeTypeCategory.Container },
            {  ExpressionNodeType.LeftShift, ExpressionNodeTypeCategory.Operator },
            {  ExpressionNodeType.List, ExpressionNodeTypeCategory.Collection },
            {  ExpressionNodeType.Matrix, ExpressionNodeTypeCategory.Collection },
            {  ExpressionNodeType.Modulo, ExpressionNodeTypeCategory.Operator },
            {  ExpressionNodeType.Multiply, ExpressionNodeTypeCategory.Operator },
            {  ExpressionNodeType.Number, ExpressionNodeTypeCategory.Number },
            {  ExpressionNodeType.OpenIndex, ExpressionNodeTypeCategory.ContainerDelimiter },
            {  ExpressionNodeType.OpenParenthesis, ExpressionNodeTypeCategory.ContainerDelimiter },
            {  ExpressionNodeType.OpenSet, ExpressionNodeTypeCategory.ContainerDelimiter },
            {  ExpressionNodeType.RightShift, ExpressionNodeTypeCategory.Operator },
            {  ExpressionNodeType.Set, ExpressionNodeTypeCategory.Collection },
            {  ExpressionNodeType.SetContainer, ExpressionNodeTypeCategory.Container },
            {  ExpressionNodeType.Subtract, ExpressionNodeTypeCategory.Operator },
            {  ExpressionNodeType.UserVariable, ExpressionNodeTypeCategory.UserVariable },
            {  ExpressionNodeType.Vector, ExpressionNodeTypeCategory.Collection }
        };

        [Serializable]
        public class Vector<T>
        {
            public T[] Values { get; set; }

            public double[] DoubleValues { get
                {
                    if (typeof(T) == typeof(double))
                    {
                        double[] d = new double[Values.Length];
                        for (int z=0;z<Values.Length;z++)
                        {
                            d[z] = Convert.ToDouble(Values[z]);
                        }
                        return d;
                    }
                    return null;
                }
                set
                {

                }
            }

            public Vector(int dimensions)
            {
                Values = new T[dimensions];
            }

            public Vector(int dimensions, T[] initialData)
            {
                Values = new T[dimensions];
                for (int v=0;v<initialData.Length;v++)
                {
                    if (v < dimensions)
                    {
                        Values[v] = initialData[v];
                    }
                }
            }
        }

        [Serializable]
        public class Matrix<T>
        {
            public T[,] Values { get; set; }
            public int Rows { get; set; }
            public int Columns { get; set; }

            public Matrix(int rows, int columns)
            {
                Rows = rows;
                Columns = columns;
                Values = new T[Rows, Columns];
            }

            public Matrix(int rows, int columns, T[] initialData)
            {
                Rows = rows;
                Columns = columns;
                Values = new T[Rows, Columns];
                int z = 0;
                for (int r=0;r<Rows;r++)
                {
                    for (int c=0;c<Columns;c++)
                    {
                        if (z < initialData.Length)
                            Values[r, c] = initialData[z];
                        z++;
                    }
                }
            }
        }

        public class ExpressionNode : ICloneable
        {
            public ExpressionNodeType NodeType { get; set; }

            public int OriginalStartIndex { get; set; }
            public int OriginalLength { get; set; }
            public double ValueNumber { get; set; }
            public string ValueString { get; set; }
            public string ValueSuffix { get; set; }

            public bool IsFunctionEvaluated { get; set; }

            public ExpressionNode FirstSibling { get; set; }
            public ExpressionNode LastSibling { get; set; }
            public ExpressionNode PreviousSibling { get; set; }
            public ExpressionNode NextSibling { get; set; }
            public ExpressionNode RootNode { get; set; }
            public ExpressionNode ParentNode { get; set; }
            public ExpressionNodes ChildNodes { get; set; }

            public object Clone()
            {
                ExpressionNode node = new ExpressionNode();
                node.NodeType = this.NodeType;
                node.OriginalLength = this.OriginalLength;
                node.OriginalStartIndex = this.OriginalStartIndex;
                node.ValueNumber = this.ValueNumber;
                node.ValueString = this.ValueString;
                node.ValueSuffix = this.ValueSuffix;
                node.IsFunctionEvaluated = this.IsFunctionEvaluated;

                node.FirstSibling = this.FirstSibling == null ? null : this.FirstSibling.Clone() as ExpressionNode;
                node.LastSibling = this.LastSibling == null ? null : this.LastSibling.Clone() as ExpressionNode;
                node.PreviousSibling = this.PreviousSibling == null ? null : this.PreviousSibling.Clone() as ExpressionNode;
                node.NextSibling = this.NextSibling == null ? null : this.NextSibling.Clone() as ExpressionNode;
                node.RootNode = this.RootNode == null ? null : this.RootNode.Clone() as ExpressionNode;
                node.ParentNode = this.ParentNode == null ? null : this.ParentNode.Clone() as ExpressionNode;
                node.ChildNodes = this.ChildNodes == null ? null : this.ChildNodes.Clone() as ExpressionNodes;

                return node;
            }

            public override string ToString()
            {
                return string.Format("{0} ({1},{2}) {3} ({4}, {5})",NodeType, OriginalStartIndex, OriginalLength, ValueString, ValueNumber, ValueSuffix);
            }
        }

        public class ExpressionNodes : ICloneable
        {
            public List<ExpressionNode> Nodes { get; set; }

            public ExpressionNodes()
            {
                Nodes = new List<ExpressionNode>();
            }

            public ExpressionNodes(List<ExpressionNode> nodes)
            {
                Nodes = nodes;
            }

            public object Clone()
            {
                if (this.Nodes == null) return null;
                ExpressionNodes nodes = new ExpressionNodes();
                nodes.Nodes = new List<ExpressionNode>();
                for (int z=0;z<this.Nodes.Count;z++)
                {
                    if (this.Nodes[z] != null)
                    {
                        nodes.Nodes.Add(this.Nodes[z].Clone() as ExpressionNode);
                    }
                }
                return nodes;
            }
        }

        public class ExpressionInputParameter
        {
            public enum UserInputParameter
            {
                s, t, u, v, w, x, y, z
            };

            public UserInputParameter Parameter { get; set; }
            public double Value { get; set; }
            public double MinValue { get; set; }
            public double MaxValue { get; set; }
        }

        public class ExpressionResult
        {
            public enum ExpressionResultType
            {
                Scalar,
                Vector,
                Curve2D,
                Curve3D,
                ExpressionResult
            };

            public ExpressionResultType ResultType { get; set; }
            public double ScalarValue { get; set; }
            public Vector3 VectorValue { get; set; }
            public Vector3[] Curve2D { get; set; }
            public Vector3[,] Curve3D { get; set; }
            public ExpressionNodes ResultExpression { get; set; }
        }

        public class ExpressionSubset
        {
            public bool IsError { get; set; }
            public bool IsParenthesisSubset { get; set; }
            public bool IsFinalSubset { get; set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
            public int ParameterIndex { get; set; }
            public string ParameterIndexString { get { return "{" + ParameterIndex + "}"; } }
            public string Content { get; set; }
            public string ReplacedContent { get; set; }
            public string Input { get; set; }

            public override string ToString()
            {
                return string.Format("Final:{0} Error:{1} IsSubset:{2} Start:{3} End:{4} ParamIdx:{5} IdxStr:{6} Content:{7} Replaced:{8} Input:{9}", 
                    IsFinalSubset, IsError, IsParenthesisSubset, 
                    StartIndex, EndIndex, ParameterIndex, 
                    ParameterIndexString, Content, ReplacedContent, Input);
            }
        }

        public class ExpressionNodeEvaluationResult
        {
            public bool Success { get; set; }
            public ExpressionNode ResultNode { get; set; }
        }

        public class BuiltInMethods
        {
            public enum FunctionType
            {
                Scalar,
                DoubleInputScalar,
                TripleInputScalar,
                Derivative,
                PartialDerivative,
                UserMethod
            }

            public static Dictionary<FunctionType, int> FunctionParametersCount = new Dictionary<FunctionType, int>()
            {
                { FunctionType.Scalar, 1 },
                { FunctionType.DoubleInputScalar, 2 },
                { FunctionType.TripleInputScalar, 2 },
                { FunctionType.Derivative, 3 },
                { FunctionType.PartialDerivative, 4 },
                { FunctionType.UserMethod, 2 }
            };

            public static Dictionary<string, FunctionType> FunctionList = new Dictionary<string, FunctionType>()
            {
                { "method", FunctionType.UserMethod },
                { "sin", FunctionType.Scalar },
                { "cos", FunctionType.Scalar },
                { "tan", FunctionType.Scalar },
                { "arcsin", FunctionType.Scalar },
                { "arccos", FunctionType.Scalar },
                { "arctan", FunctionType.Scalar },
                { "secant", FunctionType.Scalar },
                { "cosecant", FunctionType.Scalar },
                { "cotangent", FunctionType.Scalar },
                { "arcsec", FunctionType.Scalar },
                { "arccsc", FunctionType.Scalar },
                { "arccot", FunctionType.Scalar },
                { "sqrt", FunctionType.Scalar },
                { "!", FunctionType.Scalar },
                { "log", FunctionType.Scalar },
                { "log10", FunctionType.Scalar },
                { "ln", FunctionType.Scalar },
                { "exp", FunctionType.Scalar },
                { "not", FunctionType.Scalar },
                { "firstderivative", FunctionType.Derivative },
                { "secondderivative", FunctionType.Derivative },
                { "thirdderivative", FunctionType.Derivative },
                { "partialderivativex", FunctionType.PartialDerivative },
                { "partialderivativey", FunctionType.PartialDerivative },
                { "lsh", FunctionType.DoubleInputScalar },
                { "rsh", FunctionType.DoubleInputScalar },
                { "and", FunctionType.DoubleInputScalar },
                { "nand", FunctionType.DoubleInputScalar },
                { "or", FunctionType.DoubleInputScalar },
                { "nor", FunctionType.DoubleInputScalar },
                { "xor", FunctionType.DoubleInputScalar },
                { "nxor", FunctionType.DoubleInputScalar },
                { "pow", FunctionType.DoubleInputScalar },
                { "poissondist", FunctionType.DoubleInputScalar },
                { "uniformdist", FunctionType.DoubleInputScalar },
                { "exponentialdist", FunctionType.DoubleInputScalar },
                { "binomialdist", FunctionType.TripleInputScalar },
                { "normaldist", FunctionType.TripleInputScalar },
                { "zscorenormaldist", FunctionType.TripleInputScalar }
            };

            public static Dictionary<string, Func<double, double>> ScalarFunctionList = new Dictionary<string, Func<double, double>>()
            {
                { "sin", new Func<double, double>(Sine) },
                { "cos", new Func<double, double>(Cosine) },
                { "tan", new Func<double, double>(Tangent) },
                { "secant", new Func<double, double>(Secant) },
                { "cosecant", new Func<double, double>(Cosecant) },
                { "cotangent", new Func<double, double>(Cotangent) },
                { "arcsin", new Func<double, double>(ArcSine) },
                { "arccos", new Func<double, double>(ArcCosine) },
                { "arctan", new Func<double, double>(ArcTangent) },
                { "arccec", new Func<double, double>(ArcSecant) },
                { "arccsc", new Func<double, double>(ArcCosecant) },
                { "arccot", new Func<double, double>(ArcCotangent) },
                { "sqrt", new Func<double, double>(SquareRoot) },
                { "!", new Func<double, double>(Factorial) },
                { "log", new Func<double, double>(Log) },
                { "log10", new Func<double, double>(Log10) },
                { "ln", new Func<double, double>(Ln) },
                { "exp", new Func<double, double>(Exp) },
                { "not", new Func<double, double>(Not) }
            };

            public static Dictionary<string, Func<double, double, double>> DoubleScalarFunctionList = new Dictionary<string, Func<double, double, double>>()
            {
                { "pow", new Func<double, double, double>(Power) },
                { "and", new Func<double, double, double>(And) },
                { "nand", new Func<double, double, double>(NotAnd) },
                { "or", new Func<double, double, double>(Or) },
                { "nor", new Func<double, double, double>(NotOr) },
                { "xor", new Func<double, double, double>(Xor) },
                { "nxor", new Func<double, double, double>(NotXor) },
                { "lsh", new Func<double, double, double>(LeftShift) },
                { "rsh", new Func<double, double, double>(RightShift) },
                { "poissondist", new Func<double, double, double>(PoissonDistribution) },
                { "uniformdist", new Func<double, double, double>(UniformDistribution) },
                { "exponentialdist", new Func<double, double, double>(ExponentialDistributionPoint) },
            };

            public static Dictionary<string, Func<double, double, double, double>> TripleScalarFunctionList = new Dictionary<string, Func<double, double, double, double>>()
            {
                { "binomialdist", new Func<double, double, double, double>(BinomialDistribution) },
                { "normaldist", new Func<double, double, double, double>(NormalDistributionPoint) },
                { "zscorenormaldist", new Func<double, double, double, double>(ZScoreNormalDistribution) }
            };

            public static Dictionary<string, Func<Func<double, double>, double, double, double>> DerivativeFunctionList = new Dictionary<string, Func<Func<double, double>, double, double, double>>()
            {
                { "firstderivative", new Func<Func<double, double>, double, double, double>(Derivative) },
                { "secondderivative", new Func<Func<double, double>, double, double, double>(SecondDerivative) },
                { "thirdderivative", new Func<Func<double, double>, double, double, double>(ThirdDerivative) }
            };

            public static Dictionary<string, Func<Func<double, double, double>, double, double, double, double>> PartialDerivativeFunctionList = new Dictionary<string, Func<Func<double, double, double>, double, double, double, double>>()
            {
                { "partialderivativex", new Func<Func<double, double, double>, double, double, double, double>(PartialDerivativeX) },
                { "partialderivativey", new Func<Func<double, double, double>, double, double, double, double>(PartialDerivativeY) },
            };

            public static Dictionary<string, double> ConstantList = new Dictionary<string, double>()
            {
                { "pi", 3.1415926535897932384626433832795 },
                { "e",  2.7182818284590452353602874713527 }
            };

            public static string[] UserVariableList =
            {
                "w", "x", "y", "z", "t", "u"
            };

            public static string[] OperatorList =
            {
                "^", "*", "/", "%", "+", "-"
            };

            public static decimal Power(decimal Base, decimal Exponent)
            {
                return Exp(Exponent * Ln(Base));
            }

            public static decimal Exp(decimal Exponent)
            {
                decimal X, P, Frac, I, L;
                X = Exponent;
                Frac = X;
                P = (1.0M + X);
                I = 1.0M;
                decimal delta = 0M;

                do
                {
                    I++;
                    Frac *= (X / I);
                    L = P;
                    P += Frac;
                    delta = Math.Abs(L - P);
                } while (delta > 0.0000000000000000000001M);

                return P;
            }

            public static decimal Log(decimal N, decimal B)
            {
                return (Ln(N) / Ln(B));

                /*
                 * To find the Log of Base 2, use
                 * return (Ln(N) / 0.69314718055995);
                 *
                 * For the Log of Base 10, use
                 * return (Ln(N) / 2.30258509299405);
                         *
                         * And make sure you get rid of the 'double B' Parameter
                 */
            }

            public static decimal Ln(decimal Power)
            {
                decimal N, P, L, R, A, E;
                E = 2.71828182845904523536028747135266249775724709369995M;
                P = Power;
                N = 0.0M;

                // This speeds up the convergence by calculating the integral
                while (P >= E)
                {
                    P /= E;
                    N++;
                }
                N += (P / E);
                P = Power;
                decimal delta = 0M;

                do
                {
                    A = N;
                    L = (P / (Exp(N - 1.0M)));
                    R = ((N - 1.0M) * E);
                    N = ((L + R) / E);
                    delta = Math.Abs(N - A);
                } while (delta > 0.00000000000000000000001M);

                return N;
            }

            public static List<decimal> TriDistribution(decimal a, decimal minimum, decimal maximum, decimal stepSize)
            {
                decimal coefficient = (100M - a) / 100M;
                List<decimal> list = new List<decimal>();
                for (decimal x = minimum; x <= maximum; x+= stepSize)
                {
                    decimal x2 = x * x;
                    decimal exponent = x2 * coefficient;
                    list.Add(Power(1M / 3M, exponent));
                }
                return list;
            }

            public static double Gaussian1D(double x, double stdDev)
            {
                return (1.0 / Math.Sqrt(2.0 * Math.PI * stdDev * stdDev)) * Math.Exp(-1.0 * x * x / (2.0 * stdDev * stdDev));
            }

            public static double Gaussian2D(double x, double y, double stdDev)
            {
                return (1.0 / (2.0 * Math.PI * stdDev * stdDev)) * Math.Exp(-1.0 * (x * x + y * y) / (2.0 * stdDev * stdDev));
            }

            public static Vector<double> GaussConvolutionMatrix1D(int size, double max, double stdDev)
            {
                Vector<double> vector = new Vector<double>(size);
                for (int x = 0; x < size; x++)
                {
                    double xx = (double)x / max;
                    double value = Gaussian1D(xx, stdDev);
                    vector.Values[x] = value;
                }
                return vector;
            }

            public static Matrix<double> GaussConvolutionMatrix2D(int size, double max, double stdDev)
            {
                Matrix<double> matrix = new Matrix<double>(size, size);
                for (int y=0;y<size;y++)
                {
                    for (int x=0;x<size;x++)
                    {
                        double xx = (double)x / max;
                        double yy = (double)y / max;
                        double value = Gaussian2D(xx, yy, stdDev);
                        matrix.Values[x,y] = value;
                    }
                }
                return matrix;
            }

            public class TimeSampledData
            {
                public double Time { get; set; }
                public double Value { get; set; }

                public TimeSampledData(double time, double value)
                {
                    Time = time;
                    Value = value;
                }
            }

            public class FrequencyOutput
            {
                public double Value { get; set; }
                public double Frequency { get; set; }
                public double Time { get; set; }

                public double Rotations
                {
                    get
                    {
                        return Time * Frequency;
                    }
                }

                public double ClampedRotation
                {
                    get
                    {
                        return Rotations - (double)(int)Rotations;
                    }
                }

                public double RotationValue
                {
                    get
                    {
                        return Rotations * Value;
                    }
                }

                public double ClampedRotationValue
                {
                    get
                    {
                        return ClampedRotation * Value;
                    }
                }
            }

            public class FrequencyChart
            {
                public List<Vector<double>> Values { get; set; }
                public List<Vector<double>> ClampedValues { get; set; }

                public List<HzValue> RotationValues { get; set; }
                public List<HzValue> ClampedRotationValues { get; set; }

                public List<HzValue> ValuesByHz { get; set; }
                public List<RotationValue> ValuesByRotations { get; set; }

                public List<FrequencyOutput> Frequencies { get; set; }

                public class HzValue
                {
                    public double Hz { get; set; }
                    public double Total { get; set; }
                }

                public class RotationValue
                {
                    public double Rotations { get; set; }
                    public double Total { get; set; }
                }

                public class TimeValue
                {
                    public double Time { get; set; }
                    public double Value { get; set; }
                }

                public FrequencyChart()
                {
                    Values = new List<Vector<double>>();
                    ClampedValues = new List<Vector<double>>();
                }

                public void ChartFrequencies(List<FrequencyOutput> frequencies)
                {
                    Frequencies = frequencies.OrderBy(x => x.Frequency).ThenBy(x => x.ClampedRotation).ToList();
                    RotationValues = Frequencies.GroupBy(x => x.Frequency).Select(x => new HzValue() { Hz = x.Key, Total = x.Sum(a => a.RotationValue) }).OrderBy(x => x.Hz).ToList();
                    ClampedRotationValues = Frequencies.GroupBy(x => x.Frequency).Select(x => new HzValue() { Hz = x.Key, Total = x.Sum(a => a.ClampedRotationValue) }).OrderBy(x => x.Hz).ToList();
                    ValuesByHz = Frequencies.GroupBy(x => x.Frequency).Select(x => new HzValue() { Hz = x.Key, Total = x.Sum(a => a.Value) }).OrderBy(x => x.Hz).ToList();
                    ValuesByRotations = Frequencies.GroupBy(x => x.ClampedRotation).Select(x => new RotationValue() { Rotations = x.Key, Total = x.Sum(a => a.Value) }).OrderBy(x => x.Rotations).ToList();
                }

                public List<TimeValue> ProduceInverseHarmonic(double rotationCutoff = 1.0)
                {
                    //List<FrequencyOutput> newFrequencies = Frequencies.Where(x => x.ClampedRotation <= rotationCutoff && (1.0 - x.ClampedRotation) > (1.0 - rotationCutoff)).ToList();
                    List<FrequencyOutput> newFrequencies = Frequencies.Where(x => x.ClampedRotation <= rotationCutoff).ToList();
                    List<TimeValue> t = newFrequencies.GroupBy(x => x.Time).Select(x => new TimeValue() { Time = x.Key, Value = x.Sum(a => a.ClampedRotation * a.Value) }).OrderBy(x => x.Time).ToList();
                    return t;
                }
            }

            public class PiCalculator
            {
                // Special thanks to Sam Allen over at https://www.dotnetperls.com/pi for this simple Pi calculation

                public static decimal PI(decimal cutoffIterations = 33)
                {
                    // Returns PI
                    return 2M * F(1M, cutoffIterations);
                }

                public static decimal F(decimal i, decimal cutoff)
                {
                    // Receives the call number
                    if (i > cutoff)
                    {
                        // Stop after 60 calls
                        return (decimal)i;
                    }
                    else
                    {
                        // Return the running total with the new fraction added
                        return 1M + (i / (1M + (2.0M * (decimal)i))) * F((decimal)i + 1M, cutoff);
                    }
                }
            }

            public static double[] SmoothDFT(double[] dataPoints, double harmonic)
            {
                double n = (double)dataPoints.Length;
                double w = Math.Floor(n / 2.0);
                double r2 = 2.0 * w + 1.0;
                double q = Math.Floor(harmonic * w);
                List<double> ctest = new List<double>();
                for (int z = 0; z < (int)(w - q); z++)
                    ctest.Add(0.0);
                for (int z = 0; z < (int)(2.0 * q + 1.0); z++)
                    ctest.Add(1.0);
                for (int z = 0; z < (int)(w - q); z++)
                    ctest.Add(0.0);
                double outSize = 2.0 * w + 1.0;

                List<Vector<double>> test1 = new List<Vector<double>>();
                int za2 = 0;
                for (int z2=(int)(-1.0 * w);z2<=(int)w;z2++)
                {
                    Vector<double> sum1 = new Vector<double>(2);
                    for (int i=0;i<(int)n;i++)
                    {
                        int k1 = z2;
                        int t1 = i;
                        double input = dataPoints[t1];
                        double csval = 2.0 * Math.PI * (double)k1 / n * (double)(t1+1);
                        Vector<double> cval = new Vector<double>(2);
                        cval.Values[0] = Math.Cos(csval);
                        cval.Values[1] = -1.0 * Math.Sin(csval);
                        cval.Values[0] *= input;
                        cval.Values[1] *= input;
                        sum1.Values[0] += cval.Values[0];
                        sum1.Values[1] += cval.Values[1];
                    }
                    sum1.Values[0] /= Math.Sqrt(n);
                    sum1.Values[1] /= Math.Sqrt(n);
                    sum1.Values[0] *= ctest[za2];
                    sum1.Values[1] *= ctest[za2];
                    test1.Add(sum1);
                    za2 += 1;
                }
                za2 = 0;
                List<double> test3 = new List<double>();
                for (int i = 0; i < (int)n; i++)
                {
                    Vector<double> sum1 = new Vector<double>(2);
                    int aindex = 0;
                    for (int z2 = (int)(-1.0 * w); z2 <= (int)w; z2++)
                    {
                        int k1 = z2;
                        int t1 = i;
                        Vector<double> input = test1[aindex];
                        double csval = 2.0 * Math.PI * (double)k1 / n * (double)(t1+1);
                        Vector<double> cval = new Vector<double>(2);
                        cval.Values[0] = Math.Cos(csval);
                        cval.Values[1] = Math.Sin(csval);
                        Vector<double> val = new Vector<double>(2);
                        val.Values[0] = cval.Values[0] * input.Values[0] - cval.Values[1] * input.Values[1];
                        val.Values[1] = cval.Values[0] * input.Values[1] + cval.Values[1] * input.Values[0];

                        sum1.Values[0] += val.Values[0];
                        sum1.Values[1] += val.Values[1];
                        aindex++;
                    }
                    test3.Add(sum1.Values[0] / Math.Sqrt(n));
                    za2++;
                }
                return test3.ToArray();
            }

            public static FrequencyChart HGT(TimeSampledData[] input, int frequencyGroups, double frequencyMax, out double inputMax, out List<FrequencyOutput> frequencies, double timeScale = 1.0, double frequencyScale = 1.0)
            {
                frequencies = new List<FrequencyOutput>();
                FrequencyChart chart = new FrequencyChart();
                inputMax = input.Max(x => x.Value);
                for (int f=0;f<frequencyGroups;f++)
                {
                    double fRange = (double)f / (double)frequencyGroups;
                    double fCurrentHz = frequencyMax * fRange;
                    for (int i=0; i < input.Length; i++)
                    {
                        double time = (double)input[i].Time * timeScale;
                        FrequencyOutput frequency = new FrequencyOutput()
                        {
                            Time = time,
                            Frequency = fCurrentHz,
                            Value = input[i].Value
                        };
                        frequencies.Add(frequency);
                    }
                }
                chart.ChartFrequencies(frequencies);
                return chart;
            }

            //public static double[] InverseHGT(FrequencyChart chart, int harmonics)
            //{
            //    for (int z=0;z<harmonics;z++)
            //    {
            //        chart.Frequencies[z].
            //    }
            //}

            public static Vector<double>[] HGT(double[] input, int frequencyGroups, double frequencyMax, out double inputMax, double timeScale = 1.0, double frequencyScale = 1.0)
            {
                Vector<double>[] vectors = new Vector<double>[frequencyGroups];
                inputMax = input.Max();
                for (int f=0;f<frequencyGroups;f++)
                {
                    double fRange = (double)f / (double)frequencyGroups;
                    double fCurrentHz = frequencyMax * fRange;
                    vectors[f] = new Vector<double>(2);
                    for (int i=0;i<input.Length;i++)
                    {
                        double time = (double)i / (double)input.Length * timeScale;
                        double piFRange = 2.0 * Math.PI * fCurrentHz;
                        double cosF = Math.Cos(piFRange * time);
                        double sineF = Math.Sin(piFRange * time);
                        double inputCurrent = input[i];
                        //inputCurrent /= inputMax;
                        double fX = cosF * inputCurrent;
                        double fY = -1.0 * sineF * inputCurrent;
                        vectors[f].Values[0] += fX;
                        vectors[f].Values[1] += fY;
                        //double mag = Math.Sqrt(fX * fX + fY * fY);
                        //frequencies[f] += mag;
                    }
                    vectors[f].Values[0] /= (double)input.Length;
                    vectors[f].Values[1] /= (double)input.Length;
                    //frequencies[f] = Math.Sqrt(vectors[f].Values[0] * vectors[f].Values[0] + vectors[f].Values[1] * vectors[f].Values[1]) / (double)input.Length * frequencyScale;
                }
                return vectors;
            }

            public static double[] InverseHGT(Vector<double>[] frequencies, int samples, double frequencyMax, double inputMax, double timeScale = 1.0, double frequencyScale = 1.0, int harmonics = 0)
            {
                int frequencyGroups = (harmonics > 0) ? harmonics : frequencies.Length;
                //int frequencyGroups = frequencies.Length;
                int freqGroupsMax = frequencies.Length;

                double[] output = new double[samples];
                Vector<double>[] vectors = new Vector<double>[samples];
                for (int i=0; i < samples;i++)
                {
                    vectors[i] = new Vector<double>(2);
                    if (harmonics == 0)
                    {
                        for (int f = 0; f < frequencyGroups; f++)
                        {
                            double fRange = (double)f / (double)freqGroupsMax;
                            double fCurrentHz = frequencyMax * fRange;
                            double time = (double)i / (double)samples * timeScale;
                            double piFRange = 2.0 * Math.PI * fCurrentHz;
                            double cosF = Math.Cos(piFRange * time);
                            double sineF = Math.Sin(piFRange * time);

                            Vector<double> inputCurrent = frequencies[f];
                            //double inputCurrent = frequencies[f];
                            //inputCurrent *= inputMax;
                            double fX = (cosF * inputCurrent.Values[0]);
                            double fY = (-1.0 * sineF * inputCurrent.Values[1]);
                            vectors[i].Values[0] += fX;
                            vectors[i].Values[1] += fY;
                        }
                    }
                    else
                    {
                        for (int f = 0; f < frequencyGroups; f++)
                        {
                            double fRange = (double)f / (double)freqGroupsMax;
                            double fCurrentHz = frequencyMax * fRange;
                            double time = (double)i / (double)samples * timeScale;
                            double piFRange = 2.0 * Math.PI * fCurrentHz;
                            double cosF = Math.Cos(piFRange * time);
                            double sineF = Math.Sin(piFRange * time);

                            Vector<double> inputCurrent = frequencies[f];
                            //double inputCurrent = frequencies[f];
                            //inputCurrent *= inputMax;
                            double fX = (cosF * inputCurrent.Values[0]);
                            double fY = (-1.0 * sineF * inputCurrent.Values[1]);
                            vectors[i].Values[0] += fX;
                            vectors[i].Values[1] += fY;
                        }
                        for (int f = 0; f < frequencyGroups; f++)
                        {
                            int fTail = (frequencies.Length - 1) - f;
                            double fRange = (double)fTail / (double)freqGroupsMax;
                            double fCurrentHz = frequencyMax * fRange;
                            double time = (double)i / (double)samples * timeScale;
                            double piFRange = 2.0 * Math.PI * fCurrentHz;
                            double cosF = Math.Cos(piFRange * time);
                            double sineF = Math.Sin(piFRange * time);

                            Vector<double> inputCurrent = frequencies[fTail];
                            //double inputCurrent = frequencies[f];
                            //inputCurrent *= inputMax;
                            double fX = (cosF * inputCurrent.Values[0]);
                            double fY = (-1.0 * sineF * inputCurrent.Values[1]);
                            vectors[i].Values[0] += fX;
                            vectors[i].Values[1] += fY;
                        }
                    }
                    double value = (vectors[i].Values[0] + vectors[i].Values[1]);
                    /*
                    if (i == 0 || i == samples - 1)
                        value /= (double)samples;
                    else
                        value /= ((double)samples / 2.0);
                        */
                    output[i] = value;
                }
                return output;
            }

            public static double[] SumMagnitudes(Vector<double>[,] input, bool clampMinimum = false)
            {
                int K = input.GetUpperBound(0) + 1;
                int N = input.GetUpperBound(1) + 1;
                double[] output = new double[K];
                for (int k=0;k<K;k++)
                {
                    double sumX = 0.0;
                    double sumY = 0.0;
                    for (int n=0;n<N;n++)
                    {
                        Vector<double> v = input[k, n];
                        sumX += v.Values[0];
                        sumY += v.Values[1];
                    }
                    //double sum = Math.Sqrt(v.Values[0] * v.Values[0] + v.Values[1] * v.Values[1]) / (double)N;
                    //double sum = Math.Sqrt(v.Values[0] * v.Values[0] + v.Values[1] * v.Values[1]);
                    double sum = Math.Sqrt(sumX * sumX + sumY * sumY) / (double)N;
                    if (clampMinimum && sum < 0.0001)
                        sum = 0.0001;
                    output[k] = sum;
                }
                return output;
            }

            public static Vector<double>[] SumVectors(Vector<double>[,] input)
            {
                int K = input.GetUpperBound(0) + 1;
                int N = input.GetUpperBound(1) + 1;
                Vector<double>[] output = new Vector<double>[K];
                for (int k = 0; k < K; k++)
                {
                    double sumX = 0.0;
                    double sumY = 0.0;
                    for (int n = 0; n < N; n++)
                    {
                        Vector<double> v = input[k, n];
                        sumX += v.Values[0];
                        sumY += v.Values[1];
                    }
                    output[k] = new Vector<double>(2) { Values = new double[] { sumX, sumY } };
                }
                return output;
            }

            public static Vector<double>[,] DFTCapture(double[] input)
            {
                int N = input.Length;
                int K = input.Length;
                Vector<double>[,] output = new Vector<double>[K,N];
                for (int k = 0; k < K; k++)
                {
                    for (int n = 0; n < N; n++)
                    {
                        output[k, n] = new Vector<double>(2);
                        //output[k, n].Values.Add(0.0);
                        //output[k, n].Values.Add(0.0);
                        double p = (2.0 * Math.PI);
                        //double kValue = (double)k / (double)K;
                        double kValue = (double)k;
                        double nValue = (double)n / (double)N;
                        double z = p * nValue * kValue;
                        //double inputClamped = input[n] / max;
                        double inputClamped = input[n];
                        output[k,n].Values[0] = Math.Cos(z) * inputClamped;
                        output[k,n].Values[1] = inputClamped * Math.Sin(z);
                    }
                }
                return output;
            }

            public static double[] CreateDFTScaleMultiplier(int N, int K)
            {
                Vector<double>[,] r1 = new Vector<double>[K, N];
                for (int k=0;k<K;k++)
                {
                    for (int n=0;n<N;n++)
                    {
                        r1[k, n] = new Vector<double>(2);
                        double p = (2.0 * Math.PI);
                        double kValue = (double)k / (double)K * (double)N;
                        double nValue = (double)n / (double)N;
                        double z = p * nValue * kValue;
                        r1[k, n].Values[0] = 100.0 * Math.Cos(z);
                        r1[k, n].Values[1] = 100.0 * Math.Sin(z);
                    }
                }
                double[] r2 = SumMagnitudes(r1, true);
                //List<List<double>> output = new List<List<double>>();
                double[] output = new double[r2.Length];
                double[] r3 = new double[r2.Length];
                int total = 0;
                for (int z=0;z<r3.Length;z++)
                {
                    if (r2[z] == 0.0001)
                        output[z] = 0.0001;
                    else
                    {
                        output[z] = 1.0 / (r2[z]);
                    }
                }
                return output;
            }

            public static Vector<double>[,] VODFTCapture(double[] input, int K)
            {
                int N = input.Length;
                Vector<double>[,] output = new Vector<double>[K, N];
                for (int k = 0; k < K; k++)
                {
                    for (int n = 0; n < N; n++)
                    {
                        output[k, n] = new Vector<double>(2);
                        double p = (2.0 * Math.PI);
                        //double kValue = (double)k / (double)K * (double)N;
                        double kValue = (double)k;
                        //double kValue = (double)k / (double)K;
                        double nValue = (double)n / (double)N;
                        double z = p * nValue * kValue;
                        double inputClamped = input[n];
                        output[k, n].Values[0] = Math.Cos(z) * inputClamped;
                        output[k, n].Values[1] = inputClamped * Math.Sin(z);
                    }
                }
                return output;
            }

            public static Vector<double>[] DFT(double[] input)
            {
                double min = Math.Abs(input.Min());
                double max = input.Max();
                max = (max > min) ? max : min;
                int N = input.Length;
                int K = input.Length;
                Vector<double>[] output = new Vector<double>[K];
                for (int k = 0; k < output.Length; k++)
                {
                    output[k] = new Vector<double>(2);
                    //output[k].Values.Add(0.0);
                    //output[k].Values.Add(0.0);
                }
                for (int k = 0; k < K; k++)
                {
                    for (int n = 0; n < N; n++)
                    {
                        double p = (2.0 * Math.PI);
                        double kValue = (double)k;
                        double nValue = (double)n / (double)N;
                        double z = p * nValue * kValue;
                        //double inputClamped = input[n] / max;
                        double inputClamped = input[n];
                        output[k].Values[0] += Math.Cos(z) * inputClamped;
                        output[k].Values[1] += -1.0 * inputClamped * Math.Sin(z);
                    }
                }
                return output;
            }

            public static Vector<double>[] VODFT(double[] input, int K)
            {
                double min = Math.Abs(input.Min());
                double max = input.Max();
                max = (max > min) ? max : min;
                int N = input.Length;
                Vector<double>[] output = new Vector<double>[K];
                for (int k=0;k<output.Length;k++)
                {
                    output[k] = new Vector<double>(2);
                }
                for (int k = 0; k < K; k++)
                {
                    for (int n=0; n < N; n++)
                    {
                        double kValue = (double)k / (double)K;
                        double nValue = (double)n / (double)N;
                        double p = (2.0 * Math.PI);
                        double z = p * kValue * nValue;
                        //double inputClamped = input[n] / max;
                        double inputClamped = input[n];
                        output[k].Values[0] += Math.Cos(z) * inputClamped;
                        output[k].Values[1] += -1.0 * inputClamped * Math.Sin(z);
                    }
                }
                return output;
            }

            public static double[,] InverseDFTCaptureHarmonicAll(Vector<double>[,] points)
            {
                int K = points.GetUpperBound(0) + 1;
                int N = points.GetUpperBound(1) + 1;
                int HMax = K / 2;
                double[,] outputfinal = new double[HMax, N];
                for (int H=0;H<HMax;H++)
                {
                    for (int n = 0; n < N; n++)
                    {
                        double value = 0.0;
                        for (int k = 0; k < H; k++)
                        {
                            double xval = points[k, n].Values[0];
                            double yval = points[k, n].Values[1];
                            double magnitude = Math.Sqrt(xval * xval + yval * yval);
                            //double kValue = (double)k / (double)K;
                            double kValue = (double)k / (double)K * (double)N;
                            double nValue = (double)n / (double)N;
                            double p = 2.0 * Math.PI;
                            double z = p * kValue * nValue;
                            double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                            value += v;
                        }
                        for (int k = K - 1; k >= (K - 1) - H; k--)
                        {
                            double xval = points[k, n].Values[0];
                            double yval = points[k, n].Values[1];
                            double magnitude = Math.Sqrt(xval * xval + yval * yval);
                            //double kValue = (double)k / (double)K;
                            double kValue = (double)k / (double)K * (double)N;
                            double nValue = (double)n / (double)N;
                            double p = 2.0 * Math.PI;
                            double z = p * kValue * nValue;
                            double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                            value += v;
                        }
                        outputfinal[H, n] = value / (double)K;
                    }
                }
                return outputfinal;
            }

            public static double[] InverseDFTCaptureHarmonicBySums(Vector<double>[,] points, int H)
            {
                int K = points.GetUpperBound(0) + 1;
                int N = points.GetUpperBound(1) + 1;
                Vector<double>[] freqSums = SumVectors(points);
                double[] output = new double[N];
                for (int n = 0; n < N; n++)
                {
                    double value = 0.0;
                    int NewK = 0;
                    for (int k = 0; k < H; k++)
                    {
                        Vector<double> magnitude = freqSums[k];
                        //double kValue = (double)k / (double)K;
                        //double kValue = (double)k / (double)K * (double)N;
                        double kValue = (double)k;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * (magnitude.Values[0]) + Math.Sin(z) * (magnitude.Values[1]);
                        value += v;
                        NewK++;
                    }
                    //for (int k = K - 1; k >= (K - 1) - H; k--)
                    //{
                    //    double magnitude = freqSums[k];
                    //    //double kValue = (double)k / (double)K;
                    //    double kValue = (double)k / (double)K * (double)N;
                    //    double nValue = (double)n / (double)N;
                    //    double p = 2.0 * Math.PI;
                    //    double z = p * kValue * nValue;
                    //    double v = Math.Cos(z) * magnitude + Math.Sin(z) * magnitude;
                    //    value += v;
                    //    NewK++;
                    //}
                    //output[n] = value / (double)K;
                    output[n] = value / ((double)N * ((double)K/(double)N));
                }
                return output;
            }

            public static double[] InverseDFTHarmonic(Vector<double>[] points, int H, int N)
            {
                int K = points.GetUpperBound(0) + 1;
                double[] output = new double[N];
                for (int z=0;z<output.Length;z++)
                {
                    double xval = points[z].Values[0];
                    double yval = points[z].Values[1];
                    output[z] = Math.Sqrt(xval * xval + yval * yval);
                }
                int h1 = K / 2 - H;
                int h2 = K / 2 + H - 1;
                for (int n = 0; n < N; n++)
                {
                    double value = 0.0;
                    int NewK = 0;
                    for (int k = h1; k <= h2; k++)
                    {
                        double xval = points[k].Values[0];
                        double yval = points[k].Values[1];
                        //double kValue = (double)k / (double)K;
                        double kValue = (double)k / (double)K * (double)N;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                        value += v;
                        NewK++;
                    }
                    //output[n] = value / (double)K;
                    int n2 = N - n - 1;
                    output[n2] = value / (double)NewK;
                }
                return output;
            }  

            public static double[] InverseDFTCaptureHarmonic(Vector<double>[,] points, int H)
            {
                int K = points.GetUpperBound(0) + 1;
                int N = points.GetUpperBound(1) + 1;
                double[] output = new double[N];
                int h1 = K / 2 - H;
                int h2 = K / 2 + H;
                for (int n = 0; n < N; n++)
                {
                    double value = 0.0;
                    int NewK = 0;
                    for (int k = h1; k <= h2; k++)
                    {
                        double xval = points[k, n].Values[0];
                        double yval = points[k, n].Values[1];
                        double magnitude = Math.Sqrt(xval * xval + yval * yval);
                        //double kValue = (double)k / (double)K;
                        double kValue = (double)k / (double)K * (double)N;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                        value += v;
                        NewK++;
                    }
                    //output[n] = value / (double)K;
                    output[n] = value / (double)NewK;
                }
                return output;
            }

            public static double[] InverseDFTCaptureHarmonicReverse(Vector<double>[,] points, int H)
            {
                int K = points.GetUpperBound(0) + 1;
                int N = points.GetUpperBound(1) + 1;
                int KHalf = K / 2;
                double[] output = new double[N];
                for (int n = 0; n < N; n++)
                {
                    double value = 0.0;
                    int NewK = 0;
                    for (int k = H; k < KHalf; k++)
                    {
                        double xval = points[k, n].Values[0];
                        double yval = points[k, n].Values[1];
                        double magnitude = Math.Sqrt(xval * xval + yval * yval);
                        //double kValue = (double)k / (double)K;
                        double kValue = (double)k / (double)K * (double)N;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                        value += v;
                        NewK++;
                    }
                    for (int k = KHalf + H; k >= KHalf; k--)
                    {
                        double xval = points[k, n].Values[0];
                        double yval = points[k, n].Values[1];
                        double magnitude = Math.Sqrt(xval * xval + yval * yval);
                        //double kValue = (double)k / (double)K;
                        double kValue = (double)k / (double)K * (double)N;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                        value += v;
                        NewK++;
                    }
                    //output[n] = value / (double)K;
                    output[n] = value / (double)NewK;
                }
                return output;
            }

            public static double[] InverseDFTCaptureHarmonicMiddleOut(Vector<double>[,] points, int H)
            {
                int K = points.GetUpperBound(0) + 1;
                int N = points.GetUpperBound(1) + 1;
                int KHalf = K / 2;
                int KFirst = H;
                int KSecond = KHalf - H;
                int KThird = KHalf + H;
                int KFourth = (K - 1) - H;
                double[] output = new double[N];
                for (int n = 0; n < N; n++)
                {
                    double value = 0.0;
                    int NewK = 0;
                    for (int k = 0; k < KFirst; k++)
                    {
                        double xval = points[k, n].Values[0];
                        double yval = points[k, n].Values[1];
                        double magnitude = Math.Sqrt(xval * xval + yval * yval);
                        //double kValue = (double)k / (double)K;
                        double kValue = (double)k / (double)K * (double)N;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                        value += v;
                        NewK++;
                    }
                    for (int k = KSecond; k <= KThird; k++)
                    {
                        double xval = points[k, n].Values[0];
                        double yval = points[k, n].Values[1];
                        double magnitude = Math.Sqrt(xval * xval + yval * yval);
                        //double kValue = (double)k / (double)K;
                        double kValue = (double)k / (double)K * (double)N;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                        value += v;
                        NewK++;
                    }
                    for (int k = KFourth; k < K; k++)
                    {
                        double xval = points[k, n].Values[0];
                        double yval = points[k, n].Values[1];
                        double magnitude = Math.Sqrt(xval * xval + yval * yval);
                        //double kValue = (double)k / (double)K;
                        double kValue = (double)k / (double)K * (double)N;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                        value += v;
                        NewK++;
                    }
                    //output[n] = value / (double)K;
                    output[n] = value / (double)NewK;
                }
                return output;
            }

            public static double[] InverseDFTCaptureHarmonicSingular(Vector<double>[,] points, int H)
            {
                int K = points.GetUpperBound(0) + 1;
                int N = points.GetUpperBound(1) + 1;
                double[] output = new double[N];
                for (int n = 0; n < N; n++)
                {
                    double value = 0.0;
                    double xval = points[H, n].Values[0];
                    double yval = points[H, n].Values[1];
                    double magnitude = Math.Sqrt(xval * xval + yval * yval);
                    //double kValue = (double)k / (double)K;
                    double kValue = (double)H / (double)K * (double)N;
                    double nValue = (double)n / (double)N;
                    double p = 2.0 * Math.PI;
                    double z = p * kValue * nValue;
                    double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                    value += v;

                    xval = points[K - 1 - H, n].Values[0];
                    yval = points[K - 1 - H, n].Values[1];
                    magnitude = Math.Sqrt(xval * xval + yval * yval);
                    //double kValue = (double)k / (double)K;
                    kValue = (double)(K - 1 - H) / (double)K * (double)N;
                    nValue = (double)n / (double)N;
                    z = p * kValue * nValue;
                    v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                    value += v;

                    output[n] = value / (double)K;
                }
                return output;
            }

            public static double InverseDFTCaptureHarmonicSingularDistance(Vector<double>[,] points, int H)
            {
                int K = points.GetUpperBound(0) + 1;
                int N = points.GetUpperBound(1) + 1;
                double[] output = new double[N];
                for (int n = 0; n < N; n++)
                {
                    double value = 0.0;
                    double xval = points[H, n].Values[0];
                    double yval = points[H, n].Values[1];
                    double magnitude = Math.Sqrt(xval * xval + yval * yval);
                    //double kValue = (double)k / (double)K;
                    double kValue = (double)H / (double)K * (double)N;
                    double nValue = (double)n / (double)N;
                    double p = 2.0 * Math.PI;
                    double z = p * kValue * nValue;
                    double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                    value += v;

                    xval = points[K - 1 - H, n].Values[0];
                    yval = points[K - 1 - H, n].Values[1];
                    magnitude = Math.Sqrt(xval * xval + yval * yval);
                    //double kValue = (double)k / (double)K;
                    kValue = (double)(K - 1 - H) / (double)K * (double)N;
                    nValue = (double)n / (double)N;
                    z = p * kValue * nValue;
                    v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                    value += v;

                    output[n] = value / (double)K;
                }
                double newoutput = output.Max() - output.Min();
                return newoutput;
            }

            public static double[] InverseDFTCapture(Vector<double>[,] points)
            {
                int K = points.GetUpperBound(0) + 1;
                int N = points.GetUpperBound(1) + 1;
                double[] output = new double[N];
                for (int n=0;n<N;n++)
                {
                    double value = 0.0;
                    for (int k=0;k<K;k++)
                    {
                        double xval = points[k, n].Values[0];
                        double yval = points[k, n].Values[1];
                        double magnitude = Math.Sqrt(xval * xval + yval * yval);
                        //double kValue = (double)k / (double)K;
                        double kValue = (double)k / (double)K * (double)N;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * xval + Math.Sin(z) * yval;
                        value += v;
                    }
                    output[n] = value / (double)K;
                }
                return output;
            }

            public static double[] InverseVODFTHarmonic(Vector<double>[] points, int N, int H)
            {
                double[] input = new double[N];
                int K = points.Length;
                for (int n = 0; n < N; n++)
                {
                    double value = 0.0;
                    for (int k = 0; k < H; k++)
                    {
                        double xval = points[k].Values[0];
                        double yval = points[k].Values[1];
                        double magnitude = Math.Sqrt(xval * xval + yval * yval);
                        double kValue = (double)k / (double)K;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * magnitude + Math.Sin(z) * magnitude;
                        value += v;
                    }
                    input[n] = value / (double)N / (double)K;
                }
                return input;
            }

            public static double[] InverseVODFT(Vector<double>[] points, int N)
            {
                double[] input = new double[N];
                int K = points.Length;
                for (int n=0;n<N;n++)
                {
                    double value = 0.0;
                    for (int k = 0; k < K; k++)
                    {
                        double xval = points[k].Values[0];
                        double yval = points[k].Values[1];
                        double magnitude = Math.Sqrt(xval * xval + yval * yval);
                        double kValue = (double)k / (double)K;
                        double nValue = (double)n / (double)N;
                        double p = 2.0 * Math.PI;
                        double z = p * kValue * nValue;
                        double v = Math.Cos(z) * magnitude + Math.Sin(z) * magnitude;
                        value += v;
                    }
                    input[n] = value / (double)N / (double)K;
                }
                return input;
            }

            public static double NormalDistributionPoint(double x, double mean, double stdDev)
            {
                return (1.0 / (Math.Abs(stdDev) * Math.Sqrt(2.0 * Math.PI))) * Math.Exp((-1.0 * Math.Pow(x - mean, 2.0)) / (2.0 * stdDev * stdDev));
            }

            public static double ZScoreNormalDistribution(double x, double mean, double stdDev)
            {
                return (x - mean) / stdDev;
            }

            public static double ExponentialDistributionPoint(double x, double mean)
            {
                return (1.0 / mean) * Math.Exp(-1.0 * x / mean);
            }

            public static double BinomialDistribution(double successes, double trials, double probabilityOfSuccessInOneTrial)
            {
                return (Factorial(trials) / Factorial(successes) * Factorial(trials - successes)) * Math.Pow(probabilityOfSuccessInOneTrial, successes) * Math.Pow(1 - probabilityOfSuccessInOneTrial, trials - successes);
            }

            public static double PoissonDistribution(double occurrences, double averageOccurrencesPerInterval)
            {
                return Math.Pow(averageOccurrencesPerInterval, occurrences) * Math.Exp(averageOccurrencesPerInterval) / Factorial(occurrences);
            }

            public static double UniformDistribution(double a, double b)
            {
                return 1.0 / (b - a);
            }

            public static double Derivative(Func<double, double> function, double x, double h)
            {
                return (function(x + h) - function(x)) / h;
            }

            public static double SecondDerivative(Func<double, double> function, double x, double h)
            {
                return (function(x + h) - 2 * function(x) + function(x - h)) / (h * h);
            }

            public static double ThirdDerivative(Func<double, double> function, double x, double h)
            {
                return (function(x + 3 * h) - 3 * function(x + 2 * h) + 3 * function(x + h) - function(x)) / (h * h * h);
            }

            public static double PartialDerivativeX(Func<double, double, double> function, double x, double y, double h)
            {
                return (function(x + h, y) - function(x, y)) / h;
            }

            public static double PartialDerivativeY(Func<double, double, double> function, double x, double y, double h)
            {
                return (function(x, y + h) - function(x, y)) / h;
            }

            public static void TestAngles()
            {
                double z = -20.0;
                Debug.Log("Cosine");
                while (z <= 20.0)
                {
                    double angle = z;
                    double result = AngleCosine(angle);
                    Debug.Log(z + " - " + result);

                    z += 0.01;
                }
                z = -20.0;
                Debug.Log("Sine");
                while (z <= 20.0)
                {
                    double angle = z;
                    double result = AngleSine(angle);
                    Debug.Log(z + " - " + result);

                    z += 0.01;
                }

            }

            public static decimal PiConstant = 3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442881097566593344612847564823378678316527120190914564856692346034861045432664821339360726024914127372458700660631558817488152092096282925409171536436789259036001133053M;

            public static decimal CosineTaylor(decimal angle, int terms)
            {
                while (angle > 2.0M)
                    angle -= 2.0M;
                while (angle < -2.0M)
                    angle += 2.0M;
                bool isPositive = false;
                decimal result = 1.0M;
                decimal xValue = (angle * PiConstant) / 2.0M;
                for (int z=1;z<=terms;z++)
                {
                    int power = z * 2;
                    decimal factorial = Factorial((decimal)power);
                    decimal powerValue = PowerInt(xValue, power);
                    decimal r = powerValue / factorial;
                    if (isPositive)
                        result += r;
                    else
                        result -= r;
                    isPositive = !isPositive;
                }
                return result;
            }

            public static decimal CosineTaylorPi(decimal angle, int terms)
            {
                while (angle > PiConstant)
                    angle -= PiConstant;
                while (angle < -PiConstant)
                    angle += PiConstant;
                bool isPositive = false;
                decimal result = 1.0M;
                decimal xValue = angle;
                for (int z = 1; z <= terms; z++)
                {
                    int power = z * 2;
                    decimal factorial = Factorial((decimal)power);
                    decimal powerValue = PowerInt(xValue, power);
                    decimal r = powerValue / factorial;
                    if (isPositive)
                        result += r;
                    else
                        result -= r;
                    isPositive = !isPositive;
                }
                return result;
            }

            public static decimal SineTaylor(decimal angle, int terms)
            {
                while (angle > 2.0M)
                    angle -= 2.0M;
                while (angle < -2.0M)
                    angle += 2.0M;
                bool isPositive = false;
                decimal xValue = (angle * PiConstant) / 2.0M;
                decimal result = xValue;
                for (int z = 1; z <= terms; z++)
                {
                    int power = z * 2 + 1;
                    decimal factorial = Factorial((decimal)power);
                    decimal powerValue = PowerInt(xValue, power);
                    decimal r = powerValue / factorial;
                    if (isPositive)
                        result += r;
                    else
                        result -= r;
                    isPositive = !isPositive;
                }
                return result;
            }

            public static decimal SineTaylorPi(decimal angle, int terms)
            {
                while (angle > PiConstant)
                    angle -= PiConstant;
                while (angle < PiConstant)
                    angle += PiConstant;
                bool isPositive = false;
                decimal xValue = angle;
                decimal result = xValue;
                for (int z = 1; z <= terms; z++)
                {
                    int power = z * 2 + 1;
                    decimal factorial = Factorial((decimal)power);
                    decimal powerValue = PowerInt(xValue, power);
                    decimal r = powerValue / factorial;
                    if (isPositive)
                        result += r;
                    else
                        result -= r;
                    isPositive = !isPositive;
                }
                return result;
            }

            public static decimal CalculatePi(decimal iterations)
            {
                decimal result = 0M;
                decimal i = 1;
                bool isPositive = true;
                for (decimal z=0M;z<iterations;z++)
                {
                    if (isPositive)
                        result += (decimal)1 / (decimal)i;
                    else
                        result -= (decimal)1 / (decimal)i;

                    isPositive = !isPositive;
                    i += 2;
                }
                result *= 4.0M;
                return result;
            }

            public static decimal CalculateNumberByPiCalculationMethod(decimal iterations, decimal multiplier = 4.0M, decimal i = 1, bool isPositive = true)
            {
                decimal result = 0M;
                //decimal i = 1;
                //bool isPositive = true;
                for (decimal z = 0M; z < iterations; z++)
                {
                    if (isPositive)
                        result += (decimal)1 / (decimal)i;
                    else
                        result -= (decimal)1 / (decimal)i;

                    isPositive = !isPositive;
                    i += 2;
                }
                result *= multiplier;
                return result;
            }

            public static decimal AngleCosine(decimal angle)
            {
                decimal abs = Math.Abs(angle);
                int angleInt = (int)angle;
                int absInt = (int)abs;
                int angleShift = (absInt % 2 == 0) ? absInt : absInt + 1;
                int signFlip = (angleShift % 4 == 0) ? 1 : -1;
                int signAngle = (angle < 0.0M) ? -1 : 1;
                angleShift *= signAngle;
                decimal shiftedAngle = angle - (decimal)angleShift;
                decimal shift8 = shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle;
                decimal shift6 = shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle;
                decimal shift4 = shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle;
                decimal shift2 = shiftedAngle * shiftedAngle;
                decimal sFlip2 = (decimal)signFlip;
                decimal coefficient = (2.0M / 9.0M);
                decimal coefficient2 = (11.0M / 9.0M);
                decimal term2 = (0.888M * shift2 - 2.444M * shift4 + 1.641M * shift6 - 0.085M * shift8) / 77.0M;
                decimal value = sFlip2 * ((coefficient * shift4 - coefficient2 * shift2 + 1) - term2);
                return value;
            }

            public static decimal AngleSine(decimal angle)
            {
                angle -= 1.0M;
                decimal abs = Math.Abs(angle);
                int angleInt = (int)angle;
                int absInt = (int)abs;
                int angleShift = (absInt % 2 == 0) ? absInt : absInt + 1;
                int signFlip = (angleShift % 4 == 0) ? 1 : -1;
                int signAngle = (angle < 0.0M) ? -1 : 1;
                angleShift *= signAngle;
                decimal shiftedAngle = angle - (decimal)angleShift;
                decimal shift8 = shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle;
                decimal shift6 = shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle;
                decimal shift4 = shiftedAngle * shiftedAngle * shiftedAngle * shiftedAngle;
                decimal shift2 = shiftedAngle * shiftedAngle;
                decimal sFlip2 = (decimal)signFlip;
                decimal coefficient = (2.0M / 9.0M);
                decimal coefficient2 = (11.0M / 9.0M);
                decimal term2 = (0.888M * shift2 - 2.444M * shift4 + 1.641M * shift6 - 0.085M * shift8) / 77.0M;
                decimal value = sFlip2 * ((coefficient * shift4 - coefficient2 * shift2 + 1) - term2);
                return value;
            }

            public static double AngleCosine(double angle)
            {
                double abs = Math.Abs(angle);
                int angleInt = (int)angle;
                int absInt = (int)abs;
                int angleShift = (absInt % 2 == 0) ? absInt : absInt + 1;
                int signFlip = (angleShift % 4 == 0) ? 1 : -1;
                int signAngle = (angle < 0.0) ? -1 : 1;
                angleShift *= signAngle;
                double shiftedAngle = angle - (double)angleShift;
                double shift8 = Math.Pow(shiftedAngle, 8.0);
                double shift6 = Math.Pow(shiftedAngle, 6.0);
                double shift4 = Math.Pow(shiftedAngle, 4.0);
                double shift2 = Math.Pow(shiftedAngle, 2.0);
                double sFlip2 = (double)signFlip;
                double coefficient = (2.0 / 9.0);
                double coefficient2 = (11.0 / 9.0);
                double term2 = (0.888 * shift2 - 2.444 * shift4 + 1.641 * shift6 - 0.085 * shift8)/ 77.0;
                double value = sFlip2 * ((coefficient * shift4 - coefficient2 * shift2 + 1) - term2);
                return value;
            }

            public static double AngleSine(double angle)
            {
                angle -= 1.0;
                double abs = Math.Abs(angle);
                int angleInt = (int)angle;
                int absInt = (int)abs;
                int angleShift = (absInt % 2 == 0) ? absInt : absInt + 1;
                int signFlip = (angleShift % 4 == 0) ? 1 : -1;
                int signAngle = (angle < 0.0) ? -1 : 1;
                angleShift *= signAngle;
                double shiftedAngle = angle - (double)angleShift;
                double shift8 = Math.Pow(shiftedAngle, 8.0);
                double shift6 = Math.Pow(shiftedAngle, 6.0);
                double shift4 = Math.Pow(shiftedAngle, 4.0);
                double shift2 = Math.Pow(shiftedAngle, 2.0);
                double sFlip2 = (double)signFlip;
                double coefficient = (2.0 / 9.0);
                double coefficient2 = (11.0 / 9.0);
                double term2 = (0.888 * shift2 - 2.444 * shift4 + 1.641 * shift6 - 0.085 * shift8) / 77.0;
                double value = sFlip2 * ((coefficient * shift4 - coefficient2 * shift2 + 1) - term2);
                return value;
            }

            public static double Sine(double theta)
            {
                return Math.Sin(theta);
            }

            public static double Cosine(double theta)
            {
                return Math.Cos(theta);
            }

            public static double Tangent(double theta)
            {
                return Math.Tan(theta);
            }

            public static double Secant(double theta)
            {
                return 1.0 / Math.Cos(theta);
            }

            public static double Cosecant(double theta)
            {
                return 1.0 / Math.Sin(theta);
            }

            public static double Cotangent(double theta)
            {
                return Math.Cos(theta) / Math.Sin(theta);
            }

            public static decimal PowerInt(decimal value, int power)
            {
                if (power == 0) return 0M;
                if (power == 1) return value;
                decimal result = value;
                for (int z=2;z<=power;z++)
                {
                    result *= value;
                }
                return result;
            }

            public static decimal Factorial(decimal value)
            {
                decimal v = (decimal)value;
                decimal sum = 1;
                for (decimal z = 1; z <= v; z+=1M)
                {
                    sum *= z;
                }
                return (decimal)sum;
            }

            public static double Factorial(double value)
            {
                int v = (int)value;
                int sum = 1;
                for (int z=1;z<=v;z++)
                {
                    sum *= z;
                }
                return (double)sum;
            }

            public static double SquareRoot(double input)
            {
                return Math.Sqrt(input);
            }

            public static double Log(double input)
            {
                return Math.Log(input);
            }

            public static double Log10(double input)
            {
                return Math.Log10(input);
            }

            public static double Ln(double input)
            {
                return Math.Log(input);
            }

            public static double Exp(double input)
            {
                return Math.Exp(input);
            }

            public static double ArcSine(double theta)
            {
                return Math.Asin(theta);
            }

            public static double ArcCosine(double theta)
            {
                return Math.Acos(theta);
            }

            public static double ArcTangent(double theta)
            {
                return Math.Atan(theta);
            }

            public static double ArcSecant(double theta)
            {
                return Math.Acos(1.0 / theta);
            }

            public static double ArcCosecant(double theta)
            {
                return Math.Asin(1.0 / theta);
            }

            public static double ArcCotangent(double theta)
            {
                return Math.Atan(1.0 / theta);
            }

            public static double Power(double x, double y)
            {
                return Math.Pow(x, y);
            }

            public static double And(double x, double y)
            {
                return (double)((int)x & (int)y);
            }

            public static double NotAnd(double x, double y)
            {
                return (double)(~((int)x & (int)y));
            }

            public static double Or(double x, double y)
            {
                return (double)((int)x | (int)y);
            }

            public static double NotOr(double x, double y)
            {
                return (double)(~((int)x | (int)y));
            }

            public static double Xor(double x, double y)
            {
                return (double)((int)x ^ (int)y);
            }

            public static double NotXor(double x, double y)
            {
                return (double)(~((int)x ^ (int)y));
            }

            public static double Not(double x)
            {
                return (double)(~(int)x);
            }

            public static double LeftShift(double x, double y)
            {
                return (double)((int)x << (int)y);
            }

            public static double RightShift(double x, double y)
            {
                return (double)((int)x >> (int)y);
            }

            public static ExpressionNodeEvaluationResult Power(ExpressionNode left, ExpressionNode right)
            {
                if (left.NodeType == ExpressionNodeType.Number && right.NodeType == ExpressionNodeType.Number)
                {
                    if (left.ValueSuffix == right.ValueSuffix)
                    {
                        // TO DO: Implement Decimal Power Function
                        double value = Math.Pow(left.ValueNumber, right.ValueNumber);
                        ExpressionNodeEvaluationResult result = new ExpressionNodeEvaluationResult();
                        result.Success = true;
                        result.ResultNode = left;
                        result.ResultNode.ValueNumber = value;
                        result.ResultNode.ValueString = value.ToString();
                        return result;
                    }
                }
                return new ExpressionNodeEvaluationResult() { Success = false };
            }

            public static ExpressionNodeEvaluationResult Multiply(ExpressionNode left, ExpressionNode right)
            {
                if (left == null || right == null)
                {
                    return null;
                }
                if (left.NodeType == ExpressionNodeType.Number && right.NodeType == ExpressionNodeType.Number)
                {
                    if (left.ValueSuffix == right.ValueSuffix)
                    {
                        double value = left.ValueNumber * right.ValueNumber;
                        ExpressionNodeEvaluationResult result = new ExpressionNodeEvaluationResult();
                        result.Success = true;
                        result.ResultNode = left;
                        result.ResultNode.ValueNumber = value;
                        result.ResultNode.ValueString = value.ToString();
                        return result;
                    }
                }
                return new ExpressionNodeEvaluationResult() { Success = false };
            }

            public static ExpressionNodeEvaluationResult Divide(ExpressionNode left, ExpressionNode right)
            {
                if (left == null || right == null)
                {
                    return null;
                }
                if (left.NodeType == ExpressionNodeType.Number && right.NodeType == ExpressionNodeType.Number)
                {
                    if (left.ValueSuffix == right.ValueSuffix)
                    {
                        double value = left.ValueNumber / right.ValueNumber;
                        ExpressionNodeEvaluationResult result = new ExpressionNodeEvaluationResult();
                        result.Success = true;
                        result.ResultNode = left;
                        result.ResultNode.ValueNumber = value;
                        result.ResultNode.ValueString = value.ToString();
                        return result;
                    }
                }
                return new ExpressionNodeEvaluationResult() { Success = false };
            }

            public static ExpressionNodeEvaluationResult Add(ExpressionNode left, ExpressionNode right)
            {
                if (left == null && right != null)
                {
                    double value = right.ValueNumber;
                    ExpressionNodeEvaluationResult result = new ExpressionNodeEvaluationResult();
                    result.Success = true;
                    result.ResultNode = right;
                    result.ResultNode.ValueNumber = value;
                    result.ResultNode.ValueString = value.ToString();
                    return result;
                }
                if (left != null && right == null)
                {
                    double value = left.ValueNumber;
                    ExpressionNodeEvaluationResult result = new ExpressionNodeEvaluationResult();
                    result.Success = true;
                    result.ResultNode = left;
                    result.ResultNode.ValueNumber = value;
                    result.ResultNode.ValueString = value.ToString();
                    return result;
                }
                if (left.NodeType == ExpressionNodeType.Number && right.NodeType == ExpressionNodeType.Number)
                {
                    if (left.ValueSuffix == right.ValueSuffix)
                    {
                        double value = left.ValueNumber + right.ValueNumber;
                        ExpressionNodeEvaluationResult result = new ExpressionNodeEvaluationResult();
                        result.Success = true;
                        result.ResultNode = left;
                        result.ResultNode.ValueNumber = value;
                        result.ResultNode.ValueString = value.ToString();
                        return result;
                    }
                }
                return new ExpressionNodeEvaluationResult() { Success = false };
            }

            public static ExpressionNodeEvaluationResult Subtract(ExpressionNode left, ExpressionNode right)
            {
                ExpressionNodeType[] types = { left.NodeType, right.NodeType };
                ExpressionNodeTypeCategory[] categories = { ExpressionNodeTypeCategories[left.NodeType], ExpressionNodeTypeCategories[right.NodeType] };
                if (left == null && right != null)
                {
                    double value = right.ValueNumber;
                    ExpressionNodeEvaluationResult result = new ExpressionNodeEvaluationResult();
                    result.Success = true;
                    result.ResultNode = right;
                    result.ResultNode.ValueNumber = value;
                    result.ResultNode.ValueString = value.ToString();
                    return result;
                }
                if (left != null && right == null)
                {
                    double value = left.ValueNumber;
                    ExpressionNodeEvaluationResult result = new ExpressionNodeEvaluationResult();
                    result.Success = true;
                    result.ResultNode = left;
                    result.ResultNode.ValueNumber = value;
                    result.ResultNode.ValueString = value.ToString();
                    return result;
                }
                if (left.NodeType == ExpressionNodeType.Number && right.NodeType == ExpressionNodeType.Number)
                {
                    if (left.ValueSuffix == right.ValueSuffix)
                    {
                        double value = left.ValueNumber - right.ValueNumber;
                        ExpressionNodeEvaluationResult result = new ExpressionNodeEvaluationResult();
                        result.Success = true;
                        result.ResultNode = left;
                        result.ResultNode.ValueNumber = value;
                        result.ResultNode.ValueString = value.ToString();
                        return result;
                    }
                }
                return new ExpressionNodeEvaluationResult() { Success = false };
            }

        }

        public List<ExpressionSubset> OperationGroups { get; set; }

        public static string RegexPattern = @"((?<List>list(?=\())|(?<Matrix>matrix(?=\())|(?<Vector>vector(?=\())|(?<Set>set(?=\())|(?<Function>(?<FunctionName>[A-Za-z]+)(?=\())|(?<Constant>(e|(pi)))|(?<UserVariable>(s|t|u|v|w|x|y|z))|(?<Number>(?<Value>[0-9\.]+)(?<Suffix>i)?)|(?<Complex>i)|(?<Exponent>\^)|(?<Modulo>\%)|(?<Multiply>\*)|(?<Divide>\/)|(?<Add>\+)|(?<Subtract>\-)|(?<LeftShift>\<\<)|(?<RightShift>\>\>))|(?<OpenParenthesis>\()|(?<CloseParenthesis>\))|(?<OpenSet>\{)|(?<CloseSet>\})|(?<OpenIndex>\[)|(?<CloseIndex>\])|(?<Equals>\=)|(?<Comma>\,)|(?<DoubleQuote>\"")";

        public static bool ContainsVariableName(ExpressionNodes input, string variableName)
        {
            for (int z=0;z<input.Nodes.Count;z++)
            {
                if (input.Nodes[z].NodeType == ExpressionNodeType.UserVariable)
                {
                    if (input.Nodes[z].ValueString == variableName) return true;
                }
                else
                {
                    if (input.Nodes[z].ChildNodes != null)
                        if (ContainsVariableName(input.Nodes[z].ChildNodes, variableName)) return true;
                }
            }
            return false;
        }

        public static ExpressionNodes ReplaceVariable(ExpressionNodes input, string variableName, double value, string suffix = "")
        {
            for (int z = 0; z < input.Nodes.Count; z++)
            {
                if (input.Nodes[z].NodeType == ExpressionNodeType.UserVariable)
                {
                    if (input.Nodes[z].ValueString == variableName)
                    {
                        input.Nodes[z].NodeType = ExpressionNodeType.Number;
                        input.Nodes[z].ValueString = value.ToString();
                        input.Nodes[z].ValueNumber = value;
                        input.Nodes[z].ValueSuffix = suffix;
                    }
                }
                else
                {
                    if (input.Nodes[z].ChildNodes != null)
                    {
                        input.Nodes[z].ChildNodes = ReplaceVariable(input.Nodes[z].ChildNodes, variableName, value);
                    }
                }
            }
            return input;
        }

        public static ExpressionResult Evaluate(ExpressionNodes input, int steps, List<ExpressionInputParameter> userParameters = null, double stepMin = -10.0, double stepMax = 10.0)
        {
            ExpressionResult result = new ExpressionResult();
            if (ContainsVariableName(input, "y"))
            {
                // Curve 3D
                result.ResultType = ExpressionResult.ExpressionResultType.Curve3D;
                result.Curve3D = new Vector3[steps + 1, steps + 1];
                double stepSize = (stepMax - stepMin) / (double)steps;
                int y1 = 0;
                int x1 = 0;
                for (double y = stepMin; y <= stepMax; y += stepSize)
                {
                    x1 = 0;
                    for (double x = stepMin; x <= stepMax; x += stepSize)
                    {
                        ExpressionNodes inputNew = input.Clone() as ExpressionNodes;
                        inputNew = ReplaceVariable(inputNew, "x", x);
                        inputNew = ReplaceVariable(inputNew, "y", y);
                        inputNew = EvaluateNodes(inputNew);
                        if (inputNew.Nodes.Count == 1 && inputNew.Nodes[0].NodeType == ExpressionNodeType.Number)
                        {
                            if (double.IsPositiveInfinity(inputNew.Nodes[0].ValueNumber))
                                result.Curve2D[x1] = new Vector3((float)x, (float)stepMax, (float)y);
                            else if (double.IsNegativeInfinity(inputNew.Nodes[0].ValueNumber))
                                result.Curve2D[x1] = new Vector3((float)x, (float)stepMin, (float)y);
                            else if (!double.IsNaN(inputNew.Nodes[0].ValueNumber))
                            {
                                result.Curve3D[x1, y1] = new Vector3((float)x, (float)inputNew.Nodes[0].ValueNumber, (float)y);
                            }
                            else
                            {
                                result.Curve3D[x1, y1] = new Vector3((float)x, (float)stepMin, (float)y);
                            }
                        }
                        x1++;
                    }
                    y1++;
                }
            }
            else if (ContainsVariableName(input, "x"))
            {
                // Curve 2D
                result.ResultType = ExpressionResult.ExpressionResultType.Curve2D;
                result.Curve2D = new Vector3[steps + 1];
                double stepSize = (stepMax - stepMin) / (double)steps;
                int x1 = 0;
                for (double x = stepMin; x <= stepMax; x += stepSize)
                {
                    ExpressionNodes inputNew = input.Clone() as ExpressionNodes;
                    inputNew = ReplaceVariable(inputNew, "x", x);
                    inputNew = EvaluateNodes(inputNew);
                    if (inputNew.Nodes.Count == 1 && inputNew.Nodes[0].NodeType == ExpressionNodeType.Number)
                    {
                        if (double.IsPositiveInfinity(inputNew.Nodes[0].ValueNumber))
                            result.Curve2D[x1] = new Vector3((float)x, (float)stepMax, 0f);
                        else if (double.IsNegativeInfinity(inputNew.Nodes[0].ValueNumber))
                            result.Curve2D[x1] = new Vector3((float)x, (float)stepMin, 0f);
                        else if (!double.IsNaN(inputNew.Nodes[0].ValueNumber))
                            result.Curve2D[x1] = new Vector3((float)x, (float)inputNew.Nodes[0].ValueNumber, 0f);
                        else
                            result.Curve2D[x1] = new Vector3((float)x, (float)stepMin, 0f);
                    }
                    x1++;
                }
            }
            else
            {
                // Return Scalar
                // TO DO: Also need to check for a singular Matrix or Vector output
                result.ResultType = ExpressionResult.ExpressionResultType.Scalar;
                ExpressionNodes inputNew = input.Clone() as ExpressionNodes;
                inputNew = EvaluateNodes(inputNew);
                if (inputNew.Nodes.Count == 1 && inputNew.Nodes[0].NodeType == ExpressionNodeType.Number)
                {
                    result.ScalarValue = inputNew.Nodes[0].ValueNumber;
                }
            }
            return result;
        }

        public static ExpressionNodes Parse(string input, ExpressionResultType resultType = ExpressionResultType.Scalar, List<ExpressionInputParameter> userParameters = null)
        {
            input = input.Replace(" ", "").ToLower();
            Regex regex = new Regex(RegexPattern, RegexOptions.IgnoreCase);
            int placeholder = 0;
            string[] groupNames = regex.GetGroupNames().Where(x => !Int32.TryParse(x, out placeholder)).ToArray();
            MatchCollection collection = regex.Matches(input);
            ExpressionNodes nodes = new ExpressionNodes();
            foreach (Match match in collection)
            {
                foreach(string g in groupNames)
                {
                    if (match.Groups[g].Success && g != "Value" && g != "Suffix" && g != "FunctionName")
                    {
                        Group gg = match.Groups[g];
                        ExpressionNode node;
                        if (g == "Function")
                        {
                            node = ParseNode(g, gg, new Group[] { match.Groups["FunctionName"] });
                        }
                        else if (g == "Number")
                        {
                            node = ParseNode(g, match.Groups["Value"], new Group[] { match.Groups["Suffix"] });
                        }
                        else
                        {
                            node = ParseNode(g, gg);
                        }

                        nodes.Nodes.Add(node);
                        Debug.Log(node.ToString());
                    }
                }
            }
            nodes.Nodes = nodes.Nodes.OrderBy(x => x.OriginalStartIndex).ToList();

            for(int z=0;z<nodes.Nodes.Count;z++)
            {
                ExpressionNode node = nodes.Nodes[z];
                if (node.NodeType == ExpressionNodeType.Constant)
                {
                    foreach(KeyValuePair<string, double> constant in BuiltInMethods.ConstantList)
                    {
                        if (constant.Key.ToLower() == node.ValueString.ToLower())
                        {
                            node.ValueString = constant.Value.ToString();
                            node.ValueNumber = constant.Value;
                            node.ValueSuffix = "";
                            node.NodeType = ExpressionNodeType.Number;
                        }
                    }
                }
                /*
                else if (node.NodeType == ExpressionNodeType.UserVariable)
                {
                    foreach (string variable in BuiltInMethods.UserVariableList)
                    {
                        if (variable.ToLower() == node.ValueString.ToLower())
                        {
                            foreach(ExpressionInputParameter p in userParameters)
                            {
                                if (p.Parameter.ToString().ToLower() == variable.ToLower())
                                {
                                    node.ValueString = p.Value.ToString();
                                    node.ValueNumber = p.Value;
                                }
                            }
                        }
                    }
                }
                */
                nodes.Nodes[z] = node;
            }


            nodes = OrganizeContainers(nodes, ExpressionNodeType.OpenParenthesis, ExpressionNodeType.CloseParenthesis, ExpressionNodeType.ExpressionContainer);
            nodes = OrganizeContainers(nodes, ExpressionNodeType.OpenSet, ExpressionNodeType.CloseSet, ExpressionNodeType.SetContainer);
            nodes = OrganizeContainers(nodes, ExpressionNodeType.OpenIndex, ExpressionNodeType.CloseIndex, ExpressionNodeType.IndexContainer);
            nodes = ParseSubtraction(nodes);
            nodes = OrganizeFunctions(nodes);

            return EvaluateNodes(nodes);
        }

        public static ExpressionNodes EvaluateNodes(ExpressionNodes nodes, List<ExpressionInputParameter> userParameters = null)
        {
            int iterationMax = 100;
            int i = 0;
            while (!(IsSimplified(nodes) || i >= iterationMax))
            {
                nodes = EvaluateScalarType(nodes, ExpressionNodeType.Exponent, BuiltInMethods.Power);
                nodes = EvaluateScalarType(nodes, ExpressionNodeType.Multiply, BuiltInMethods.Multiply);
                nodes = EvaluateScalarType(nodes, ExpressionNodeType.Divide, BuiltInMethods.Divide);
                nodes = EvaluateScalarType(nodes, ExpressionNodeType.Add, BuiltInMethods.Add);
                nodes = EvaluateScalarType(nodes, ExpressionNodeType.Subtract, BuiltInMethods.Subtract);

                nodes = EvaluateFunctions(nodes);

                nodes = SimplifyNodes(nodes);

                i++;
            }
            return nodes;
        }

        public static ExpressionNodes SimplifyNodes(ExpressionNodes nodes)
        {
            for (int z=0;z<nodes.Nodes.Count;z++)
            {
                if (nodes.Nodes[z].NodeType == ExpressionNodeType.ExpressionContainer)
                {
                    if (nodes.Nodes[z].ChildNodes.Nodes.Count == 1)
                    {
                        nodes.Nodes[z] = nodes.Nodes[z].ChildNodes.Nodes[0];
                    }
                }
            }

            return nodes;
        }

        public static bool IsSimplified(ExpressionNodes nodes)
        {
            if (nodes.Nodes.Count == 1 && nodes.Nodes[0].NodeType == ExpressionNodeType.Number)
            {
                return true;
            }
            else if (nodes.Nodes.Count == 1 && nodes.Nodes[0].NodeType == ExpressionNodeType.ExpressionContainer)
            {
                return IsSimplified(nodes.Nodes[0].ChildNodes);
            }
            return false;
        }

        public static ExpressionNodes EvaluateFunctions(ExpressionNodes nodes)
        {
            for (int z=0;z<nodes.Nodes.Count;z++)
            {
                if (nodes.Nodes[z].ChildNodes != null)
                {
                    nodes.Nodes[z].ChildNodes = EvaluateFunctions(nodes.Nodes[z].ChildNodes);
                }
                if (nodes.Nodes[z].NodeType == ExpressionNodeType.Function)
                {
                    string functionName = nodes.Nodes[z].ValueString;
                    BuiltInMethods.FunctionType functionType = BuiltInMethods.FunctionList[functionName];
                    int paramCount = BuiltInMethods.FunctionParametersCount[functionType];
                    switch (functionType)
                    {
                        case BuiltInMethods.FunctionType.Scalar:
                            if (nodes.Nodes[z].ChildNodes.Nodes.Count == 1 && nodes.Nodes[z].ChildNodes.Nodes[0].NodeType == ExpressionNodeType.Number)
                            {
                                Func<double, double> func = BuiltInMethods.ScalarFunctionList[functionName];
                                double input = nodes.Nodes[z].ChildNodes.Nodes[0].ValueNumber;
                                string suffix = nodes.Nodes[z].ChildNodes.Nodes[0].ValueSuffix;
                                double output = func(input);
                                nodes.Nodes[z].ValueNumber = output;
                                nodes.Nodes[z].ValueString = output.ToString();
                                nodes.Nodes[z].ValueSuffix = suffix;
                                nodes.Nodes[z].NodeType = ExpressionNodeType.Number;
                            }
                            break;
                        case BuiltInMethods.FunctionType.DoubleInputScalar:
                            if (nodes.Nodes[z].ChildNodes.Nodes.Count == 3 && nodes.Nodes[z].ChildNodes.Nodes[0].NodeType == ExpressionNodeType.Number)
                            {
                                Func<double, double, double> func = BuiltInMethods.DoubleScalarFunctionList[functionName];
                                double input = nodes.Nodes[z].ChildNodes.Nodes[0].ValueNumber;
                                string suffix = nodes.Nodes[z].ChildNodes.Nodes[0].ValueSuffix;
                                double input2 = nodes.Nodes[z].ChildNodes.Nodes[2].ValueNumber;
                                double output = func(input, input2);
                                nodes.Nodes[z].ValueNumber = output;
                                nodes.Nodes[z].ValueString = output.ToString();
                                nodes.Nodes[z].ValueSuffix = suffix;
                                nodes.Nodes[z].NodeType = ExpressionNodeType.Number;
                            }
                            break;
                        case BuiltInMethods.FunctionType.TripleInputScalar:
                            if (nodes.Nodes[z].ChildNodes.Nodes.Count == 5 && nodes.Nodes[z].ChildNodes.Nodes[0].NodeType == ExpressionNodeType.Number)
                            {
                                Func<double, double, double, double> func = BuiltInMethods.TripleScalarFunctionList[functionName];
                                double input = nodes.Nodes[z].ChildNodes.Nodes[0].ValueNumber;
                                string suffix = nodes.Nodes[z].ChildNodes.Nodes[0].ValueSuffix;
                                double input2 = nodes.Nodes[z].ChildNodes.Nodes[2].ValueNumber;
                                double input3 = nodes.Nodes[z].ChildNodes.Nodes[4].ValueNumber;
                                double output = func(input, input2, input3);
                                nodes.Nodes[z].ValueNumber = output;
                                nodes.Nodes[z].ValueString = output.ToString();
                                nodes.Nodes[z].ValueSuffix = suffix;
                                nodes.Nodes[z].NodeType = ExpressionNodeType.Number;
                            }
                            break;
                            break;
                        case BuiltInMethods.FunctionType.Derivative:
                            break;
                        case BuiltInMethods.FunctionType.PartialDerivative:
                            break;
                        case BuiltInMethods.FunctionType.UserMethod:
                            break;
                        default:
                            break;
                    }

                }
            }

            return nodes;
        }

        public static ExpressionNodes ParseSubtraction(ExpressionNodes nodes)
        {
            for (int z = nodes.Nodes.Count - 1; z >= 0; z--)
            {
                if (nodes.Nodes[z].ChildNodes != null)
                {
                    nodes.Nodes[z].ChildNodes = ParseSubtraction(nodes.Nodes[z].ChildNodes);
                }
                if (z == 0 && nodes.Nodes[z].NodeType == ExpressionNodeType.Subtract)
                {
                    ExpressionNode right = nodes.Nodes[z + 1].Clone() as ExpressionNode;
                    right.ValueNumber *= -1.0;
                    nodes.Nodes[z] = right;
                    nodes.Nodes.RemoveAt(z + 1);
                }

                //if (nodes.Nodes[z].NodeType == ExpressionNodeType.Subtract)
                //{
                //    int zl = z - 1;
                //    int zr = z + 1;
                //    if (zl >= 0)
                //    {
                //        ExpressionNode left = nodes.Nodes[zl];
                //        ExpressionNode right = nodes.Nodes[zr];
                //        if (right.NodeType == ExpressionNodeType.Number && ExpressionNodeTypeCategories[left.NodeType] == ExpressionNodeTypeCategory.Operator)
                //        {
                //            nodes.Nodes[z].NodeType = ExpressionNodeType.Number;
                //            nodes.Nodes[z].ValueSuffix = right.ValueSuffix;
                //            nodes.Nodes[z].ValueNumber = -1.0 * right.ValueNumber;
                //            nodes.Nodes[z].ValueString = nodes.Nodes[z].ValueNumber.ToString();
                //            nodes.Nodes.RemoveAt(zr);
                //            z = nodes.Nodes.Count - 1;
                //        }
                //    }
                //    else
                //    {
                //        ExpressionNode right = nodes.Nodes[zr];
                //        if (right.NodeType == ExpressionNodeType.Number)
                //        {
                //            nodes.Nodes[z].NodeType = ExpressionNodeType.Number;
                //            nodes.Nodes[z].ValueSuffix = right.ValueSuffix;
                //            nodes.Nodes[z].ValueNumber = -1.0 * right.ValueNumber;
                //            nodes.Nodes[z].ValueString = nodes.Nodes[z].ValueNumber.ToString();
                //            nodes.Nodes.RemoveAt(zr);
                //            z = nodes.Nodes.Count - 1;
                //        }
                //    }
                //}
            }
            return nodes;
        }

        public static ExpressionNodes EvaluateScalarType(ExpressionNodes nodes, ExpressionNodeType nodeType, Func<ExpressionNode, ExpressionNode, ExpressionNodeEvaluationResult> evaluationFunction)
        {
            for (int z = 0; z < nodes.Nodes.Count; z++)
            {
                if (nodes.Nodes[z].ChildNodes != null)
                {
                    nodes.Nodes[z].ChildNodes = EvaluateScalarType(nodes.Nodes[z].ChildNodes, nodeType, evaluationFunction);
                }
                else if (nodes.Nodes[z].NodeType == nodeType)
                {
                    ExpressionNode left = new ExpressionNode() { NodeType = ExpressionNodeType.Number, ValueNumber = 0.0, ValueString = "0", ValueSuffix = "" };
                    if (z > 0)
                        left = nodes.Nodes[z - 1];
                    ExpressionNode right = nodes.Nodes[z + 1];
                    ExpressionNodeType t = nodes.Nodes[z].NodeType;
                    if (left.ValueSuffix == right.ValueSuffix)
                    {
                        ExpressionNodeEvaluationResult newNode = evaluationFunction(left, right);

                        if (newNode.Success)
                        {
                            if (z > 0)
                            {
                                nodes.Nodes[z - 1] = newNode.ResultNode;
                                nodes.Nodes.RemoveAt(z);
                                nodes.Nodes.RemoveAt(z);
                            }
                            else
                            {
                                nodes.Nodes.RemoveAt(z);
                                nodes.Nodes.RemoveAt(z);
                                nodes.Nodes[z] = newNode.ResultNode;
                            }
                            z = 0;
                        }
                    }
                }
            }
            return nodes;
        }

        public static ExpressionNodes OrganizeFunctions(ExpressionNodes nodes)
        {
            for (int z = nodes.Nodes.Count - 1; z >= 0;)
            {
                if (nodes.Nodes[z].ChildNodes != null)
                {
                    nodes.Nodes[z].ChildNodes = OrganizeFunctions(nodes.Nodes[z].ChildNodes);
                }
                else if (nodes.Nodes[z].NodeType == ExpressionNodeType.Function && z < nodes.Nodes.Count - 1)
                {
                    if (nodes.Nodes[z + 1].NodeType == ExpressionNodeType.ExpressionContainer)
                    {
                        nodes.Nodes[z].ChildNodes = nodes.Nodes[z + 1].ChildNodes;
                    }
                    nodes.Nodes.RemoveAt(z + 1);
                }
                z--;
            }
            return nodes;
        }

        public static ExpressionNodes OrganizeContainers(ExpressionNodes nodes, ExpressionNodeType containerOpen, ExpressionNodeType containerClose, ExpressionNodeType containerType)
        {
            for (int z = nodes.Nodes.Count - 1; z >= 0;)
            {
                if (nodes.Nodes[z].ChildNodes != null)
                {
                    nodes.Nodes[z].ChildNodes = OrganizeContainers(nodes.Nodes[z].ChildNodes, containerOpen, containerClose, containerType);
                }
                else if (nodes.Nodes[z].NodeType == containerOpen)
                {
                    int t = z;
                    int lastt = 0;
                    ExpressionNodes childNodes = new ExpressionNodes();
                    for (int w = t + 1; w < nodes.Nodes.Count; w++)
                    {
                        if (nodes.Nodes[w].NodeType != containerClose)
                        {
                            childNodes.Nodes.Add(nodes.Nodes[w]);
                        }
                        else
                        {
                            lastt = w;
                            break;
                        }
                    }
                    int length = lastt - t + 1;
                    for (int v = t + 1; v <= lastt; v++)
                    {
                        nodes.Nodes.RemoveAt(t + 1);
                    }
                    nodes.Nodes[z].NodeType = containerType;
                    nodes.Nodes[z].ChildNodes = childNodes;
                }
                z--;
            }
            return nodes;
        }

        public static ExpressionNodes ParseParentheses(ExpressionNodes nodes)
        {
            for (int z = nodes.Nodes.Count - 1; z >= 0;)
            {
                if (nodes.Nodes[z].ChildNodes != null)
                {
                    nodes.Nodes[z].ChildNodes = ParseParentheses(nodes.Nodes[z].ChildNodes);
                }
                else if (nodes.Nodes[z].NodeType == ExpressionNodeType.OpenParenthesis)
                {
                    int t = z;
                    int lastt = 0;
                    ExpressionNodes childNodes = new ExpressionNodes();
                    for (int w = t + 1; w < nodes.Nodes.Count; w++)
                    {
                        if (nodes.Nodes[w].NodeType != ExpressionNodeType.CloseParenthesis)
                        {
                            childNodes.Nodes.Add(nodes.Nodes[w]);
                        }
                        else
                        {
                            lastt = w;
                            break;
                        }
                    }
                    int length = lastt - t + 1;
                    for (int v = t + 1; v <= lastt; v++)
                    {
                        nodes.Nodes.RemoveAt(t + 1);
                    }
                    nodes.Nodes[z].NodeType = ExpressionNodeType.ExpressionContainer;
                    nodes.Nodes[z].ChildNodes = childNodes;
                }
                z--;
            }
            return nodes;
        }

        public static ExpressionNodes ParseSets(ExpressionNodes nodes)
        {
            for (int z = nodes.Nodes.Count - 1; z >= 0;)
            {
                if (nodes.Nodes[z].ChildNodes != null)
                {
                    nodes.Nodes[z].ChildNodes = ParseSets(nodes.Nodes[z].ChildNodes);
                }
                else if (nodes.Nodes[z].NodeType == ExpressionNodeType.OpenSet)
                {
                    int t = z;
                    int lastt = 0;
                    ExpressionNodes childNodes = new ExpressionNodes();
                    for (int w = t + 1; w < nodes.Nodes.Count; w++)
                    {
                        if (nodes.Nodes[w].NodeType != ExpressionNodeType.CloseSet)
                        {
                            childNodes.Nodes.Add(nodes.Nodes[w]);
                        }
                        else
                        {
                            lastt = w;
                            break;
                        }
                    }
                    int length = lastt - t + 1;
                    for (int v = t + 1; v <= lastt; v++)
                    {
                        nodes.Nodes.RemoveAt(t + 1);
                    }
                    nodes.Nodes[z].NodeType = ExpressionNodeType.SetContainer;
                    nodes.Nodes[z].ChildNodes = childNodes;
                }
                z--;
            }
            return nodes;
        }

        public static ExpressionNodes ParseIndexes(ExpressionNodes nodes)
        {
            for (int z = nodes.Nodes.Count - 1; z >= 0;)
            {
                if (nodes.Nodes[z].ChildNodes != null)
                {
                    nodes.Nodes[z].ChildNodes = ParseIndexes(nodes.Nodes[z].ChildNodes);
                }
                else if (nodes.Nodes[z].NodeType == ExpressionNodeType.OpenIndex)
                {
                    int t = z;
                    int lastt = 0;
                    ExpressionNodes childNodes = new ExpressionNodes();
                    for (int w = t + 1; w < nodes.Nodes.Count; w++)
                    {
                        if (nodes.Nodes[w].NodeType != ExpressionNodeType.CloseIndex)
                        {
                            childNodes.Nodes.Add(nodes.Nodes[w]);
                        }
                        else
                        {
                            lastt = w;
                            break;
                        }
                    }
                    int length = lastt - t + 1;
                    for (int v = t + 1; v <= lastt; v++)
                    {
                        nodes.Nodes.RemoveAt(t + 1);
                    }
                    nodes.Nodes[z].NodeType = ExpressionNodeType.IndexContainer;
                    nodes.Nodes[z].ChildNodes = childNodes;
                }
                z--;
            }
            return nodes;
        }

        public static ExpressionNode ParseNode(string groupName, Group g, Group [] extraGroups = null)
        {
            ExpressionNode node = new ExpressionNode();
            node.OriginalStartIndex = g.Index;
            node.OriginalLength = g.Length;
            node.ValueString = g.Value;
            double outDecimal = 0;
            Double.TryParse(g.Value, out outDecimal);
            node.ValueNumber = outDecimal;

            switch (groupName)
            {
                case "List": node.NodeType = ExpressionNodeType.List; break;
                case "Matrix": node.NodeType = ExpressionNodeType.Matrix; break;
                case "Vector": node.NodeType = ExpressionNodeType.Vector; break;
                case "Set": node.NodeType = ExpressionNodeType.Set; break;
                case "Function":
                    node.ValueString = extraGroups[0].Value;
                    node.NodeType = ExpressionNodeType.Function; break;
                case "Constant": node.NodeType = ExpressionNodeType.Constant; break;
                case "UserVariable": node.NodeType = ExpressionNodeType.UserVariable; break;
                case "Number":
                    node.ValueSuffix = extraGroups[0].Value;
                    node.NodeType = ExpressionNodeType.Number; break;
                case "Complex": node.NodeType = ExpressionNodeType.ComplexNumber; break;
                case "Exponent": node.NodeType = ExpressionNodeType.Exponent; break;
                case "Modulo": node.NodeType = ExpressionNodeType.Modulo; break;
                case "Multiply": node.NodeType = ExpressionNodeType.Multiply; break;
                case "Divide": node.NodeType = ExpressionNodeType.Divide; break;
                case "Add": node.NodeType = ExpressionNodeType.Add; break;
                case "Subtract": node.NodeType = ExpressionNodeType.Subtract; break;
                case "LeftShift": node.NodeType = ExpressionNodeType.LeftShift; break;
                case "RightShift": node.NodeType = ExpressionNodeType.RightShift; break;
                case "Equals": node.NodeType = ExpressionNodeType.Equals; break;
                case "OpenParenthesis": node.NodeType = ExpressionNodeType.OpenParenthesis; break;
                case "CloseParenthesis": node.NodeType = ExpressionNodeType.CloseParenthesis; break;
                case "OpenSet": node.NodeType = ExpressionNodeType.OpenSet; break;
                case "CloseSet": node.NodeType = ExpressionNodeType.CloseSet; break;
                case "OpenIndex": node.NodeType = ExpressionNodeType.OpenIndex; break;
                case "CloseIndex": node.NodeType = ExpressionNodeType.CloseIndex; break;
                case "Comma": node.NodeType = ExpressionNodeType.Comma; break;

                default: return node;
            }
            return node;
        }

        public static ExpressionResult EvaluateExpressionSubset(ExpressionSubset subset, List<ExpressionInputParameter> userParameters = null)
        {
            ExpressionResult result = new ExpressionResult();

            // Here's the order of operations.
            // First we're going to process all user functions, 
            //   then built-in functions, 
            //   then constants, 
            //   then user variables, 
            //   then operators.
            // Our order of operations with operators will be: 
            //   "^", "*", "/", "%", "+", "-", 
            //   where ^ denotes powers, and % is a modulo operation.

            // We need to also look for comma separators in our subset. These are parameters for functions.

            string content = subset.Content;

            // Process the user functions

            // Process the built-in functions

            // Process the constants
            foreach (KeyValuePair<string, double> constant in BuiltInMethods.ConstantList)
            {
                content = content.Replace(constant.Key, constant.Value.ToString());
            }

            // Process the user variables
            if (userParameters != null)
            {
                foreach(ExpressionInputParameter parameter in userParameters)
                {
                    content = content.Replace(parameter.Parameter.ToString(), parameter.Value.ToString());
                }
            }

            return result;
        }

        public static ExpressionSubset ParseParentheses(string input, int index = 0)
        {
            ExpressionSubset result = new ExpressionSubset();
            result.IsParenthesisSubset = true;
            result.Input = input;
            result.ParameterIndex = index;

            int parenthesisStart = input.LastIndexOf('(');
            if (parenthesisStart == -1)
            {
                result.IsParenthesisSubset = false;
                result.IsFinalSubset = true;
                result.Content = input;
                result.ReplacedContent = input;
                return result;
            }

            int parenthesisEnd = input.IndexOf(')', parenthesisStart);
            if (parenthesisStart == -1)
            {
                result.IsError = true;
                return result;
            }

            int length = parenthesisEnd - parenthesisStart + 1;
            string content = input.Substring(parenthesisStart, length);
            string replacedContent = input.Remove(parenthesisStart, length).Insert(parenthesisStart, result.ParameterIndexString);
            result.Content = content;
            result.ReplacedContent = replacedContent;
            result.StartIndex = parenthesisStart;
            result.EndIndex = parenthesisEnd;
            result.IsError = false;

            return result;
        }
    }
}
