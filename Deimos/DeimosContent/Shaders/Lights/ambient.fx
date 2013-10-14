///////////////////////
// Variables
///////////////////////
// Main variables
float4x4 World;
float4x4 View;
float4x4 Projection;

// Used for ambient lighting
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

// Used for difused lighting
float4x4 WorldInverseTranspose;

float3 DiffuseLightDirection = float3(1, 0, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;



///////////////////////
// Structures
///////////////////////
struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

////////////////////////
// Functions
////////////////////////
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 normal = mul(input.Normal, WorldInverseTranspose);
	float lightIntensity = dot(normal, DiffuseLightDirection);
	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return saturate(input.Color + AmbientColor * AmbientIntensity);
}

technique Ambient
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}