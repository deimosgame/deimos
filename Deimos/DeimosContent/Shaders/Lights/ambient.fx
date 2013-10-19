float4x4 WVP;
float3x3 World;

float3 Ke;
float3 Ka;
float3 Kd;
float3 Ks;
float specularPower;

float3 globalAmbient;
float3 lightColor;

float3 eyePosition;
float3 lightDirection;
float3 lightPosition;
float spotPower;

texture2D Texture;
sampler2D texSampler = sampler_state
{
    Texture = <Texture>;
    MinFilter = anisotropic;
    MagFilter = anisotropic;
    MipFilter = linear;
    MaxAnisotropy = 16;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 Texture  : TEXCOORD0;
    float3 Normal   : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 Texture  : TEXCOORD0;
    float3 PositionO: TEXCOORD1;
    float3 Normal   : NORMAL0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = mul(input.Position, WVP);

    output.Normal = input.Normal;

    output.PositionO = input.Position.xyz;

    output.Texture = input.Texture;
  
    return output;
}

float4 PSAmbient(VertexShaderOutput input) : COLOR0
{
    return float4(Ka*globalAmbient + Ke,1) * tex2D(texSampler,input.Texture);
}

float4 PSDirectionalLight(VertexShaderOutput input) : COLOR0
{
    //Difuze
    float3 L = normalize(-lightDirection);
    float diffuseLight = max(dot(input.Normal,L), 0);
    float3 diffuse = Kd*lightColor*diffuseLight;

    //Specular
    float3 V = normalize(eyePosition - input.PositionO);
    float3 H = normalize(L + V);
    float specularLight = pow(max(dot(input.Normal,H),0),specularPower);
    if(diffuseLight<=0) specularLight=0;
    float3 specular = Ks * lightColor * specularLight;

    //sum all light components
    float3 light = diffuse + specular;

    return float4(light,1) * tex2D(texSampler,input.Texture);
}

float4 PSPointLight(VertexShaderOutput input) : COLOR0
{
    //Difuze
    float3 L = normalize(lightPosition - input.PositionO);
    float diffuseLight = max(dot(input.Normal,L), 0);
    float3 diffuse = Kd*lightColor*diffuseLight;

    //Specular
    float3 V = normalize(eyePosition - input.PositionO);
    float3 H = normalize(L + V);
    float specularLight = pow(max(dot(input.Normal,H),0),specularPower);
    if(diffuseLight<=0) specularLight=0;
    float3 specular = Ks * lightColor * specularLight;

    //sum all light components
    float3 light = diffuse + specular;

    return float4(light,1) * tex2D(texSampler,input.Texture);
}

float4 PSSpotLight(VertexShaderOutput input) : COLOR0
{
    //Difuze
    float3 L = normalize(lightPosition - input.PositionO);
    float diffuseLight = max(dot(input.Normal,L), 0);
    float3 diffuse = Kd*lightColor*diffuseLight;

    //Specular
    float3 V = normalize(eyePosition - input.PositionO);
    float3 H = normalize(L + V);
    float specularLight = pow(max(dot(input.Normal,H),0),specularPower);
    if(diffuseLight<=0) specularLight=0;
    float3 specular = Ks * lightColor * specularLight;

    //spot scale
    float spotScale = pow(max(dot(L,-lightDirection),0),spotPower);

    //sum all light components
    float3 light = (diffuse + specular)*spotScale;

    return float4(light,1) * tex2D(texSampler,input.Texture);
}

technique MultiPassLight
{
    pass Ambient
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PSAmbient();
    }
    pass Directional
    {
        PixelShader = compile ps_3_0 PSDirectionalLight();
    }
    pass Point
    {
        PixelShader = compile ps_3_0 PSPointLight();
    }
    pass Spot
    {
        PixelShader = compile ps_3_0 PSSpotLight();
    }
}