/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Represents the status codes for statement transfer operations.
    /// Mirrors HTTP status codes with additional custom statuses.
    /// </summary>
    public enum TransferCode
    {
        /// <summary>
        /// Statements were queued and sent later.
        /// </summary>
        Queued = 202,
        
        /// <summary>
        /// No statements to transfer (HTTP 204 No Content)
        /// </summary>
        NoStatements = 204,

        /// <summary>
        /// Authentication failed due to invalid credentials (HTTP 401 Unauthorized)
        /// </summary>
        InvalidCredentials = 401,

        /// <summary>
        /// Generic server-side error occurred (HTTP 500 Internal Server Error)
        /// </summary>
        Error = 500,

        /// <summary>
        /// Server is currently unable to handle the request (HTTP 503 Service Unavailable)
        /// </summary>
        Busy = 503,

        /// <summary>
        /// Transfer completed successfully (HTTP 200 OK)
        /// </summary>
        Success = 200
    }
}