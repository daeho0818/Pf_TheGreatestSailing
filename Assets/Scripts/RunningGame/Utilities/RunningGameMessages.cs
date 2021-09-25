public static class RunningGameMessages
{
    public readonly struct KeyDownMessage
    {
        public UnityEngine.KeyCode KeyCode { get; }

        public KeyDownMessage(UnityEngine.KeyCode keyCode)
        {
            KeyCode = keyCode;
        }
    }

    public readonly struct KeyUpMessage
    {
        public UnityEngine.KeyCode KeyCode { get; }

        public KeyUpMessage(UnityEngine.KeyCode keyCode)
        {
            KeyCode = keyCode;
        }
    }

    public readonly struct SystemSpeedCoefficientChangedMessage
    {
        public float SystemSpeedCoefficient { get; }

        public SystemSpeedCoefficientChangedMessage(float speedCoefficient)
        {
            SystemSpeedCoefficient = speedCoefficient;
        }
    }

    public readonly struct CoefficientChangedMessage
    {
        public float SpeedCoefficient { get; }
        public float SpawnCoefficient { get; }

        public CoefficientChangedMessage(float speedCoefficient, float spawnCoefficient)
        {
            SpeedCoefficient = speedCoefficient;
            SpawnCoefficient = spawnCoefficient;
        }
    }
}
