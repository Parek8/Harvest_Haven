namespace BgTools.PlayerPrefsEditor
{
    [System.Serializable]
    internal class PreferenceEntry
    {
        internal enum PrefTypes
        {
            String = 0,
            Int = 1,
            Float = 2
        }

        internal PrefTypes m_typeSelection;
        internal string m_key;

        // Need diffrend ones for auto type selection of serilizedProerty
        internal string m_strValue;
        internal int m_intValue;
        internal float m_floatValue;

        internal string ValueAsString()
        {
            switch(m_typeSelection)
            {
                case PrefTypes.String:
                    return m_strValue;
                case PrefTypes.Int:
                    return m_intValue.ToString();
                case PrefTypes.Float:
                    return m_floatValue.ToString();
                default:
                    return string.Empty;
            }
        }
    }
}