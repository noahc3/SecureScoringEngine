using System;
using System.Collections.Generic;
using System.Text;

namespace SSECommon {
    public static class Constants {
        public const string OFFLINE_SCORING_CONFIGURATION_PASSWORD = "SSE_CONFIGURATION_PASSWORD";

        public const string KEY_EXCHANGE_SANITY_CHECK = "KEY EXCHANGE SUCCESS";

        public const string CRYPTO_API_WINDOWS = "CNG";
        public const string CRYPTO_API_UNIX = "OPENSSL";

        public const string SCORING_PROCESS_START = "START SCORING PROCESS";
        public const string SCORING_PROCESS_CONTINUE = "CONTINUE SCORING PROCESS";
        public const string SCORING_PROCESS_EXECUTE_PAYLOAD = "SCORING PROCESS EXECUTE PAYLOAD";
        public const string SCORING_PROCESS_FINISHED = "SCORING PROCESS FINISHED";
    }
}
