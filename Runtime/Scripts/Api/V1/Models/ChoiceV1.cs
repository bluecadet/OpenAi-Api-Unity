using OpenAi.Json;

using System;
using UnityEngine;

namespace OpenAi.Api.V1
{
    /// <summary>
    /// A single choice returned by the OpenAi Api completion endpoint
    /// </summary>
    public class ChoiceV1 : AModelV1
    {
        /// <summary>
        /// The returned text
        /// </summary>
        public string text;

        /// <summary>
        /// the index of the choice
        /// </summary>
        public int index;

        /// <summary>
        /// The log probabilities
        /// </summary>
        public string logprobs;

        /// Start of Bluecadet addition ///

        /// <summary>
        /// Log probability 0
        /// </summary>
        public float? logprob0 = null;

        /// <summary>
        /// Log probability 1
        /// </summary>
        public float? logprob1 = null;

        /// <summary>
        /// Log probability 2
        /// </summary>
        public float? logprob2 = null;

        /// End of Bluecadet addition ///

        /// <summary>
        /// The reason the engine ended the completion
        /// </summary>
        public string finish_reason;

        /// <inheritdoc />
        public override string ToJson()
        {
            JsonBuilder jb = new JsonBuilder();

            jb.StartObject();
            jb.Add(nameof(text), text);
            jb.Add(nameof(index), index);
            jb.Add(nameof(logprobs), logprobs);

            /// Start of Bluecadet addition ///
            /// 
            jb.Add(nameof(logprob0), logprob0);
            jb.Add(nameof(logprob1), logprob1);
            jb.Add(nameof(logprob2), logprob2);

            /// End of Bluecadet addition ///


            jb.Add(nameof(finish_reason), finish_reason);
            jb.EndObject();

            return jb.ToString();
        }

        /// <inheritdoc />
        public override void FromJson(JsonObject jsonObj)
        {
            if (jsonObj.Type != EJsonType.Object) throw new Exception("Must be an object");

            foreach (JsonObject jo in jsonObj.NestedValues)
            {
                switch (jo.Name)
                {
                    case nameof(text):
                        text = jo.StringValue;
                        break;
                    case nameof(index):
                        index = int.Parse(jo.StringValue);
                        break;
                    case nameof(logprobs):
                        // Original code (doesn't work):
                        logprobs = jo.StringValue;

                        /// Start of Bluecadet addition ///

                        // Bluecadet code to get toplogprobs:
                        foreach (JsonObject njo in jo.NestedValues)
                        {
                            if (njo.Name == "top_logprobs")
                            {
                                JsonObject nnjo = njo.NestedValues[0];

                                foreach (JsonObject nnnjo in nnjo.NestedValues)
                                {
                                    switch (nnnjo.Name)
                                    {
                                        case "0":
                                            {
                                                float value = float.Parse(nnnjo.StringValue);

                                                if (logprob0 == null || value > logprob0)
                                                {
                                                    logprob0 = value;
                                                }

                                                break;
                                            }
                                        case "1":
                                            {
                                                float value = float.Parse(nnnjo.StringValue);

                                                if (logprob1 == null || value > logprob1)
                                                {
                                                    logprob1 = value;
                                                }

                                                break;
                                            }

                                        case "2":
                                            {
                                                float value = float.Parse(nnnjo.StringValue);

                                                if (logprob2 == null || value > logprob2)
                                                {
                                                    logprob2 = value;
                                                }

                                                break;
                                            }
                                    }
                                }
                            }
                        }

                        /// End of Bluecadet addition ///

                        break;
                    case nameof(finish_reason):
                        finish_reason = jo.StringValue;
                        break;
                }
            }
        }
    }
}