﻿namespace NecManager.Common;
using System;

/// <summary>
///     Record of all ids use to track a technical calls to subsystems.
/// </summary>
/// <param name="CorrelationId">Unique Id for the whole transaction.</param>
/// <param name="FunctionalId">Unique Id generated by the service initiating the transaction.</param>
/// <param name="TechnicalId">Unique Id that represent the caller component.</param>
public record ServiceMonitoringDefinition(Guid CorrelationId, string FunctionalId, Guid TechnicalId)
{
    /// <summary>
    ///     The correlation id key.
    ///     <remarks>
    ///         It's generated by the first call.
    ///     </remarks>
    /// </summary>
    public const string CorrelationIdKey = "correlation-id";

    /// <summary>
    ///     The functional id key.
    /// </summary>
    public const string FunctionalIdKey = "functional-id";

    /// <summary>
    ///     The technical id key.
    /// </summary>
    public const string TechnicalIdKey = "technical-id";

    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceMonitoringDefinition" /> class.
    /// </summary>
    /// <param name="functionalId">
    ///     The functional id.
    ///     <remarks>
    ///         Depends of functional process.
    ///     </remarks>
    /// </param>
    /// <param name="technicalId">
    ///     <remarks>
    ///         Depends of technical application.
    ///     </remarks>
    /// </param>
    public ServiceMonitoringDefinition(string functionalId, Guid technicalId)
        : this(Guid.NewGuid(), functionalId, technicalId)
    {
    }
}