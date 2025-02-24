namespace PGCTools.Helpers.Playfab.DataStructs
{
    /// <summary>
    /// The struct taht defines the data to convert to send.
    /// </summary>
    [System.Serializable]
    public struct CloudScriptParameterData
    {
        /// <summary>
        /// Statistic name.
        /// </summary>
        public string Key;
        /// <summary>
        /// Statistic value
        /// </summary>
        public string Value;
    }
    /// <summary>
    /// is the paramaters to pass to the helper class containing the values of static name and static value.
    /// </summary>
    [System.Serializable]
    public struct CloudScriptParameterValues
    {
        /// <summary>
        /// The key value of the key field of CloudScriptParamterData.
        /// </summary>
        public string KeyValue;
        /// <summary>
        /// The value of the field value of CloudScriptParamterData.
        /// /// </summary>
        public string ValueValue;
    }
}
