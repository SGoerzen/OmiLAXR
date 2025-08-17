/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR
{
    /// <summary>
    /// Interface for components that extend DataProvider functionality through composition.
    /// Enables modular enhancement of data providers without inheritance or core modification.
    /// Extensions can add composers, hooks, and endpoints to existing data providers.
    /// </summary>
    public interface IDataProviderExtension
    {
        /// <summary>
        /// Gets the DataProvider instance that this extension is associated with.
        /// Used to access the extended DataProvider's functionality and state.
        /// </summary>
        /// <returns>The DataProvider instance being extended</returns>
        DataProvider GetDataProvider();
        
        /// <summary>
        /// Extends the specified DataProvider with additional functionality.
        /// Called during initialization to register extension components with the target provider.
        /// </summary>
        /// <param name="dataProvider">The DataProvider to extend with additional capabilities</param>
        void Extend(DataProvider dataProvider);
    }
}