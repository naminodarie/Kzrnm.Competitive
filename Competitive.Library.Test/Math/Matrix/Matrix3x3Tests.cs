﻿using AtCoder;
using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.MathNS.Matrix
{
    public class Matrix3x3Tests
    {
        [Fact]
        [Trait("Category", "Operator")]
        public void SingleMinus()
        {
            (-new Matrix3x3<long, LongOperator>(
                (1, 2, 3),
                (5, 6, 7),
                (9, 10, 11)
            )).Should().Be(new Matrix3x3<long, LongOperator>(
                (-1, -2, -3),
                (-5, -6, -7),
                (-9, -10, -11)
            ));
        }

        public static TheoryData Add_Data = new TheoryData<Matrix3x3<long, LongOperator>, Matrix3x3<long, LongOperator>, Matrix3x3<long, LongOperator>>
        {
            {
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                ),
                Matrix3x3<long, LongOperator>.Identity,
                new Matrix3x3<long, LongOperator>(
                    (2, 2, 3),
                    (5, 7, 7),
                    (9, 10, 12)
                )
            },
            {
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                ),
                new Matrix3x3<long, LongOperator>(
                    (1, -2, 3),
                    (5, -6, 7),
                    (9, -10, 11)
                ),
                new Matrix3x3<long, LongOperator>(
                    (2, 0, 6),
                    (10, 0, 14),
                    (18, 0, 22)
                )
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Add_Data))]
        public void Add(Matrix3x3<long, LongOperator> mat1, Matrix3x3<long, LongOperator> mat2, Matrix3x3<long, LongOperator> expected)
        {
            (mat1 + mat2).Should().Be(expected);
            (mat2 + mat1).Should().Be(expected);
            default(Matrix3x3Operator<long, LongOperator>).Add(mat1, mat2).Should().Be(expected);
            default(Matrix3x3Operator<long, LongOperator>).Add(mat2, mat1).Should().Be(expected);
        }

        public static TheoryData Subtract_Data = new TheoryData<Matrix3x3<long, LongOperator>, Matrix3x3<long, LongOperator>, Matrix3x3<long, LongOperator>>
        {
            {
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                ),
                new Matrix3x3<long, LongOperator>(
                    (1, -2, 3),
                    (5, -6, 7),
                    (9, -10, 11)
                ),
                new Matrix3x3<long, LongOperator>(
                    (0, 4, 0),
                    (0, 12, 0),
                    (0, 20, 0)
                )
            },
            {
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                ),
                Matrix3x3<long, LongOperator>.Identity,
                new Matrix3x3<long, LongOperator>(
                    (0, 2, 3),
                    (5, 5, 7),
                    (9, 10, 10)
                )
            },
            {
                Matrix3x3<long, LongOperator>.Identity,
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                ),
                new Matrix3x3<long, LongOperator>(
                    (0, -2, -3),
                    (-5, -5, -7),
                    (-9, -10, -10)
                )
            },
        };
        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Subtract_Data))]
        public void Subtract(Matrix3x3<long, LongOperator> mat1, Matrix3x3<long, LongOperator> mat2, Matrix3x3<long, LongOperator> expected)
        {
            (mat1 - mat2).Should().Be(expected);
            default(Matrix3x3Operator<long, LongOperator>).Subtract(mat1, mat2).Should().Be(expected);
        }

        public static TheoryData Multiply_Data = new TheoryData<Matrix3x3<long, LongOperator>, Matrix3x3<long, LongOperator>, Matrix3x3<long, LongOperator>>
        {
            {
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                ),
                new Matrix3x3<long, LongOperator>(
                    (1, -2, 3),
                    (5, -6, -7),
                    (9, -10, 11)
                ),
                new Matrix3x3<long, LongOperator>(
                    (38, -44, 22),
                    (98, -116, 50),
                    (158, -188, 78)
                )
            },
            {
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                ),
                Matrix3x3<long, LongOperator>.Identity,
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                )
            },
            {
                Matrix3x3<long, LongOperator>.Identity,
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                ),
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                )
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(Multiply_Data))]
        public void Multiply(Matrix3x3<long, LongOperator> mat1, Matrix3x3<long, LongOperator> mat2, Matrix3x3<long, LongOperator> expected)
        {
            (mat1 * mat2).Should().Be(expected);
            default(Matrix3x3Operator<long, LongOperator>).Multiply(mat1, mat2).Should().Be(expected);
        }

        public static TheoryData MultiplyScalar_Data = new TheoryData<long, Matrix3x3<long, LongOperator>, Matrix3x3<long, LongOperator>>
        {
            {
                3,
                Matrix3x3<long, LongOperator>.Identity,
                new Matrix3x3<long, LongOperator>(
                    (3, 0, 0),
                    (0, 3, 0),
                    (0, 0, 3)
                )
            },
            {
                3,
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                ),
                new Matrix3x3<long, LongOperator>(
                    (3, 6, 9),
                    (15, 18, 21),
                    (27, 30, 33)
                )
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyScalar_Data))]
        public void MultiplyScalar(long a, Matrix3x3<long, LongOperator> mat, Matrix3x3<long, LongOperator> expected)
        {
            (a * mat).Should().Be(expected);
        }

        public static TheoryData MultiplyVector_Data = new TheoryData<Matrix3x3<long, LongOperator>, (long, long, long), (long, long, long)>
        {
            {
                new Matrix3x3<long, LongOperator>(
                    (3, 0, 0),
                    (0, 3, 0),
                    (0, 0, 3)
                ),
                (1,2,3),
                (3,6,9)
            },
            {
                new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (4, 5, 6),
                    (7, 8, 9)
                ),
                (1,2,3),
                (14, 32, 50)
            },
        };

        [Theory]
        [Trait("Category", "Operator")]
        [MemberData(nameof(MultiplyVector_Data))]
        public void MultiplyVector(Matrix3x3<long, LongOperator> mat, (long v0, long v1, long v2) vector, (long, long, long) expected)
        {
            (mat * vector).Should().Be(expected);
            mat.Multiply(vector).Should().Be(expected);
            mat.Multiply(vector.v0, vector.v1, vector.v2).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Pow()
        {
            var orig = new Matrix3x3<long, LongOperator>(
                    (1, 2, 3),
                    (5, 6, 7),
                    (9, 10, 11)
                );
            orig.Pow(5).Should().Be(144 * new Matrix3x3<long, LongOperator>(
                    (1825, 2162, 2499),
                    (4847, 5742, 6637),
                    (7869, 9322, 10775)
                ));
            var cur = orig;
            for (int i = 1; i < 10; i++)
            {
                orig.Pow(i).Should().Be(cur);
                cur *= orig;
            }
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Determinant()
        {
            new Matrix3x3<Fraction, FractionOperator>(
                (10, -9, -12),
                (7, -12, 11),
                (-10, 10, 3)
            ).Determinant().Should().Be(319);
        }

        [Fact]
        [Trait("Category", "Normal")]
        public void Inv()
        {
            var orig = new Matrix3x3<Fraction, FractionOperator>(
                (10, -9, -12),
                (7, -12, 11),
                (-10, 10, 3)
            );
            var inv = orig.Inv();
            inv.Should().Be(new Matrix3x3<Fraction, FractionOperator>(
                (new Fraction(-146, 319), new Fraction(-93, 319), new Fraction(-243, 319)),
                (new Fraction(-131, 319), new Fraction(-90, 319), new Fraction(-194, 319)),
                (new Fraction(-50, 319), new Fraction(-10, 319), new Fraction(-57, 319))
            ));
            (orig * inv).Should().Be(Matrix3x3<Fraction, FractionOperator>.Identity);
            (inv * orig).Should().Be(Matrix3x3<Fraction, FractionOperator>.Identity);
        }
    }
}