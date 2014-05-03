texture SceneTexture;
float2 halfPixel;
float blurDistance = 0.003f;

sampler SceneTextureSampler = sampler_state
{
    Texture = (SceneTexture);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;
};

struct VertexShaderInput
{
    float3 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = float4(input.Position,1);
    output.TexCoord = input.TexCoord - halfPixel;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 blurredColor;
    blurredColor = tex2D(SceneTextureSampler, float2(input.TexCoord[0] + blurDistance, input.TexCoord[1] + blurDistance));
    blurredColor += tex2D(SceneTextureSampler, float2(input.TexCoord[0] - blurDistance, input.TexCoord[1] + blurDistance));
    blurredColor += tex2D(SceneTextureSampler, float2(input.TexCoord[0] + blurDistance, input.TexCoord[1] - blurDistance));
    blurredColor += tex2D(SceneTextureSampler, float2(input.TexCoord[0] - blurDistance, input.TexCoord[1] - blurDistance));

    return blurredColor / 4;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
