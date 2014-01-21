using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Deimos
{
    class LightManager
    {
        // Attributes
        private Dictionary<string, DirectionalLight> DirectionalLights = 
            new Dictionary<string, DirectionalLight>();
        private Dictionary<string, PointLight> PointLights = 
            new Dictionary<string, PointLight>();
        private Dictionary<string, SpotLight> SpotLights = 
            new Dictionary<string, SpotLight>();


        // Methods
        public DirectionalLight AddDirectionalLight(string name,
            Vector3 direction, Color color)
        {
            DirectionalLight thisLight = 
                new DirectionalLight(direction, color);
            DirectionalLights.Add(name, thisLight);

            return thisLight;
        }

        public PointLight AddPointLight(string name, Vector3 position, 
            float radius, float intensity, Color color)
        {
            PointLight thisLight = 
                new PointLight(position, radius, intensity, color);
            PointLights.Add(name, thisLight);

            return thisLight;
        }

        public SpotLight AddSpotLight(string name, Vector3 position, 
            Vector3 direction, Color color, float power)
        {
            SpotLight thisLight = new SpotLight(
                position, 
                direction, 
                color, 
                power
            );
            SpotLights.Add(name, thisLight);

            return thisLight;
        }

        public Dictionary<string, DirectionalLight> GetDirectionalLights()
        {
            return DirectionalLights;
        }
        public Dictionary<string, PointLight> GetPointLights()
        {
            return PointLights;
        }
        public Dictionary<string, SpotLight> GetSpotLights()
        {
            return SpotLights;
        }

        public DirectionalLight GetDirectionalLight(string name)
        {
            return DirectionalLights[name];
        }
        public PointLight GetPointLight(string name)
        {
            return PointLights[name];
        }
        public SpotLight GetSpotLight(string name)
        {
            return SpotLights[name];
        }
    }
}
