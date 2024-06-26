﻿namespace Supercell.Laser.Server.Helpers.Json
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Supercell.Laser.Titan.Logic.Math;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class TimerConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified type.
        /// </summary>
        public override bool CanConvert(Type type)
        {
            return type == typeof(LogicTimer);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JsonConverter" /> can read JSON.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JsonConverter" /> can write JSON.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Reads the json.
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jTimer = JObject.Load(reader);

            int remainingTime = (int)(jTimer["t"] ?? -1);

            if (remainingTime >= 0)
            {
                LogicTimer timer = new LogicTimer(new LogicTime());

                if (timer.Time != null)
                {
                    timer.StartTimer(remainingTime);
                }
                else
                {
                    timer.TotalSeconds = remainingTime;
                    timer.StartSubTick = 0;
                }

                existingValue = (object)timer;
            }

            return existingValue;
        }

        /// <summary>
        /// Writes the json.
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            int remaining = -1;

            if (value != null)
            {
                LogicTimer timer = (LogicTimer)value;

                if (timer.Started)
                {
                    remaining = timer.RemainingSecs;
                }
            }

            writer.WriteStartObject();
            writer.WritePropertyName("t");
            writer.WriteValue(remaining);
            writer.WriteEndObject();
        }
    }
}
