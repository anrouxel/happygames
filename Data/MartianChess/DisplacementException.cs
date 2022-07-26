namespace happygames.Data
{
    class DisplacementException : Exception
    {
        public DisplacementException() { }
        public DisplacementException(String message) : base(message) { }
        public DisplacementException(String message, Exception inner) : base(message, inner) { }
    }
}