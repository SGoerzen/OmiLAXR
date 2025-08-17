/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Endpoints
{
    /// <summary>
    /// Defines a delegate for endpoint actions with no additional parameters.
    /// </summary>
    /// <param name="sender">The endpoint that triggered the action</param>
    public delegate void EndpointAction(Endpoint sender);

    /// <summary>
    /// Defines a delegate for endpoint actions with one additional parameter.
    /// </summary>
    /// <typeparam name="T">Type of the first parameter</typeparam>
    /// <param name="sender">The endpoint that triggered the action</param>
    /// <param name="obj">The first parameter passed with the action</param>
    public delegate void EndpointAction<in T>(Endpoint sender, T obj);

    /// <summary>
    /// Defines a delegate for endpoint actions with two additional parameters.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <param name="sender">The endpoint that triggered the action</param>
    /// <param name="obj1">The first parameter passed with the action</param>
    /// <param name="obj2">The second parameter passed with the action</param>
    public delegate void EndpointAction<in T1, in T2>(Endpoint sender, T1 obj1, T2 obj2);

    /// <summary>
    /// Defines a delegate for endpoint actions with three additional parameters.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <typeparam name="T3">Type of the third parameter</typeparam>
    /// <param name="sender">The endpoint that triggered the action</param>
    /// <param name="obj1">The first parameter passed with the action</param>
    /// <param name="obj2">The second parameter passed with the action</param>
    /// <param name="obj3">The third parameter passed with the action</param>
    public delegate void EndpointAction<in T1, in T2, in T3>(Endpoint sender, T1 obj1, T2 obj2, T3 obj3);

    /// <summary>
    /// Defines a delegate for endpoint actions with four additional parameters.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <typeparam name="T3">Type of the third parameter</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter</typeparam>
    public delegate void EndpointAction<in T1, in T2, in T3, in T4>(Endpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4);

    /// <summary>
    /// Defines a delegate for endpoint actions with five additional parameters.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <typeparam name="T3">Type of the third parameter</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter</typeparam>
    public delegate void EndpointAction<in T1, in T2, in T3, in T4, in T5>(Endpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5);

    /// <summary>
    /// Defines a delegate for endpoint actions with six additional parameters.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <typeparam name="T3">Type of the third parameter</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter</typeparam>
    public delegate void EndpointAction<in T1, in T2, in T3, in T4, in T5, in T6>(Endpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6);
}