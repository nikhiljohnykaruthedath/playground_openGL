#version 330 core 

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    sampler2D emission;
    float     shininess;
};

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

out vec4 color;

uniform vec3 viewPos;
uniform Material material;
uniform Light light;
uniform float time;

void main() {
    // Ambient
    // Use specular texture
    vec3 ambient = light.ambient * vec3(texture(material.specular, TexCoords));  
    
    // Diffuse
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    // Use specular texture
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.specular, TexCoords));   
    
    // Specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // Use specular texture
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));  
    
    
    // Emission
    vec3 emission = vec3(0.0);

    // Rough check for blackbox inside spec texture
    if (texture(material.specular, TexCoords).r == 0.0)   
    {
        // Apply emission texture
        emission = texture(material.emission, TexCoords).rgb;
        
    }
    
   // Output
    color = vec4(ambient + diffuse + specular + emission, 1.0f);
}