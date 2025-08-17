/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.ComponentModel;
using OmiLAXR.Composers;
using OmiLAXR.Utils;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Debug endpoint that prints all received statements to the Unity Editor console.
    /// Provides multiple formatting options for statement visualization during development and testing.
    /// Useful for debugging statement flow, verifying data content, and monitoring system behavior.
    /// </summary>
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Statement Printer")]
    [Description("Prints all received statements to Unity Editor console. May be used for testing purposes.")]
    public class StatementPrinter : Endpoint
    {
        /// <summary>
        /// Conservative batch size for console output to prevent overwhelming the Unity console.
        /// Lower than typical endpoints to maintain readable console output during debugging.
        /// </summary>
        protected override int MaxBatchSize => 10;

        /// <summary>
        /// Available formatting options for statement output.
        /// Each type provides different levels of detail and formatting styles for various debugging needs.
        /// </summary>
        [Serializable]
        public enum PrintType
        {
            /// <summary>
            /// Standard ToString() representation of the statement.
            /// Provides basic statement information in the default format.
            /// </summary>
            Default,

            /// <summary>
            /// Condensed representation with key information only.
            /// Useful for quick overview without overwhelming detail.
            /// </summary>
            Short,

            /// <summary>
            /// Full JSON serialization of the statement.
            /// Provides complete data structure visibility for detailed analysis.
            /// </summary>
            Json,

            /// <summary>
            /// CSV format with standard structure preservation.
            /// Shows how the statement would appear in CSV export format.
            /// Maintains object hierarchy and nested structures.
            /// </summary>
            Csv,

            /// <summary>
            /// Flattened CSV format with all nested structures expanded.
            /// Provides comprehensive field-by-field view of all statement data.
            /// Useful for understanding complete data transformation.
            /// </summary>
            CsvFlat
        }
        
        /// <summary>
        /// Selected print format for statement output.
        /// Configurable in the Unity Inspector to switch between different visualization modes.
        /// </summary>
        public PrintType printType = PrintType.Default;

        /// <summary>
        /// Handles the processing of individual statements for console output.
        /// Applies the selected formatting and prints the result to the Unity console.
        /// Always returns success as console printing cannot fail in typical scenarios.
        /// </summary>
        /// <param name="statement">The statement to process and display</param>
        /// <returns>Always returns TransferCode.Success as console output cannot fail</returns>
        protected override TransferCode HandleSending(IStatement statement)
        {
            // Process the statement based on the selected print format
            switch (printType)
            {
                case PrintType.Json:
                    // Convert statement to JSON string for detailed structure analysis
                    PrintStatement(statement.ToJsonString());
                    break;

                case PrintType.Short:
                    // Use condensed format for quick overview
                    PrintStatement(statement.ToShortString());
                    break;

                case PrintType.Csv:
                {
                    // Convert to CSV format maintaining original structure
                    var csvFormat = statement.ToCsvFormat();
                    PrintStatement(csvFormat.ToString());
                    break;
                }

                case PrintType.CsvFlat:
                {
                    // Convert to flattened CSV format with all nested data expanded
                    // The 'true' parameter enables deep flattening of complex objects
                    var csvFormat = statement.ToCsvFormat(true);
                    PrintStatement(csvFormat.ToString());
                    break;
                }

                default:
                    // Use default ToString() implementation for standard representation
                    PrintStatement(statement.ToString());
                    break;
            }

            // Console printing operations don't fail, so always return success
            return TransferCode.Success;
        }
        
        /// <summary>
        /// Static utility method for consistent statement printing to the console.
        /// Prefixes all output with "Sent statement:" for easy identification in console logs.
        /// Uses the OmiLAXR debug logging system for consistent formatting and filtering.
        /// </summary>
        /// <param name="message">The formatted statement string to print</param>
        private static void PrintStatement(string message)
            => DebugLog.OmiLAXR.Print("Sent statement: " + message);

        public override DataMap ProvideDataMap()
        {
            return new DataMap()
            {
                ["printType"] = printType.ToString()
            };
        }

        public override void ConsumeDataMap(DataMap map)
        {
            if (Enum.TryParse(map["printType"] as string, out PrintType p))
                printType = p;
        }
    }
}