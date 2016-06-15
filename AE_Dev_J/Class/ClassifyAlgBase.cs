using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AE_Dev_J.Class
{
    class ClassifyAlgBase
    {
        /// <summary>
        /// 分类方法枚举
        /// </summary>
        public enum ClassifyMethod
        {
            None,
            Parallelepiped,
            MinimumDistance,
            MahalanobisDistance,
            MaximumLikelihood,
            SpectralAngleMapper,
            SpectralInformationDivergence,
            BinaryEncoding,
            NeuralNet,
            SupportVectrorMachine,
            IsoData,
            KMeans
        };

        /// <summary>
        /// 根据classifyMethod枚举，获取方法字符串
        /// </summary>
        /// <param name="method">classifyMethod枚举</param>
        /// <returns></returns>
        public static string getMethodString(ClassifyMethod method)
        {
            switch (method)
            {
                case ClassifyMethod.Parallelepiped:
                    return "Paralleleepiped";

                case ClassifyMethod.MinimumDistance:
                    return "Minimum Distance";

                case ClassifyMethod.MahalanobisDistance:
                    return "Mahalanobis Distance";

                case ClassifyMethod.MaximumLikelihood:
                    return "Maximum Likelihood";

                case ClassifyMethod.SpectralAngleMapper:
                    return "Spectral Angle Mapper";

                case ClassifyMethod.SpectralInformationDivergence:
                    return "Spectral Information Divergence";

                case ClassifyMethod.BinaryEncoding:
                    return "Binary Encoding";

                case ClassifyMethod.NeuralNet:
                    return "Artifical Neural Net";

                case ClassifyMethod.SupportVectrorMachine:
                    return "Support Vector Machine";

                case ClassifyMethod.IsoData:
                    return "ISODATA";

                case ClassifyMethod.KMeans:
                    return "K-Means";

                default:
                    return "None";
            }
        }
    }
}
